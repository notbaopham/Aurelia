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

    [SerializeField] private ParticleSystem windParticleLeft;
    [SerializeField] private ParticleSystem windParticleRight;

    void Start()
    {
        // Stop the particle effect

        windParticleLeft.Stop();
        windParticleLeft.Clear();
        windParticleRight.Stop();
        windParticleRight.Clear();
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
            Wind.WindDirection windDir = (currentDirection == WindDirection.Left)
                ? Wind.WindDirection.Left
                : Wind.WindDirection.Right;

            Wind.Instance.EnableWind(windDir, windForce);

            // Enable the corresponding particle effect
            if (currentDirection == WindDirection.Left)
            {
                windParticleLeft.Play();
                windParticleRight.Stop(); // Just in case the other one is still playing
            }
            else
            {
                windParticleRight.Play();
                windParticleLeft.Stop();
            }

            float windDuration = Random.Range(windMinTime, windMaxTime);
            yield return new WaitForSeconds(windDuration);
            uiManager.ShowWindDuration(windDuration);
            yield return new WaitForSeconds(windDuration);

            // End wind
            Wind.Instance.DisableWind();
            windActive = false;

            // Stop the particle effect
            windParticleLeft.Stop();
            windParticleRight.Stop();
        }
    }
}
