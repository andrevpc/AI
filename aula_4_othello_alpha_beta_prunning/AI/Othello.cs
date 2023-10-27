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

    public bool Directions(int playIndex, int i, int j, ulong myColorInfo, ulong enemyColorInfo)
    {
        int index;
        int myCount = 0; //TODO
        int enemyCount = 0;
        ulong myTable = myColorInfo;
        ulong enemyTable = enemyColorInfo;
        ulong u = 1;
        ulong play;
        for (int k = 1; k < 8; k++)
        {
            index = playIndex + i * 8 * k + j * k;
            if(!checkIndex(playIndex, i * k, j * k))
                break;
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

    private bool checkAdjacent(int playIndex, byte isWhite, Func<int, int, int, ulong, ulong, bool> direction)
    {
        ulong enemyColorInfo;
        ulong myColorInfo;

        bool checkColor;
        int index;
        for (int i = -1; i < 2; i++)
        {
            if (!checkIndexI(playIndex, i))
                break;
            for (int j = -1; j < 2; j++)
            {
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
                if (!checkIndexJ(playIndex, j))
                    break;
                index = playIndex + i * 8 + j;

                checkColor = ((u << index) & enemyColorInfo) > 0;
                if (checkColor)
                    if (direction(playIndex, i, j, myColorInfo, enemyColorInfo))
                        return true;
            }
        }
        return false;
    }

    private bool checkDirections(int playIndex, int i, int j, ulong myColorInfo, ulong enemyColorInfo)
    {
        int index;
        for (int k = 2; k < 8; k++)
        {
            index = playIndex + i * 8 * k + j * k;
            if(!checkIndex(playIndex, i * k, j * k))
                break;

            var checkMyColor = ((myColorInfo >> index) & 1) == 1;
            var checkEnemyColor = ((enemyColorInfo >> index) & 1) == 1;
            if (checkMyColor)
                return true;
            else if (!checkEnemyColor && !checkMyColor)
                return false;
        }
        return false;
    }

    private bool checkIndex(int index, int i, int j)
    {
        if (!checkIndexI(index, i))
            return false;
        if (!checkIndexJ(index, j))
            return false;
        return true;
    }

    private bool checkIndexJ(int index, int j)
    {
        var x = index % 8 + j;
        if (x < 0 || x > 7)
            return false;
        return true;
    }
    private bool checkIndexI(int index, int i)
    {
        var y = index / 8 + i;
        if (y < 0 || y > 7)
            return false;
        return true;
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
                // clone.Print();
                // System.Console.WriteLine("\n\n");
                yield return clone;
            }
        }
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