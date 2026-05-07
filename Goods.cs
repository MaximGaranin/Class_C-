using System.Text.Json.Serialization;

namespace Items{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(Product), "Product")]
    [JsonDerivedType(typeof(Party), "Party")]
    [JsonDerivedType(typeof(Complect), "Complect")]
    abstract class Goods : IComparable<Goods>{
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        
        public Goods(string Name, decimal Price){
            Name = name;
            Price = price;
        }

        public abstract override string ToString();
        public abstract bool IsShelfLife();

        public int CompareTo(Goods? other)
        {
            if (other == null || !(other is Goods)) return 1;
            return Price.CompareTo(other.Price);
        }
    }
}
