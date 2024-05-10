using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Leaf : Spatial
{
    private Spatial m_Parent;
    private Rect m_Rect;
    private LeafData[] m_Data;

    public Spatial Parent { get => m_Parent; set => m_Parent = value; }
    public LeafData[] Data { get => m_Data; set => m_Data = value; }
    public override Rect Rect { get => m_Rect; set => m_Rect = value; }
    public int EntryCount { get => m_Data.Length; }

    public Leaf(Spatial _Parent, Rect _Rect, LeafData[] _Data)
    {
        m_Parent = _Parent;
        m_Rect = _Rect;
        m_Data = _Data;
    }
}