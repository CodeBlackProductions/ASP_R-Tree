public abstract class Spatial
{
    public abstract Node Parent { get; set; }

    public abstract Rect Rect { get; set; }

    public abstract int EntryCount { get; }

    public abstract int NodeCapacity { get; }
}