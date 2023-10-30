using System;
using System.Collections.Generic;
using System.Linq;

public class GeneticAlgorithm
{
    private float x0;
    private float y0;
    private GeneticAlgorithm() { }
    public GeneticAlgorithm(float x0, float y0)
    {
        this.x0 = x0;
        this.y0 = y0;
        for (int i = 0; i < 16; i++)
            Population.Add(new CarController(x0, y0));
    }

    public List<CarController> Population { get; private set; } = new();

    public GeneticAlgorithm GetNextGeneration()
    {
        var ga = new GeneticAlgorithm
        {
            x0 = x0,
            y0 = y0
        };
        var parents = new List<byte[]>();

        var fits = this.Population
            .Select(g => g.FitnessFunction())
            .ToArray();
        var total = fits.Sum();
        for (int k = 0; k < 16; k++)
        {
            var selected = Random.Shared.NextSingle() * total;
            for (int i = 0; i < fits.Length; i++)
            {
                selected -= fits[i];
                if (selected < 0)
                {
                    parents.Add(this.Population[i].Gene);
                    break;
                }
            }
        }

        var children = new List<byte[]>();
        
        for (int i = 0; i < 16; i += 2)
        {
            var childA = new byte[256];
            var childB = new byte[256];
            
            var k = Random.Shared.Next(256);
            Array.Copy(parents[i], 0, childA, 0, k);
            Array.Copy(parents[i], k, childB, k, 256 - k);

            Array.Copy(parents[i + 1], k, childA, k, 256 - k);
            Array.Copy(parents[i + 1], 0, childB, 0, k);

            children.Add(childA);
            children.Add(childB);
        }

        for (int i = 0; i < 16; i++)
        {
            if (Random.Shared.NextSingle() > .25f)
                continue;

            for (int j = 0; j < 256; j++)
            {
                if (Random.Shared.NextSingle() > .02f)
                    continue;
                
                children[i][j] = (byte)Random.Shared.Next(256);
            }
        }

        for (int i = 0; i < 16; i++)
        {
            ga.Population.Add(
                new CarController(x0, y0, children[i])
            );
        }

        return ga;
    }
}