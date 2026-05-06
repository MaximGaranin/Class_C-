using System.Text.Json.Serialization;

namespace Items{
    class Party : Goods{

        public int Count { get; set; }
        public Product Item { get; set; } = new();

        [JsonConstructor]
        public Party() {}

        public Party(string name, decimal price, int count, Product item){
            Name  = name;
            Price = price;
            Count = count;
            Item  = item;
        }

        public override void Show(){
            Console.WriteLine($"[Партия]   {Name,-22} | Цена: {Price,6} руб. | Кол-во: {Count,5} шт.");
            Console.WriteLine($"             - {Item.Name,-20} | Произведён: {Item.ProductionDate:dd.MM.yyyy} | Годен до: {Item.ShelfLife:dd.MM.yyyy}");
        }

        // Партия свежа, если её продукт ещё в срок
        public override bool IsShelfLife() => Item.IsShelfLife();
    }
}
