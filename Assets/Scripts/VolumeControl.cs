using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;
        ChangeGlobalAudio(volume);
    }

    public void ChangeGlobalAudio(float audioLevel)
    {
        AudioListener.volume = audioLevel;
        PlayerPrefs.SetFloat("Volume", audioLevel);
    }
}
