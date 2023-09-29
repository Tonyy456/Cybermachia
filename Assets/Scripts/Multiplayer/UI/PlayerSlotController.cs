using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Input Manager for player select menu.
     */
    public class PlayerSlotController : MonoBehaviour
    {
        [SerializeField] private List<Selectable> NavButtons = new List<Selectable>();
        [SerializeField] private int playerIndex = 1;
        [SerializeField] private TMPro.TMP_Text playerTitle = null;
        [SerializeField] private Color selectedColor = Color.red;

        private UIButtonController traversal;

        /* Author: Anthony D'Alesandro
         * 
         * Initializes slot in editor only. Saves work time.
         */
        public void OnValidate()
        {
            NavButtons = new List<Selectable>(this.GetComponentsInChildren<Selectable>());
            if (playerTitle)
            {
                playerTitle.text = $"Press Button To Join!";
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Assigns this slot to a player. Initialize input and allows traversal through buttons.
         */
        public void AssignToPlayer(PlayerInput input)
        {
            traversal = new UIButtonController(NavButtons, selectedColor, 0);
            traversal.InitializeUp(input.currentActionMap.FindAction("TraverseUp"));
            traversal.InitializeDown(input.currentActionMap.FindAction("TraverseDown"));
            traversal.InitializeConfirm(input.currentActionMap.FindAction("Confirm"));
            playerIndex = input.playerIndex;
            if (playerTitle)
            {
                playerTitle.text = $"Player {input.playerIndex + 1}";
            }
        }

        /* Author: Anthony D'Alesandro
         * 
         * Removes the player on this slot. 
         */
        public void Clear()
        {
            if (playerTitle)
            {
                playerTitle.text = $"Unassigned";
            }
        }
    }
}
