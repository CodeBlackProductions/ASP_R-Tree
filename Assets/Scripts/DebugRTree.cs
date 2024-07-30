using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugRTree : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabObject;
    [SerializeField] private int m_NumberOfObjects;
    [SerializeField] private bool m_UseFixedPositions;
    [SerializeField] private float m_PositionRangeMax;
    [SerializeField] private float m_PositionRangeMin;
    [SerializeField] private bool m_RemoveAfterInsert;
    [SerializeField] private bool m_RandomizePositions;
    [SerializeField] private bool m_UseDebugLog;

    private RTree tree;
    private Node root;
    private float timer = 0.1f;
    private int currentObjectCount = 0;
    private bool doneInserting = false;
    private bool doneFlag = true;

    private List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        root = new Node(0, new Leaf(root, new Rect(new System.Numerics.Vector3(0, 0, 0), new System.Numerics.Vector3(1, 1, 1)), new LeafData[0], 10, 5), root, null);
        root.Entry.EncapsulatingNode = root;
        tree = new RTree(root, 10, 5);
        root.ParentTree = tree;
    }

    private void Update()
    {
        if (timer <= 0 && !doneInserting && currentObjectCount < m_NumberOfObjects)
        {
            GameObject temp = null;
            DebugObj objController = null;

            if (m_PrefabObject != null)
            {
                temp = Instantiate<GameObject>(m_PrefabObject);
                objController = temp.GetComponent<DebugObj>();
                objController.Tree = tree;
            }
            else
            {
                temp = new GameObject();
            }

            if (m_UseFixedPositions)
            {
                temp.transform.position = new Vector3(currentObjectCount, 0, currentObjectCount);
                temp.transform.parent = gameObject.transform;
            }
            else
            {
                temp.transform.position = new Vector3(Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax), Random.Range(m_PositionRangeMin, m_PositionRangeMax));
                temp.transform.parent = gameObject.transform;
            }

            if (objController != null)
            {
                objController.UpdatePos();
            }

            objects.Add(temp);
            tree.Insert(temp);
            SceneView.RepaintAll();
            currentObjectCount++;
            if (currentObjectCount == m_NumberOfObjects)
            {
                doneInserting = true;
            }
            timer = 0.1f;
        }
        else if (doneInserting && currentObjectCount == m_NumberOfObjects && doneFlag)
        {
            Debug.Log("Done Inserting");
            doneFlag = false;
        }
        else if (m_RemoveAfterInsert && timer <= 0 && doneInserting && currentObjectCount > 1)
        {
            tree.Remove(objects[currentObjectCount - 1]);
            SceneView.RepaintAll();
            currentObjectCount--;
            GameObject temp = objects[objects.Count - 1];
            objects.Remove(temp);
            Destroy(temp);
            timer = 0.1f;
        }
        else if (m_RandomizePositions && timer <= 0 && doneInserting)
        {
            float newX = Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            float newY = Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            float newZ = Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            objects[Random.Range(0, objects.Count - 1)].gameObject.transform.position = new Vector3(newX, newY, newZ);
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (m_UseDebugLog && timer <= 0 && (currentObjectCount % 100 == 1 || currentObjectCount % 25 == 1))
        {
            Debug.Log(currentObjectCount);
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