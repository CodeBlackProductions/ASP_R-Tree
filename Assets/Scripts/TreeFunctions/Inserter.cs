using System;

public class Inserter
{
    // return int to return the number of added nodes? maybe two values to return added levels and nodes?
    public static int InsertData(Node _Root, int _ObjIDX, float _PosX, float _PosY, float _PosZ)
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
        else if (targetNode.Entry is Branch branch)
        {
            InsertIntoBranch(targetNode, branch, insertData);
        }
        else
        {
            throw new InvalidOperationException("Invalid node while inserting into tree: " + targetNode);
        }

        RebalanceTree(_Root);

        return ???;
    }

    private static int InsertIntoLeaf(Node _LeafNode, Leaf _Leaf, LeafData _InsertData)
    {
        LeafData[] oldData = _Leaf.Data;
        LeafData[] newData = new LeafData[oldData.Length + 1];

        Array.Copy(oldData, newData, oldData.Length);
        newData[oldData.Length] = _InsertData;

        _Leaf.Data = newData;

        if (_LeafNode.IsOverflowing())
        {
            SplitNode(_LeafNode);
        }

        return ???;
    }

    private static int InsertIntoBranch(Node _BranchNode, Branch _Branch, LeafData _InsertData)
    {
        Rect rect = new Rect(LL, UR);
        LeafData[] leafArray = new LeafData[] { _InsertData };
        Leaf newLeaf = new Leaf(_BranchNode, rect, leafArray, _BranchNode.Entry.NodeCapacity);
        Node newNode = new Node(0, newLeaf);

        Node[] oldChildren = _Branch.Children;
        Node[] newChildren = new Node[oldChildren.Length + 1];

        Array.Copy(oldChildren, newChildren, oldChildren.Length);
        newChildren[oldChildren.Length] = newNode;

        _Branch.Children = newChildren;

        if (_BranchNode.IsOverflowing())
        {
            SplitNode(_BranchNode);
        }

        return ???;
    }

    private static void RebalanceTree(Node _Root)
    {
        throw new NotImplementedException();
    }

    private static void SplitNode(Node _NodeToSplit)
    {
        throw new NotImplementedException();
    }

    private static Node ChooseTargetNode(Node _Root)
    {
        throw new NotImplementedException();
    }
}