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
            Console.WriteLine($"[Партия]   {Name,-22} | Стоимость партии: {Price,6} руб. | Кол-во: {Count,5} шт.");
            Console.WriteLine($"             - {Item.Name,-20} | Цена: {Item.Price} | Произведён: {Item.ProductionDate:dd.MM.yyyy} | Годен до: {Item.ShelfLife:dd.MM.yyyy}");
        }

        public override bool IsShelfLife() => Item.IsShelfLife();
    }
}
