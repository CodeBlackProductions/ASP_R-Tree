using System;
using System.Numerics;

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
            RemoveNode(leaf.Parent);
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

    private static void RemoveNode(Node _NodeToRemove)
    {
        throw new NotImplementedException();
    }
}