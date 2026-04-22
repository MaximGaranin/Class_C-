namespace Items{
class Program{
    const string FMT  = "dd.MM.yyyy";
    const string PATH = "input.txt";

    static void Main()
    {
        var db = new List<Goods>();

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
                            p[1],
                            decimal.Parse(p[2]),
                            DateTime.ParseExact(p[3], FMT, null),
                            DateTime.ParseExact(p[4], FMT, null)));
                        break;

                    case "Партия":
                        db.Add(new Party(
                            p[1],
                            decimal.Parse(p[2]),
                            int.Parse(p[3]),
                            DateTime.ParseExact(p[4], FMT, null),
                            DateTime.ParseExact(p[5], FMT, null)));
                        break;

                    case "Комплект":
                        var products = p[3].Split('|').Select(s =>
                        {
                            var f = s.Split(':');
                            return new Product(
                                f[0],
                                decimal.Parse(f[1]),
                                DateTime.ParseExact(f[2], FMT, null),
                                DateTime.ParseExact(f[3], FMT, null));
                        }).ToList();
                        db.Add(new Complect(p[1], decimal.Parse(p[2]), products));
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
