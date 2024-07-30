using System;
using System.Collections.Generic;
using System.Numerics;

public class Remover
{
    public static void RemoveEntry(Node _Root, int _Index, Vector3 _Pos)
    {
        Rect range = new Rect(_Pos, _Pos);

        Leaf leaf = TreeScanner.SearchLeaf(_Root, _Index, range);

        if (leaf == null) 
        {
            throw new Exception("Leaf should not be Null when removing Entry!");
        }

        LeafData[] newData = new LeafData[leaf.Data.Length - 1];

        if (newData.Length > 1)
        {
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
                throw new Exception("TargetIndex could not be found when removing Entry");
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
        }

        leaf.Data = newData;

        if (leaf.EntryCount <= 0)
        {
            RemoveNode(leaf.EncapsulatingNode);
        }
        else
        {
            leaf.UpdateRect();
            leaf.EncapsulatingNode.Parent.Entry.UpdateRect();

            if (leaf.EncapsulatingNode.IsUnderflowing())
            {
                NodeRebalancer.RebalanceUnderflowNodes(leaf.EncapsulatingNode);
            }
        }
    }

    public static void RemoveNode(Node _NodeToRemove)
    {
        Stack<Node> removeStack = new Stack<Node>();
        removeStack.Push(_NodeToRemove);

        while (removeStack.Count > 0)
        {
            Node localNodeToRemove = removeStack.Pop();

            Guid guid = localNodeToRemove.ID;

            Branch parent = (localNodeToRemove.Parent.Entry as Branch);

            Node[] newData = new Node[parent.Children.Length - 1];

            if (newData.Length > 1)
            {
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
                    throw new Exception("TargetIndex could not be found when removing Node");
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
            }

            parent.Children = newData;

            if (parent.EntryCount <= 0)
            {
                removeStack.Push(parent.EncapsulatingNode);
            }
            else
            {
                parent.UpdateRect();
                parent.EncapsulatingNode.Parent.Entry.UpdateRect();

                if (parent.EncapsulatingNode.IsUnderflowing())
                {
                    NodeRebalancer.RebalanceUnderflowNodes(parent.EncapsulatingNode);
                }
            }
        }
    }
}