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
            leaf.UpdateRect();

            if (leaf.EncapsulatingNode.IsUnderflowing())
            {
                NodeMerger.RebalanceNodes(leaf.EncapsulatingNode);
            }
            
        }

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
            parent.UpdateRect();

            if (parent.EncapsulatingNode.IsUnderflowing())
            {
                NodeMerger.RebalanceNodes(parent.EncapsulatingNode);
            }
        }
    }
}