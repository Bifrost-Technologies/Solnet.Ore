using Solnet.Wallet;
using Solnet.Programs;
using Solnet.Rpc.Models;
using Solnet.Programs.Utilities;
using Solnet.Ore.Types;
using Solnet.Ore.Models;
namespace Solnet.Ore
{
    public static class OreProgram
    {
        public static TransactionInstruction Auth(PublicKey proof)
        {
            return new TransactionInstruction
            {
                ProgramId = OreProperties.NOOP_PROGRAM_ID,
                Keys = new List<AccountMeta>(),
                Data = proof.KeyBytes
            };
        }
        public static TransactionInstruction SetCUlimit(ulong units)
        {
            List<AccountMeta> keys = new List<AccountMeta>();
            byte[] data = new byte[9];
            data.WriteU8(2, 0);
            data.WriteU64(units, 1);
            return new TransactionInstruction
            {
                ProgramId = ComputeBudgetProgram.ProgramIdKey,
                Keys = keys,
                Data = data
            };
        }
        public static TransactionInstruction Claim(PublicKey signer, PublicKey beneficiary, ulong amount)
        {
            var proof = PDALookup.FindProofPDA(signer);
            PublicKey treasury = PDALookup.FindTreasuryPDA(signer);
            var treasuryTokens = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(treasury, PDALookup.FindMintPDA());

            var data = new List<byte>();
            data.Add((byte)OreInstruction.Claim);
            data.AddRange(BitConverter.GetBytes(amount));

            return new TransactionInstruction
            {
                ProgramId = OreProperties.PROGRAM_ID,
                Keys = new List<AccountMeta>
                {
                    AccountMeta.Writable(signer, true),
                    AccountMeta.Writable(beneficiary, false),
                    AccountMeta.Writable(proof.address, false),
                    AccountMeta.ReadOnly(treasury, false),
                    AccountMeta.Writable(treasuryTokens, false),
                    AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
                },
                Data = data.ToArray()
            };
        }

        public static TransactionInstruction Close(PublicKey signer)
        {
            var proof = PDALookup.FindProofPDA(signer);
            var _data = new List<byte>();
            _data.Add((byte)OreInstruction.Close);
            return new TransactionInstruction
            {
                ProgramId = OreProperties.PROGRAM_ID,
                Keys = new List<AccountMeta>
                {
                    AccountMeta.Writable(signer, true),
                    AccountMeta.Writable(proof.address, false),
                    AccountMeta.ReadOnly(SystemProgram.ProgramIdKey, false)
                },

                Data = _data.ToArray()
            };
        }

        public static TransactionInstruction Mine(PublicKey signer, PublicKey proofAuthority, PublicKey bus, Solution solution)
        {
            var proof = PDALookup.FindProofPDA(signer);
            var config = PDALookup.FindConfigPDA();
            var data = new List<byte>();
            data.Add((byte)OreInstruction.Mine);
            data.AddRange(solution.Digest);
            data.AddRange(solution.Nonce);

            return new TransactionInstruction
            {
                ProgramId = OreProperties.PROGRAM_ID,
                Keys = new List<AccountMeta>
                {
                    AccountMeta.Writable(signer, true),
                    AccountMeta.Writable(bus, false),
                    AccountMeta.ReadOnly(config, false),
                    AccountMeta.Writable(proof.address, false),
                    AccountMeta.ReadOnly(OreProperties.SystemInstructions_ID, false),
                    AccountMeta.ReadOnly(new PublicKey("SysvarS1otHashes111111111111111111111111111"), false)
                },
                Data = data.ToArray()
            };
        }

        public static TransactionInstruction Open(PublicKey signer, PublicKey miner, PublicKey payer)
        {
            var proof = PDALookup.FindProofPDA(signer);

            var data = new List<byte>();
            data.Add((byte)OreInstruction.Open);
            data.Add(proof.bump);

            return new TransactionInstruction
            {
                ProgramId = OreProperties.PROGRAM_ID,
                Keys = new List<AccountMeta>
                {
                    AccountMeta.Writable(signer, true),
                    AccountMeta.ReadOnly(miner, false),
                    AccountMeta.Writable(payer, true),
                    AccountMeta.Writable(proof.address, false),
                    AccountMeta.ReadOnly(SystemProgram.ProgramIdKey, false),
                    AccountMeta.ReadOnly(new PublicKey("SysvarS1otHashes111111111111111111111111111"), false)
                },
                Data = data.ToArray()
            };
        }
    }
}