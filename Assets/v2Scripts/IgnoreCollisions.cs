using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class IgnoreCollisions : MonoBehaviour
{
    [SerializeField] private List<TwoInt> ignoreLayers;
    void Start()
    {
        foreach(var pair in ignoreLayers)
        {
            Physics2D.IgnoreLayerCollision(pair.int1, pair.int2);
        }
    }
}

[Serializable]
public class TwoInt
{
    public int int1;
    public int int2;
}
