using UnityEditor;
using UnityEngine;

public class DebugRTree : MonoBehaviour
{
    [SerializeField] private int m_NumberOfObjects;
    [SerializeField] private float m_PositionRangeMax;
    [SerializeField] private float m_PositionRangeMin;

    private RTree tree;
    private Node root;
    private float timer = 0.1f;
    private int currentObjectCount = 0;
    private bool doneInserting = false;

    private void Awake()
    {
        root = new Node(0, new Leaf(root, new Rect(new System.Numerics.Vector3(0, 0, 0), new System.Numerics.Vector3(1, 1, 1)), new LeafData[0], 5), root, null);
        root.Entry.EncapsulatingNode = root;
        tree = new RTree(root, 5);
        root.ParentTree = tree;
    }

    private void Update()
    {
        if (timer <= 0 && !doneInserting && currentObjectCount < m_NumberOfObjects)
        {
            GameObject temp = new GameObject();
            temp.transform.position = new Vector3(Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax));
            temp.transform.parent = gameObject.transform;
            tree.Insert(temp);
            SceneView.RepaintAll();
            currentObjectCount++;
            if (currentObjectCount == m_NumberOfObjects)
            {
                doneInserting = true;
            }
            timer = 0.1f;
        }
        else if (timer <= 0 && doneInserting && currentObjectCount > 1)
        {
            tree.Remove(currentObjectCount - 1);
            SceneView.RepaintAll();
            currentObjectCount--;
            timer = 0.1f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            TreeDebugger.Instance?.DrawDebug(tree.Root);
        }
    }
}