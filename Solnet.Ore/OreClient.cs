using Solnet.Ore.Models;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Wallet;

namespace Solnet.Ore
{
    public class OreClient
    {
        IRpcClient rpcClient {  get; set; }
        public OreClient(string rpc_provider) 
        {
            rpcClient = ClientFactory.GetClient(rpc_provider);
        }
        public async Task<RequestResult<string>> MineOre(Account miner, Solution solution)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(500000);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(600000);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var mine = OreProgram.Mine(miner, null, new PublicKey("AbCULZaMMB1TqzFkLMG2Tika6P1B52Q1mkjHnSWXiksC"), solution);
            tb.AddInstruction(auth);
            tb.AddInstruction(mine);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> ClaimOre(Account miner, PublicKey beneficiary, ulong amount)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(500000);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(600000);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var claim = OreProgram.Claim(miner, beneficiary, amount);
            tb.AddInstruction(auth);
            tb.AddInstruction(claim);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> OpenProof(Account signer, PublicKey miner, PublicKey payer)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(500000);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(600000);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var open = OreProgram.Open(signer, miner, payer);
            tb.AddInstruction(auth);
            tb.AddInstruction(open);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { signer });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> CloseProof(Account miner)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(500000);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(600000);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var close = OreProgram.Close(miner);
            tb.AddInstruction(auth);
            tb.AddInstruction(close);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
    }
}
