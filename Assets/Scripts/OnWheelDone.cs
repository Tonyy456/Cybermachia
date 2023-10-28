using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWheelDone : MonoBehaviour
{
    [SerializeField] private float delayTillLoad = 2f;
    private string sceneName;
    public void StartMode(SelectWheelController controller)
    {
        sceneName = controller.selected.sceneName;
        StartCoroutine(LoadWithDelay(delayTillLoad));
    }

    public IEnumerator LoadWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Tony.Helper.LoadScene(sceneName);
        yield return null;
    }
}
