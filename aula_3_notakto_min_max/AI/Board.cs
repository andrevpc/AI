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
            Positions = (bool[])Positions.Clone(),
            Finished = Finished
        };
        return clone;
    }
}