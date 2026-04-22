using System.Text.Json;
using System.Text.Json.Serialization;

namespace Items{
class Program{
    const string FMT      = "dd.MM.yyyy";
    const string PATH     = "input.txt";
    const string JSON_OUT = "output.json";

    static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    static void Main()
    {
        var db = new List<Goods>();

        if (!File.Exists(PATH))
        {
            Console.WriteLine($"[!] Файл {PATH} не найден.");
            return;
        }

        foreach (var line in File.ReadAllLines(PATH))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var p = line.Split(';');

            try
            {
                switch (p[0].Trim())
                {
                    case "Продукт":
                        db.Add(new Product(
                            p[1].Trim(),
                            decimal.Parse(p[2]),
                            DateTime.ParseExact(p[3].Trim(), FMT, null),
                            DateTime.ParseExact(p[4].Trim(), FMT, null)));
                        break;

                    case "Партия":
                        db.Add(new Party(
                            p[1].Trim(),
                            decimal.Parse(p[2]),
                            int.Parse(p[3]),
                            DateTime.ParseExact(p[4].Trim(), FMT, null),
                            DateTime.ParseExact(p[5].Trim(), FMT, null)));
                        break;

                    case "Комплект":
                        var products = p[3].Split('|').Select(s =>
                        {
                            var f = s.Split(':');
                            return new Product(
                                f[0].Trim(),
                                decimal.Parse(f[1]),
                                DateTime.ParseExact(f[2].Trim(), FMT, null),
                                DateTime.ParseExact(f[3].Trim(), FMT, null));
                        }).ToList();
                        db.Add(new Complect(p[1].Trim(), decimal.Parse(p[2]), products));
                        break;

                    default:
                        Console.WriteLine($"[!] Неизвестный тип: {p[0]}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Ошибка в строке: \"{line}\"\n    {ex.Message}");
            }
        }

        if (db.Count == 0)
        {
            Console.WriteLine("[!] База пуста. Проверьте файл.");
            return;
        }

        PrintHeader("ВСЕ ТОВАРЫ (исходный порядок)");
        foreach (var g in db) g.Show();

        db.Sort();
        PrintHeader("СОРТИРОВКА ПО ЦЕНЕ ↑");
        foreach (var g in db) g.Show();

        PrintHeader($"ПРОСРОЧЕННЫЕ (на {DateTime.Today:dd.MM.yyyy})");
        var expired = db.Where(g => !g.IsShelfLife()).ToList();
        if (expired.Any())
            foreach (var g in expired) g.Show();
        else
            Console.WriteLine("  Просроченных не найдено.");

        // JSON сериализация
        SaveJson(db);
        LoadJson();
    }

    static void SaveJson(List<Goods> db)
    {
        string json = JsonSerializer.Serialize(db, JsonOpts);
        File.WriteAllText(JSON_OUT, json);
        PrintHeader($"JSON сохранён → {JSON_OUT}");
        Console.WriteLine(json);
    }

    static void LoadJson()
    {
        if (!File.Exists(JSON_OUT)) return;

        string json = File.ReadAllText(JSON_OUT);
        var loaded = JsonSerializer.Deserialize<List<Goods>>(json, JsonOpts);

        if (loaded == null || loaded.Count == 0)
        {
            Console.WriteLine("[!] Десериализация вернула пустой список.");
            return;
        }

        PrintHeader("ЗАГРУЖЕНО ИЗ JSON");
        foreach (var g in loaded) g.Show();
    }

    static void PrintHeader(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('─', 70));
        Console.WriteLine($"  {title}");
        Console.WriteLine(new string('─', 70));
    }
}
}
