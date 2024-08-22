# Solnet.Ore
 Solnet.Ore is a C# sdk and client for the Ore V2 program on Solana

## How to use client example
```
using Solnet.Ore;
using Solnet.Ore.Accounts;
using Solnet.Ore.Models;
using Solnet.Programs.Utilities;
using Solnet.Wallet;

//Miner is the sender, miner, and payer in this instance
Account miner = Account.FromSecretKey("SECRET_KEY_HERE");
string rpc_provider = "RPC_URL_HERE";
OreClient oreClient = new OreClient(rpc_provider);
var tokenaccount = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(miner.PublicKey, PDALookup.FindMintPDA());
var proofRequest = await oreClient.GetProofAccountAsync(PDALookup.FindProofPDA(miner.PublicKey).address);
if (proofRequest != null)
{
    Proof proof = proofRequest.ParsedResult;
    long cut_off = await oreClient.GetCutoff(proof, 5);
    string CurrentChallenge = new PublicKey(proof.Challenge).Key;
    Console.WriteLine("Pool Balance: " + proof.Balance);
    Console.WriteLine("Current Challenge: " + CurrentChallenge);
    Console.WriteLine("Cut Off: " + Convert.ToDateTime(cut_off).ToShortTimeString());

await oreClient.OpenProof(miner, miner, miner);

//Supply a real digest and nonce from drillx via API calls to your API controller to fill this in. Worker <-> API
Solution solution = new Solution
{
    Digest = new byte[16],
    Nonce = new byte[8],
};
await oreClient.MineOre(miner, solution);
await oreClient.ClaimOre(miner, tokenaccount, proof.Balance);
await oreClient.StakeOre(miner, proof.balance);

Console.ReadKey();
}
```
