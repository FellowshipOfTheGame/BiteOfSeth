using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Slider GeneralVolumeSlider;
    public Slider BGMVolumeSlider;
    public Slider DialogueVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGeneralVolume()
    {
        ServiceLocator.Get<AudioManager>().SetMasterVolume(GeneralVolumeSlider.value);
    }

    public void UpdateBGMVolume()
    {
        ServiceLocator.Get<AudioManager>().SetBGMVolume(BGMVolumeSlider.value);
    }

    public void UpdateDialogueVolume()
    {
        ServiceLocator.Get<AudioManager>().SetDialogueVolume(DialogueVolumeSlider.value);
    }

}
