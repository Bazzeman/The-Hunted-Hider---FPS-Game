using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public int Level = 1;
    public float timeBetweenMusicSongs;
    public AudioClip[] backgroundMusicSongs;
    public GameObject FailPanel;
    public GameObject SucceedPanel;
    public GameObject ObjectivePanel;
    public TextMeshProUGUI StatusDisplay;
    public GameObject[] enemies;
    public float enemySpawnRadius = 20F;
    public float enemySpawnCooldown = 3F;
    public static bool isGameRunning = false;

    private static AudioSource audioSource;
    private static GameObject succeedPanel;
    private static GameObject failPanel;
    private static GameObject objectivePanel;
    private int musicIndex = 0;
    private static int level;
    private static int enemiesToKill;
    private static int enemyCount;
    private static float timeWhenGameStarted;
    private static bool canSpawn = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        succeedPanel = SucceedPanel;
        failPanel = FailPanel;
        objectivePanel = ObjectivePanel;
        enemiesToKill = transform.childCount;
        level = Level;

        // Shuffle the background music songs array, assign a random AudioClip to the audio source's clip and start playing.
        ShuffleArray(ref backgroundMusicSongs);
        audioSource.clip = backgroundMusicSongs[musicIndex];
        audioSource.Play();

        switch (level)
        {
            case 1:
                objectivePanel.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = $"Kill all Zombies ({enemiesToKill}/{enemiesToKill})";
                succeedPanel.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = $"Kill all Zombies ({enemiesToKill}/{enemiesToKill})";
                break;
        }
    }

    void Update()
    {
        if (level == 1)
        {
            enemyCount = transform.childCount;
            StatusDisplay.text = $"{enemyCount}/{enemiesToKill}";
        }
        else if (level == 2 && isGameRunning)
        {
            StatusDisplay.text = $"{Mathf.FloorToInt((Time.time - timeWhenGameStarted) / 60)}:{Mathf.FloorToInt((Time.time - timeWhenGameStarted) % 60)}";
            if (canSpawn) StartCoroutine(EnemySpawnCooldown(enemySpawnCooldown)); // spawn new enemy
        }

        // [Level 1] Check if all enemies have been killed, show the complete screen.
        if (level == 1 && gameObject.activeInHierarchy && transform.childCount == 0)
        {
            GameOver(true);
        }

        // Play the next background music song when the audiosource is not playing anymore
        if (!audioSource.isPlaying)
        {
            StartCoroutine(AudioCooldown(timeBetweenMusicSongs));
        }
    }

    public static void StartGame()
    {
        isGameRunning = true;
        timeWhenGameStarted = Time.time;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void GameOver(bool objectiveSucceeded)
    {
        isGameRunning = false;
        audioSource.Stop();
        Cursor.lockState = CursorLockMode.None;

        switch (level)
        {
            case 1:
                failPanel.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = $"Kill all Zombies ({enemyCount}/{enemiesToKill})";

                if (objectiveSucceeded)
                {
                    succeedPanel.SetActive(true);
                }
                else
                {
                    failPanel.SetActive(true);
                }
                break;
            case 2:
                succeedPanel.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = $"You survived {Mathf.FloorToInt(Time.time / 60)} minutes and {Mathf.FloorToInt(Time.time % 60)} seconds";
                succeedPanel.SetActive(true);
                break;
        }
    }

    // Cooldown method which waits the specified amount of seconds before playing the next song.
    private IEnumerator AudioCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        musicIndex = (musicIndex + 1) % backgroundMusicSongs.Length; // Use modulo to loop back to 0 when reaching the end of the array
        audioSource.clip = backgroundMusicSongs[musicIndex];
        audioSource.Play();
    }

    // Cooldown method to spawn a new enemy.
    private IEnumerator EnemySpawnCooldown(float seconds)
    {
        canSpawn = false;

        transform.position = new Vector3(Random.Range(-enemySpawnRadius, enemySpawnRadius),
            transform.position.y, Random.Range(-enemySpawnRadius, enemySpawnRadius));

        Instantiate(enemies[Random.Range(0, enemies.Length)], transform);

        yield return new WaitForSeconds(seconds);
        canSpawn = true;
    }

    /// <summary>
    /// Randomize the order of the given array without duplicates.
    /// </summary>
    /// <typeparam name="T">The array datatype, anything is supported</typeparam>
    /// <param name="array">The array to randomize</param>
    private void ShuffleArray<T>(ref T[] array) // The ref keyword allows a method to directly change the given paramater, instead of having to re-assign the variable outside of the method
    {
        T[] newArray = new T[array.Length]; // Create a new array to return
        int index = Random.Range(0, array.Length); // Create a random index between 0 and the original array's length

        // Loop trough every index in the new array
        for (int i = 0; i < newArray.Length; i++)
        {
            // Make sure the index in the original array has not been used before, if so try a different index
            while (EqualityComparer<T>.Default.Equals(array[index], default)) {
                index = Random.Range(0, array.Length);
            }

            newArray[i] = array[index]; // Get the variable from the original array on the random index and store this variable on the index of the new array
            array[index] = default; // Set the variable of the original array on the random index to be a default value, this makes sure that there will be no duplications in the new array
        }

        array = newArray; // Lastly assign the given array to the new shuffled array
    }
}
