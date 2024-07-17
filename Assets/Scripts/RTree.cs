using System.Collections.Generic;
using System.Numerics;

public class RTree
{
    private Node m_Root;
    private int m_NodeCapacity;
    private int m_MinNodeCapacity;

    private int m_IndexCounter = 0;
    private Dictionary<int, UnityEngine.GameObject> m_GameObjects;
    private Dictionary<UnityEngine.GameObject, int> m_Indices;

    public Node Root { get => m_Root; set => m_Root = value; }

    public RTree(Node _Root, int _NodeCapacity, int nodeMinCapacity)
    {
        m_Root = _Root;
        m_NodeCapacity = _NodeCapacity;
        m_GameObjects = new Dictionary<int, UnityEngine.GameObject>();
        m_Indices = new Dictionary<UnityEngine.GameObject, int>();
        m_MinNodeCapacity = nodeMinCapacity;
    }

    #region External Access

    public void Insert(UnityEngine.GameObject _Obj)
    {
        m_GameObjects.Add(m_IndexCounter, _Obj);
        m_Indices.Add(_Obj, m_IndexCounter);

        Vector3 pos = new Vector3(_Obj.transform.position.x, _Obj.transform.position.y, _Obj.transform.position.z);

        if (m_Root.Entry == null)
        {
            Vector3 lowerLeft = new Vector3(pos.X - 10, pos.Y, pos.Z - 10);
            Vector3 upperRight = new Vector3(pos.X + 10, pos.Y, pos.Z + 10);

            Rect rect = new Rect(lowerLeft, upperRight);
            LeafData[] leafData = new LeafData[] { new LeafData(m_IndexCounter, pos.X, pos.Y, pos.Z) };
            m_Root.Entry = new Leaf(m_Root, rect, leafData, m_NodeCapacity, m_MinNodeCapacity);
            return;
        }

        Inserter.InsertData(m_Root, m_IndexCounter, pos.X, pos.Y, pos.Z);

        m_IndexCounter++;
    }

    public void Remove(UnityEngine.GameObject _Obj)
    {
        if (m_GameObjects.ContainsValue(_Obj))
        {
            int index = m_Indices[_Obj];

            Vector3 pos = new Vector3(_Obj.transform.position.x, _Obj.transform.position.y, _Obj.transform.position.z);

            Remover.RemoveEntry(m_Root, index, pos);

            m_GameObjects.Remove(m_Indices[_Obj]);
            m_Indices.Remove(_Obj);
        }
    }

    public void Remove(int _Idx)
    {
        if (m_GameObjects[_Idx])
        {
            Vector3 pos = new Vector3(m_GameObjects[_Idx].transform.position.x,
                                      m_GameObjects[_Idx].transform.position.y,
                                      m_GameObjects[_Idx].transform.position.z);

            Remover.RemoveEntry(m_Root, _Idx, pos);

            m_Indices.Remove(m_GameObjects[_Idx]);
            m_GameObjects.Remove(_Idx);
        }
    }

    public void BatchInsert(LeafData[] _Batch)
    {
    }

    public void BatchRemove(UnityEngine.GameObject[] _Batch)
    {
    }

    public LeafData[] FindRange(Rect _Range)
    {
        LeafData[] searchData = TreeScanner.SearchLeafData(m_Root, _Range);

        return searchData;
    }

    #endregion External Access
}