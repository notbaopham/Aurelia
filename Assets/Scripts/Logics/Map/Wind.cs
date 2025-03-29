using UnityEngine;

public class Wind : MonoBehaviour
{
    public enum WindDirection { Left, Right }

    public bool windActive = false;
    public WindDirection windDirection = WindDirection.Right;
    public float windForce = 0.1f;

    private static Wind _instance;
    public static Wind Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void EnableWind(WindDirection dir, float force)
    {
        windActive = true;
        windDirection = dir;
        windForce = force;
    }

    public void DisableWind()
    {
        windActive = false;
    }
}
