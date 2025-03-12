using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaX, float deltaY);
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector2 oldPosition;

    void Start()
    {
        oldPosition = transform.position;
    }

    void Update()
    {
        if (transform.position.x != oldPosition.x || transform.position.y != oldPosition.y)
        {
            if (onCameraTranslate != null)
            {
                float deltaX = oldPosition.x - transform.position.x;
                float deltaY = oldPosition.y - transform.position.y;
                onCameraTranslate(deltaX, deltaY);
            }

            oldPosition = transform.position;
        }
    }
}
