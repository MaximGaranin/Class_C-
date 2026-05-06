using System.Text.Json.Serialization;

namespace Items{
    class Complect : Goods{

        public List<Product> Products { get; set; } = new();

        [JsonConstructor]
        public Complect() {}

        public Complect(string name, decimal price, List<Product> products){
            Name     = name;
            Price    = price;
            Products = products;
        }

        public override void Show(){
            Console.WriteLine($"[Комплект] {Name,-22} | Стоимость комплекта: {Price,6} руб. | Состав ({Products.Count} шт.):");
            foreach (var p in Products)
                Console.WriteLine($"             - {p.Name,-20}| Цена: {p.Price} | Произведён: {p.ProductionDate:dd.MM.yyyy} | Годен до: {p.ShelfLife:dd.MM.yyyy}");
        }

        public override bool IsShelfLife() =>
            Products.Count > 0 && Products.All(p => p.IsShelfLife());
    }
}
