using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    [SerializeField] RoomBehavior room = null;
    public GameObject spriteActive;
    public GameObject spriteOff;
    int curScore;
    public AudioObject sfx;
    private bool active = false;

    private void Start()
    {
        SetCheckpointActive(false);
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetCheckpointActive(bool value)
    {
        active = value;
        if (spriteActive)
        {
            spriteActive.SetActive(value);
        }
        if (spriteOff) 
        { 
            spriteOff.SetActive(!value); 
        }
    }

    public void SetRoom(RoomBehavior r)
    {
        room = r;
    }

    public void RewindRoom()
    {
        if (room == null)
        {
            Debug.LogError("checkpoint room shouldn't be null");
        }
        else
        {
            room.DespawnRoom();
            room.SpawnRoom();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision");
        PlayerController p = collision.collider.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            if (!IsActive()) {
                p.AssignCheckpoint(this);
                ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
                SetCheckpointActive(true);
            }
        }
    }
}
