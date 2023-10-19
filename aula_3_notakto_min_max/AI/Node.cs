namespace AI;

public class Node
{
    public Notakto State { get; set; }
    public List<Node> Children { get; set; } = new();
    public int Evaluation { get; set; } = 0;
    public bool YouPlays { get; set; } = true;
    public bool Expanded { get; set; } = false;
    public float MinMax(Node node, bool maximize, int depth)
    {
        if (depth == 0 || node.Children.Count == 0)
            return 0f;

        float value;

        if (maximize)
        {
            value = float.NegativeInfinity;
            foreach (var child in node.Children)
            {
                value = Math.Max(value, MinMax(child, false, depth - 1));
            }
            return value;
        }
        else
        {
            value = float.PositiveInfinity;
            foreach (var child in node.Children)
            {
                value = Math.Min(value, MinMax(child, true, depth - 1));
            }
            return value;
        }
    }

    public void Expand(Node node, int depth)
    {
        if (depth == 0 || node.Children.Count == 0)
            return;

        foreach (var child in node.Children)
        {
            if (child.Expanded)
                continue;

            eval();
            node.Expanded = true;
            Expand(child, depth - 1);
        }
    }
    public Node Play(int board, int position)
        => Children.First(child => child.State.GetLast() == (board, position));
        
    public Node PlayBest()
        => Children.MaxBy(child => child.Evaluation);

    private float eval()
    {

    }
}


