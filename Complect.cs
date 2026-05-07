using System.Text.Json.Serialization;

namespace Items{
    class Complect : Goods{
        public List<Product> Products { get; set; } = new();

        [JsonConstructor]
        public Complect() {}

        public Complect(string name, decimal price, List<Product> products)
            : base(name, price)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (products.Count == 0)
                throw new ArgumentException("Комплект не может быть пустым.");

            Products = products.Select(p => new Product(p)).ToList();
        }

        public override bool IsShelfLife() =>
            Products.Count > 0 && Products.All(p => p.IsShelfLife());

        public override string ToString()
        {
            string info = string.Join("\n", Products.Select(
                p => $"             - {p.Name,-20} | Произведён: {p.ProductionDate:dd.MM.yyyy} | Годен до: {p.ShelfLife:dd.MM.yyyy}"
            ));

            return $"[Комплект] {Name,-22} | Цена: {Price,6} руб. | Состав ({Products.Count} шт.):\n{info}";
        }
    }
}
