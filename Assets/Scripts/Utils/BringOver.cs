using UnityEngine;

public class BringOver : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(true);
        if (FindObjectsByType<SettingsCanvas>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return; 
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
