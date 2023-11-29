using UnityEngine;
using UnityEngine.SceneManagement;
using Tony;
using UnityEngine.Events;

/* Author: Anthony D'Alesandro
    * 
    * Simple interface to allow loading scene from the scene name.
    */
public class LoadSceneBehaviour : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private UnityEvent onError;
    public void Load(string name)
    {
        Helper.LoadScene(name);
    }
    public void LoadIfPlayerCount(int count)
    {
        int num = PlayerConnector.numPlayers;
        if (num >= count)
        {        
            Helper.LoadScene(sceneName);
        } else
        {
            onError?.Invoke();
        }
    }
}

