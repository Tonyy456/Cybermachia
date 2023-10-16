using UnityEngine;
using Tony;

/* Author: Anthony D'Alesandro
    * 
    * Simple interface to allow quitting application.
    */
public class QuitAppBehaviour : MonoBehaviour
{
    public void Quit()
    {
        Helper.Quit();
    }
}
