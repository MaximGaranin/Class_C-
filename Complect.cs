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
            string composition = string.Join(", ", Products.Select(p => p.Name));
            Console.WriteLine($"[Комплект] {Name,-22} | Цена: {Price,6} руб. | Состав: {composition}");
        }

        public override bool IsShelfLife() => Products.Count > 0 && Products.All(p => p.IsShelfLife());
    }
}
