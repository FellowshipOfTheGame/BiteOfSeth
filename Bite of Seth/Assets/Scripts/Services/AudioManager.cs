using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Manager/Audio Manager")]
public class AudioManager : GameService
{
    public float masterVolume = 1;
    public AudioObject defaultBGM = null;
    private List<AudioSource> sources = new List<AudioSource>();

    public override void Start()
    {
        sources = new List<AudioSource>();
        PlayAudio(defaultBGM);
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
    
}