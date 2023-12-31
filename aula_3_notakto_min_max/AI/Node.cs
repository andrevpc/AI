namespace AI;

public class Node
{
    public Notakto State { get; set; }
    public List<Node> Children { get; set; } = new();
    public float Evaluation { get; set; } = 0;
    public bool YouPlays { get; set; } = true;
    public bool Expanded { get; set; } = false;
    public float MinMax()
    {
        if (Children.Count == 0)
        {
            Evaluation = eval();
            return Evaluation;
        }

        float value;

        if (YouPlays)
        {
            value = float.NegativeInfinity;
            foreach (var child in Children)
            {
                value = Math.Max(value, child.MinMax());
            }
            Evaluation = value;
            return value;
        }
        else
        {
            value = float.PositiveInfinity;
            foreach (var child in Children)
            {
                value = Math.Min(value, child.MinMax());
            }
            Evaluation = value;
            return value;
        }
    }

    public void Expand(int depth)
    {
        if (depth == 0)
            return;

        if (Expanded)
            return;

        var possiblesNotakto = State.Next();

        foreach (var possibleNotakto in possiblesNotakto)
        {
            Node newNode = new()
            {
                YouPlays = !YouPlays,
                State = possibleNotakto
            };

            newNode.Expand(depth - 1);

            Children.Add(newNode);
        }
        Expanded = true;
    }
    public Node Play(int board, int position)
        => Children.First(child => child.State.GetLast() == (board, position));

    public Node PlayBest()
        => Children.MaxBy(child => child.Evaluation);

    private float eval()
    {
        if (State.GameEnded() && YouPlays)
            return float.PositiveInfinity;
        else if (State.GameEnded() && !YouPlays)
            return float.NegativeInfinity;

        return 0;
    }
}


