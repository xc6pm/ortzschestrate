using Nethereum.RPC.Eth.DTOs;
using Ortzschestrate.Web3.Contracts.ORTBet.ContractDefinition;
using Ortzschestrate.Web3.Utilities;

namespace Ortzschestrate.Web3.Actions;

public class StartGame
{
    public async Task<bool> DoAsync(Guid gameId, string player1Address, string player2Address, double stakeAmount)
    {
        var stakeInWei = Nethereum.Web3.Web3.Convert.ToWei(stakeAmount);

        var web3 = Web3Factory.GetDefault();
        var startGameFunction = new StartGameFunction()
        {
            GameId = gameId.ToString(),
            Player1 = player1Address,
            Player2 = player2Address,
            StakeAmount = stakeInWei
        };

        var handler = web3.Eth.GetContractTransactionHandler<StartGameFunction>();
        var contractAddress = await Deployment.ReadContractAddressAsync();

        var receipt = await handler.SendRequestAndWaitForReceiptAsync(contractAddress, startGameFunction);

        return receipt.Succeeded();
    }
}