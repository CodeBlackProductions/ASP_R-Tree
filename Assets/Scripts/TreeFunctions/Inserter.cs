using System;
using System.Numerics;

public class Inserter
{
    public static void InsertData(Node _Root, int _ObjIDX, float _PosX, float _PosY, float _PosZ)
    {
        LeafData insertData = new LeafData(_ObjIDX, _PosX, _PosY, _PosZ);

        Node targetNode = ChooseTargetNode(_Root, new Vector3(_PosX, _PosY, _PosZ));

        if (targetNode.Entry is Leaf leaf)
        {
            InsertIntoLeaf(targetNode, leaf, insertData);
        }
        else 
        {
            throw new InvalidOperationException("Invalid node while inserting into tree: " + targetNode);
        }

        TreeBalancer.RebalanceTree(_Root);
    }

    private static void InsertIntoLeaf(Node _LeafNode, Leaf _Leaf, LeafData _InsertData)
    {
        LeafData[] oldData = _Leaf.Data;
        LeafData[] newData = new LeafData[oldData.Length + 1];

        Array.Copy(oldData, newData, oldData.Length);
        newData[oldData.Length] = _InsertData;

        _Leaf.Data = newData;

        Rect rect = UpdateRect(_LeafNode, _InsertData);
        _Leaf.Rect = rect;

        if (_LeafNode.IsOverflowing())
        {
            NodeSplitter.SplitNode(_LeafNode);
        }
    }

    private static Rect UpdateRect(Node _LeafNode, LeafData _InsertData)
    {
        float x;
        float y;
        float z;

        x = Math.Min(_InsertData.PosX, _LeafNode.Entry.Rect.LowerLeft.X);
        y = Math.Min(_InsertData.PosY, _LeafNode.Entry.Rect.LowerLeft.Y);
        z = Math.Min(_InsertData.PosZ, _LeafNode.Entry.Rect.LowerLeft.Z);

        Vector3 lowerLeft = new Vector3(x, y, z);

        x = Math.Max(_InsertData.PosX, _LeafNode.Entry.Rect.UpperRight.X);
        y = Math.Max(_InsertData.PosY, _LeafNode.Entry.Rect.UpperRight.Y);
        z = Math.Max(_InsertData.PosZ, _LeafNode.Entry.Rect.UpperRight.Z);

        Vector3 upperRight = new Vector3(x, y, z);

        return new Rect(lowerLeft, upperRight);
    }

    private static Node ChooseTargetNode(Node _Root, Vector3 _ObjPos)
    {
        return TreeScanner.SearchLeaf(_Root, new Rect(_ObjPos, _ObjPos)).Parent;
    }
}