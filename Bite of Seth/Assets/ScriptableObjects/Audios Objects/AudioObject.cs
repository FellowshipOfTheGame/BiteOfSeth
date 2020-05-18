using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio Object")]
public class AudioObject : ScriptableObject
{
    public AudioClip clip = null;
    public float relativeVolume = 1f;
    public float pitch = 1f;
    public bool loop = false;
}
