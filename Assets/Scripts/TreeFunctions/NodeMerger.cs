using System;
using System.Numerics;

public class NodeMerger
{
    public static void RebalanceNodes(Node _TargetNode)
    {
        if (_TargetNode == null)
        {
            throw new ArgumentNullException("Node should not be Null when Rebalancing");
        }

        Branch targetParent = _TargetNode.Parent.Entry as Branch;

        if (_TargetNode.Entry is Leaf leaf)
        {

            if (targetParent.EntryCount > 1)
            {
                Leaf redistributeLeaf = null;
                Leaf mergeLeaf = null;
                int amountToRedistribute = leaf.MinNodeCapacity - leaf.EntryCount;

                for (int i = 0; i < targetParent.EntryCount; i++)
                {
                    if (targetParent.Children[i] == _TargetNode)
                    {
                        continue;
                    }

                    Leaf sibling = targetParent.Children[i].Entry as Leaf;

                    if ((sibling.EntryCount - amountToRedistribute) >= leaf.MinNodeCapacity)
                    {
                        redistributeLeaf = sibling;
                        break;
                    }
                    else if ((sibling.EntryCount + leaf.EntryCount) <= leaf.NodeCapacity)
                    {
                        mergeLeaf = sibling;
                    }
                }

                if (redistributeLeaf != null)
                {
                    RedistributeEntries(leaf, redistributeLeaf, amountToRedistribute);
                }
                else
                {
                    MergeNode(leaf, mergeLeaf);
                }
            }
        }
        else if (_TargetNode.Entry is Branch branch)
        {
            if (targetParent.EntryCount > 1)
            {
                Branch redistributeBranch = null;
                Branch mergeBranch = null;
                int amountToRedistribute = branch.MinNodeCapacity - branch.EntryCount;

                for (int i = 0; i < targetParent.EntryCount; i++)
                {
                    if (targetParent.Children[i].ID == _TargetNode.ID)
                    {
                        continue;
                    }

                    Branch sibling = targetParent.Children[i].Entry as Branch;

                    if ((sibling.EntryCount - amountToRedistribute) >= branch.MinNodeCapacity)
                    {
                        redistributeBranch = sibling;
                        break;
                    }
                    else if ((sibling.EntryCount + branch.EntryCount) <= branch.NodeCapacity)
                    {
                        mergeBranch = sibling;
                    }
                }

                if (redistributeBranch != null)
                {
                    RedistributeEntries(branch, redistributeBranch, amountToRedistribute);
                }
                else
                {
                    MergeNode(branch, mergeBranch);
                }
            }
        }
    }

    private static void RedistributeEntries(Leaf _TargetLeaf, Leaf _PartnerLeaf, int _AmountToRedistribute)
    {
        for (int i = 0; i < _AmountToRedistribute; i++)
        {

        }
    }

    private static void RedistributeEntries(Branch _TargetBranch, Branch _PartnerBranch, int _AmountToRedistribute)
    {
    }

    private static void MergeNode(Leaf _TargetLeaf, Leaf _PartnerLeaf)
    {
    }

    private static void MergeNode(Branch _TargetBranch, Branch _PartnerBranch)
    {
    }
}