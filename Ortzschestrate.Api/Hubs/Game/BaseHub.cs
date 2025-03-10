using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Ortzschestrate.Api.Hubs.Game;
using Ortzschestrate.Api.Models;
using Ortzschestrate.Data.Models;
using Ortzschestrate.Web3.Actions;
using GameResult = Ortzschestrate.Web3.Actions.GameResult;

namespace Ortzschestrate.Api.Hubs;

[Authorize]
public partial class GameHub : Hub<IGameClient>
{
    private static readonly ConcurrentDictionary<string, PendingGame> _pendingGamesByCreatorId = new();
    private static readonly ConcurrentDictionary<Guid, Models.Game> _ongoingShortGames = new();
    private static readonly SemaphoreSlim _lobbySemaphore = new(1, 1);
    private readonly PlayerCache _playerCache;
    private readonly StartGame _startGame;
    private readonly UserManager<User> _userManager;
    private readonly ResolveGame _resolveGame;

    public GameHub(PlayerCache playerCache, StartGame startGame, UserManager<User> userManager, ResolveGame resolveGame)
    {
        _playerCache = playerCache;
        _startGame = startGame;
        _userManager = userManager;
        _resolveGame = resolveGame;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await _playerCache.OnNewConnectionAsync(Context);
        Debug.WriteLine($"New client connected");
        // To send the pending games to the newly joined client.
        await Clients.Caller.LobbyUpdated(_pendingGamesByCreatorId.Values.ToList());
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        bool lobbyUpdated = false;
        Models.Game? endedGame = null;
        await _lobbySemaphore.WaitAsync();
        try
        {
            if (_playerCache.GetRemainingConnections(Context.UserIdentifier!) == 1) // Last connection's closing.
            {
                var leavingPlayer = _playerCache.GetPlayer(Context.UserIdentifier!);

                if (leavingPlayer.OngoingShortGame != null)
                {
                    if (leavingPlayer.OngoingShortGame.MovesMade >= 10)
                    {
                        leavingPlayer.OngoingShortGame.Resign(leavingPlayer);
                    }
                    else
                    {
                        leavingPlayer.OngoingShortGame.Draw();
                    }

                    endedGame = leavingPlayer.OngoingShortGame;
                }
                else if (_pendingGamesByCreatorId.TryRemove(Context.UserIdentifier!, out _))
                {
                    lobbyUpdated = true;
                }
            }
        }
        finally
        {
            _lobbySemaphore.Release();
            _playerCache.OnDisconnect(Context);
            if (endedGame != null)
                await finalizeEndedGameAsync(endedGame);
            if (lobbyUpdated)
                await Clients.All.LobbyUpdated(_pendingGamesByCreatorId.Values.ToList());
        }

        Debug.WriteLine("Connection closed");
        Debug.WriteLine(exception);
        await base.OnDisconnectedAsync(exception);
    }

    private async Task finalizeEndedGameAsync(Models.Game game)
    {
        if (game.IsWagered)
            await _resolveGame.DoAsync(game.Id, findWeb3GameResult(game));

        await Clients.Users([game.Player1.UserId, game.Player2.UserId])
            .GameEnded(new(game.EndGame!.EndgameType.ToString(), game.EndGame.WonSide?.AsChar));
        game.Player1.OngoingShortGame = null;
        game.Player2.OngoingShortGame = null;
        _ongoingShortGames.TryRemove(game.Id, out _);
    }

    private GameResult findWeb3GameResult(Models.Game game)
    {
        if (game.EndGame!.WonSide == null)
        {
            return GameResult.Draw;
        }

        if (game.EndGame.WonSide == game.Player1Color)
        {
            return GameResult.Player1Won;
        }

        return GameResult.Player2Won;
    }
}