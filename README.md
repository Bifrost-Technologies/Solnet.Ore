# Solnet.Ore
 Solnet.Ore is a C# sdk and client for the Ore V2 program on Solana

## How to use client example
```
using Solnet.Ore;
using Solnet.Ore.Models;
using Solnet.Wallet;

//Miner is the sender, miner, and payer in this instance
Account miner = Account.FromSecretKey("SECRET_KEY_HERE");
string rpc_provider = "RPC_URL_HERE";
OreClient oreClient = new OreClient(rpc_provider);

await oreClient.OpenProof(miner, miner, miner);

//Supply a real digest and nonce from drillx via API calls to your API controller to fill this in. Worker <-> Relayer
Solution solution = new Solution
{
    Digest = new byte[16],
    Nonce = new byte[8],
};
await oreClient.MineOre(miner, solution);
Console.ReadKey();
```
