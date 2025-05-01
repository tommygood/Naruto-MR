using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;
    public AudioClip[] BGMs;
    public float fadeDuration = 2.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator FadeIn(int audioClipIndex)
    {
        audioSource.clip = BGMs[audioClipIndex];
        audioSource.volume = 0f;
        audioSource.Play();

        float timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    public IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public IEnumerator IncreaseVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;

        float timer = 0f;
        while (timer < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    public IEnumerator DecreaseVolume(float targetVolume, float duration)
    {
        float startVolume = audioSource.volume;

        float timer = 0f;
        while (timer < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
