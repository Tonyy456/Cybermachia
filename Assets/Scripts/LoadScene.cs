using UnityEngine;
using UnityEngine.SceneManagement;

namespace Machia.Helper
{
    public class LoadScene : MonoBehaviour
    {
        public void Load(string name)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }
    }
}
