public struct OthelloGame
{
    private const ulong u = 1;

    public bool WhitePlays => whitePlays == 1;
    public byte WhitePoints => whiteCount;
    public byte BlackPoints => blackCount;

    public void Pass()
        => whitePlays = (byte)(1 - whitePlays);

    private ulong whiteInfo;
    private ulong blackInfo;
    private byte whiteCount;
    private byte blackCount;
    private byte whitePlays;

    public static OthelloGame New()
    {
        return new OthelloGame
        {
            whiteInfo = (u << 27) + (u << 36),
            blackInfo = (u << 28) + (u << 35),
            whiteCount = 2,
            blackCount = 2,
            whitePlays = 1
        };
    }

    public static OthelloGame New(
        byte whitePlays,
        ulong white, ulong black,
        byte wCount, byte bCount
    )
    {
        return new OthelloGame
        {
            whiteInfo = white,
            blackInfo = black,
            whiteCount = wCount,
            blackCount = bCount,
            whitePlays = whitePlays
        };
    }

    public int this[int i, int j]
    {
        get
        {
            int index = i + j * 8;
            return ((whiteInfo & (u << index)) > 0) ? 1 : 
                   ((blackInfo & (u << index)) > 0) ? 2 : 0;
        }
    }

    public override string ToString()
        => $"{whitePlays} {whiteInfo} {whiteCount} {blackInfo} {blackCount}";
}