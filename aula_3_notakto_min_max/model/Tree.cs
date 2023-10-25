public class Node
{
    public Notakto State { get; set; }
    public float Evaluation { get; set; } = 0;
    public List<Node> Children { get; set; } = new();
    public bool Expanded { get; set; } = false;
    public bool YouPlays { get; set; } = true;

    public Node Play(int board, int position)
    {
        // Seu código aqui...
    }

    public void Expand(int deep)
    {
        // Seu código aqui...
    }

    public Node PlayBest()
    {
        // Seu código aqui...
    }

    public float MiniMax()
    {
        // Seu código aqui...
    }

    private float eval()
    {
        // Seu código aqui...
    }
}