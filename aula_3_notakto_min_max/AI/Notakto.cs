namespace AI;

public class Table
{
    public int ITop { get; set; } = 0;
    public int IMiddle { get; set; } = 0;
    public int IBottom { get; set; } = 0;
    public int JLeft { get; set; } = 0;
    public int JMiddle { get; set; } = 0;
    public int JRight { get; set; } = 0;
    public int MainDiagonal { get; set; } = 0;
    public int Diagonal { get; set; } = 0;
    public bool[] Positions { get; set; } = new bool[9];
    public bool Finished { get; set; } = false;

    public void Play(int pos)
    {
        Positions[pos] = true;
        SumPos(pos);
    }

    public void SumPos(int pos)
    {
        if(pos <= 2)
            ITop++;
        else if(pos >= 3 && pos <= 5)
            IMiddle++;
        else if(pos >= 6 && pos <= 8)
            IBottom++;
        if(pos == 0 || pos == 3 || pos == 6)
            JLeft++;
        else if(pos == 1 || pos == 4 || pos == 7)
            JMiddle++;
        else if(pos == 2 || pos == 5 || pos == 8)
            JRight++;
        if(pos == 0 || pos == 4 || pos == 8)
            MainDiagonal++;
        if(pos == 2 || pos == 4 || pos == 6)
            Diagonal++;

        Verify();
    }

    public void Verify()
    {
        if(ITop == 3 || IMiddle == 3 || IBottom == 3 || JLeft == 3 || JMiddle == 3 || JRight == 3 || MainDiagonal == 3 || Diagonal == 3)
            Finished = true;
    }
}

public class Notakto
{
    public int TableCount { get; private set; }
    public List<Table> Tables { get; set; } = new();

    public Notakto(int tableCount)
    {
        this.TableCount = tableCount;
        for (int i = 0; i < tableCount; i++)
            this.Tables.Add(new Table());
    }

    public bool NotaktoPlay(int table, int pos)
    {
        Tables[table].Play(pos);
        if(Tables[table].Finished)
            return true;
        return false;
    }
}