using System.IO;
using System.Text.Json;
using System.Collections.Generic;

public class UserData
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsDown { get; set; }
    public string Text { get; set; }

    public static List<UserData> Read(string file)
    {
        using (StreamReader r = new StreamReader(file))
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            string json = r.ReadToEnd();
            return JsonSerializer.Deserialize<List<UserData>>(json, options);
        }
    }

    public override string ToString()
        => $"{{ x:{X} y:{Y} isDown:{IsDown} text:{Text} }}";
}