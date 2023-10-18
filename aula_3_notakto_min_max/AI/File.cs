namespace AI;
using System.IO;
using System.Linq;

public static class FileFunction
{
    public static int Player { get; set; }
    public static void UpdateFile(string file, Notakto notakto)
    {
        string fileName;

        if (Player == 1)
            fileName = "m1.txt";
        else
            fileName = "m2.txt";

        if (file == fileName.Replace(".txt", " last.txt"))
        {
            var content = File.ReadAllText(file); //ReadLine FirstOrDefault
            var data = content
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

            bool isFinished = notakto.NotaktoPlay(data[0], data[1]);

            System.Console.WriteLine(data[0]);
            System.Console.WriteLine(data[1]);

            File.Delete(file);
            File.WriteAllText(fileName, "TODO");
        }
    }
}