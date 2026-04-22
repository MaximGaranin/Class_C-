namespace Items{
    abstract class Goods : IComparable<Goods>{
    public string Name { get; protected set; }
    public decimal Price { get; protected set; }

    public abstract void Show();
    public abstract bool IsShelfLife();

    public int CompareTo(Goods other)
    {
        if (other == null) return 1;
        return this.Price.CompareTo(other.Price);
    }
}
}
