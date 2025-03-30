using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject titleImage;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float startTime;

    void Awake()
    {
        settingsMenu.SetActive(false);
        // if (audioSource != null)
        // {
        //     audioSource.time = startTime; // Set start time
        //     audioSource.Play(); // Play from the specified time
        // }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(GameObject.Find("Player 1"));
        Destroy(GameObject.Find("Player After Image Pool"));
        Destroy(GameObject.Find("Game Manager"));
        Destroy(GameObject.Find("Settings Canvas"));
        Destroy(GameObject.Find("Player UI Canvas"));
        Destroy(GameObject.Find("Bring Over"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        titleImage.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void CloseSettings()
    {
        mainMenu.SetActive(true);
        titleImage.SetActive(true);
        settingsMenu.SetActive(false);
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume); // Ensure volume stays between 0 and 1
    }

}
