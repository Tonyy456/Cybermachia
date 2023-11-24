using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUIController : MonoBehaviour
{
    [SerializeField] private v2_AttackManager attack;
    [SerializeField] private string formatString;
    [SerializeField] private TMPro.TMP_Text text;
    public void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        text.text = string.Format(formatString, attack.Ammo);
    }
}
