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
        // Seu código aqui...
    }
    
    /// <summary>
    /// Testa e retorna verdadeiro se você pode jogar em um tabuleiro
    /// </summary>
    public bool CanPlay(int board)
    {
        // Seu código aqui...
    }
    
    /// <summary>
    /// Retorna verdadeiro se o jogo acabou
    /// </summary>
    public bool GameEnded()
    {
        // Seu código aqui...
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
        // Seu código aqui...
    }
}