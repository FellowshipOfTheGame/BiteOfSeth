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
        GeneralVolumeSlider.value = GameData.generalSave.generalVolume;
        UpdateGeneralVolume();
        BGMVolumeSlider.value = GameData.generalSave.BGMVolume;
        UpdateBGMVolume();
        DialogueVolumeSlider.value = GameData.generalSave.dialogueVolume;
        UpdateDialogueVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateGeneralVolume()
    {
        ServiceLocator.Get<AudioManager>().SetMasterVolume(GeneralVolumeSlider.value);
        GameData.generalSave.generalVolume = ServiceLocator.Get<AudioManager>().masterVolume;
        SaveSystem.SaveGeneral();
    }

    public void UpdateBGMVolume()
    {
        ServiceLocator.Get<AudioManager>().SetBGMVolume(BGMVolumeSlider.value);
        GameData.generalSave.BGMVolume = ServiceLocator.Get<AudioManager>().BGMVolume;
        SaveSystem.SaveGeneral();
    }

    public void UpdateDialogueVolume()
    {
        ServiceLocator.Get<AudioManager>().SetDialogueVolume(DialogueVolumeSlider.value);
        GameData.generalSave.dialogueVolume = ServiceLocator.Get<AudioManager>().DialogueVolume;
        SaveSystem.SaveGeneral();
    }

}
