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
    }

    private static void InsertIntoLeaf(Node _LeafNode, Leaf _Leaf, LeafData _InsertData)
    {
        LeafData[] oldData = _Leaf.Data;
        LeafData[] newData = new LeafData[oldData.Length + 1];

        Array.Copy(oldData, newData, oldData.Length);
        newData[oldData.Length] = _InsertData;

        _Leaf.Data = newData;

        _Leaf.UpdateRect();

        if (_LeafNode.IsOverflowing())
        {
            NodeSplitter.SplitNode(_LeafNode);
        }
    }

    private static Node ChooseTargetNode(Node _Root, Vector3 _ObjPos)
    {
        return TreeScanner.SearchLeaf(_Root, new Rect(_ObjPos, _ObjPos)).EncapsulatingNode;
    }
}