using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Leaf : Spatial
{
    private Node m_Parent;
    private Rect m_Rect;
    private LeafData[] m_Data;
    private int m_NodeCapacity;

    public LeafData[] Data { get => m_Data; set => m_Data = value; }
    public override Node Parent { get => m_Parent; set => m_Parent = value; }
    public override Rect Rect { get => m_Rect; set => m_Rect = value; }
    public override int EntryCount { get => m_Data.Length; }
    public override int NodeCapacity { get => m_NodeCapacity; }

    public Leaf(Node _Parent, Rect _Rect, LeafData[] _Data, int _MaxCapacity)
    {
        m_Parent = _Parent;
        m_Rect = _Rect;
        m_Data = _Data;
        m_NodeCapacity = _MaxCapacity;
    }
}