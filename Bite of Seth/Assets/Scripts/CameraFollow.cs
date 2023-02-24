using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject _player;
    private GameObject player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerController>().gameObject;
            }
            return _player;
        }
    }

    public Vector3 offset;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 playerPos;

    private Vector3 cameraPos;

    private bool inTransition = false;

    public float defaultSize = 3f;

    private bool focusOnOthers = false;

    private void Start()
    {
        GetComponent<Camera>().orthographicSize = defaultSize;
        focusOnOthers = false;
    }

    void LateUpdate()
    {
        if (!focusOnOthers) {
            cameraPos = player.transform.position + offset;
        }

        if (!inTransition) {
            transform.position = cameraPos;
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, smoothTime);
            inTransition = (Vector3.Distance(transform.position, cameraPos) > 0.1);
            //transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
            //transform.position = Vector3.Lerp(transform.position, playerPos, smoothTime);
        }
    }

    public void SmoothTransition()
    {
        inTransition = true;
    }

    public void ChangeToCustomSize(float size)
    {
        GetComponent<Camera>().orthographicSize = size;
    }

    public void ChangeToDefaultSize()
    {
        GetComponent<Camera>().orthographicSize = defaultSize;
    }

    public void FocusCameraOnXDuringYSeconds(Vector3 x, float y)
    {
        focusOnOthers = true;
        cameraPos = x + offset;
        ServiceLocator.Get<GameManager>().StopPlayerControls();
        Invoke("FocusOnPlayer", y);
    }

    public void FocusOnPlayer()
    {
        focusOnOthers = false;
        ServiceLocator.Get<GameManager>().ResumePlayerControls();
    }

}
