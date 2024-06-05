//using System;
//using System.Linq;
//using Unity.Collections;
//using Unity.Jobs;

//public static class ParallelSearch
//{
//    private struct SearchJob : IJobParallelFor
//    {
//        public NativeArray<NativeList<LeafData>> resultDataSegments;
//        public Rect range;
//        public NativeArray<Node> nodes;

//        public void Execute(int index)
//        {
//            if (index >= 0 && index < nodes.Length)
//            {
//                NativeList<LeafData> segmentResultData = resultDataSegments[index];
//                segmentResultData.AddRange(ScanRange(range, nodes[index]));
//            }
//        }
//    }

//    public static void StartSearch(Node _Root, Rect _Range, out LeafData[] _Result)
//    {
//        if (_Root.Entry == null)
//        {
//            throw new InvalidOperationException("Empty Node while scanning R-Tree: " + _Root);
//        }
//        if (_Range.LowerLeft.x > _Range.UpperRight.x || _Range.LowerLeft.y > _Range.UpperRight.y)
//        {
//            throw new InvalidOperationException("Invalid Range for scanning R-Tree: " + _Range.LowerLeft.x + " | " + _Range.LowerLeft.y + " - " + _Range.UpperRight.x + " | " + _Range.UpperRight.y);
//        }

//        NativeArray<Node> nodes;

//        if (_Root.Entry is Branch)
//        {
//            nodes = new NativeArray<Node>(((Branch)_Root.Entry).Children, Allocator.Persistent);
//        }
//        else
//        {
//            if (!Intersects(_Root.Entry.Rect, _Range))
//            {
//                _Result = new LeafData[0];
//                return;
//            }
//            else
//            {
//                _Result = ((Leaf)_Root.Entry).Data;
//                return;
//            }
//        }

//        NativeArray<NativeList<LeafData>> resultDataSegments = new NativeArray<NativeList<LeafData>>(nodes.Length, Allocator.Persistent);

//        for (int i = 0; i < nodes.Length; i++)
//        {
//            resultDataSegments[i] = new NativeList<LeafData>(Allocator.Persistent);
//        }

//        SearchJob searchJob = new SearchJob
//        {
//            resultDataSegments = resultDataSegments,
//            range = _Range,
//            nodes = nodes
//        };

//        JobHandle jobHandle = searchJob.Schedule(nodes.Length, 64);

//        jobHandle.Complete();

//        NativeList<LeafData> resultData = new NativeList<LeafData>();
//        for (int i = 0; i < resultDataSegments.Length; i++)
//        {
//            resultData.AddRange(resultDataSegments[i].AsArray());
//        }
//        _Result = resultData.ToArray();

//        for (int i = 0; i < resultDataSegments.Length; i++)
//        {
//            resultDataSegments[i].Dispose();
//        }
//        resultDataSegments.Dispose();

//        nodes.Dispose();
//    }

//    private static NativeArray<LeafData> ScanRange(Rect _Range, Node _Start)
//    {
//        NativeList<LeafData> resultData = new NativeList<LeafData>();

//        if (Intersects(_Start.Entry.Rect, _Range))
//        {
//            if (_Start.Entry is Branch)
//            {
//                Branch branch = (Branch)_Start.Entry;

//                for (int i = 0; i < branch.Children.Length; i++)
//                {
//                    if (Intersects(branch.Children[i].Entry.Rect, _Range))
//                    {
//                        //resultData.AddRange(ScanRange(branch.Children[i].Entry.Rect, branch.Children[i]).AsArray());
//                        //Alternative Version. Benchmark necessary! Performance better with or without nested Jobs?
//                        UnityEngine.Debug.Log("Please Remove the comments above when done. No relics of ye olde in me code!");

//                        if (branch.Children[i].Entry is Branch)
//                        {
//                            LeafData[] result;
//                            StartSearch(branch.Children[i], _Range, out result);
//                            resultData.AddRange(new NativeArray<LeafData>(result, Allocator.Persistent));
//                        }
//                    }
//                }
//            }
//            else if (_Start.Entry is Leaf)
//            {
//                NativeArray<LeafData> nativeArray = new NativeArray<LeafData>(((Leaf)_Start.Entry).Data, Allocator.Persistent);
//                resultData.AddRange(nativeArray);
//            }

//            return resultData.AsArray();
//        }
//        else
//        {
//            return new NativeArray<LeafData>();
//        }
//    }

//    private static bool Intersects(Rect A, Rect B)
//    {
//        return !(A.UpperRight.x < B.LowerLeft.x ||
//                 B.UpperRight.x < A.LowerLeft.x ||
//                 A.UpperRight.y < B.LowerLeft.y ||
//                 B.UpperRight.y < A.LowerLeft.y);
//    }
//}

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