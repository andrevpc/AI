public class Notakto
{
    int boards = 0;
    bool[] data;
    byte[] sums;
    int lastBoard = 0;
    int lastPosition = 0;

    public Notakto(int boards)
    {
        this.boards = boards;
        data = new bool[9 * boards];
        sums = new byte[8 * boards];
    }
    
    /// <summary>
    /// Obtém a última jogada
    /// </summary>
    public (int board, int position) GetLast()
        => (lastBoard, lastPosition);

    /// <summary>
    /// Joga em uma posição em um tabuleiro
    /// </summary>
    public void Play(int board, int posit)
    {
        lastBoard = board;
        lastPosition = posit;

        var tableIndex = 9 * board;
        var sumsIndex = 8 * board;
        data[tableIndex + posit] = true;
        sums[sumsIndex + (posit / 3)]++;
        sums[sumsIndex + (posit % 3) + 3]++;
        if (posit % 4 == 0)
            sums[sumsIndex + 6]++;
        if (posit % 2 == 0 && posit > 0 && posit < 8)
            sums[sumsIndex + 7]++;
    }
    
    /// <summary>
    /// Testa e retorna verdadeiro se você pode jogar em um tabuleiro
    /// </summary>
    public bool CanPlay(int board)
    {
        int boardIndex = 8 * board;
        for (int i = 0; i < 8; i++)
        {
            if (sums[boardIndex + i] == 3)
                return false;
        }
        return true;
    }
    
    /// <summary>
    /// Retorna verdadeiro se o jogo acabou
    /// </summary>
    public bool GameEnded()
    {
        for (int i = 0; i < boards; i++)
        {
            if (CanPlay(i))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Cria uma cópia indentica do estado.
    /// </summary>
    public Notakto Clone()
    {
        Notakto copy = new Notakto(boards);
        Array.Copy(
            this.data, 
            copy.data, 
            this.data.Length
        );
        Array.Copy(
            this.sums, 
            copy.sums, 
            this.sums.Length
        );
        return copy;
    }

    /// <summary>
    /// Obtém próximas jogadas válidas
    /// </summary>
    public IEnumerable<Notakto> Next()
    {
        var clone = this.Clone();
        for (int b = 0; b < boards; b++)
        {
            if (!CanPlay(b))
                continue;
            
            for (int p = 0; p < 9; p++)
            {
                if (data[b * 9 + p])
                    continue;
                
                clone.Play(b, p);
                yield return clone;
                clone = this.Clone();
            }
        }
    }
}