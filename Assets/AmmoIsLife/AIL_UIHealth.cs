using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIL_UIHealth : MonoBehaviour
{
    [SerializeField] private GameObject healthParent;

    public void UpdateUI(int currentHealth)
    {
        healthParent.SetActive(true);
        for(int i = 0; i < healthParent.transform.childCount; i++)
        {
            var child = healthParent.transform.GetChild(i);
            child.gameObject.SetActive(i < currentHealth);
        }
    }
}
