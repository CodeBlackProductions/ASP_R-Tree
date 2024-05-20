using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
    }

    #region External Access

    public void Insert(GameObject _Data)
    {
        m_GameObjects.Add(_Data);
        Vector3 pos = _Data.transform.position;

        if (m_Root.Entry == null)
        {
            Rect rect = new Rect(LL,UR);
            LeafData[] leafData = new LeafData[] {new LeafData(m_GameObjects.Count, pos.x, pos.y, pos.z)};
            m_Root.Entry = new Leaf(m_Root, rect, leafData, m_NodeCapacity) ;
            return;
        }

        //return added values?
        Inserter.InsertData(m_GameObjects.Count, pos.x, pos.y, pos.z);

        if (m_Root.IsOverflowing())
        {
            Rect rect = new Rect(LL, UR);
            Node newRoot = new Node(m_Depth + 1, new Branch(new Node(), rect, new Node[] {m_Root}, m_NodeCapacity));
            SplitNode(newRoot);
            m_Root = newRoot;
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