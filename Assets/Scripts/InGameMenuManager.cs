using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public static object Instance { get; internal set; }
    // [SerializeField] private GameObject mainMenu;
    // [SerializeField] private GameObject titleImage;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject settingsMenu;

    

    // [SerializeField] private AudioSource audioSource;
    // [SerializeField] private float startTime;
    private bool isSettingsMenuActive = false;
    // this creates a public getter for the bool
    // this way the variable is read only without makingit public

    void Awake()
    {
        settingsMenu.SetActive(false);

        inputManager.OnSettingsMenu.AddListener(ToggleSettingsMenu);
        // if (audioSource != null)
        // {
        //     audioSource.time = startTime; // Set start time
        //     audioSource.Play(); // Play from the specified time
        // }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void ToggleSettingsMenu()
    {
        Debug.Log("Settings Menu Toggled");
        if (!isSettingsMenuActive) OpenSettings();
        else CloseSettings();
    }
    public void OpenSettings()
    {
        // mainMenu.SetActive(false);  
        // titleImage.SetActive(false);
        settingsMenu.SetActive(true);
        isSettingsMenuActive = true;
    }
    public void CloseSettings()
    {
        // mainMenu.SetActive(true);
        // titleImage.SetActive(true);
        settingsMenu.SetActive(false);
        isSettingsMenuActive = false;
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // public void SetVolume(float volume)
    // {
    //     audioSource.volume = Mathf.Clamp01(volume); // Ensure volume stays between 0 and 1
    // }

}
