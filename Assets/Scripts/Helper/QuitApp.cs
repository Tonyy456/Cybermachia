using UnityEngine;

namespace Machia.Helper
{
    /* Author: Anthony D'Alesandro
     * 
     * Simple interface to allow quitting application.
     */
    public class QuitApp : MonoBehaviour
    {
        public void Quit()
        {
            Helper.Quit();
        }
    }
}
