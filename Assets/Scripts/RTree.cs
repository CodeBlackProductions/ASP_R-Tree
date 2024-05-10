using UnityEngine;

public struct Rect
{
    public Vector3 lowerLeft;
    public Vector3 upperRight;
}

public class RTree
{
    private Node m_Root;
    private int m_Depth;
    private int m_NodeCapacity;

    public RTree(Node _Root, int _NodeCapacity)
    {
        m_Root = _Root;
        m_Depth = 0;
        m_NodeCapacity = _NodeCapacity;
    }

    #region External Access

    public void Insert(LeafData _Data)
    {
    }

    public void Remove(GameObject _Obj)
    {
    }

    public void BatchInsert(LeafData[] _Batch)
    {
    }

    public void BatchRemove(GameObject[] _Batch)
    {
    }

    public LeafData[] FindRange(Rect _Range)
    {
        return null;
    }

    #endregion External Access

    #region Internal Methods

    private Node[] ScanRange(Rect _Range)
    {
        return null;
    }

    #endregion Internal Methods
}