using System.Text.Json.Serialization;

namespace Items{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(Product), "Product")]
    [JsonDerivedType(typeof(Party), "Party")]
    [JsonDerivedType(typeof(Complect), "Complect")]
    abstract class Goods : IComparable<Goods>{
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public abstract void Show();
        public abstract bool IsShelfLife();

        public int CompareTo(Goods? other)
        {
            if (other == null) return 1;
            return this.Price.CompareTo(other.Price);
        }
    }
}
