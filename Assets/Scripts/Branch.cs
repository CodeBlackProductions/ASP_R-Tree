public class Branch : Spatial
{
    private Spatial m_Parent;
    private Rect m_Rect;
    private Spatial[] m_Children;

    public Spatial Parent { get => m_Parent; set => m_Parent = value; }
    public Spatial[] Children { get => m_Children; set => m_Children = value; }
    public override Rect Rect { get => m_Rect; set => m_Rect = value; }
    public int EntryCount { get => m_Children.Length;}

    public Branch(Spatial _Parent, Rect _Rect, Spatial[] _Children)
    {
        m_Parent = _Parent;
        m_Rect = _Rect;
        m_Children = _Children;
    }
}