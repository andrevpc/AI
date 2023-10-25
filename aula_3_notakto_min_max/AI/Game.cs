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
            File.WriteAllText($"{file}.txt", $"0 4");

            Tree = Tree.Play(0, 4);
            Tree.Expand(Deep);
            IsFirst = false;
        }

        if (File.Exists($"{file} last.txt"))
        {
            var content = File.ReadAllText($"{file} last.txt");
            var data = content
                .Split(' ')
                .Select(int.Parse)
                .ToArray();
            File.Delete($"{file} last.txt");


            Tree = Tree.Play(data[0], data[1]);

            Tree.Expand(Deep);

            Tree.MinMax();
            Tree = Tree.PlayBest();
            Tree.Expand(Deep);

            var last = Tree.State.GetLast();
            File.WriteAllText($"{file}.txt", $"{last.board} {last.position}");
        }
    }
}