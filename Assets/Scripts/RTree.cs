using System;
using System.Collections.Generic;
using System.Numerics;

public class RTree
{
    private Node m_Root;
    private int m_Depth;
    private int m_NodeCapacity;

    private List<UnityEngine.GameObject> m_GameObjects;

    public RTree(Node _Root, int _NodeCapacity)
    {
        m_Root = _Root;
        m_Depth = 0;
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

        //return added values?
        Inserter.InsertData(m_Root, m_GameObjects.Count, pos.X, pos.Y, pos.Z);

        if (m_Root.IsOverflowing())
        {
            SplitRoot(pos);
        }
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

    #region Internal Methods

    private void SplitRoot(Vector3 _InsertPos)
    {
        float x;
        float y;
        float z;

        x = Math.Min(_InsertPos.X, m_Root.Entry.Rect.LowerLeft.X);
        y = Math.Min(_InsertPos.Y, m_Root.Entry.Rect.LowerLeft.Y);
        z = Math.Min(_InsertPos.Z, m_Root.Entry.Rect.LowerLeft.Z);

        Vector3 lowerLeft = new Vector3(x, y, z);

        x = Math.Max(_InsertPos.X, m_Root.Entry.Rect.UpperRight.X);
        y = Math.Max(_InsertPos.Y, m_Root.Entry.Rect.UpperRight.Y);
        z = Math.Max(_InsertPos.Z, m_Root.Entry.Rect.UpperRight.Z);

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