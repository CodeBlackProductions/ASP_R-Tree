using System;
using UnityEngine;

public class Inserter
{
    public static void InsertData(Node _Root, int _ObjIDX, float _PosX, float _PosY, float _PosZ)
    {
        LeafData insertData = new LeafData(_ObjIDX, _PosX, _PosY, _PosZ);

        Node targetNode = ChooseTargetNode(_Root);

        if (targetNode.Entry == null)
        {
            throw new InvalidOperationException("Invalid node while inserting into tree: " + targetNode);
        }

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

        x = _InsertData.PosX < _LeafNode.Entry.Rect.LowerLeft.x ? _InsertData.PosX : _LeafNode.Entry.Rect.LowerLeft.x;
        y = _InsertData.PosY < _LeafNode.Entry.Rect.LowerLeft.y ? _InsertData.PosY : _LeafNode.Entry.Rect.LowerLeft.y;
        z = _InsertData.PosZ < _LeafNode.Entry.Rect.LowerLeft.z ? _InsertData.PosZ : _LeafNode.Entry.Rect.LowerLeft.z;
        Vector3 lowerLeft = new Vector3(x, y, z);
        x = _InsertData.PosX > _LeafNode.Entry.Rect.UpperRight.x ? _InsertData.PosX : _LeafNode.Entry.Rect.UpperRight.x;
        y = _InsertData.PosY > _LeafNode.Entry.Rect.UpperRight.y ? _InsertData.PosY : _LeafNode.Entry.Rect.UpperRight.y;
        z = _InsertData.PosZ > _LeafNode.Entry.Rect.UpperRight.z ? _InsertData.PosZ : _LeafNode.Entry.Rect.UpperRight.z;
        Vector3 upperRight = new Vector3(x, y, z);

        return new Rect(lowerLeft, upperRight);
    }

    private static Node ChooseTargetNode(Node _Root)
    {
        throw new NotImplementedException();
    }
}