using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

using Pamella;

public class OthelloView : View
{
    const string m1 = "m1.txt";
    const string m2 = "m2.txt";
    int passCount = 0;

    OthelloGame game = OthelloGame.New();

    protected override void OnStart(IGraphics g)
    {
        g.SubscribeKeyDownEvent(key =>
        {
            if (key == Input.Escape)
                App.Close();
        });
    }

    protected override void OnFrame(IGraphics g)
    {
        if (passCount > 1)
            return;

        try
        {
            if (game.WhitePlays)
                get(m1, m2);
            else get(m2, m1);

        }
        catch
        {

        }

        void get(string path, string other)
        {
            if (!File.Exists(path))
                return;

            var text = File.ReadAllText(path);
            if (text != "pass")
            {
                var content = text.Split(' ');
                this.game = OthelloGame.New(
                    byte.Parse(content[0]),
                    ulong.Parse(content[1]),
                    ulong.Parse(content[3]),
                    byte.Parse(content[2]),
                    byte.Parse(content[4])
                );
                passCount = 0;
            }
            else
            {
                passCount++;
                this.game.Pass();
            }
            File.Delete(path);

            File.WriteAllText("[OUTPUT]" + other, this.game.ToString());
            Invalidate();
        }
    }

    protected override void OnRender(IGraphics g)
    {
        const int boardMargin = 20;
        const int boardPadding = 10;
        const int squareMargin = 5;
        const int squarePadding = 5;

        int min = int.Min(g.Width, g.Height);
        int boardSize = min - 2 * boardMargin;
        int squareSize = (boardSize - 7 * squareMargin - 2 * boardPadding) / 8;
        int radius = (squareSize - 2 * squarePadding) / 2;

        float x0 = (g.Width - boardSize) / 2 + boardPadding;
        float x = x0;
        float y = (g.Height - boardSize) / 2 + boardPadding;

        g.Clear(Color.DarkRed);
        g.DrawText(
            new RectangleF(0, 0, 150, 150),
            StringAlignment.Center, StringAlignment.Center,
            Brushes.White, "debugInfo:\n" + game.ToString());

        string winInfo = string.Empty;
        if (passCount > 1)
        {
            if (game.WhitePoints > game.BlackPoints)
                winInfo = "White Wins!";
            else if (game.WhitePoints == game.BlackPoints)
                winInfo = "Tie!";
            else winInfo = "Black Wins!";
        }
        g.DrawText(
            new RectangleF(x0 + boardSize, 0, 150, 150),
            StringAlignment.Center, StringAlignment.Center,
            Brushes.White, $"W {game.WhitePoints} x {game.BlackPoints} B\n{winInfo}");
        g.FillRectangle(
            x - boardPadding, y - boardPadding,
            boardSize, boardSize,
            Brushes.DarkGreen
        );

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = game[i, j];
                g.FillRectangle(x, y,
                    squareSize, squareSize,
                    Brushes.Green
                );

                if (piece != 0)
                {
                    List<PointF> pts = new List<PointF>();

                    for (float theta = 0; theta < MathF.Tau; theta += 0.2f)
                    {
                        pts.Add(new PointF(
                            x + squareSize / 2 + radius * MathF.Cos(theta),
                            y + squareSize / 2 + radius * MathF.Sin(theta)
                        ));
                    }

                    g.FillPolygon(pts.ToArray(),
                        piece == 1 ? Brushes.White : Brushes.Black
                    );
                }

                x += squareSize + squareMargin;
            }
            x = x0;
            y += squareSize + squareMargin;
        }
    }
}