using UnityEngine;
using UnityEngine.SceneManagement;

namespace Machia.Helper
{
    /* Author: Anthony D'Alesandro
     * 
     * Simple interface to allow loading scene from the scene name.
     */
    public class LoadScene : MonoBehaviour
    {
        public void Load(string name)
        {
            Helper.LoadScene(name);
        }
    }
}
