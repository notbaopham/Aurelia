using System.Collections.Generic;

using UnityEngine;

public class PlayerAfterImagesPool : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImagePrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static PlayerAfterImagesPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
        if (FindObjectsByType<PlayerAfterImagesPool>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return; 
        }
        DontDestroyOnLoad(gameObject);
    }

    private void GrowPool() {
        for (int i = 0; i < 10; i++) {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance) {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool() {

        if (availableObjects.Count == 0) {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.SetActive(true);

        return instance;
    }

}
