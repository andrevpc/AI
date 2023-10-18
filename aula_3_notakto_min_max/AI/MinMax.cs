namespace AI;

public class Node
{
    public List<Node> Children { get; set; } = new();
    public int Value { get; set; }
    public float MinMax(Node node, bool maximize, int depth)
    {
        if (depth == 0 || node.Children.Count() == 0)
            return 1f;

        float value;

        if(maximize)
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
}


