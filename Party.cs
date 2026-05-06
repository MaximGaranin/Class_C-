using System.Text.Json.Serialization;

namespace Items{
    class Party : Goods{

        public List<Product> Products { get; set; } = new();

        [JsonConstructor]
        public Party() {}

        public Party(string name, decimal price, List<Product> products){
            Name     = name;
            Price    = price;
            Products = products;
        }

        public override void Show(){
            Console.WriteLine($"[Партия]   {Name,-22} | Цена: {Price,6} руб. | Состав ({Products.Count} шт.):");
            foreach (var p in Products)
                Console.WriteLine($"             - {p.Name,-20} | Произведён: {p.ProductionDate:dd.MM.yyyy} | Годен до: {p.ShelfLife:dd.MM.yyyy}");
        }

        // Партия свежа, если все её продукты ещё в срок
        public override bool IsShelfLife() =>
            Products.Count > 0 && Products.All(p => p.IsShelfLife());
    }
}
