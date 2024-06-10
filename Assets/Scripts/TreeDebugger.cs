using UnityEngine;

public static class TreeDebugger
{

    public static void DrawDebug(Node _Root)
    {
        

        if (_Root.Entry is Leaf)
        {
            DrawCube(CalculateDrawCenter(_Root), CalculateDrawSize(_Root), _Root.Level);
        }
        else if (_Root.Entry is Branch branch)
        {
            for (int i = 0; i < branch.Children.Length; i++)
            {
                DrawDebug(branch.Children[i]);
            }         
        }
    }

    private static Vector3 CalculateDrawCenter(Node _Root)
    {
        float X = (_Root.Entry.Rect.LowerLeft.X + _Root.Entry.Rect.UpperRight.X) * 0.5f;
        float Y = (_Root.Entry.Rect.LowerLeft.Y + _Root.Entry.Rect.UpperRight.Y) * 0.5f;
        float Z = (_Root.Entry.Rect.LowerLeft.Z + _Root.Entry.Rect.UpperRight.Z) * 0.5f;
        return new Vector3(X, Y, Z);
    }

    private static Vector3 CalculateDrawSize(Node _Root)
    {
        float X = (_Root.Entry.Rect.UpperRight.X - _Root.Entry.Rect.LowerLeft.X);
        float Y = (_Root.Entry.Rect.UpperRight.Y - _Root.Entry.Rect.LowerLeft.Y);
        float Z = (_Root.Entry.Rect.UpperRight.Z - _Root.Entry.Rect.LowerLeft.Z);
        return new Vector3(X, Y, Z);
    }

    private static void DrawCube(Vector3 _Center, Vector3 _Size, int _Depth)
    {
        Gizmos.color = new Color(0 + (_Depth *0.1f), 0 + (_Depth * 0.1f), 0 + (_Depth * 0.1f));
        Gizmos.DrawWireCube(_Center, _Size);
        Gizmos.color = Color.white;
    }
}