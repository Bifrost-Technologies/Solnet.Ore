﻿using Solnet.Ore.Models;
using Solnet.Ore;
using Solnet.Programs;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using Solnet.Programs.Models;
using Solnet.Rpc.Types;
using Solnet.Ore.Accounts;
using static System.Net.Mime.MediaTypeNames;

namespace Solnet.Ore
{
    public class OreClient
    {
        IRpcClient rpcClient {  get; set; }
        public OreClient(string rpc_provider) 
        {
            rpcClient = ClientFactory.GetClient(rpc_provider);
        }

        public async Task<long> GetClock()
        {
            var slot = await rpcClient.GetSlotAsync();
            var network_clock = await rpcClient.GetBlockTimeAsync(slot.Result);
            return (long)network_clock.Result;
        }

        public async Task<long> GetCutoff(Proof proof, ulong bufferTime)
        {
            long clock = await GetClock();
            long cutoff = proof.LastHashAt + (long)60 - (long)bufferTime - clock;

            return cutoff;
        }

        public async Task<AccountResultWrapper<Proof>> GetProofAccountAsync(string proofAddress, Commitment commitment = Commitment.Finalized)
        {
            var res = await rpcClient.GetAccountInfoAsync(proofAddress, commitment);
            if (!res.WasSuccessful)
                return new AccountResultWrapper<Proof>(res);
            var resultingAccount = Proof.Deserialize(Convert.FromBase64String(res.Result.Value.Data[0]));
            return new AccountResultWrapper<Proof>(res, resultingAccount);
        }
        public async Task<Bus> GetTopBusAccountAsync(Commitment commitment = Commitment.Finalized)
        {
            List<string> addresses = new List<string>();
            OreProperties.BUS_ADDRESSES.ToList().ForEach(e => addresses.Add(e.Key));
            var res = await rpcClient.GetMultipleAccountsAsync(addresses, commitment);
            ulong top_balance = 0;
            ulong top_bus_id = 0;
            Bus? top_bus = new Bus();
            foreach(var data in res.Result.Value)
            {

                var resultingAccount = Bus.Deserialize(Convert.FromBase64String(data.Data[0]));
                if(resultingAccount.Rewards > top_balance)
                {
                   
                    top_balance = resultingAccount.Rewards;
                    top_bus_id = resultingAccount.Id;
                    top_bus = resultingAccount;
                }
       
            }
            
            return top_bus;
        }

        public async Task<RequestResult<string>> MineOre(Account miner, Solution solution,int bus_index = 0, ulong computelimit = 500000, ulong priorityfee = 600000)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(computelimit);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(priorityfee);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            Random rng = new Random();
            Bus top_bus = await GetTopBusAccountAsync();
            if (top_bus != null && top_bus.Id != null) 
            {
                bus_index = (int)top_bus.Id;
            }
            else
            {
                bus_index = rng.Next(0, 7);
            }
            
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var mine = OreProgram.Mine(miner, miner, OreProperties.BUS_ADDRESSES[bus_index], solution);
            tb.AddInstruction(auth);
            tb.AddInstruction(mine);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> ClaimOre(Account miner, PublicKey minerTokenAccount, ulong amount, ulong computelimit = 500000, ulong priorityfee = 600000)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(computelimit);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(priorityfee);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var auth = OreProgram.Auth(proof.address);
            var claim = OreProgram.Claim(miner, minerTokenAccount, amount);
            tb.AddInstruction(auth);
            tb.AddInstruction(claim);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> StakeOre(Account miner, ulong amount, ulong computelimit = 500000, ulong priorityfee = 600000)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(computelimit);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(priorityfee);
            tb.AddInstruction(CUlimit);
            tb.AddInstruction(priorityFee);
            var proof = PDALookup.FindProofPDA(miner);
            var minerTokenAccount = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(miner, PDALookup.FindMintPDA());
            var auth = OreProgram.Auth(proof.address);
            var stake = OreProgram.Stake(miner, minerTokenAccount, amount);
            tb.AddInstruction(auth);
            tb.AddInstruction(stake);
            tb.SetRecentBlockHash((await rpcClient.GetLatestBlockHashAsync()).Result.Value.Blockhash);
            tb.SetFeePayer(miner);
            var signedtx = tb.Build(new Account[] { miner });
            return await rpcClient.SendTransactionAsync(signedtx, false);
        }
        public async Task<RequestResult<string>> OpenProof(Account signer, PublicKey miner, PublicKey payer, ulong computelimit = 500000, ulong priorityfee = 600000)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(computelimit);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(priorityfee);
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
        public async Task<RequestResult<string>> CloseProof(Account miner, ulong computelimit = 500000, ulong priorityfee = 600000)
        {
            TransactionBuilder tb = new TransactionBuilder();
            TransactionInstruction CUlimit = OreProgram.SetCUlimit(computelimit);
            TransactionInstruction priorityFee = ComputeBudgetProgram.SetComputeUnitPrice(priorityfee);
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
