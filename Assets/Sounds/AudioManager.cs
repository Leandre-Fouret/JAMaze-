using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioClip sound1;  // First sound to play
    public AudioClip sound2;  // Second sound to play
    public AudioClip amb;  // Ambient sound to play in loop

    private AudioSource audioSource;
    public float ambientVolume = 0.2f;  // Max volume for ambient sound
    public float crescendoDuration = 5f; // Duration for crescendo

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Play the first sound
        PlaySound(sound1);

        // Start a coroutine to delay playing sound2
        StartCoroutine(PlaySoundWithDelay(5f, sound2)); // 10 seconds delay after sound1
        StartCoroutine(PlayAmbWithDelay(15f, amb));
    }

    // Method to play a sound clip
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            if (clip != amb)
                audioSource.loop = false;
            else
                audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip is not assigned.");
        }
    }

    // Coroutine to play the second sound after a delay
    private IEnumerator PlaySoundWithDelay(float delay, AudioClip clip)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Play the second sound
        PlaySound(clip);
    }

    private IEnumerator PlayAmbWithDelay(float delay, AudioClip clip)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        audioSource.clip = amb;
        audioSource.loop = true;  // Set loop to true for the ambient sound
        audioSource.Play();

        // Start the crescendo effect to increase volume gradually
        StartCoroutine(CrescendoAudio(0f, ambientVolume, crescendoDuration));
    }
    private IEnumerator CrescendoAudio(float startVolume, float endVolume, float duration)
    {
        float timeElapsed = 0f;

        // Set the initial volume to 0
        audioSource.volume = startVolume;

        // Gradually increase volume to the desired level
        while (timeElapsed < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime; // Increment time by frame
            yield return null; // Wait until the next frame
        }

        // Ensure it reaches the final volume
        audioSource.volume = endVolume;
    }
}
