using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Pamella;

App.Open<GameView>();

public class GameView : View
{
    List<PointF> street = new List<PointF>();
    List<PointF> middlePoints = new List<PointF>();
    float camX = -500;
    float camY = -500;
    float camXTg = -500;
    float camYTg = -500;
    float zoom = 1f;
    float speed = 1f;
    float lst = 0f, bst = 0f;
    GeneticAlgorithm ga = null;

    void startGame()
    {
        lst = ga?.Population?.Max(x => x.FitnessFunction()) ?? 0;
        if (lst > bst)
            bst = lst;
        ga = ga?.GetNextGeneration() ??
            new GeneticAlgorithm(2000, 0);
        dt = DateTime.Now;
        last = DateTime.Now;
    }
    void resetGame()
    {
        ga = null;
        generateStreet(
            generateFunc(8)
        );
        startGame();
    }
    protected override void OnStart(IGraphics g)
    {
        resetGame();
        g.SubscribeKeyDownEvent(key => {
            switch (key)
            {
                case Input.Space:
                    resetGame();
                    Invalidate();
                    break;
                case Input.Escape:
                    App.Close();
                    break;
                case Input.F:
                    speed *= 2;
                    if (speed > 16)
                        speed = 16;
                    Invalidate();
                    break;
                case Input.D:
                    speed = 1;
                    Invalidate();
                    break;
                case Input.S:
                    speed /= 2;
                    if (speed < 1)
                        speed = 1;
                    Invalidate();
                    break;
            }
        });
    }

    DateTime dt = DateTime.Now;
    DateTime controlTime = DateTime.Now;
    DateTime last = DateTime.Now;
    protected override void OnFrame(IGraphics g)
    {
        Car bestCar = null;
        float bestFit = float.MinValue;
        var passed = (DateTime.Now - dt).TotalMilliseconds * speed;
        foreach (var controller in ga.Population)
        {
            var car = controller.Car;
            var dtObj = 0f;
            while (dtObj < passed)
            {
                car.Move(0.01f);
                dtObj += 10;
            }

            car.TryCollect(middlePoints.ToArray());
            var fit = controller.FitnessFunction();
            if (fit >= bestFit)
            {
                bestCar = car;
                bestFit = fit;
            }
        }
        dt = DateTime.Now;
        if (bestCar is not null)
        {
            camXTg = zoom * bestCar.X - g.Width / 2;
            camYTg = zoom * bestCar.Y - g.Height / 2;
        }

        camX += (camXTg - camX) / 20f;
        camY += (camYTg - camY) / 20f;

        if (speed * (DateTime.Now - controlTime).TotalMilliseconds > 100)
        {
            foreach (var controller in ga.Population)
                controller.Control();
            controlTime = DateTime.Now;
        }
        Invalidate();
    }

    double millisPassed = 0;
    int gen = 1;
    protected override void OnRender(IGraphics g)
    {
        g.Clear(Color.LightGreen);

        drawStreet(g);

        foreach (var controller in ga.Population)
            controller.Car.Draw(g, camX, camY, zoom, controller.FitnessFunction());
        
        millisPassed += speed * (DateTime.Now - last).TotalMilliseconds;
        last = DateTime.Now;
        var time = Math.Round(30 - millisPassed / 1000, 1);
        g.DrawText(
            new RectangleF(0, 0, 100, 100),
            SystemFonts.MessageBoxFont,
            StringAlignment.Center,
            StringAlignment.Center,
            Brushes.Red,
            $"{time} s\nx{speed}\ngen{gen}\nlst:{lst}\nbst:{bst}"
        );

        if (time < 0)
        {
            millisPassed = 0;
            startGame();
            gen++;
        }
    }

    Func<float, float> generateFunc(int complexity)
    {
        float[] amps = new float[complexity];
        float[] freqs = new float[complexity];

        for (int i = 0; i < complexity; i++)
        {
            amps[i] = 2f * Random.Shared.NextSingle();
            freqs[i] = 0.5f * Random.Shared.Next(complexity);
        }

        return t =>
        {
            float total = 0;

            for (int i = 0; i < complexity; i++)
                total += amps[i] * MathF.Sin(freqs[i] * t);

            return total;
        };
    }

    void drawStreet(IGraphics g)
    {
        int N = street.Count;
        PointF[] outStreet = new PointF[N];
        PointF[] innerStreet = new PointF[N];
        
        var last = street[N - 1];
        for (int i = 0; i < N; i++)
        {
            var crr = street[i];

            var lx = last.X;
            var ly = last.Y;
            var x = crr.X;
            var y = crr.Y;

            var dx = x - lx;
            var dy = y - ly;
            var mod = MathF.Sqrt(dx * dx + dy * dy);

            var v = (x: 200 * dy / mod, y: -200 * dx / mod);

            outStreet[i] = new PointF(
                zoom * (x + v.x) - camX, zoom * (y + v.y) - camY
            );
            innerStreet[i] = new PointF(
                zoom * (x - v.x) - camX, zoom * (y - v.y) - camY
            );

            last = crr;
        }

        g.FillPolygon(outStreet, new SolidBrush(Color.FromArgb(10, 10, 40)));
        g.FillPolygon(innerStreet, Brushes.LightGreen);

        var middlePoints = outStreet
            .Zip(innerStreet, (p, q) 
                => new PointF((p.X + q.X) / 2, (p.Y + q.Y) / 2)
            )
            .ToArray();
        this.middlePoints.Clear();
        
        for (int i = 0; i < middlePoints.Length; i += 20)
        {
            var pt = middlePoints[i];
            this.middlePoints.Add(
                new PointF((pt.X + camX) / zoom, (pt.Y + camY) / zoom));
            g.FillRectangle(pt.X - 5, pt.Y - 5, 10, 10, Brushes.LightBlue);
        }
    }

    void generateStreet(Func<float, float> f)
    {
        street.Clear();

        float theta = 0f;
        float rho = 2000;
        while (theta < MathF.Tau)
        {
            float x = rho * MathF.Cos(theta),
                  y = rho * MathF.Sin(theta);
            street.Add(new PointF(x, y));

            theta += MathF.Tau / 1000;
            rho = 2000 + 500 * f(theta);
        }
    }
}