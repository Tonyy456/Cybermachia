using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinInPlace : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private float RadiansPerSecond;
    public void Update()
    {
        this.transform.Rotate(rotationVector * RadiansPerSecond * Time.deltaTime);
;    }
}
