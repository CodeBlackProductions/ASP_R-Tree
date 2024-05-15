using System.Collections.Generic;
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

    private Dictionary<int, GameObject> m_GameObjects;

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