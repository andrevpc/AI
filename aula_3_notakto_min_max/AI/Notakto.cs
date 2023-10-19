namespace AI;

public class Board
{
    public int ITop = 0;
    public int IMiddle = 0;
    public int IBottom = 0;
    public int JLeft = 0;
    public int JMiddle = 0;
    public int JRight = 0;
    public int MainDiagonal = 0;
    public int Diagonal = 0;
    public bool[] Positions = new bool[9];
    public bool Finished = false;

    public void Play(int pos)
    {
        Positions[pos] = true;
        SumPos(pos);
    }

    public void SumPos(int pos)
    {
        if(pos / 3 == 0) ITop++;
        if(pos / 3 == 1) IMiddle++;
        if(pos / 3 == 2) IBottom++;

        if(pos % 3 == 0) JLeft++;
        if(pos % 3 == 1) JMiddle++;
        if(pos % 3 == 2) JRight++;

        if(pos % 4 == 0) MainDiagonal++;
        if(pos % 2 == 0 && pos != 8) Diagonal++;

        Verify();
    }

    public void Verify()
    {
        if (ITop == 3 || IMiddle == 3 || IBottom == 3 ||
            JLeft == 3 || JMiddle == 3 || JRight == 3 ||
            MainDiagonal == 3 || Diagonal == 3) Finished = true;
    }

    public Board Clone()
    {
        Board clone = new()
        {
            ITop = ITop,
            IMiddle = IMiddle,
            IBottom = IBottom,
            JLeft = JLeft,
            JMiddle = JMiddle,
            JRight = JRight,
            MainDiagonal = MainDiagonal,
            Diagonal = Diagonal,
            Positions = Positions,
            Finished = Finished
        };
        return clone;
    }
}

public class Notakto
{
    public int BoardCount { get; private set; }
    public List<Board> Boards { get; set; } = new();
    int lastBoard = 0;
    int lastPosition = 0;

    public Notakto(int boardCount)
    {
        BoardCount = boardCount;
        for (int i = 0; i < boardCount; i++)
            Boards.Add(new Board());
    }
    public (int board, int position) GetLast()
        => (lastBoard, lastPosition);

    public void Play(int board, int pos)
    {
        Boards[board].Play(pos);
        lastBoard = board;
        lastPosition = pos;
    }

    public bool CanPlay(int boardNum)
    {
        Board board = Boards[boardNum];
        board.Verify();
        return board.Finished;
    }

    public bool GameEnded()
    {
        for (int i = 0; i < Boards.Count; i++)
        {
            if (CanPlay(i))
                return false;
        }
        return true;
    }

    public IEnumerable<Notakto> Next()
    {
        foreach (var board in Boards)
        {
            if (board.Finished)
                continue;

            for(int i = 0; i < board.Positions.Length; i++)
            {
                if (board.Positions[i] == true) continue;
            }
        }
    }

    public Notakto Clone()
    {
        List<Board> cloneList = new();

        foreach (var board in Boards)
        {
            var newBoard = board.Clone();
            cloneList.Add(newBoard);
        }

        Notakto notakto = new(BoardCount)
        {
            lastBoard = lastBoard,
            lastPosition = lastPosition,
            Boards = cloneList
        };

        return notakto;
    }
}