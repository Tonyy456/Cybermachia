using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    [SerializeField] private GameObject m_SpawnObject;

    private bool isQuitting = false;
    public void SetPrefabToSpawn(GameObject prefab)
    {
        m_SpawnObject = prefab;
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if (!isQuitting && m_SpawnObject != null)
        {
            var result = GameObject.Instantiate(m_SpawnObject);
            result.transform.position = this.transform.position;
        }
    }
}
