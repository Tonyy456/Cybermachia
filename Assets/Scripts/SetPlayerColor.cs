using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

enum HandleType
{
    SpriteRenderer,
    Image,
    Text
}
public class SetPlayerColor : IPlayerConnectedHandler
{
    [Header("WARNING! Need one for each player or game sad :(")]
    [SerializeField] private HandleType handleType;
    [SerializeField] private List<Material> playerMaterials;

    public override void ConnectPlayer(PlayerInput input)
    {
        switch (handleType)
        {
            case HandleType.SpriteRenderer:
                SpriteRenderer renderer = input.GetComponentInChildren<SpriteRenderer>();
                if (renderer) renderer.material = playerMaterials[input.playerIndex];
                break;
            case HandleType.Image:
                Image image = input.GetComponentInChildren<Image>();
                if (image) image.material = playerMaterials[input.playerIndex];
                break;
            case HandleType.Text:
                TMPro.TMP_Text text = input.GetComponentInChildren<TMPro.TMP_Text>();
                if (text)
                {
                    text.color = playerMaterials[input.playerIndex].GetColor("OutlineColor");
                }
                break;
            default:
                break;
        }
    }
}
