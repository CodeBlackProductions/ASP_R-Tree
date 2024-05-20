using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inserter
{
    // return int to return the number of added nodes? maybe two values to return added levels and nodes?
    public static int InsertData(Node _Root, int _ObjIDX, float _PosX, float _PosY, float _PosZ)
    {
        LeafData leafData = new LeafData(_ObjIDX, _PosX, _PosY, _PosZ);

        Node targetNode = ChooseTargetNode(_Root);

        if (targetNode.Entry is Leaf)
        {
            LeafData[] oldData = ((Leaf)targetNode.Entry).Data;
            LeafData[] newData = new LeafData[oldData.Length + 1];

            for (int i = 0; i < oldData.Length; i++)
            {
                newData[i] = oldData[i];
            }
            newData[oldData.Length] = leafData;

            ((Leaf)targetNode.Entry).Data = newData;

            if (targetNode.IsOverflowing())
            {
                SplitNode(targetNode);
            }
        }
        if (targetNode is Branch)
        {
            //What to do when there is no overlapping leaf but a branch?
        }
        else
        {
            throw new InvalidOperationException("Invalid node while inserting into tree: " + targetNode);
        }

        RebalanceTree(_Root);

        return ???;
    }

    private static void RebalanceTree(Node _Root)
    {
        throw new NotImplementedException();
    }

    private static void SplitNode(Node _NodeToSplit)
    {
        throw new NotImplementedException();
    }

    private static Node ChooseTargetNode(Node _Root)
    {
        throw new NotImplementedException();
    }
}