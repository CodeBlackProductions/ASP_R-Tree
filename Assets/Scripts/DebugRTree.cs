using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRTree : MonoBehaviour
{
    RTree tree;
    Node root;

    private void Awake()
    {
        root = new Node(0, new Leaf(root, new Rect(new System.Numerics.Vector3(0,0,0), new System.Numerics.Vector3(1,1,1)), new LeafData[0], 5));
        root.Entry.Parent = root;
        tree = new RTree(root,5);



        for (int i = 0; i < 10; i++)
        {
            GameObject temp = new GameObject();
            temp.transform.position = new Vector3(Random.Range(0.0f, 10f), Random.Range(0.0f, 10.0f), Random.Range(0.0f, 10.0f));
            tree.Insert(temp);
        }
    }

    private void OnDrawGizmos()
    {
        if (tree != null)
        {
            TreeDebugger.DrawDebug(root);
        }
    }
}