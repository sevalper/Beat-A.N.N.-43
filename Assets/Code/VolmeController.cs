using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolmeController : MonoBehaviour
{
    AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SaveAndLoad.SaveVolumeExists())
        {
            _audioSource.volume = SaveAndLoad.LoadVolume();
        }
    }
}
