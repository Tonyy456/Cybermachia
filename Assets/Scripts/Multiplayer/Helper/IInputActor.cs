using UnityEngine.InputSystem;

namespace Machia.Input
{
    /* Author: Anthony D'Alesandro
     * 
     * Interfaces for classes that want information about a player when they join the game.
     */
    public interface IInputActor 
    {
        public void Initialize(PlayerInput inputManager);
    }
    public interface IPlayerConnectorHandler
    {
        public void Initialize(PlayerInput input);
    }
}
