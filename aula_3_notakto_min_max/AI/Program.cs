using AI;

var notakto = new Notakto(int.Parse(args[1]));

FileFunction.Player = args[0] == "m1" ? 1 : 2;

if (args[0] == "m1")
    FileFunction.IsFirst = true;

while (true)
{
    Thread.Sleep(1000);
    FileFunction.UpdateFile(notakto);

    // var move = Console.ReadLine();

    // var data = move
    //             .Split(' ')
    //             .Select(int.Parse)
    //             .ToArray();

    // notakto.NotaktoPlay(data[0], data[1]);
}