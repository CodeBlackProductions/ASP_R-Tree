using System;

public struct Node
{
    private Spatial m_Entry;
    private int m_Level;
    private Guid m_ID;

    public Spatial Entry { get => m_Entry; set => m_Entry = value; }
    public int Level { get => m_Level; set => m_Level = value; }
    public Guid ID { get => m_ID; }

    public Node(int _Level, Spatial _Entry)
    {
        m_Level = _Level;
        m_Entry = _Entry;
        m_ID = Guid.NewGuid();
    }

    public bool IsOverflowing()
    {
        if (m_Entry.EntryCount > m_Entry.NodeCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}