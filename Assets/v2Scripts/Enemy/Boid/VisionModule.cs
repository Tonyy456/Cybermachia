using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisionModule : MonoBehaviour
{
    public List<Transform> obstacles = new List<Transform>();
    public List<Transform> boids = new List<Transform>();
    public List<Transform> targets = new List<Transform>();
}
