namespace Items{
    class Party : Goods{

        public int Count { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ShelfLife { get; set; }

        public Party(string name, decimal price, int count, DateTime productionDate, DateTime shelfLife){
            Name           = name;
            Price          = price;
            Count          = count;
            ProductionDate = productionDate;
            ShelfLife      = shelfLife;
        }

        public override void Show(){
            
            Console.WriteLine($"[Партия]    {Name,-22} | Цена: {Price,6} руб. | " +
                              $"Кол-во: {Count,5} шт. | " +
                              $"Произведён: {ProductionDate:dd.MM.yyyy} | " +
                              $"Годен до: {ShelfLife:dd.MM.yyyy}");
        }

        public override bool IsShelfLife() => DateTime.Today <= ShelfLife;
    }
}
