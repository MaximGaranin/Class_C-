using System.Text.Json;
using System.Text.Json.Serialization;

namespace Items{
class Program{
    const string FMT       = "dd.MM.yyyy";
    const string TXT_PATH  = "input.txt";
    const string JSON_IN   = "input.json";

    static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    static void SaveJson(List<Goods> db, string path)
    {
        string json = JsonSerializer.Serialize(db, JsonOpts);
        File.WriteAllText(path, json);
    }

    static List<Goods> LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[!] Файл {path} не найден.");
            return new List<Goods>();
        }

        string json = File.ReadAllText(path);
        var loaded = JsonSerializer.Deserialize<List<Goods>>(json, JsonOpts);

        if (loaded == null || loaded.Count == 0)
        {
            Console.WriteLine($"[!] Десериализация {path} вернула пустой список.");
            return new List<Goods>();
        }

        return loaded;
    }

    static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('─', 70));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('─', 70));
    }

    static void Main()
    {
        List<Goods> db;

        if (File.Exists(JSON_IN))
        {
            db = LoadFromJson(JSON_IN);
        }
        else
        {
            Console.WriteLine("[!] Не найден ни input.json, ни input.txt.");
            return;
        }

        if (db.Count == 0)
        {
            Console.WriteLine("[!] База пуста. Проверьте входной файл.");
            return;
        }

        PrintHeader("ВСЕ ТОВАРЫ (исходный порядок)");
        foreach (var g in db) Console.WriteLine(g);

        db.Sort();
        PrintHeader("СОРТИРОВКА ПО ЦЕНЕ ↑");
        foreach (var g in db) Console.WriteLine(g);

        PrintHeader($"ПРОСРОЧЕННЫЕ (на {DateTime.Today:dd.MM.yyyy})");
        var expired = db.Where(g => !g.IsShelfLife()).ToList();
        if (expired.Any())
            foreach (var g in expired) Console.WriteLine(g);
        else
            Console.WriteLine("  Просроченных не найдено.");

        SaveJson(db, JSON_IN);

        var reloaded = LoadFromJson(JSON_IN);
        PrintHeader($"ЗАГРУЖЕНО ИЗ {JSON_IN}");
        foreach (var g in reloaded) Console.WriteLine(g);
    }

}
}
