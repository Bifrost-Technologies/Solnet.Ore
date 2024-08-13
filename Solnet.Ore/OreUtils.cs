using Solnet.Wallet;

namespace Solnet.Ore
{
    public class ProgramDerivedAddress
    {
        public PublicKey address { get; set; }

        public byte bump { get; set; }

    }
    /// <summary>
    /// PDA Lookup Class to make finding PDAs simple
    /// </summary>
    public static class PDALookup
    {
        public static PublicKey FindBusPDA(PublicKey signer)
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.BUS_SEED,
                signer
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }

        public static ProgramDerivedAddress FindProofPDA(PublicKey signer)
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.PROOF_SEED,
                signer
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out byte _bump);

            return new ProgramDerivedAddress { address = PDA, bump = _bump };
        }
        public static PublicKey FindTreasuryPDA(PublicKey signer)
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.TREASURY_SEED
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }

        public static PublicKey FindConfigPDA()
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.CONFIG_SEED,
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }
        public static PublicKey FindTreasuryTokensPDA(PublicKey treasuryPDA, PublicKey mintPDA)
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                treasuryPDA,
                OreProperties.tokenProgram_ID,
                mintPDA
                
            },
            OreProperties.associatedTokenProgram_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }
        public static PublicKey FindMintPDA()
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.MINT_SEED,
                OreProperties.MINT_NOISE_SEED
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }
        public static PublicKey FindMetadataPDA(PublicKey signer)
        {
            PublicKey.TryFindProgramAddress(new List<byte[]>()
            {
                OreProperties.PROOF_SEED,
                signer
            },
            OreProperties.PROGRAM_ID,
            out PublicKey PDA,
            out _);

            return PDA;
        }
   
    }
}
