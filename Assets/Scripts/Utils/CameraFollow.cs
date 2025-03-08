using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Lerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
