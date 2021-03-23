using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu(menuName = "Manager/Audio Manager")]
public class AudioManager : GameService
{
    public float masterVolume = 1;
    public AudioObject defaultBGM = null;
    public AudioObject logicBGM = null;
    private AudioSource currentBGM = null;
    private float stoppedBGMTime = 0.0f;
    private List<AudioSource> sources = new List<AudioSource>();

    public override void Start()
    {
        sources = new List<AudioSource>();
        currentBGM = PlayDefaultBGM();
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

    public AudioSource PlayDefaultBGM()
    {
        StopPlayingCurrentBGM();
        return (currentBGM = PlayAudio(defaultBGM));
    }

    public AudioSource PlayLogicBGM()
    {
        StopPlayingCurrentBGM();
        return (currentBGM = PlayAudio(logicBGM));
    }

    public void ChangeToDefaultBGM()
    {
        StopPlayingCurrentBGM();
        currentBGM = PlayAudio(defaultBGM, stoppedBGMTime);
    }

    public void ChangeToLogicBGM()
    {
        StopPlayingCurrentBGM();
        currentBGM = PlayAudio(logicBGM, stoppedBGMTime);
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

}