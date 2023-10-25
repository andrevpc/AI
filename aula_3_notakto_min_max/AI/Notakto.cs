namespace AI;

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
        List<Notakto> children = new();

        for (int i = 0; i < Boards.Count; i++)
        {
            if (Boards[i].Finished)
                continue;

            for(int j = 0; j < Boards[i].Positions.Length; j++)
            {
                if (Boards[i].Positions[j]) continue;

                var clone = Clone();
                clone.Play(i, j);

                children.Add(clone);
            }
        }
        return children;
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