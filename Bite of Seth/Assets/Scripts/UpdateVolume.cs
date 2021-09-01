using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVolume : MonoBehaviour {

    private AudioSource audioSource;
    private AudioManager manager;

    private float masterVolume;
    private float volume;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        manager = ServiceLocator.Get<AudioManager>();
        volume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        masterVolume = GetMasterVolume();
        audioSource.volume = volume * masterVolume;
    }

    private float GetMasterVolume()
    {
        return manager.masterVolume;
    }

}
