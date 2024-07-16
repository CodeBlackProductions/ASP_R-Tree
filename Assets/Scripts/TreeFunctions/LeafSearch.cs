using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public class LeafSearch
{
    public void StartSearch(Node _Root, Rect _Range, out Leaf _Result)
    {
        IEnumerable<Node> nodes;

        if (_Root.Entry is Branch branch)
        {
            nodes = branch.Children;
        }
        else
        {
            _Result = (Leaf)_Root.Entry;
            return;
        }

        List<Leaf> intersectingLeaves = new List<Leaf>();
        List<Leaf> nonIntersectingLeaves = new List<Leaf>();

        nodes.AsParallel().ForAll(node =>
        {
            if (TreeScanner.Intersects(node.Entry.Rect, _Range))
            {
                List<Leaf> leaves = ScanRange(_Range, node, true);
                lock (intersectingLeaves)
                {
                    intersectingLeaves.AddRange(leaves);
                }
            }
            else
            {
                List<Leaf> leaves = ScanRange(_Range, node, false);
                lock (nonIntersectingLeaves)
                {
                    nonIntersectingLeaves.AddRange(leaves);
                }
            }
        });

        Vector3 rangeCenter = _Range.GetCenter();

        if (intersectingLeaves.Count > 0)
        {
            _Result = intersectingLeaves.OrderBy(leaf =>
            Vector3.DistanceSquared(rangeCenter, leaf.Rect.GetCenter())).First();
        }
        else
        {
            _Result = nonIntersectingLeaves.OrderBy(leaf =>
             Vector3.DistanceSquared(rangeCenter, leaf.Rect.GetCenter())).First();
        }
    }

    private List<Leaf> ScanRange(Rect _Range, Node _Start, bool _Intersecting)
    {
        List<Leaf> resultData = new List<Leaf>();

        if (_Start.Entry is Branch branch)
        {
            Leaf result;
            foreach (Node child in branch.Children)
            {
                if (_Intersecting)
                {
                    if (TreeScanner.Intersects(child.Entry.Rect, _Range))
                    {
                        if (child.Entry is Branch)
                        {
                            StartSearch(child, _Range, out result);
                            resultData.Add(result);
                        }
                        else if (child.Entry is Leaf leaf)
                        {
                            resultData.Add(leaf);
                        }
                    }
                }
                else
                {
                    if (child.Entry is Branch)
                    {
                        StartSearch(child, _Range, out result);
                        resultData.Add(result);
                    }
                    else if (child.Entry is Leaf leaf)
                    {
                        resultData.Add(leaf);
                    }
                }
            }
        }
        else if (_Start.Entry is Leaf leaf)
        {
            resultData.Add(leaf);
        }

        return resultData;
    }
}