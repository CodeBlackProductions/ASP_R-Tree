using System.Collections.Generic;
using UnityEngine;

public class TreeDebugger : MonoBehaviour
{
    public static TreeDebugger Instance;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private bool m_UseGizmos;
    [SerializeField] private Material m_WhiteMat;
    [SerializeField] private Material m_GreenMat;
    [SerializeField] private Material m_RedMat;

    private Dictionary<Node, GameObject> cubeMap = new Dictionary<Node, GameObject>();
    private Node m_Root;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DrawDebug(Node root)
    {
        m_Root = root;
        if (!m_UseGizmos)
        {
            ClearCubes();
        }
        DrawNode(root);
    }

    private void DrawNode(Node node)
    {
        if (node.Entry is Leaf)
        {
            CreateCube(node, "Leaf");
        }
        else if (node.Entry is Branch branch)
        {
            CreateCube(node, "Branch");
            for (int i = 0; i < branch.Children.Length; i++)
            {
                DrawNode(branch.Children[i]);
            }
        }
    }

    private void CreateCube(Node node, string name)
    {
        if (!m_UseGizmos && cubePrefab == null)
        {
            Debug.LogError("Cube prefab is not set in TreeDebugger.");
            return;
        }

        Rect rect = node.Entry.Rect;
        System.Numerics.Vector3 center = (rect.LowerLeft + rect.UpperRight) * 0.5f;
        System.Numerics.Vector3 size = rect.UpperRight - rect.LowerLeft;

        if (m_UseGizmos)
        {
            if (node.Entry is Leaf)
            {
                Gizmos.color = Color.green;
            }
            if (node == m_Root) 
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireCube(ToUnityVector3(center), ToUnityVector3(size));
            Gizmos.color = Color.white;
        }
        else
        {
            GameObject cube = Instantiate(cubePrefab, ToUnityVector3(center), Quaternion.identity);
            cube.transform.localScale = ToUnityVector3(size);
            cube.name = name;

            if (node.Entry is Leaf)
            {
                cube.GetComponent<Renderer>().material = m_GreenMat;
            }
            if (node == m_Root)
            {
                cube.GetComponent<Renderer>().material = m_RedMat;
            }

            cubeMap[node] = cube;

            if (node.Parent != null && cubeMap.ContainsKey(node.Parent))
            {
                cube.transform.parent = cubeMap[node.Parent].transform;
            }
        }
    }

    private Vector3 ToUnityVector3(System.Numerics.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }

    private void ClearCubes()
    {
        foreach (var cube in cubeMap.Values)
        {
            Destroy(cube);
        }
        cubeMap.Clear();
    }
}