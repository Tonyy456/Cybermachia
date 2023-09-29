using UnityEngine;

namespace Machia.Helper
{
    /* Author: Anthony D'Alesandro
     * 
     * Simple interface to allow quitting application.
     */
    public class QuitApp : MonoBehaviour
    {
        public static void QuitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void Quit()
        {
            QuitApp.QuitApplication();
        }
    }
}
