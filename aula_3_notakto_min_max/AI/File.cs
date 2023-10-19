namespace AI;
using System.IO;
using System.Linq;

public static class FileFunction
{
    public static int Player { get; set; }
    public static bool IsFirst { get; set; }
    public static void UpdateFile(Notakto notakto)
    {
        string file;

        if (Player == 1)
            file = "m1";
        else
            file = "m2";

        if(IsFirst)
        {
            File.WriteAllText($"{file}.txt", $"0 4");
            notakto.Play(0, 4);
            IsFirst = false;
            return;
        }

        if (File.Exists($"{file} last.txt"))
        {
            var content = File.ReadAllText($"{file} last.txt"); //ReadLine FirstOrDefault
            var data = content
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            bool isFinished = notakto.Play(data[0], data[1]);

            System.Console.WriteLine(data[0]);
            System.Console.WriteLine(data[1]);

            File.Delete($"{file} last.txt");

            File.WriteAllText($"{file}.txt", "TODO");
            // notakto.NotaktoPlay(0, 0);
        }
    }
}