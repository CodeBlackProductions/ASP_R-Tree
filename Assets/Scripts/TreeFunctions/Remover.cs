using System;
using System.Numerics;
using System.Reflection;

public class Remover
{
    public static void Remove(Node _Root, int _Index, Vector3 _Pos)
    {
        Rect range = new Rect(_Pos, _Pos);

        Leaf leaf = TreeScanner.SearchLeaf(_Root, range);

        LeafData[] newData = new LeafData[leaf.Data.Length - 1];

        int targetIndex = -1;
        for (int i = 0; i < leaf.Data.Length; i++)
        {
            if (leaf.Data[i].ObjIDX != _Index)
            {
                continue;
            }
            targetIndex = i;
            break;
        }

        if (targetIndex == -1)
        {
            return;
        }

        if (targetIndex > 0)
        {
            Array.Copy(leaf.Data, 0, newData, 0, targetIndex);
            Array.Copy(leaf.Data, targetIndex + 1, newData, targetIndex, leaf.Data.Length - (targetIndex + 1));
        }
        else
        {
            Array.Copy(leaf.Data, 1, newData, 0, newData.Length);
        }

        leaf.Data = newData;

        if (leaf.EntryCount <= 0)
        {
            RemoveNode(leaf.EncapsulatingNode);
        }
        else
        {
            Rect rect = UpdateRect(leaf);
            leaf.Rect = rect;
        }

        //TreeBalancer.RebalanceTree(_Root);
    }

    private static Rect UpdateRect(Leaf _Leaf)
    {
        float x = _Leaf.Data[0].PosX;
        float y = _Leaf.Data[0].PosY;
        float z = _Leaf.Data[0].PosZ;

        Vector3 lowerLeft = new Vector3(x, y, z);
        Vector3 upperRight = new Vector3(x, y, z);

        for (int i = 0; i < _Leaf.Data.Length; i++)
        {
            lowerLeft.X = Math.Min(lowerLeft.X, _Leaf.Data[i].PosX);
            lowerLeft.Y = Math.Min(lowerLeft.Y, _Leaf.Data[i].PosY);
            lowerLeft.Z = Math.Min(lowerLeft.Z, _Leaf.Data[i].PosZ);

            upperRight.X = Math.Max(upperRight.X, _Leaf.Data[i].PosX);
            upperRight.Y = Math.Max(upperRight.Y, _Leaf.Data[i].PosY);
            upperRight.Z = Math.Max(upperRight.Z, _Leaf.Data[i].PosZ);
        }

        return new Rect(lowerLeft, upperRight);
    }

    private static Rect UpdateRect(Branch _Branch)
    {
        float x = _Branch.Children[0].Entry.Rect.LowerLeft.X;
        float y = _Branch.Children[0].Entry.Rect.LowerLeft.Y;
        float z = _Branch.Children[0].Entry.Rect.LowerLeft.Z;

        Vector3 lowerLeft = new Vector3(x, y, z);

        x = _Branch.Children[0].Entry.Rect.UpperRight.X;
        y = _Branch.Children[0].Entry.Rect.UpperRight.Y;
        z = _Branch.Children[0].Entry.Rect.UpperRight.Z;

        Vector3 upperRight = new Vector3(x, y, z);

        for (int i = 0; i < _Branch.Children.Length; i++)
        {
            lowerLeft.X = Math.Min(lowerLeft.X,  _Branch.Children[i].Entry.Rect.LowerLeft.X);
            lowerLeft.Y = Math.Min(lowerLeft.Y,  _Branch.Children[i].Entry.Rect.LowerLeft.Y);
            lowerLeft.Z = Math.Min(lowerLeft.Z, _Branch.Children[i].Entry.Rect.LowerLeft.Z);

            upperRight.X = Math.Max(upperRight.X,  _Branch.Children[i].Entry.Rect.UpperRight.X);
            upperRight.Y = Math.Max(upperRight.Y,  _Branch.Children[i].Entry.Rect.UpperRight.Y);
            upperRight.Z = Math.Max(upperRight.Z, _Branch.Children[i].Entry.Rect.UpperRight.Z);
        }

        return new Rect(lowerLeft, upperRight);
    }

    private static void RemoveNode(Node _NodeToRemove)
    {

        Guid guid = _NodeToRemove.ID;

        Branch parent = (_NodeToRemove.Parent.Entry as Branch);

        Node[] newData = new Node[parent.Children.Length - 1];

        int targetIndex = -1;
        for (int i = 0; i < parent.Children.Length; i++)
        {
            if (parent.Children[i].ID != guid)
            {
                continue;
            }
            targetIndex = i;
            break;
        }

        if (targetIndex == -1)
        {
            return;
        }

        if (targetIndex > 0)
        {
            Array.Copy(parent.Children, 0, newData, 0, targetIndex);
            Array.Copy(parent.Children, targetIndex + 1, newData, targetIndex, parent.Children.Length - (targetIndex + 1));
        }
        else
        {
            Array.Copy(parent.Children, 1, newData, 0, newData.Length);
        }

        parent.Children = newData;

        if (parent.EntryCount <= 0)
        {
            RemoveNode(parent.EncapsulatingNode);
        }
        else
        {
            Rect rect = UpdateRect(parent);
            parent.Rect = rect;
        }
    }
}