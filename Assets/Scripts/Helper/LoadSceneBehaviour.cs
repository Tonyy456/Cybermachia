using UnityEngine;
using UnityEngine.SceneManagement;
using Tony;

/* Author: Anthony D'Alesandro
    * 
    * Simple interface to allow loading scene from the scene name.
    */
public class LoadSceneBehaviour : MonoBehaviour
{
    public void Load(string name)
    {
        Helper.LoadScene(name);
    }
}

