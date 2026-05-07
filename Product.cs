using System.Text.Json.Serialization;

namespace Items{
    class Product : Goods{
        public DateTime ProductionDate { get; set; }
        public DateTime ShelfLife { get; set; }

        [JsonConstructor]
        public Product() : base() {}

        public Product(string name, decimal price, DateTime productionDate, DateTime shelfLife)
            : base(name, price)
        {
            if (shelfLife < productionDate)
                throw new ArgumentException("Срок годности не может быть раньше даты производства.");

            ProductionDate = productionDate;
            ShelfLife = shelfLife;
        }

        // Copy constructor — создаёт глубокую копию объекта
        public Product(Product other)
            : base(other?.Name ?? throw new ArgumentNullException(nameof(other)), other.Price)
        {
            ProductionDate = other.ProductionDate;
            ShelfLife = other.ShelfLife;
        }

        public override bool IsShelfLife() => DateTime.Today <= ShelfLife;

        public override string ToString()
        {
            return $"[Продукт]  {Name,-22} | Цена: {Price,6} руб. | " +
                   $"Произведён: {ProductionDate:dd.MM.yyyy} | " +
                   $"Годен до: {ShelfLife:dd.MM.yyyy}";
        }
    }
}
