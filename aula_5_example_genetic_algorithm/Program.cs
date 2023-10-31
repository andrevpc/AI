using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

float a = 0, b = 0, c = 0;
a = 1 * Random.Shared.NextSingle();
b = 2 * Random.Shared.NextSingle() - 1;
c = 10 * Random.Shared.NextSingle() - 5;

const int N = 40; // Number of individuals in the population
const float m = 0.05f; // Mutation prob

byte[] population = new byte[N];
float[] fitness = new float[N]; // Fitness function grades
Random.Shared.NextBytes(population);

Console.WriteLine($"{a} x^2 + {b} x + {c}".Replace(',', '.'));

for (int g = 0; g < 1000; g++)
{
    for (int i = 0; i < N; i++)
    {
        var gene = population[i];
        var x = (gene % 2 == 0 ? 1f : -1f) * (gene >> 1) / 10f; // gene % 2 == if it is positive or not and multiply the value without th signal
        var y = a * x * x + b * x + c;

        var fitnessFunction = y > 10 ? 0 : 10 - y; // fitnessFunction gets the maxValue, as we need the lowest we invert the value
        fitness[i] = fitnessFunction;
    }

    var max = fitness.Max();
    var bestX = population
        .Select((n, i) => new { n, i })
        .MaxBy(b => fitness[b.i])
        .n;
    Console.WriteLine($"x = {bestX}, fitness = {max}");
    

    var sum = fitness.Sum(b => b);
    List<byte> parents = new List<byte>();
    for (int i = 0; i < N; i++)
    {
        float next = Random.Shared.NextSingle() * sum; // From 0 to sum
        for (int j = 0; j < N; j++)
        {
            next -= fitness[j];
            if (next > 0) // if is not the selected
                continue;
            
            parents.Add(population[j]); // Select randomly, but with more weight for the biggest fitness
        }
    }

    const byte mask1 = 0b1111_0000;
    const byte mask2 = 0b0000_1111;

    for (int i = 0; i < N; i += 2)
    {
        var parentA = parents[i];
        var parentB = parents[i + 1];

        // CrossOver
        // 4 least significant bits of the father A and 4 
        // most significant bits of the father B
        var childA = (parentA & mask1) + (parentB & mask2);
        var childB = (parentB & mask1) + (parentA & mask2);

        population[i] = (byte)childA;
        population[i + 1] = (byte)childB;
    }

    for (int i = 0; i < N; i++)
    {
        // Mutation
        if (Random.Shared.NextSingle() > m) // Do the mutation or not
            continue;
        
        int k = Random.Shared.Next(8);
        var bit = 1 << k; // Random bit 00001000
        population[i] = (byte)(population[i] ^ bit); // Invert the random bit
    }
}