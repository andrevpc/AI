namespace AI;

public class Othello
{
    private ulong whiteInfo;
    private ulong blackInfo;
    private byte whiteCount;
    private byte blackCount;
    private byte whitePlays;
    private int lastPlayIndex = 0;

    public int GetLast()
        => lastPlayIndex;

    public void Play(int playIndex) // TODO
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
        var lastIndex = playIndex + i * 8 + j;
        int index;
        int myCount = 0;
        int enemyCount = 0;
        ulong myTable;
        ulong enemyTable;
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
            else
            {
                myCount++;
                enemyCount--;

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

                checkColor = ((enemyColorInfo >> index) & 1) == 1;
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
}