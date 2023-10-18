// See https://aka.ms/new-console-template for more information
using AI;

var notakto = new Notakto(int.Parse(args[1]));

while (true)
{
    FileFunction.Player = args[0] == "m1" ? 1 : 2;

    FileFunction.UpdateFile("", notakto);

    var move = Console.ReadLine();

    var data = move
                .Split(' ')
                .Select(int.Parse)
                .ToArray();

    notakto.NotaktoPlay(data[0], data[1]);

}