using System.Collections;
using UnityEngine;

public class SlowmoManager : MonoBehaviour
{
    public void StartSlowmo()
    {
        SetTimeScale(.2f);
        StartCoroutine(NormalTime());
    }

    public void ResetTime()
    {
        SetTimeScale(1f);
    }

    private IEnumerator NormalTime()
    {
        yield return new WaitForSeconds(.7f);
        while(Time.timeScale < 1)
        {
            float newTimeScale = Time.timeScale + .2f;
            if(newTimeScale > 1)
                newTimeScale = 1;
            SetTimeScale(newTimeScale);
            yield return new WaitForSeconds(.1f);
        }
    }

    private void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = .02f * timeScale;
    }
}