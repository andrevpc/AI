namespace AI;
using System.IO;
using System.Linq;

public static class Game
{
    public static int Player { get; set; }
    public static bool IsFirst { get; set; }
    public static Node Tree { get; set; }
    public static int Deep { get; set; }
    public static void UpdateGame()
    {
        string file;

        if (Player == 1)
            file = "m1";
        else
            file = "m2";

        if (IsFirst)
        {
            Tree = Tree.Play(20);
            File.WriteAllText($"{file}.txt", Tree.State.ToString());
            Tree.Expand(Deep);
            IsFirst = false;
            Tree.State.Print();
            System.Console.WriteLine("\n\n");
        }

        if (File.Exists($"[OUTPUT]{file}.txt"))
        {
            var content = File.ReadAllText($"[OUTPUT]{file}.txt");
            var data = content
                .Split(' ')
                .Select(ulong.Parse)
                .ToArray();
            File.Delete($"[OUTPUT]{file}.txt");

            Tree.Expand(Deep);
            Tree = Tree.Play(data[1], data[3]);

            Tree.Expand(Deep);

            Tree.AlphaBeta();
            Tree = Tree.PlayBest();
            Tree.Expand(Deep);

            var last = Tree.State.GetLastUlong();
            File.WriteAllText($"{file}.txt", Tree.State.ToString());
            Tree.State.Print();
            System.Console.WriteLine("\n\n");
        }
    }
}