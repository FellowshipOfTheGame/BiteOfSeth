using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "Manager/Audio Manager")]
public class AudioManager : GameService
{
    public float masterVolume = 1f;
    public AudioObject defaultBGM = null;
    public AudioObject logicBGM = null;
    private AudioSource currentBGM = null;
    private float stoppedBGMTime = 0.0f;
    private List<AudioSource> sources = new List<AudioSource>();
    private List<AudioObject> audioSources = new List<AudioObject>();

    public float BGMVolume = 1f;
    public float DialogueVolume = 1f;

    private AudioSource currentDialogue = null;
    private AudioObject currentDialogueAudio = null;
    private AudioObject currentBGMAudio = null;

    public override void Start()
    {
        sources = new List<AudioSource>();
        //currentBGM = PlayDefaultBGM();
    }
    public override void Update()
    {
        if (Application.isFocused)
        {
            for(int i = sources.Count-1; i >= 0; i--)
            {
                if (sources[i].isPlaying == false)
                {
                    Destroy(sources[i]);
                    sources.Remove(sources[i]);
                    audioSources.Remove(audioSources[i]);
                }
            }
        }
    }

    public AudioSource PlayAudio(AudioObject audio)
    {
        if (audio == null) {
            Debug.LogError("Can't play null audioObject");
            return null;
        }
        Debug.Log($"Playing {audio}");
        AudioSource audSrc = ServiceLocator.AddComponentToHelperObject<AudioSource>();
        sources.Add(audSrc);
        audioSources.Add(audio);
        audSrc.clip = audio.clip;
        audSrc.volume = audio.relativeVolume * masterVolume;
        audSrc.pitch = audio.pitch;
        audSrc.loop = audio.loop;

        audSrc.Play();
        return audSrc;
    }

    public AudioSource PlayAudio(AudioObject audio, float timeToStart)
    {
        if (audio == null) {
            Debug.LogError("Can't play null audioObject");
            return null;
        }
        Debug.Log($"Playing {audio}");
        AudioSource audSrc = ServiceLocator.AddComponentToHelperObject<AudioSource>();
        sources.Add(audSrc);
        audioSources.Add(audio);
        audSrc.clip = audio.clip;
        audSrc.volume = audio.relativeVolume * masterVolume;
        audSrc.pitch = audio.pitch;
        audSrc.loop = audio.loop;
        audSrc.time = timeToStart;

        audSrc.Play();
        return audSrc;
    }

    public void StopPlayingAudio(AudioSource source)
    {
        if (source) {
            source.Stop();
            sources.Remove(source);
        }
    }

    public void StopPlayingCurrentBGM()
    {
        if (currentBGM) {
            stoppedBGMTime = currentBGM.time;
            currentBGM.Stop();
            sources.Remove(currentBGM);
            currentBGM = null;
        }
    }

    public void UpdateDefaultBGM(AudioObject newBGM)
    {
        if(newBGM != defaultBGM) {
            defaultBGM = newBGM;
            PlayDefaultBGM();
        }
    }

    public AudioSource PlayDefaultBGM()
    {
        StopPlayingCurrentBGM();
        return (currentBGM = PlayBGM(defaultBGM));
    }

    public AudioSource PlayLogicBGM()
    {
        StopPlayingCurrentBGM();
        return (currentBGM = PlayBGM(logicBGM));
    }

    public void ChangeToDefaultBGM()
    {
        StopPlayingCurrentBGM();
        currentBGM = PlayBGM(defaultBGM, stoppedBGMTime);
    }

    public void ChangeToLogicBGM()
    {
        StopPlayingCurrentBGM();
        currentBGM = PlayBGM(logicBGM, stoppedBGMTime);
    }

    public void PauseAudio(AudioSource source)
    {
        if (source) {
            source.Pause();
        }
    }

    public void ContinueAudio(AudioSource source)
    {
        if (source) {
            source.Play();
        }
    }

    public AudioSource PlayCustomVolumeAudio(AudioObject audio, float volume)
    {
        if (audio == null) {
            Debug.LogError("Can't play null audioObject");
            return null;
        }
        Debug.Log($"Playing {audio}");
        AudioSource audSrc = ServiceLocator.AddComponentToHelperObject<AudioSource>();
        sources.Add(audSrc);
        audioSources.Add(audio);
        audSrc.clip = audio.clip;
        audSrc.volume = audio.relativeVolume * masterVolume * volume;
        audSrc.pitch = audio.pitch;
        audSrc.loop = audio.loop;

        audSrc.Play();
        return audSrc;
    }

    public AudioSource PlayCustomVolumeAudio(AudioObject audio, float volume, float timeToStart)
    {
        if (audio == null) {
            Debug.LogError("Can't play null audioObject");
            return null;
        }
        Debug.Log($"Playing {audio}");
        AudioSource audSrc = ServiceLocator.AddComponentToHelperObject<AudioSource>();
        sources.Add(audSrc);
        audioSources.Add(audio);
        audSrc.clip = audio.clip;
        audSrc.volume = audio.relativeVolume * masterVolume * volume;
        audSrc.pitch = audio.pitch;
        audSrc.loop = audio.loop;
        audSrc.time = timeToStart;

        audSrc.Play();
        return audSrc;
    }

    public AudioSource PlayBGM(AudioObject audio)
    {
        currentBGMAudio = audio;
        return PlayCustomVolumeAudio(audio, BGMVolume);
    }

    public AudioSource PlayBGM(AudioObject audio, float timeToStart)
    {
        currentBGMAudio = audio;
        return PlayCustomVolumeAudio(audio, BGMVolume, timeToStart);
    }

    public AudioSource PlayDialogue(AudioObject audio)
    {
        currentDialogueAudio = audio;
        return PlayCustomVolumeAudio(audio, DialogueVolume);
    }

    public AudioSource PlayDialogue(AudioObject audio, float timeToStart)
    {
        currentDialogueAudio = audio;
        return PlayCustomVolumeAudio(audio, DialogueVolume, timeToStart);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        for (int i = sources.Count - 1; i >= 0; i--) {
            sources[i].volume =  audioSources[i].relativeVolume * masterVolume;
        }
        SetBGMVolume(BGMVolume);
        SetDialogueVolume(DialogueVolume);
    }

    public void SetBGMVolume(float volume)
    {
        BGMVolume = volume;
        if (currentBGM == null) {
            return;
        }
        currentBGM.volume = BGMVolume * currentBGMAudio.relativeVolume * masterVolume;
    }

    public void SetDialogueVolume(float volume)
    {
        DialogueVolume = volume;
        if (currentDialogue == null) {
            return;
        }
        currentDialogue.volume = DialogueVolume * currentDialogueAudio.relativeVolume * masterVolume;
    }

}