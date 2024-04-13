using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySoundManager : MonoBehaviour
{
    public int chanceOfPlaying = 3;
    public AudioClip[] audioClips;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Play a random audio clip in the audioClips array if the chance of playing a audioclip is met
        if (Random.value <= chanceOfPlaying/100)
        {

            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
