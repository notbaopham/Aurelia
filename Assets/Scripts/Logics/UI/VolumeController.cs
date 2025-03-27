using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; // The slider in the first scene

    void Start()
    {   
        // StartCoroutine(WaitForAudioManager());
        // Ensure the slider reflects the current volume
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.masterVolume; // Set slider value to current volume
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged); // Bind to the AudioManager's method
        }
    }

    // Called when the slider value changes
    private void OnVolumeChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value); // Update volume in AudioManager
    }
    private IEnumerator WaitForAudioManager()
    {
        yield return new WaitForSeconds(0.5f);
        if (AudioManager.Instance != null)
        {
            volumeSlider.value = AudioManager.Instance.masterVolume; // Set slider value to current volume
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged); // Bind to the AudioManager's method
        }
    }
}