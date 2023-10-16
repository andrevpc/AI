using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

int captcherPoints = 0;
int crackerPoints = 2;

var files = Directory.GetFiles("../user-data");
files = files
    .OrderBy(f => Random.Shared.Next())
    .Take(8)
    .ToArray();

Process cmd = new Process();
cmd.StartInfo.FileName = "cmd.exe";
cmd.StartInfo.RedirectStandardInput = true;
cmd.StartInfo.RedirectStandardOutput = true;
cmd.StartInfo.CreateNoWindow = true;
cmd.StartInfo.UseShellExecute = false;
cmd.Start();

cmd.StandardInput.WriteLine("cd ..");
cmd.StandardInput.WriteLine("cd captcher-app");

// Skip lines
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();

foreach (var file in files)
{
    cmd.StandardInput.WriteLine($"dotnet run '{file}'");
    // Skip lines
    cmd.StandardOutput.ReadLine();
    cmd.StandardOutput.ReadLine();
    if (cmd.StandardOutput.ReadLine() == "User")
        captcherPoints++;
}

cmd.StandardInput.WriteLine("cd ..");
cmd.StandardInput.WriteLine("cd cracker-code");
cmd.StandardInput.WriteLine($"dotnet run fake");
cmd.StandardInput.WriteLine("cd ..");
cmd.StandardInput.WriteLine("cd captcher-app");

cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();

cmd.StandardInput.WriteLine($"dotnet run '..\\cracker-code\\user-data fake 0.json'");
// Skip lines
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
if (cmd.StandardOutput.ReadLine() == "User")
    crackerPoints += 4;

cmd.StandardInput.WriteLine($"dotnet run '..\\cracker-code\\user-data fake 1.json'");
// Skip lines
cmd.StandardOutput.ReadLine();
cmd.StandardOutput.ReadLine();
if (cmd.StandardOutput.ReadLine() == "User")
    crackerPoints += 4;

Console.WriteLine(
    $"Captcher {captcherPoints} x Crakcer {crackerPoints}"
);
Console.WriteLine((captcherPoints >= crackerPoints ? "Captchers" : "Crackers") + " wins!!");