using Solnet.Programs.Utilities;

namespace Solnet.Ore.Types
{

    public enum OreInstruction : byte
    {
        Claim = 0,
        Close = 1,
        Mine = 2,
        Open = 3,
        Reset = 4,
        Stake = 5,
        Update = 6,
        Upgrade = 7,

    }

    public struct MineArgs
    {
        public byte[] Digest;
        public byte[] Nonce;

        public byte[] Encode()
        {
            byte[] result = new byte[Digest.Length + Nonce.Length];
            result.WriteSpan(Digest, 0);
            result.WriteSpan(Nonce, Digest.Length);
            return result;
        }
    }
    public struct OpenArgs
    {
        public byte Bump;
    }
    public struct ClaimArgs
    {
        public byte[] Amount;
    }

    public struct StakeArgs
    {
        public byte[] Amount;
    }

    public struct UpgradeArgs
    {
        public byte[] Amount;

    }
}
