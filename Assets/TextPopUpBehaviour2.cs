using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TMP_Text))]
public class TextPopUpBehaviour2 : MonoBehaviour
{
    [SerializeField] private float upSpeed = 1f;
    [SerializeField] private float upSlowSpeed = 1f;
    [SerializeField] private float aliveTime = 1f;
    [SerializeField] private float dp = 0.5f;
    public void Initialize(string text, Vector2 spawnCenter, Color color)
    {
        Vector3 spawn = new Vector3(spawnCenter.x, spawnCenter.y, this.transform.position.z)
                        + new Vector3(Random.Range(-dp, dp), 0, 0);
        this.transform.position = spawn;
        TMPro.TMP_Text textMesh = this.GetComponent<TMPro.TMP_Text>();
        color.a = 1;
        textMesh.text = text;
        textMesh.color = color;
        StartCoroutine(DestroyInTime(aliveTime));
    }
    public IEnumerator DestroyInTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject.Destroy(this.gameObject);
    }

    public void Update()
    {
        this.transform.position += (new Vector3(0, 1, 0) * Time.deltaTime * upSpeed);
        float newUpSpeed = upSpeed - (upSlowSpeed * Time.deltaTime);
        upSpeed = Mathf.Clamp(newUpSpeed, 0, float.MaxValue);
    }
}
