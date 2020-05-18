using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionControls : MonoBehaviour
{
    public Slider sliderVolume;
    public AudioSource audioSource;

    public void SaveVolume()
    {
        SaveAndLoad.SaveVolume(sliderVolume.value);
        audioSource.volume = sliderVolume.value;
    }
}
