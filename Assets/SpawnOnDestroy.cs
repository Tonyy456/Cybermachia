using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject m_SpawnObject;
    public void SetPrefabToSpawn(GameObject prefab)
    {
        m_SpawnObject = prefab;
    }

    public void OnDestroy()
    {
        if(m_SpawnObject != null)
        {
            GameObject.Instantiate(m_SpawnObject);
        }
    }
}
