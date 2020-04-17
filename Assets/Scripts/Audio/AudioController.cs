using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{   public float duration;
    public float volume;
    public float startTimeSec;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
         audioSource.time = startTimeSec;
         StartCoroutine(StartFade(audioSource, duration, volume));
         audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
