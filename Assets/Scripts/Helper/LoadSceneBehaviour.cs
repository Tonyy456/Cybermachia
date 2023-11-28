using UnityEngine;
using UnityEngine.SceneManagement;
using Tony;

/* Author: Anthony D'Alesandro
    * 
    * Simple interface to allow loading scene from the scene name.
    */
public class LoadSceneBehaviour : MonoBehaviour
{
    [SerializeField] private string sceneName; 
    public void Load(string name)
    {
        Helper.LoadScene(name);
    }
    public void LoadIfPlayerCount(int count)
    {
        int num = PlayerConnector.numPlayers;
        Debug.Log(num);
        if (num >= count)
        {        
            Helper.LoadScene(sceneName);
        }
    }
}

