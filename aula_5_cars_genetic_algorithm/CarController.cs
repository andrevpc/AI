using System;
using System.Drawing;
using System.Linq;

public class CarController
{
    public CarController(float x0, float y0)
    {
        this.Car = new Car(x0, y0)
        {
            Color = Color.FromArgb(
                Random.Shared.Next(255),
                Random.Shared.Next(255),
                Random.Shared.Next(255)
            )
        };
        this.Gene = new byte[256];
        Random.Shared.NextBytes(this.Gene);
    }
    public CarController(float x0, float y0, byte[] genes)
    {
        this.Car = new Car(x0, y0)
        {
            Color = Color.FromArgb(
                Random.Shared.Next(255),
                Random.Shared.Next(255),
                Random.Shared.Next(255)
            )
        };
        this.Gene = genes;
    }

    public Car Car { get; set; }
    public byte[] Gene { get; set; }
    public int Time { get; set; }
    public void Control()
    {
        if(Time > int.MaxValue || Time > Gene.Length * 8)
            Time = 0;
        Time++;
        switch (Gene[Time] % 8)
        {
            case 0:
                Car.StartBrake();
                break;
            case 2:
                Car.StopBrake();
                break;
            case 3:
                Car.StartAccelerate();
                break;
            case 4:
                Car.StopAccelerate();
                break;
            case 5:
                Car.TurnLeft();
                break;
            case 6:
                Car.TurnRight();
                break;
            case 7:
                Car.StopTurn();
                break;
        }
    }

    public float FitnessFunction()
        => Car.Points * Car.Points;
}