using System.Text.Json.Serialization;

namespace Items{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(Product), "Product")]
    [JsonDerivedType(typeof(Party), "Party")]
    [JsonDerivedType(typeof(Complect), "Complect")]
    abstract class Goods : IComparable{
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Пустой конструктор — нужен для [JsonConstructor] в наследниках
        protected Goods() {}

        protected Goods(string name, decimal price){
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название не может быть пустым.");

            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной.");

            Name = name;
            Price = price;
        }

        public abstract override string ToString();
        public abstract bool IsShelfLife();

        // Проверка типа: если obj не является Goods — бросаем исключение
        public int CompareTo(object? obj){
            if (obj is null) return 1;

            if (obj is not Goods other)
                throw new ArgumentException(
                    $"Невозможно сравнить Goods с объектом типа {obj.GetType().Name}.");

            return Price.CompareTo(other.Price);
        }
    }
}
