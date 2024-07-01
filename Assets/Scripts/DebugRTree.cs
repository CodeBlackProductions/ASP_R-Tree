using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRTree : MonoBehaviour
{
    [SerializeField] private int m_NumberOfObjects;
    [SerializeField] private float m_PositionRangeMax;
    [SerializeField] private float m_PositionRangeMin;

    RTree tree;
    Node root;
    float timer = 0.5f;
    private int currentObjectCount = 0;

    private void Awake()
    {
        root = new Node(0, new Leaf(root, new Rect(new System.Numerics.Vector3(0,0,0), new System.Numerics.Vector3(1,1,1)), new LeafData[0], 5),root,null);
        root.Entry.Parent = root;
        tree = new RTree(root,5);
        root.ParentTree = tree;
    }

    private void Update() 
    {
        if (timer <= 0 && currentObjectCount < m_NumberOfObjects)
        {
            GameObject temp = new GameObject();
            temp.transform.position = new Vector3(Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax));
            temp.transform.parent = gameObject.transform;
            tree.Insert(temp);
            currentObjectCount++;
            TreeDebugger.Instance?.DrawDebug(tree.Root);
            timer = 0.5f;
        }
        else 
        {
            timer -= Time.deltaTime;
        }
    }
}
