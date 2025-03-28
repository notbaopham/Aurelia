using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private float bgmAudioSourceLowerBy = 0.5f; // Reference to the BGM AudioSource

    private List<AudioSource> audioSources = new List<AudioSource>();
    [Range(0f, 1f)] public float masterVolume = 1f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        // Load saved volume (if available)
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    void Start()
    {
        // if (FindObjectsByType<AudioManager>(FindObjectsSortMode.None).Length > 1)
        // {
        //     Destroy(gameObject);
        //     return; 
        // }
        // DontDestroyOnLoad(gameObject);
        // Automatically register all AudioSources in the scene
        RegisterAllAudioSources();
        UpdateAudioSourcesVolume();
    }

    // Register all AudioSources in the scene automatically
    public void RegisterAllAudioSources()
    {
        audioSources.Clear();
        AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (AudioSource source in sources)
        {
            if (!audioSources.Contains(source))
            {
                audioSources.Add(source);
                // If the AudioSource is named "BGM Audio", apply lower volume
                if (source.gameObject.name == "BGM Audio")
                {
                    source.volume = masterVolume * bgmAudioSourceLowerBy; // 50% of master volume
                }
                else
                {
                    source.volume = masterVolume;
                }
            }
        }
    }

    // Update the volume of all audio sources
    public void UpdateAudioSourcesVolume()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source != null)
            {
                if (source.gameObject.name == "BGM Audio")
                {
                    source.volume = masterVolume * bgmAudioSourceLowerBy; // 50% of master volume
                }
                else
                {
                    source.volume = masterVolume;
                }
            }
        }
    }

    // Change master volume and apply it to all audio sources
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAudioSourcesVolume();

        // Save volume setting
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }
}
