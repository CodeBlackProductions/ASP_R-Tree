using System;
using System.Collections.Generic;
using System.Numerics;

public class RTree
{
    private Node m_Root;
    private int m_NodeCapacity;

    private List<UnityEngine.GameObject> m_GameObjects;

    public Node Root { get => m_Root; set => m_Root = value; }

    public RTree(Node _Root, int _NodeCapacity)
    {
        m_Root = _Root;
        m_NodeCapacity = _NodeCapacity;
        m_GameObjects = new List<UnityEngine.GameObject>();
    }

    #region External Access

    public void Insert(UnityEngine.GameObject _Data)
    {
        m_GameObjects.Add(_Data);
        Vector3 pos = new Vector3();
        pos.X = _Data.transform.position.x;
        pos.Y = _Data.transform.position.y;
        pos.Z = _Data.transform.position.z;

        if (m_Root.Entry == null)
        {
            Vector3 lowerLeft = new Vector3(pos.X - 10, pos.Y, pos.Z - 10);
            Vector3 upperRight = new Vector3(pos.X + 10, pos.Y, pos.Z + 10);

            Rect rect = new Rect(lowerLeft, upperRight);
            LeafData[] leafData = new LeafData[] { new LeafData(m_GameObjects.Count, pos.X, pos.Y, pos.Z) };
            m_Root.Entry = new Leaf(m_Root, rect, leafData, m_NodeCapacity);
            return;
        }

        Inserter.InsertData(m_Root, m_GameObjects.Count, pos.X, pos.Y, pos.Z);
    }

    public void Remove(UnityEngine.GameObject _Obj)
    {
    }

    public void BatchInsert(LeafData[] _Batch)
    {
    }

    public void BatchRemove(UnityEngine.GameObject[] _Batch)
    {
    }

    public LeafData[] FindRange(Rect _Range)
    {
        //searchData = ScanRange(_Range, m_Root);
        LeafData[] searchData = TreeScanner.SearchLeafData(m_Root, _Range);

        return searchData;
    }

    #endregion External Access
}