using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIL_UIHealth : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;
    [SerializeField] private Gradient healthColorGradient;

    public void UpdateUI(float currentHealth, float maxHealth)
    {
        Color healthColor = healthColorGradient.Evaluate(currentHealth / maxHealth);
        textMesh.color = healthColor;
        textMesh.text = currentHealth.ToString("F0");
    }
}
