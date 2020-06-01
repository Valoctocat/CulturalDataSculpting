using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    /*
    Manages the Shimmering sound of a particular dataset
    Requires an AudioSource
    When Triggered by CubeController, starts sounds Coroutine
    */
    public float durationFade;
    public float volume = 1.0f;
    public float startTimeSec;
    private AudioSource audioSource;

    void Start()
    {
          audioSource = this.GetComponent<AudioSource>();
          if(audioSource != null) {
              audioSource.loop = false;
          }
          else {
              Debug.LogWarning("DataSet requires an audio source: " + this.gameObject.name);
          }
    }

    public bool isPlaying() {
        return audioSource.isPlaying;
    }

    public void PlayCharacteristic(float duration) {
        StartCoroutine(Play(audioSource, duration));
        StartCoroutine(StartFade(audioSource, duration, volume));
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = 0.0f;

        while (currentTime < (duration / 2))
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, (currentTime / duration));
            yield return null;
        }
        float volume =   audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(volume, start, (currentTime / duration ));
            yield return null;
        }
        yield break;
    }

    public IEnumerator Play(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = 0;
        audioSource.time = startTimeSec;
        audioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
        yield break;
    }
}
