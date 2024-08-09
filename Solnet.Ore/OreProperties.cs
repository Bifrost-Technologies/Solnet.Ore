using Solnet.Wallet;


namespace Solnet.Ore
{

    public static class OreProperties
    {  
        // Program id for const pda derivations
        public static readonly PublicKey PROGRAM_ID = new PublicKey("oreV2ZymfyeXgNgBdqMkumTqqAprVqgBWQfoYkrtKWQ");

        public static readonly PublicKey tokenProgram_ID = new PublicKey("TokenkegQfeZyiNwAJbNbGKPFXCWuBvf9Ss623VQ5DA");

        public static readonly PublicKey associatedTokenProgram_ID = new PublicKey("ATokenGPvbdGVxr1b2hvZbsiqW5xWH25efTNsLJA8knL");

        public static readonly PublicKey systemProgram_ID = new PublicKey("11111111111111111111111111111111");

        public static readonly PublicKey SystemInstructions_ID = new PublicKey("Sysvar1nstructions1111111111111111111111111");

        // The address of the v1 mint account.
        public static readonly PublicKey MINT_V1_ADDRESS = new PublicKey("oreoN2tQbHXVaZsr3pf66A48miqcBXCDJozganhEJgz");

        // The address of the CU-optimized Solana noop program.
        public static readonly PublicKey NOOP_PROGRAM_ID = new PublicKey("noop8ytexvkpCuqbf6FB89BSuNemHtPRqaNC31GWivW");
        // The authority allowed to initialize the program.
        public static readonly PublicKey INITIALIZER_ADDRESS = new PublicKey("HBUh9g46wk2X89CvaNN15UmsznP59rh6od1h8JwYAopk");

        // The name for token metadata.
        public const string METADATA_NAME = "ORE";

        // The ticker symbol for token metadata.
        public const string METADATA_SYMBOL = "ORE";

        // The uri for token metadata.
        public const string METADATA_URI = "https://ore.supply/metadata-v2.json";

        // The base reward rate to initialize the program with.
        public const ulong INITIAL_BASE_REWARD_RATE = BASE_REWARD_RATE_MIN_THRESHOLD;

        // The minimum allowed base reward rate, at which point the min difficulty should be increased
        public const ulong BASE_REWARD_RATE_MIN_THRESHOLD = 1UL << 5;

        // The maximum allowed base reward rate, at which point the min difficulty should be decreased.
        public const ulong BASE_REWARD_RATE_MAX_THRESHOLD = 1UL << 8;

        // The spam/liveness tolerance in seconds.
        public const long TOLERANCE = 5;

        // The minimum difficulty to initialize the program with.
        public const uint INITIAL_MIN_DIFFICULTY = 1;

        // The decimal precision of the ORE token.
        // There are 100 billion indivisible units per ORE (called "grains").
        public const byte TOKEN_DECIMALS = 11;

        // The decimal precision of the ORE v1 token.
        public const byte TOKEN_DECIMALS_V1 = 9;

        // One ORE token, denominated in indivisible units.
        public static readonly ulong ONE_ORE = (ulong)Math.Pow(10, TOKEN_DECIMALS);

        // The duration of one minute, in seconds.
        public const long ONE_MINUTE = 60;

        // The number of minutes in a program epoch.
        public const long EPOCH_MINUTES = 1;

        // The duration of a program epoch, in seconds.
        public static readonly long EPOCH_DURATION = ONE_MINUTE * EPOCH_MINUTES;

        // The maximum token supply (21 million).
        public static readonly ulong MAX_SUPPLY = ONE_ORE * 21_000_000;

        // The target quantity of ORE to be mined per epoch.
        public static readonly ulong TARGET_EPOCH_REWARDS = ONE_ORE * (ulong)EPOCH_MINUTES;

        // The maximum quantity of ORE that can be mined per epoch.
        // Inflation rate ≈ 1 ORE / min (min 0, max 8)
        public static readonly ulong MAX_EPOCH_REWARDS = TARGET_EPOCH_REWARDS * (ulong)BUS_COUNT;

        // The quantity of ORE each bus is allowed to issue per epoch.
        public static readonly ulong BUS_EPOCH_REWARDS = MAX_EPOCH_REWARDS / (ulong)BUS_COUNT;

        // The number of bus accounts, for parallelizing mine operations.
        public const int BUS_COUNT = 8;

        // The smoothing factor for reward rate changes. The reward rate cannot change by more or less
        // than a factor of this constant from one epoch to the next.
        public const ulong SMOOTHING_FACTOR = 2;

        // The seed of the bus account PDA.
        public static readonly byte[] BUS_SEED = System.Text.Encoding.UTF8.GetBytes("bus");

        // The seed of the config account PDA.
        public static readonly byte[] CONFIG_SEED = System.Text.Encoding.UTF8.GetBytes("config");

        // The seed of the metadata account PDA.
        public static readonly byte[] METADATA_SEED = System.Text.Encoding.UTF8.GetBytes("metadata");

        // The seed of the mint account PDA.
        public static readonly byte[] MINT_SEED = System.Text.Encoding.UTF8.GetBytes("mint");

        // The seed of proof account PDAs.
        public static readonly byte[] PROOF_SEED = System.Text.Encoding.UTF8.GetBytes("proof");

        // The seed of the treasury account PDA.
        public static readonly byte[] TREASURY_SEED = System.Text.Encoding.UTF8.GetBytes("treasury");

        // Noise for deriving the mint pda
        public static readonly byte[] MINT_NOISE_SEED = { 89, 157, 88, 232, 243, 249, 197, 132, 199, 49, 19, 234, 91, 94, 150, 41 };

        // The addresses of the bus accounts.
        public static readonly PublicKey[] BUS_ADDRESSES = [new PublicKey("C8cEcoWJh6yMEZCm5Pga2PEMVpFq96YM5EBnb7eSVFAi"), new PublicKey("4Sykpy3XtAyTQUddVeSeXwx2pWQqz8jUJS582mVY5hFd"), new PublicKey("M9MspudY8SgTXBq78XTJBoNKF2Z9pHcuY2Jti7YzbFd"), new PublicKey("7PaX43nkYDySeHZndWHNNznqMrPZ7Rkmmo735iHJFyY1"), new PublicKey("AbCULZaMMB1TqzFkLMG2Tika6P1B52Q1mkjHnSWXiksC"), new PublicKey("AbaPgzLdr1SppgPBX1WwrrJamhtJVAyMv6GZQ3mQEw6i"), new PublicKey("2oLNTQKRb4a2117kFi6BYTUDu3RPrMVAHFhCfPKMosxX"), new PublicKey("5HngGmYzvSuh3XyU11brHDpMTHXQQRQQT4udGFtQSjgR")];

        







    }

}
