using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Ortzschestrate.Api.Hubs.Game;
using Ortzschestrate.Api.Models;

namespace Ortzschestrate.Api.Hubs;

[Authorize]
public partial class GameHub : Hub<IGameClient>
{
    private static readonly ConcurrentDictionary<string, PendingGame> _pendingGamesByCreatorId = new();
    private static readonly ConcurrentDictionary<Guid, Models.Game> _ongoingShortGames = new();
    private static readonly SemaphoreSlim _lobbySemaphore = new(1, 1);
    private readonly PlayerCache _playerCache;

    public GameHub(PlayerCache playerCache)
    {
        _playerCache = playerCache;
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

        await _lobbySemaphore.WaitAsync();
        try
        {
            var player = _playerCache.GetPlayer(Context.UserIdentifier!);

            if (player.OngoingShortGame != null)
            {
                // Finish the game, if more than 10 moves were made the abandoning player must lose.
            }
            else if (_playerCache.GetRemainingConnections(Context.UserIdentifier!) == 1) // Last connection's closing.
            {
                _pendingGamesByCreatorId.TryRemove(Context.UserIdentifier!, out _);
                lobbyUpdated = true;
            }
        }
        finally
        {
            _lobbySemaphore.Release();
            _playerCache.OnDisconnect(Context);
            if (lobbyUpdated)
                await Clients.All.LobbyUpdated(_pendingGamesByCreatorId.Values.ToList());
        }

        Debug.WriteLine("Connection closed");
        Debug.WriteLine(exception);
        await base.OnDisconnectedAsync(exception);
    }
}