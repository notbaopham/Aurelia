using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 2f;
    public float yOffset = 1f;

    [SerializeField] private Camera cam;
    [SerializeField] private float zoomLevel = 5f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;
    public float timeUntilAttach = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StartCoroutine(WaitUntilAttach());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return; // Exit if target is not set
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Lerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);

        // Adjust zoom level
        cam.orthographicSize = Mathf.Clamp(zoomLevel, minZoom, maxZoom);
    }

    IEnumerator WaitUntilAttach() {
        yield return new WaitForSeconds(timeUntilAttach);
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
