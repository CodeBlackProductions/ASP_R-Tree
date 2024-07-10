public class Branch : Spatial
{
    private Node m_EncapsulatingNode;
    private Rect m_Rect;
    private Node[] m_Children;
    private int m_NodeCapacity;

    public Node[] Children { get => m_Children; set => m_Children = value; }
    public override Node EncapsulatingNode { get => m_EncapsulatingNode; set => m_EncapsulatingNode = value; }
    public override Rect Rect { get => m_Rect; set => m_Rect = value; }
    public override int EntryCount { get => m_Children.Length; }
    public override int NodeCapacity { get => m_NodeCapacity; }

    public Branch(Node _Parent, Rect _Rect, Node[] _Children, int _MaxCapacity)
    {
        m_EncapsulatingNode = _Parent;
        m_Rect = _Rect;
        m_Children = _Children;
        m_NodeCapacity = _MaxCapacity;
    }
}