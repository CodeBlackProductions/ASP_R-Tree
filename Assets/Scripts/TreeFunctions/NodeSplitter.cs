using System;
using System.Linq;
using System.Numerics;

internal enum Axis
{
    x, y, z
}

public class NodeSplitter
{
    public static void SplitNode(Node _NodeToSplit)
    {
        Axis splitAxis;

        if (_NodeToSplit.Entry is Branch branch)
        {
            float spreadX = CalculateSpread(Axis.x, branch);
            float spreadY = CalculateSpread(Axis.y, branch);
            float spreadZ = CalculateSpread(Axis.z, branch);

           splitAxis = FindLargestSpread(spreadX, spreadY, spreadZ);
        }
        else if (_NodeToSplit.Entry is Leaf leaf)
        {
            float spreadX = CalculateSpread(Axis.x, leaf);
            float spreadY = CalculateSpread(Axis.y, leaf);
            float spreadZ = CalculateSpread(Axis.z, leaf);

            splitAxis = FindLargestSpread(spreadX, spreadY, spreadZ);
        }

        //Do Split Stuff here
    }

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
            case Axis.x:
                return _Vector.X;

            case Axis.y:
                return _Vector.Y;

            case Axis.z:
                return _Vector.Z;

            default:
                throw new InvalidOperationException("Invalid Axis while trying to split Node!");
        }
    }

    private static float GetCoordinate(LeafData _Object, Axis _Axis)
    {
        switch (_Axis)
        {
            case Axis.x:
                return _Object.PosX;

            case Axis.y:
                return _Object.PosY;

            case Axis.z:
                return _Object.PosZ;

            default:
                throw new InvalidOperationException("Invalid Axis while trying to split Node!");
        }
    }

    private static Axis FindLargestSpread(float _X, float _Y, float _Z) 
    {
        float[] val = new float[]{ _X,_Y,_Z};

        int maxIndex = Array.IndexOf(val, val.Max());

        return (Axis)maxIndex;
    }
}