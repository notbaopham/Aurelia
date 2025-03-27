using UnityEngine;
using System.Collections;

public class WindManager : MonoBehaviour
{
    public enum WindDirection { Left, Right }
    public WindDirection currentDirection;

    public float windForce = 5f;
    public float warningTime = 5f;
    public float windMinTime = 5f;
    public float windMaxTime = 10f;
    public float minDelay = 20f;
    public float maxDelay = 30f;
    private bool windActive;

    public Player player; // reference to your player script
    public UiManager uiManager; // reference to your UI manager

    void Start()
    {
        StartCoroutine(WindCycle());
    }

    void Update()
    {

    }

    IEnumerator WindCycle()
    {
        while (true)
        {
            // Pick a random delay for next wind
            float timeUntilNextWind = Random.Range(minDelay, maxDelay);
            currentDirection = (Random.value > 0.5f) ? WindDirection.Left : WindDirection.Right;

            // Wait until it is 5 seconds away from wind
            yield return new WaitForSeconds(timeUntilNextWind - warningTime);

            // Flickering countdown
            uiManager.ShowWarning(currentDirection.ToString(), warningTime);
            yield return new WaitForSeconds(warningTime);

            // Start wind
            windActive = true;
            player.EnableWindEffect(currentDirection, windForce);

            float windDuration = Random.Range(windMinTime, windMaxTime);
            yield return new WaitForSeconds(windDuration);
            uiManager.ShowWindDuration(windDuration);
            yield return new WaitForSeconds(windDuration);

            // End wind
            player.DisableWindEffect();
            windActive = false;
        }
    }
}
