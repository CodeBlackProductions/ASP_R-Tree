using System;
using System.Linq;
using System.Numerics;

internal enum Axis
{
    X, Y, Z
}

public class NodeSplitter
{
    public static void SplitNode(Node _NodeToSplit)
    {
        Axis splitAxis;

        if (_NodeToSplit.Entry is Branch branch)
        {
            splitAxis = CalculateSplitAxis(branch);

            Node[] sortArray;
            SortAlongAxis(branch, splitAxis, out sortArray);

            int pivot = FindPivotEntry(sortArray);

            Node[][] splitChildren = SplitChildren(sortArray, pivot);

            Node[] newNodes = CreateNewNodes(branch.Parent, _NodeToSplit.Level, splitChildren, branch.NodeCapacity);

            Branch parent = branch.Parent.Entry as Branch;

            UpdateParentChildren(parent, _NodeToSplit, newNodes);
        }
        else if (_NodeToSplit.Entry is Leaf leaf)
        {
            splitAxis = CalculateSplitAxis(leaf);

            LeafData[] sortArray;
            SortAlongAxis(leaf, splitAxis, out sortArray);

            int pivot = FindPivotEntry(sortArray);

            LeafData[][] splitChildren = SplitChildren(sortArray, pivot);

            Node[] newNodes = CreateNewNodes(leaf.Parent, _NodeToSplit.Level, splitChildren, leaf.NodeCapacity);

            Branch parent = leaf.Parent.Entry as Branch;

            UpdateParentChildren(parent, _NodeToSplit, newNodes);
        }
    }

    #region Methods for the actual split

    private static void UpdateParentChildren(Branch _Parent, Node _OldNode, Node[] _NewNodes)
    {
        Node[] newChildren = new Node[_Parent.Children.Length + 1];

        for (int i = 0; i < _Parent.Children.Length; i++)
        {
            if (_Parent.Children[i].ID == _OldNode.ID)
            {
                newChildren[i] = _NewNodes[0];
            }
            else
            {
                newChildren[i] = _Parent.Children[i];
            }
        }
        newChildren[_Parent.Children.Length] = _NewNodes[1];

        _Parent.Children = newChildren;
    }

    private static Node[] CreateNewNodes(Node _Parent, int _Level, Node[][] _SplitChildren, int _NodeCapacity)
    {
        Node nodeA = new Node(_Level, new Branch(_Parent, CreateNewNodeRect(_SplitChildren[0]), _SplitChildren[0], _NodeCapacity));
        Node nodeB = new Node(_Level, new Branch(_Parent, CreateNewNodeRect(_SplitChildren[1]), _SplitChildren[1], _NodeCapacity));
        return new Node[] { nodeA, nodeB };
    }

    private static Node[] CreateNewNodes(Node _Parent, int _Level, LeafData[][] _SplitChildren, int _NodeCapacity)
    {
        Node nodeA = new Node(_Level, new Leaf(_Parent, CreateNewNodeRect(_SplitChildren[0]), _SplitChildren[0], _NodeCapacity));
        Node nodeB = new Node(_Level, new Leaf(_Parent, CreateNewNodeRect(_SplitChildren[1]), _SplitChildren[1], _NodeCapacity));
        return new Node[] { nodeA, nodeB };
    }

    private static Rect CreateNewNodeRect(Node[] _Nodes)
    {
        throw new NotImplementedException();
    }

    private static Rect CreateNewNodeRect(LeafData[] _Nodes)
    {
        throw new NotImplementedException();
    }

    private static int FindPivotEntry(Node[] _Data)
    {
        int pivotIndex = 0;

        pivotIndex = (int)Math.Round(_Data.Length * 0.5f);

        return pivotIndex;
    }

    private static int FindPivotEntry(LeafData[] _Data)
    {
        int pivotIndex = 0;

        pivotIndex = (int)Math.Round(_Data.Length * 0.5f) - 1;

        return pivotIndex;
    }

    private static Node[][] SplitChildren(Node[] _SortedArray, int _PivotIndex)
    {
        Node[] nodesA = new Node[_PivotIndex + 1];
        Node[] nodesB = new Node[_SortedArray.Length - (_PivotIndex + 1)];

        Array.Copy(_SortedArray, 0, nodesA, 0, _PivotIndex + 1);
        Array.Copy(_SortedArray, _PivotIndex + 1, nodesB, 0, nodesB.Length);

        return new Node[][] { nodesA, nodesB };
    }

    private static LeafData[][] SplitChildren(LeafData[] _SortedArray, int _PivotIndex)
    {
        LeafData[] dataA = new LeafData[_PivotIndex + 1];
        LeafData[] dataB = new LeafData[_SortedArray.Length - (_PivotIndex + 1)];

        Array.Copy(_SortedArray, 0, dataA, 0, _PivotIndex + 1);
        Array.Copy(_SortedArray, _PivotIndex + 1, dataB, 0, dataB.Length);

        return new LeafData[][] { dataA, dataB };
    }

    #endregion Methods for the actual split

    #region Methods to sort along axis

    private static void SortAlongAxis(Branch _NodeEntry, Axis _SplitAxis, out Node[] _SortedArray)
    {
        _SortedArray = _NodeEntry.Children
                                 .OrderBy(item => GetAxisCoordinate(CalculateCenterOfRect(item.Entry.Rect), _SplitAxis))
                                 .ToArray();
    }

    private static void SortAlongAxis(Leaf _NodeEntry, Axis _SplitAxis, out LeafData[] _SortedArray)
    {
        _SortedArray = _NodeEntry.Data
                                 .OrderBy(item => GetAxisCoordinate(new Vector3(item.PosX, item.PosY, item.PosZ), _SplitAxis))
                                 .ToArray();
    }

    private static Vector3 CalculateCenterOfRect(Rect _Rect)
    {
        return (_Rect.LowerLeft + _Rect.UpperRight) * 0.5f;
    }

    private static float GetAxisCoordinate(Vector3 _Pos, Axis _Axis)
    {
        switch (_Axis)
        {
            case Axis.X:
                return _Pos.X;

            case Axis.Y:
                return _Pos.Y;

            case Axis.Z:
                return _Pos.Z;

            default:
                throw new ArgumentOutOfRangeException(nameof(_Axis), "Invalid axis specified for sorting.");
        }
    }

    #endregion Methods to sort along axis

    #region Methods for finding target axis

    private static float CalculateSpread(Axis _Axis, Branch _Branch)
    {
        float lowMin;
        float lowMax;
        float highMin;
        float highMax;

        lowMin = GetCoordinate(_Branch.Children[0].Entry.Rect.LowerLeft, _Axis);
        lowMax = lowMin;
        highMin = GetCoordinate(_Branch.Children[0].Entry.Rect.UpperRight, _Axis);
        highMax = highMin;

        for (int i = 1; i < _Branch.Children.Length; i++)
        {
            Node child = _Branch.Children[i];
            lowMin = GetCoordinate(child.Entry.Rect.LowerLeft, _Axis) < lowMin ? GetCoordinate(child.Entry.Rect.LowerLeft, _Axis) : lowMin;
            lowMax = GetCoordinate(child.Entry.Rect.LowerLeft, _Axis) > lowMax ? GetCoordinate(child.Entry.Rect.LowerLeft, _Axis) : lowMax;
            highMin = GetCoordinate(child.Entry.Rect.UpperRight, _Axis) < highMin ? GetCoordinate(child.Entry.Rect.UpperRight, _Axis) : highMin;
            highMax = GetCoordinate(child.Entry.Rect.UpperRight, _Axis) > highMax ? GetCoordinate(child.Entry.Rect.UpperRight, _Axis) : highMax;
        }

        return (lowMax - lowMin) + (highMax - highMin);
    }

    private static float CalculateSpread(Axis _Axis, Leaf _Leaf)
    {
        float min;
        float max;

        min = GetCoordinate(_Leaf.Data[0], _Axis);
        max = min;

        for (int i = 1; i < _Leaf.Data.Length; i++)
        {
            LeafData obj = _Leaf.Data[i];
            min = GetCoordinate(obj, _Axis) < min ? GetCoordinate(obj, _Axis) : min;
            max = GetCoordinate(obj, _Axis) > max ? GetCoordinate(obj, _Axis) : max;
        }

        return max - min;
    }

    private static float GetCoordinate(Vector3 _Vector, Axis _Axis)
    {
        switch (_Axis)
        {
            case Axis.X:
                return _Vector.X;

            case Axis.Y:
                return _Vector.Y;

            case Axis.Z:
                return _Vector.Z;

            default:
                throw new ArgumentOutOfRangeException("Invalid Axis while trying to split Node!");
        }
    }

    private static float GetCoordinate(LeafData _Object, Axis _Axis)
    {
        switch (_Axis)
        {
            case Axis.X:
                return _Object.PosX;

            case Axis.Y:
                return _Object.PosY;

            case Axis.Z:
                return _Object.PosZ;

            default:
                throw new ArgumentOutOfRangeException("Invalid Axis while trying to split Node!");
        }
    }

    private static Axis FindLargestSpread(float _X, float _Y, float _Z)
    {
        float[] val = new float[] { _X, _Y, _Z };

        int maxIndex = Array.IndexOf(val, val.Max());

        return (Axis)maxIndex;
    }

    private static Axis CalculateSplitAxis(Branch _Branch)
    {
        float spreadX = CalculateSpread(Axis.X, _Branch);
        float spreadY = CalculateSpread(Axis.Y, _Branch);
        float spreadZ = CalculateSpread(Axis.Z, _Branch);
        return FindLargestSpread(spreadX, spreadY, spreadZ);
    }

    private static Axis CalculateSplitAxis(Leaf _Leaf)
    {
        float spreadX = CalculateSpread(Axis.X, _Leaf);
        float spreadY = CalculateSpread(Axis.Y, _Leaf);
        float spreadZ = CalculateSpread(Axis.Z, _Leaf);
        return FindLargestSpread(spreadX, spreadY, spreadZ);
    }

    #endregion Methods for finding target axis
}