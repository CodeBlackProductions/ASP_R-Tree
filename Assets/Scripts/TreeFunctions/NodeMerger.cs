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
        LeafData[] newTargetData = new LeafData[_TargetLeaf.EntryCount + _AmountToRedistribute];
        LeafData[] newPartnerData = new LeafData[_PartnerLeaf.EntryCount - _AmountToRedistribute];

        Array.Sort(_PartnerLeaf.Data, (a, b) => CompareProximity(_TargetLeaf.Rect.GetCenter(), a, b));

        Array.Copy(_TargetLeaf.Data, 0, newTargetData, 0, _TargetLeaf.EntryCount);
        Array.Copy(_PartnerLeaf.Data, 0, newTargetData, _TargetLeaf.EntryCount, _AmountToRedistribute);

        _TargetLeaf.Data = newTargetData;
        _TargetLeaf.UpdateRect();

        Array.Copy(_PartnerLeaf.Data, _AmountToRedistribute, newPartnerData, 0, _PartnerLeaf.EntryCount - _AmountToRedistribute);

        _PartnerLeaf.Data = newPartnerData;
        _PartnerLeaf.UpdateRect();
    }

    private static void RedistributeEntries(Branch _TargetBranch, Branch _PartnerBranch, int _AmountToRedistribute)
    {
        Node[] newTargetData = new Node[_TargetBranch.EntryCount + _AmountToRedistribute];
        Node[] newPartnerData = new Node[_PartnerBranch.EntryCount - _AmountToRedistribute];

        Array.Sort(_PartnerBranch.Children, (a, b) => CompareProximity(_TargetBranch.Rect.GetCenter(), a, b));

        Array.Copy(_TargetBranch.Children, 0, newTargetData, 0, _TargetBranch.EntryCount);
        Array.Copy(_PartnerBranch.Children, 0, newTargetData, _TargetBranch.EntryCount, _AmountToRedistribute);

        _TargetBranch.Children = newTargetData;
        _TargetBranch.UpdateRect();

        Array.Copy(_PartnerBranch.Children, _AmountToRedistribute, newPartnerData, 0, _PartnerBranch.EntryCount - _AmountToRedistribute);

        _PartnerBranch.Children = newPartnerData;
        _PartnerBranch.UpdateRect();
    }

    private static void MergeNode(Leaf _TargetLeaf, Leaf _PartnerLeaf)
    {
    }

    private static void MergeNode(Branch _TargetBranch, Branch _PartnerBranch)
    {
    }

    private static int CompareProximity(Vector3 _TargetCenter, LeafData _A, LeafData _B)
    {
        float distanceA = Vector3.DistanceSquared(_TargetCenter, new Vector3(_A.PosX, _A.PosY, _A.PosZ));
        float distanceB = Vector3.DistanceSquared(_TargetCenter, new Vector3(_B.PosX, _B.PosY, _B.PosZ));

        if (distanceA < distanceB)
        {
            return -1;
        }
        else if (distanceA > distanceB)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private static int CompareProximity(Vector3 _TargetCenter, Node _A, Node _B)
    {
        float distanceA = Vector3.DistanceSquared(_TargetCenter, _A.Entry.Rect.GetCenter());
        float distanceB = Vector3.DistanceSquared(_TargetCenter, _B.Entry.Rect.GetCenter());

        if (distanceA < distanceB)
        {
            return -1;
        }
        else if (distanceA > distanceB)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}