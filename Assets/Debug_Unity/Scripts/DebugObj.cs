using UnityEngine;

public class DebugObj : MonoBehaviour
{
    private Vector3 pos;
    private Vector3 newPos;
    private RTree tree;

    public RTree Tree { get => tree; set => tree = value; }

    private void Awake()
    {
        pos = transform.position;
    }

    public void UpdatePos()
    {
        pos = transform.position;
    }

    private void Update()
    {
        newPos = transform.position;

        float dist = (pos - newPos).sqrMagnitude;
        if (dist > 25)
        {
            tree.UpdateObjectPosition(gameObject);
            UpdatePos();
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(pos, 0.25f);
            Gizmos.color = Color.white;
        }
    }
}