using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Machia.Helper
{
    public class QuitApp : MonoBehaviour
    {
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }
    }
}
