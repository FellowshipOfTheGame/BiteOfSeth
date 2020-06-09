using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDelay : MonoBehaviour
{
    private bool started;
    public float delayTime = 0f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0) timer -= Time.fixedDeltaTime;
    }

    public bool IsOff()
    {
        return !started;
    }

    public bool IsOn()
    {
        return started;
    }

    public void TurnOn()
    {
        started = true;
        timer = delayTime;
    }

    public void TurnOff()
    {
        started = false;
        //...
    }

    public bool IsFinished()
    {
        return (timer <= 0); 
    }

    public void Restart()
    {
        timer = delayTime;
    }

}
