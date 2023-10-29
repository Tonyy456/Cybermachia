using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmoUIController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text m_Text;
    [SerializeField] private Image image;
    public void SetAmmo(int ammo)
    {
        m_Text.text = $"{ammo}";
    }

    public void SetColor(Color c)
    {
        image.color = c;
    }
}
