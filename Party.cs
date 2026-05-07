using System.Text.Json.Serialization;

namespace Items{
    class Party : Goods{
        public int Count { get; set; }
        public Product Item { get; set; } = new();

        [JsonConstructor]
        public Party() : base() {}

        public Party(string name, decimal price, int count, Product item)
            : base(name, price)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (count <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            Count = count;
            Item = new Product(item);
        }

        public override bool IsShelfLife() => Item.IsShelfLife();

        public override string ToString()
        {
            return $"[Партия]   {Name,-22} | Цена: {Price,6} руб. | Кол-во: {Count,5} шт.\n" +
                   $"             - {Item.Name,-20} | Произведён: {Item.ProductionDate:dd.MM.yyyy} | " +
                   $"Годен до: {Item.ShelfLife:dd.MM.yyyy}";
        }
    }
}
