using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayHitStop(float seconds)
    {
        StartCoroutine(DoHitStop(seconds));
    }

    private IEnumerator DoHitStop(float time)
    {
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = prevTimeScale;
    }
}
