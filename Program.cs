using System.Text.Json;
using System.Text.Json.Serialization;

namespace Items{
class Program{
    const string FMT       = "dd.MM.yyyy";
    const string TXT_PATH  = "input.txt";
    const string JSON_IN   = "input.json";
    const string JSON_OUT  = "output.json";

    static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    static void Main()
    {
        List<Goods> db;

        // Приоритет: если есть input.json — читаем из него,
        // иначе читаем из input.txt
        if (File.Exists(JSON_IN))
        {
            PrintHeader($"ИСТОЧНИК: {JSON_IN}");
            db = LoadFromJson(JSON_IN);
        }
        else if (File.Exists(TXT_PATH))
        {
            PrintHeader($"ИСТОЧНИК: {TXT_PATH}");
            db = LoadFromTxt(TXT_PATH);
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

        // Сериализация в JSON
        SaveJson(db, JSON_OUT);

        // Десериализация обратно — проверка round-trip
        var reloaded = LoadFromJson(JSON_OUT);
        PrintHeader($"ЗАГРУЖЕНО ИЗ {JSON_OUT} (round-trip проверка)");
        foreach (var g in reloaded) g.Show();
    }

    // ── Чтение из TXT ────────────────────────────────────────────────────────
    static List<Goods> LoadFromTxt(string path)
    {
        var db = new List<Goods>();

        foreach (var line in File.ReadAllLines(path))
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

                    // Партия теперь состоит из продуктов.
                    // Формат строки: Партия;Название;Цена;Продукт1:цена1:дата_пр1:дата_сг1|Продукт2:...
                    case "Партия":
                        var partyProducts = p[3].Split('|').Select(s =>
                        {
                            var f = s.Split(':');
                            return new Product(
                                f[0].Trim(),
                                decimal.Parse(f[1]),
                                DateTime.ParseExact(f[2].Trim(), FMT, null),
                                DateTime.ParseExact(f[3].Trim(), FMT, null));
                        }).ToList();
                        db.Add(new Party(p[1].Trim(), decimal.Parse(p[2]), partyProducts));
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

        return db;
    }

    // ── Сериализация в JSON-файл ──────────────────────────────────────────────
    static void SaveJson(List<Goods> db, string path)
    {
        string json = JsonSerializer.Serialize(db, JsonOpts);
        File.WriteAllText(path, json);
        PrintHeader($"JSON сохранён → {path}");
        Console.WriteLine(json);
    }

    // ── Десериализация из JSON-файла ──────────────────────────────────────────
    static List<Goods> LoadFromJson(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[!] Файл {path} не найден.");
            return new List<Goods>();
        }

        string json = File.ReadAllText(path);

        // JsonPolymorphic на Goods обеспечивает корректное восстановление
        // конкретных типов (Product / Party / Complect) по дискриминатору "Type"
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
}
}
