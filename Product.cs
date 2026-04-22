using System.Text.Json.Serialization;

namespace Items{
    class Product : Goods{

        public DateTime ProductionDate { get; set; }
        public DateTime ShelfLife { get; set; }

        [JsonConstructor]
        public Product() {}

        public Product(string name, decimal price, DateTime productionDate, DateTime shelfLife){
            Name           = name;
            Price          = price;
            ProductionDate = productionDate;
            ShelfLife      = shelfLife;
        }

        public override void Show(){
            Console.WriteLine($"[Продукт]  {Name,-22} | Цена: {Price,6} руб. | " +
                              $"Произведён: {ProductionDate:dd.MM.yyyy} | " +
                              $"Годен до: {ShelfLife:dd.MM.yyyy}");
        }

        public override bool IsShelfLife() => DateTime.Today <= ShelfLife;
    }
}
