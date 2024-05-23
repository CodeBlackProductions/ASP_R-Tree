using System.Collections.Generic;
using UnityEngine;

public class RTree
{
    private Node m_Root;
    private int m_Depth;
    private int m_NodeCapacity;

    private List<GameObject> m_GameObjects;

    public RTree(Node _Root, int _NodeCapacity)
    {
        m_Root = _Root;
        m_Depth = 0;
        m_NodeCapacity = _NodeCapacity;
        m_GameObjects = new List<GameObject>();
    }

    #region External Access

    public void Insert(GameObject _Data)
    {
        m_GameObjects.Add(_Data);
        Vector3 pos = _Data.transform.position;

        if (m_Root.Entry == null)
        {
            Vector3 lowerLeft = new Vector3(pos.x - 10, pos.y, pos.z - 10);
            Vector3 upperRight = new Vector3(pos.x + 10, pos.y, pos.z + 10);

            Rect rect = new Rect(lowerLeft, upperRight);
            LeafData[] leafData = new LeafData[] { new LeafData(m_GameObjects.Count, pos.x, pos.y, pos.z) };
            m_Root.Entry = new Leaf(m_Root, rect, leafData, m_NodeCapacity);
            return;
        }

        //return added values?
        Inserter.InsertData(m_Root, m_GameObjects.Count, pos.x, pos.y, pos.z);

        if (m_Root.IsOverflowing())
        {
            SplitRoot(pos);
        }
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
        LeafData[] searchData;

        //searchData = ScanRange(_Range, m_Root);
        ParallelSearch.StartSearch(m_Root, _Range, out searchData);

        return searchData;
    }

    #endregion External Access

    #region Internal Methods

    private void SplitRoot(Vector3 _InsertPos)
    {
        float x;
        float y;
        float z;

        x = _InsertPos.x < m_Root.Entry.Rect.LowerLeft.x ? _InsertPos.x : m_Root.Entry.Rect.LowerLeft.x;
        y = _InsertPos.y < m_Root.Entry.Rect.LowerLeft.y ? _InsertPos.y : m_Root.Entry.Rect.LowerLeft.y;
        z = _InsertPos.z < m_Root.Entry.Rect.LowerLeft.z ? _InsertPos.z : m_Root.Entry.Rect.LowerLeft.z;

        Vector3 lowerLeft = new Vector3(x, y, z);

        x = _InsertPos.x > m_Root.Entry.Rect.UpperRight.x ? _InsertPos.x : m_Root.Entry.Rect.UpperRight.x;
        y = _InsertPos.y > m_Root.Entry.Rect.UpperRight.y ? _InsertPos.y : m_Root.Entry.Rect.UpperRight.y;
        z = _InsertPos.z > m_Root.Entry.Rect.UpperRight.z ? _InsertPos.z : m_Root.Entry.Rect.UpperRight.z;

        Vector3 upperRight = new Vector3(x, y, z);

        Rect rect = new Rect(lowerLeft, upperRight);
        Node newRoot = new Node(m_Depth + 1, new Branch(new Node(), rect, new Node[] { m_Root }, m_NodeCapacity));
        NodeSplitter.SplitNode(m_Root);
        m_Root = newRoot;
    }

    //private LeafData[] ScanRange(Rect _Range, Node _Start)
    //{
    //    List<LeafData> resultData = new List<LeafData>();

    //    if (Intersects(_Start.Entry.Rect, _Range))
    //    {
    //        if (_Start.Entry is Branch)
    //        {
    //            Branch branch = (Branch)_Start.Entry;

    //            for (int i = 0; i < branch.Children.Length; i++)
    //            {
    //                if (Intersects(branch.Children[i].Entry.Rect, _Range))
    //                {
    //                    resultData.AddRange(ScanRange(branch.Children[i].Entry.Rect, branch.Children[i]));
    //                }
    //            }
    //        }
    //        else if (_Start.Entry is Leaf)
    //        {
    //            resultData.AddRange(((Leaf)_Start.Entry).Data);
    //        }

    //        return resultData.ToArray();
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

    //private bool Intersects(Rect A, Rect B)
    //{
    //    return !(A.upperRight.x < B.lowerLeft.x ||
    //             B.upperRight.x < A.lowerLeft.x ||
    //             A.upperRight.y < B.lowerLeft.y ||
    //             B.upperRight.y < A.lowerLeft.y);
    //}

    //Benchmark test: Parallel or not Parallel? whats faster?

    #endregion Internal Methods
}