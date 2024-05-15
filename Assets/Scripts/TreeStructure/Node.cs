using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Node
{
    Spatial m_Entry;
    int m_Level;

    public Spatial Entry { get => m_Entry; set => m_Entry = value; }
    public int Level { get => m_Level; set => m_Level = value; }

    public Node(int _Level, Spatial _Entry) 
    {
        m_Level = _Level;
        m_Entry = _Entry;
    }
}
