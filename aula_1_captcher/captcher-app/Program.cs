using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var defaultJsons = new string[]
{
    "user-data 10.json",
    "user-data 16.json",
};
var file = args.Length == 0 || !File.Exists(args[0]) ?
      defaultJsons[Random.Shared.Next(2)] : args[0];
List<UserData> data = UserData.Read(file);

// add code here
var velocidadeConstante = false;
var constanciaDigitacao = false;
var caracterEspecialImpossivel = false;

var velocidades = new List<int>();

for (int i = 0; i < data.Count; i++)
{
    if (i > 0)
        if ((data[i].X - data[i - 1].X) + (data[i].Y - data[i - 1].Y) != 0)
        {
            var velocidadeAtual = (data[i].X - data[i - 1].X) + (data[i].Y - data[i - 1].Y);
            velocidades.Add(velocidadeAtual);
        }
}
// verificando a velocidade 
var velAgrupado = velocidades.GroupBy(x => x);

foreach (var speed in velocidades.Distinct())
{
    if (
        velocidades.Count(x => x == speed) > velocidades.Count * 0.20
        && speed != 0 
        && Math.Abs(speed) != 1
    )
    {
        velocidadeConstante = true;
    }
}

//verificando repetições de inputs
var especialCharacters = new string[]{"!","@","#", "$","%","¨","&","(",")","_"};
for (int i = 0; i < data.Count; i++)
{
    if (i > 0)
    {
        if (especialCharacters.Contains(data[i].Text))
        {
            var current = i;

            while(current > -1)
            {
                if (data[current].Text != data[i].Text)
                {
                    caracterEspecialImpossivel = data[current].Text == "Shift" ? false : true;
                    
                    if (caracterEspecialImpossivel == false)
                        break;
                }
                current--;
            }       
        }
    }
}


// Velocidade digitação
var teclas = new List<string>();

for (int i = 0; i < data.Count; i++)
{
    if (data[i].Text != "")
        teclas.Add(data[i].Text);
}

// var teclasAgrupadas = teclas.GroupBy(x => x);
var max = 0;
var repeated = 0;

foreach (var tecla in teclas.Distinct())
{
    if (teclas.Count(x => x == tecla) > max)
    {
        max = teclas.Count(x => x == tecla);
        repeated = 0;
    }
    else if (teclas.Count(x => x == tecla) == max)
        repeated++;
}

if (repeated >= teclas.Count * 0.5)
    constanciaDigitacao = true;

// deafult implementation example
// defeat instaclick bot
if (data.Count < 5 || velocidadeConstante || caracterEspecialImpossivel || constanciaDigitacao)
    isCracker();
else isUser();

void isCracker()
    => Console.WriteLine("Cracker");

void isUser()
    => Console.WriteLine("User");