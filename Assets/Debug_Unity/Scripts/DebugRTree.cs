using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugRTree : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabObject;
    [SerializeField] [Range(1,4000)] private int m_NumberOfObjects = 4000;
    [SerializeField] private float m_DelayTime = 0.1f;
    [SerializeField] private bool m_UseFixedPositions = false;
    [SerializeField] private float m_PositionRangeMax = 50;
    [SerializeField] private float m_PositionRangeMin = 0;
    [SerializeField] private bool m_RemoveAfterInsert = false;
    [SerializeField] private bool m_RandomizePositions = true;
    [SerializeField] private bool m_UseDebugLog = true;

    private RTree tree;
    private Node root;
    private float timer;
    private int currentObjectCount = 0;
    private bool doneInserting = false;
    private bool doneFlag = true;

    private List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        tree = new RTree(10, 5);
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

            temp.name = temp.name + Time.time;

            if (m_UseFixedPositions)
            {
                temp.transform.position = new Vector3(this.transform.position.x + currentObjectCount, 0, this.transform.position.z + currentObjectCount);
                temp.transform.parent = gameObject.transform;
            }
            else
            {
                temp.transform.position = new Vector3(this.transform.position.x + Random.Range(m_PositionRangeMin, m_PositionRangeMax), this.transform.position.y + Random.Range(m_PositionRangeMin, m_PositionRangeMax), this.transform.position.z + Random.Range(m_PositionRangeMin, m_PositionRangeMax));
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
            timer = m_DelayTime;
        }
        else if (doneInserting && currentObjectCount == m_NumberOfObjects && doneFlag)
        {
            Debug.Log("Done Inserting" + " " + this.gameObject.name);
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
            timer = m_DelayTime;
        }
        else if (m_RandomizePositions && timer <= 0 && doneInserting)
        {
            float newX = this.transform.position.x + Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            float newY = this.transform.position.y + Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            float newZ = this.transform.position.z + Random.Range(m_PositionRangeMin, m_PositionRangeMax);
            objects[Random.Range(0, objects.Count - 1)].gameObject.transform.position = new Vector3(newX, newY, newZ);
            timer = m_DelayTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        if (m_UseDebugLog && timer <= 0 && (currentObjectCount % 100 == 1 || currentObjectCount % 25 == 1))
        {
            Debug.Log(currentObjectCount + " " + this.gameObject.name);
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