using System.Runtime.InteropServices;

namespace AI;

public class Othello
{
    private const ulong u = 1;
    public ulong whiteInfo;
    public ulong blackInfo;
    public byte whiteCount;
    public byte blackCount;
    public byte whitePlays;
    private int lastPlayIndex = 0;
    public static Othello New()
    {
        return new Othello
        {
            whiteInfo = (u << 27) + (u << 36),
            blackInfo = (u << 28) + (u << 35),
            whiteCount = 2,
            blackCount = 2,
            whitePlays = 1
        };
    }
    public int GetLast()
        => lastPlayIndex;

    public ulong GetLastUlong()
    {
        ulong u = 1;
        var play = u << lastPlayIndex;
        return play;
    }
    public void Play(int playIndex)
    {
        ulong u = 1;
        var play = u << playIndex;
        if (whitePlays == 1)
        {
            whiteInfo |= play;
            whiteCount++;
        }
        else
        {
            blackInfo |= play;
            blackCount++;
        }
        checkAdjacent(playIndex, whitePlays, Directions);
        whitePlays = (byte)(1 - whitePlays);
        lastPlayIndex = playIndex;
    }

    public bool Directions(int playIndex, int i, int j, ulong myColorInfo, ulong enemyColorInfo,
                           bool checkTop, bool checkBottom, bool checkLeft, bool checkRight)
    {
        var lastIndex = 0;
        int index;
        int myCount = 0;
        int enemyCount = 0;
        ulong myTable = myColorInfo;
        ulong enemyTable = enemyColorInfo;
        ulong u = 1;
        ulong play;
        for (int k = 1; k < 8; k++)
        {
            index = playIndex + i * 8 * k + j * k;
            // TODO!!!!!
            // if (index > 64 || index < 0 ||
            //     checkTop && lastIndex % 8 == 0 || checkBottom && lastIndex % 8 == 0 ||
            //     checkLeft && lastIndex < 8 || checkRight && lastIndex > 55)
            //     return false;

            var checkMyColor = ((myColorInfo >> index) & 1) == 1;
            var checkEnemyColor = ((enemyColorInfo >> index) & 1) == 1;
            if (checkMyColor)
            {
                if (whitePlays == 1)
                {
                    whiteInfo = myTable;
                    blackInfo = enemyTable;
                }
                else
                {
                    blackInfo = myTable;
                    whiteInfo = enemyTable;
                }
                return false;
            }
            else if (!checkEnemyColor && !checkMyColor)
                return false;
            else
            {
                myCount++;
                enemyCount--;
                play = u << index;
                myTable |= play;
                enemyTable ^= play;
            }
            lastIndex = index;
        }
        return false;
    }

    public bool CanPlay(byte isWhite, int playIndex)
    {
        if (GameEnded())
            return false;
        if (!checkPositionEmpty(playIndex))
            return false;
        return checkAdjacent(playIndex, isWhite, checkDirections);
    }

    #region CanPlayVerification
    private bool checkPositionEmpty(int playIndex)
    {
        var whitePlay = ((whiteInfo >> playIndex) & 1) == 1;
        var blackPlay = ((blackInfo >> playIndex) & 1) == 1;
        if (whitePlay || blackPlay)
            return false;
        return true;
    }

    private bool checkAdjacent(int playIndex, byte isWhite, Func<int, int, int, ulong, ulong,
                                 bool, bool, bool, bool, bool> direction)
    {
        ulong enemyColorInfo;
        ulong myColorInfo;
        if (isWhite == 1)
        {
            myColorInfo = whiteInfo;
            enemyColorInfo = blackInfo;
        }
        else
        {
            myColorInfo = blackInfo;
            enemyColorInfo = whiteInfo;
        }

        bool checkLeft,
             checkRight,
             checkTop,
             checkBottom,
             checkColor;
        int index;
        for (int i = -1; i < 2; i++)
        {
            checkLeft = i == -1;
            checkRight = i == 1;
            for (int j = -1; j < 2; j++)
            {
                checkTop = j == -1;
                checkBottom = j == 1;
                index = playIndex + i * 8 + j;
                if (index > 64 || index < 0 ||
                    checkTop && playIndex % 8 == 0 || checkBottom && playIndex + 1 % 8 == 0 ||
                    checkLeft && playIndex < 8 || checkRight && playIndex > 55)
                    continue;

                checkColor = ((u << index) & enemyColorInfo) > 0;
                if (checkColor)
                    if (direction(playIndex, i, j, myColorInfo, enemyColorInfo, checkTop, checkBottom, checkLeft, checkRight))
                        return true;
            }
        }
        return false;
    }

    private bool checkDirections(int playIndex, int i, int j, ulong myColorInfo, ulong enemyColorInfo,
                                 bool checkTop, bool checkBottom, bool checkLeft, bool checkRight)
    {
        var lastIndex = playIndex + i * 8 + j;
        int index;
        for (int k = 2; k < 8; k++)
        {
            index = playIndex + i * 8 * k + j * k;
            if (index > 64 || index < 0 ||
                checkTop && lastIndex % 8 == 0 || checkBottom && lastIndex % 8 == 0 ||
                checkLeft && lastIndex < 8 || checkRight && lastIndex > 55)
                return false;

            var checkMyColor = ((myColorInfo >> index) & 1) == 1;
            var checkEnemyColor = ((enemyColorInfo >> index) & 1) == 1;
            if (checkMyColor)
                return true;
            else if (!checkEnemyColor && !checkMyColor)
                return false;

            lastIndex = index;
        }
        return false;
    }

    public bool GameEnded() // TODO: PASS
        => ulong.MaxValue == (whiteInfo | blackInfo);
    #endregion
    public IEnumerable<Othello> Next()
    {
        List<Othello> children = new();

        for (int i = 0; i < 64; i++)
        {
            if (CanPlay(whitePlays, i))
            {
                var clone = Clone();
                clone.Play(i);

                children.Add(clone);
                clone.Print();
                System.Console.WriteLine("\n\n");
            }
        }
        return children;
    }

    public Othello Clone()
        => new()
        {
            whitePlays = whitePlays,
            whiteInfo = whiteInfo,
            blackInfo = blackInfo,
            whiteCount = whiteCount,
            blackCount = blackCount,
            lastPlayIndex = lastPlayIndex
        };

    public void Print()
    {
        for (int i = 0; i < 64; i++)
        {
            if (i % 8 == 0)
                Console.WriteLine();

            bool isWhite = ((whiteInfo >> i) & 1) > 0;
            bool isBlack = ((blackInfo >> i) & 1) > 0;

            if (isWhite)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" 0 ");
            }
            else if (isBlack)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(" 0 ");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("   ");
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(" | ");
        }
    }
    public override string ToString()
        => $"{whitePlays} {whiteInfo} {whiteCount} {blackInfo} {blackCount}";
}