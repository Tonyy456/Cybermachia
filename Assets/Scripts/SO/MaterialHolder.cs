using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialHolder", menuName = "MaterialHolder")]
public class MaterialHolder : ScriptableObject
{
    public List<Material> playerMaterials;
}
