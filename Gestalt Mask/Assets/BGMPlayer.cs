using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    public static BGMPlayer Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float defaultFadeDuration = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
    }

    // Play current clip
    public void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    // Stop BGM
    public void Stop()
    {
        audioSource.Stop();
    }

    // Change BGM instantly
    public void ChangeClip(AudioClip newClip, bool playImmediately = true)
    {
        if (audioSource.clip == newClip)
            return;

        audioSource.clip = newClip;

        if (playImmediately)
            audioSource.Play();
    }

    // Change BGM with fade
    public void ChangeClipWithFade(AudioClip newClip, float fadeDuration = -1f)
    {
        if (fadeDuration < 0)
            fadeDuration = defaultFadeDuration;

        StartCoroutine(FadeAndChange(newClip, fadeDuration));
    }

    private IEnumerator FadeAndChange(AudioClip newClip, float duration)
    {
        float startVolume = audioSource.volume;

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, startVolume, t / duration);
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
