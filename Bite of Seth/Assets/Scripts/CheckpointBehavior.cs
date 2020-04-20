using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour
{
    RoomBehavior room = null;
    
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
            p.AssignCheckpoint(this);
        }
    }
}
