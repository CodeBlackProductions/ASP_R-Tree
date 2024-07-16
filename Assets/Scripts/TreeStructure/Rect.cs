using System.Numerics;

public class Rect
{
    private Vector3 lowerLeft;
    private Vector3 upperRight;

    public Vector3 LowerLeft { get => lowerLeft; set => lowerLeft = value; }
    public Vector3 UpperRight { get => upperRight; set => upperRight = value; }

    public Rect(Vector3 _LowerLeft, Vector3 _UpperRight)
    {
        lowerLeft = _LowerLeft;
        upperRight = _UpperRight;
    }

    public Vector3 GetCenter()
    {
        return (lowerLeft + upperRight) * 0.5f;
    }
}