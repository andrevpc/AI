using System;

ICracker cracker = 
    args.Length > 0 && args[0] == "fake" ?
    new FakeCracker() : new Cracker();

cracker.Init(() =>
{
    // Add code here
    // Example of a dumb bot
    cracker.MoveTo(0, 0);
    var pos = cracker.GetPosition();

    #region Email
    // Test
    Move(pos, (890, 598));
    Move(pos, (1065, 618));

    pos = BotMoveTo(pos, (890, 598), (175, 20), Random.Shared.Next(50, 80));
    cracker.MouseLeftDown();
    cracker.Wait(50);
    cracker.MouseLeftUp();


    BotWrite("hackerJunior@email.com");
    
    #endregion

    cracker.Wait(Random.Shared.Next(100, 250));

    #region Password
    // Test
    // Move(pos, (900, 635));
    // Move(pos, (1075, 655));

    pos = BotMoveTo(pos, (900, 635), (175, 20), Random.Shared.Next(80, 100));
    cracker.MouseLeftDown();
    cracker.Wait(50);
    cracker.MouseLeftUp();

    BotWrite("strongPassword1234");
    
    #endregion

    cracker.Wait(Random.Shared.Next(100, 250));

    #region Login Button
    // Test
    // Move(pos, (905, 676));
    // Move(pos, (945, 691));

    pos = BotMoveTo(pos, (905, 676), (40, 15), Random.Shared.Next(60, 100));

    cracker.MouseLeftDown();
    cracker.Wait(50);
    cracker.MouseLeftUp();

    #endregion

    // Final version of all program may
    // contains exit function
    cracker.Exit();
});

(int x, int y) BotMoveTo((int x, int y) pos, (int x, int y) target, (int width, int height) error, int delay = 50)
{
    (int x, int y) vel = (5, 5);
    int flagX = 1;
    int flagY = 1;

    while (true)
    {
        var (x, y) = pos;
        pos = Move(pos, (pos.x + flagX * randomMoveX(vel.x), pos.y + flagY * randomMoveY(vel.y)), delay);

        vel = (pos.x - x, pos.y - y);

        if (flagX > 0 && pos.x > target.x + error.width)
            flagX = -1;
        else if (flagX < 0 && pos.x < target.x)
            flagX = 1;

        if (flagY > 0 && pos.y > target.y + error.height)
            flagY = -1;
        else if (flagY < 0 && pos.y < target.y)
            flagY = 1;

        if (pos.x > target.x && pos.x < target.x + error.width &&
            pos.y > target.y && pos.y < target.y + error.height)
            return cracker.GetPosition();
    }
}

int randomMoveX(int vel)
{
    const int minVel = 15;
    const int maxVel = 25;
    
    if (vel < minVel)
        vel = minVel;
    else if (vel > maxVel)
        vel = maxVel;
    
    return Random.Shared.Next(10, 2 * vel);
}

int randomMoveY(int vel)
{
    const int minVel = 5;
    const int maxVel = 15;
    
    if (vel < minVel)
        vel = minVel;
    else if (vel > maxVel)
        vel = maxVel;
    
    return Random.Shared.Next(10, 2 * vel);
}

(int x, int y) Move((int x, int y) pos, (int x, int y) target, int delay = 50)
{
    cracker.Print(pos);
    cracker.Wait(delay);
    cracker.MoveTo(target.x, target.y);
    return cracker.GetPosition();
}

void BotWrite(string text, int delay = 200)
{
    foreach (var letter in text)
    {
        cracker.Write(letter.ToString());
        cracker.Wait(delay);
    }
}
