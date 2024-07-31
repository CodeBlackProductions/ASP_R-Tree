using System;

public static class TreeScanner
{
    private static LeafDataSearch m_DataSearcher = new LeafDataSearch();
    private static LeafSearch m_LeafSearcher = new LeafSearch();

    public static LeafData[] SearchLeafData(Node _Root, Rect _Range)
    {
        if (_Root.Entry == null)
        {
            throw new InvalidOperationException("Empty Node while scanning R-Tree: " + _Root);
        }
        if (_Range.LowerLeft.X > _Range.UpperRight.X || _Range.LowerLeft.Y > _Range.UpperRight.Y)
        {
            throw new InvalidOperationException("Invalid Range for scanning R-Tree: " + _Range.LowerLeft.X +
                                                " | " + _Range.LowerLeft.Y + " - " + _Range.UpperRight.X +
                                                " | " + _Range.UpperRight.Y);
        }

        LeafData[] result;
        m_DataSearcher.StartSearch(_Root, _Range, out result);
        return result;
    }

    public static Leaf SearchLeaf(Node _Root, Rect _Range)
    {
        if (_Root.Entry == null)
        {
            throw new InvalidOperationException("Empty Node while scanning R-Tree: " + _Root);
        }
        if (_Range.LowerLeft.X > _Range.UpperRight.X || _Range.LowerLeft.Y > _Range.UpperRight.Y)
        {
            throw new InvalidOperationException("Invalid Range for scanning R-Tree: " + _Range.LowerLeft.X +
                                                " | " + _Range.LowerLeft.Y + " - " + _Range.UpperRight.X +
                                                " | " + _Range.UpperRight.Y);
        }

        Leaf result;
        m_LeafSearcher.StartSearch(_Root, _Range, out result);
        return result;
    }

    public static Leaf SearchLeaf(Node _Root, int _EntryIndex, Rect _Range)
    {
        if (_Root.Entry == null)
        {
            throw new InvalidOperationException("Empty Node while scanning R-Tree: " + _Root);
        }
        if (_Range.LowerLeft.X > _Range.UpperRight.X || _Range.LowerLeft.Y > _Range.UpperRight.Y)
        {
            throw new InvalidOperationException("Invalid Range for scanning R-Tree: " + _Range.LowerLeft.X +
                                                " | " + _Range.LowerLeft.Y + " - " + _Range.UpperRight.X +
                                                " | " + _Range.UpperRight.Y);
        }

        Leaf result;
        m_LeafSearcher.StartSearch(_Root, _EntryIndex, _Range, out result);
        return result;
    }

    public static bool Intersects(Rect A, Rect B)
    {
        return (
                    (A.UpperRight.X > B.LowerLeft.X || B.UpperRight.X > A.LowerLeft.X) &&
                    (A.UpperRight.Y > B.LowerLeft.Y || B.UpperRight.Y > A.LowerLeft.Y) &&
                    (A.UpperRight.Z > B.LowerLeft.Z || B.UpperRight.Z > A.LowerLeft.Z)
                );
    }
}