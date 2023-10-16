using System;
using System.Text.Json;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

public class FakeCracker : ICracker
{
    List<UserData> data = new List<UserData>();

    int x = Random.Shared.Next(Screen.PrimaryScreen.Bounds.Width);
    int y = Random.Shared.Next(Screen.PrimaryScreen.Bounds.Height);
    bool isDown = false;
    string text = "";
    int time = 0;

    int files = 0;
    public void Exit()
    {
        var r = Random.Shared.Next(10);
        for (int i = 0; i < r; i++)
            data.Add(new UserData
            {
                IsDown = isDown,
                X = x,
                Y = y,
                Text = text
            });

        File.WriteAllText($"use-data fake {files++}.json",
            JsonSerializer.Serialize(data)
            .Replace("},{", "},\n\t{")
            .Replace("[", "[\n\t")
            .Replace("]", "\n]")
        );
    }

    public void Init(Action impl)
    {
        impl();
        impl();
    }

    public void Copy(string text)
        => Clipboard.SetText(text);

    public (int x, int y) GetPosition()
        => (x, y);

    public void MouseLeftDown()
        => isDown = true;

    public void MouseLeftUp()
        => isDown = false;

    public void MouseRightDown() { }

    public void MouseRightUp() { }

    public void MoveTo(int x, int y)
    {
        this.x = x;
        this.y = y;
        if (Random.Shared.Next(4) == 0)
        {
            time++;
            printScreen();
        }
    }

    public void Paste() { }

    public void Print(object text) { }

    public void Wait(int milli = 50)
    {
        for (int i = 0; i < milli; i++)
        {
            time++;
            printScreen();
        }
    }

    public void Write(string text)
    {
        foreach (var c in text)
        {
            text = c.ToString();
            Wait(Random.Shared.Next(40, 120));
        }
    }

    private void printScreen()
    {
        if (time % 50 != 0)
            return;
        
        data.Add(new UserData
        {
            IsDown = isDown,
            X = x,
            Y = y,
            Text = text
        });
    }
}