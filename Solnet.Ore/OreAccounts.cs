using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solnet.Wallet;
using Solnet.Programs.Utilities;

namespace Solnet.Ore.Accounts
{
        public class Proof
        {
            public PublicKey Authority { get; set; }
            public ulong Balance { get; set; }
            public byte[] Challenge { get; set; } = new byte[32];
            public byte[] LastHash { get; set; } = new byte[32];
            public long LastHashAt { get; set; }
            public long LastStakeAt { get; set; }
            public PublicKey Miner { get; set; }
            public ulong TotalHashes { get; set; }
            public ulong TotalRewards { get; set; }


            public byte[] Serialize()
            {
                var buffer = new byte[176]; 
                int offset = 8;

                // Serialize fields to byte array
                Array.Copy(Authority.KeyBytes, 0, buffer, offset, 32);
                offset += 32;
                BitConverter.GetBytes(Balance).CopyTo(buffer, offset);
                offset += 8;
                Array.Copy(Challenge, 0, buffer, offset, 32);
                offset += 32;
                Array.Copy(LastHash, 0, buffer, offset, 32);
                offset += 32;
                BitConverter.GetBytes(LastHashAt).CopyTo(buffer, offset);
                offset += 8;
                BitConverter.GetBytes(LastStakeAt).CopyTo(buffer, offset);
                offset += 8;
                Array.Copy(Miner.KeyBytes, 0, buffer, offset, 32);
                offset += 32;
                BitConverter.GetBytes(TotalHashes).CopyTo(buffer, offset);
                offset += 8;
                BitConverter.GetBytes(TotalRewards).CopyTo(buffer, offset);

                return buffer;
            }

            public static Proof Deserialize(byte[] data)
            {
                var proof = new Proof();
                int offset = 8;

                proof.Authority = new PublicKey(data.AsSpan(offset, 32).ToArray());
                offset += 32;
                proof.Balance = BitConverter.ToUInt64(data, offset);
                offset += 8;
                proof.Challenge = data.AsSpan(offset, 32).ToArray();
                offset += 32;
                proof.LastHash = data.AsSpan(offset, 32).ToArray();
                offset += 32;
                proof.LastHashAt = BitConverter.ToInt64(data, offset);
                offset += 8;
                proof.LastStakeAt = BitConverter.ToInt64(data, offset);
                offset += 8;
                proof.Miner = new PublicKey(data.AsSpan(offset, 32).ToArray());
                offset += 32;
                proof.TotalHashes = BitConverter.ToUInt64(data, offset);
                offset += 8;
                proof.TotalRewards = BitConverter.ToUInt64(data, offset);

                return proof;
            }
        }

    }


