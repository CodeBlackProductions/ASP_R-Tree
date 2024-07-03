using System.Collections.Generic;
using System.Linq;

public class LeafDataSearch
{
    public void StartSearch(Node _Root, Rect _Range, out LeafData[] _Result)
    {
        IEnumerable<Node> nodes;
        if (_Root.Entry is Branch)
        {
            nodes = ((Branch)_Root.Entry).Children;
        }
        else
        {
            if (!TreeScanner.Intersects(_Root.Entry.Rect, _Range))
            {
                _Result = new LeafData[0];
                return;
            }
            else
            {
                _Result = ((Leaf)_Root.Entry).Data;
                return;
            }
        }

        List<List<LeafData>> results = nodes.AsParallel()
            .Where(node => TreeScanner.Intersects(node.Entry.Rect, _Range))
            .Select(node => ScanRange(_Range, node))
            .ToList();

        IEnumerable<LeafData> combinedResults = results.SelectMany(list => list);

        _Result = combinedResults.ToArray();
    }

    private List<LeafData> ScanRange(Rect _Range, Node _Start)
    {
        List<LeafData> resultData = new List<LeafData>();

        if (_Start.Entry is Branch)
        {
            Branch branch = (Branch)_Start.Entry;
            foreach (Node child in branch.Children)
            {
                if (TreeScanner.Intersects(child.Entry.Rect, _Range))
                {
                    if (child.Entry is Branch)
                    {
                        LeafData[] result;
                        StartSearch(child, _Range, out result);
                        resultData.AddRange(result);
                    }
                    else if (child.Entry is Leaf)
                    {
                        resultData.AddRange(((Leaf)child.Entry).Data);
                    }
                }
            }
        }
        else if (_Start.Entry is Leaf)
        {
            resultData.AddRange(((Leaf)_Start.Entry).Data);
        }

        return resultData;
    }
}