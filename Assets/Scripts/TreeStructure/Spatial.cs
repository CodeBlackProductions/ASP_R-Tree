using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public abstract class Spatial
{
    public abstract Node Parent { get; set; }

    public abstract Rect Rect { get; set; }

    public abstract int EntryCount { get; }

    public abstract int NodeCapacity { get; }

}