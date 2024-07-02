using System.Collections.Generic;
using UnityEngine;

public class TreeDebugger : MonoBehaviour
{
    public static TreeDebugger Instance;

    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private bool m_UseGizmos;

    private Dictionary<Node, GameObject> cubeMap = new Dictionary<Node, GameObject>();

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
            Gizmos.DrawWireCube(ToUnityVector3(center), ToUnityVector3(size));
        }
        else
        {
            GameObject cube = Instantiate(cubePrefab, ToUnityVector3(center), Quaternion.identity);
            cube.transform.localScale = ToUnityVector3(size);
            cube.name = name;

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