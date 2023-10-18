namespace AI;
using System.IO;
using System.Linq;

public abstract class FileFunction
{
    int Player { get; set; }
    public void UpdateFile(string file)
    {
        string fileName;

        if (Player == 1)
            fileName = "m1.txt";
        else
            fileName = "m2.txt";
        
        if(file == fileName.Replace(".txt", " last.txt"))
        {
            // TODO - play

            File.Delete(file);
            File.WriteAllText(fileName, "TODO");
        }
    }
}