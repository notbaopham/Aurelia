using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return; 
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }  
    
}
