using AI;

Node tree = new()
{
    State = new Othello()
};
int deep = 6;
tree.State = Othello.New();
tree.Expand(deep);

Game.Player = args[0] == "m1" ? 1 : 2;
Game.Tree = tree;
Game.Deep = deep;

if (args[0] == "m1")
    Game.IsFirst = true;

while (true)
{
    Thread.Sleep(1000);
    Game.UpdateGame();
}