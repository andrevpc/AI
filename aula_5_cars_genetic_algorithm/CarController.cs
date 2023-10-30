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
        Time++;
        switch (Gene[Time] % 5)
        {
            case 1:
                Car.StartAccelerate();
        }
        
        var turnLeft = Gene.Take(36).ToArray();
        var turnRight = Gene.Skip(36).Take(36).ToArray();
        
        var acell = Gene.Skip(72).Take(36).ToArray();
        var brake = Gene.Skip(108).Take(36).ToArray();


        for(int i = 0; i < 36; i++)
        {
            var tL = turnLeft[i];
            var tR = turnRight[i];

            var ac = acell[i];
            var br = brake[i];

            if(tL > tR)
                Car.TurnLeft();
            else
                Car.TurnRight();

            if(ac > br)
                Car.StartAccelerate();
            else
                Car.StartBrake();
        }


    }

    public float FitnessFunction()
    {
        return Car.Points;
    }
}