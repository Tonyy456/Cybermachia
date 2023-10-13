using UnityEngine;

namespace Machia.Helper
{
    /* Author: Anthony D'Alesandro
     * 
     * Simple interface to allow quitting application.
     */
    public class QuitAppBehaviour : MonoBehaviour
    {
        public void Quit()
        {
            TonyHelper.Quit();
        }
    }
}
