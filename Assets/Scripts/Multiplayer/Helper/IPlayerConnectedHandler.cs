
using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * interface that you attach ontop of Monobehavior that allows a spawned player to get their PlayerInput class.
     */
    public interface IPlayerConnectedHandler
    {
        public void Initialize(PlayerInput input);
    }
}
