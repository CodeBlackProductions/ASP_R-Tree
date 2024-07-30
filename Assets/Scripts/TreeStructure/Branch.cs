using System;
using System.Numerics;

public class Branch : Spatial
{
    private Node m_EncapsulatingNode;
    private Rect m_Rect;
    private Node[] m_Children;
    private int m_NodeCapacity;
    private int m_MinNodeCapacity;

    public Node[] Children { get => m_Children; set => m_Children = value; }
    public override Node EncapsulatingNode { get => m_EncapsulatingNode; set => m_EncapsulatingNode = value; }
    public override Rect Rect { get => m_Rect; set => m_Rect = value; }
    public override int EntryCount { get => m_Children.Length; }
    public override int NodeCapacity { get => m_NodeCapacity; }
    public override int MinNodeCapacity { get => m_MinNodeCapacity; }

    public Branch(Node _Parent, Rect _Rect, Node[] _Children, int _MaxCapacity, int _MinCapacity)
    {
        m_EncapsulatingNode = _Parent;
        m_Rect = _Rect;
        m_Children = _Children;
        m_NodeCapacity = _MaxCapacity;
        m_MinNodeCapacity = _MinCapacity;
    }

    public override void UpdateRect()
    {
        if (this.Children.Length == 0 || this.Children[0] == null)
        {
            Vector3 val = this.Rect.GetCenter();
            this.Rect = new Rect(val, val);
            return;
        }

        float x = this.Children[0].Entry.Rect.LowerLeft.X;
        float y = this.Children[0].Entry.Rect.LowerLeft.Y;
        float z = this.Children[0].Entry.Rect.LowerLeft.Z;

        Vector3 lowerLeft = new Vector3(x, y, z);

        x = this.Children[0].Entry.Rect.UpperRight.X;
        y = this.Children[0].Entry.Rect.UpperRight.Y;
        z = this.Children[0].Entry.Rect.UpperRight.Z;

        Vector3 upperRight = new Vector3(x, y, z);

        for (int i = 0; i < this.Children.Length; i++)
        {
            lowerLeft.X = Math.Min(lowerLeft.X, this.Children[i].Entry.Rect.LowerLeft.X);
            lowerLeft.Y = Math.Min(lowerLeft.Y, this.Children[i].Entry.Rect.LowerLeft.Y);
            lowerLeft.Z = Math.Min(lowerLeft.Z, this.Children[i].Entry.Rect.LowerLeft.Z);

            upperRight.X = Math.Max(upperRight.X, this.Children[i].Entry.Rect.UpperRight.X);
            upperRight.Y = Math.Max(upperRight.Y, this.Children[i].Entry.Rect.UpperRight.Y);
            upperRight.Z = Math.Max(upperRight.Z, this.Children[i].Entry.Rect.UpperRight.Z);
        }

        this.Rect = new Rect(lowerLeft, upperRight);
    }
}