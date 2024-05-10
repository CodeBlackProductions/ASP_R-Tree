using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafData
{
    private GameObject obj;
    private Vector3 pos;

    public GameObject Obj { get => obj; }
    public Vector3 Pos { get => pos; }

    public LeafData(GameObject _Obj, Vector3 _Pos)
    {
        obj = _Obj;
        pos = _Pos;
    }
}