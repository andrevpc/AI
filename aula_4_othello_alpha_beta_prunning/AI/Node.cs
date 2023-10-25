namespace AI;

public class Node
{
    public Othello State { get; set; }
    public List<Node> Children { get; set; } = new();
    public float Evaluation { get; set; } = 0;
    public bool YouPlays { get; set; } = true;
    public bool Expanded { get; set; } = false;
    public float AlphaBeta()
        => AlphaBeta(float.NegativeInfinity, float.PositiveInfinity);

    public float AlphaBeta(float alpha, float beta)
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
                value = Math.Max(value, child.AlphaBeta(alpha, beta));
                if (value > beta)
                    break;
                alpha = Math.Max(alpha, value);
            }
            Evaluation = value;
            return value;
        }
        else
        {
            value = float.PositiveInfinity;
            foreach (var child in Children)
            {
                value = Math.Min(value, child.AlphaBeta(alpha, beta));
                if (value < alpha)
                    break;
                beta = Math.Max(alpha, value);
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

        var possiblesOthello = State.Next();

        foreach (var possibleOthello in possiblesOthello)
        {
            Node newNode = new()
            {
                YouPlays = !YouPlays,
                State = possibleOthello
            };

            newNode.Expand(depth - 1);
            
            Children.Add(newNode);
        }

        Expanded = true;
    }
    public Node Play(ulong play)
        => Children.First(child => child.State.GetLast() == play);

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