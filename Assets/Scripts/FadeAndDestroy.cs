using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    private float fadeRate;
    private MeshRenderer meshRenderer;

    public void Awake()
    {
        fadeRate = Random.Range(3, 6);
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(FadeTime());
    }

    private IEnumerator FadeTime()
    {
        float initialWait = Random.Range(.5f, 1.3f);
        float loopWait = Random.Range(.01f, .1f);
        yield return new WaitForSeconds(initialWait);
        do
        {
            Color color = meshRenderer.material.color;
            color.a -= fadeRate * Time.deltaTime;
            if(color.a < 0)
                color.a = 0;
            meshRenderer.material.color = color;
            yield return new WaitForSeconds(loopWait);
        } while (meshRenderer.material.color.a > 0);
        Destroy(gameObject);
    }
}