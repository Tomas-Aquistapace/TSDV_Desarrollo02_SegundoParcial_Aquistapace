using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform playerRef;
    public Vector3 offset;
    [SerializeField]
    bool enableZoom = false;
    bool followPlayer = false;


    Camera cam;
    float zoomLimit;

    void Start()
    {
        cam = transform.GetComponent<Camera>();

        zoomLimit = playerRef.GetComponent<Player_Ship>().limitAltitude;
    }

    void Update()
    {
        Zoom();
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (followPlayer)
        {
            Vector3 desiredPos = playerRef.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, 0.125f);
        }
    }

    void Zoom()
    {
        if(playerRef.transform.position.y <= 0 && enableZoom == false)
        {
            StartCoroutine(TranslateToPlayer());
        }
        else if(playerRef.transform.position.y > 0 && enableZoom == true)
        {
            enableZoom = false;
            followPlayer = false;
        }
    }

    IEnumerator TranslateToPlayer()
    {
        enableZoom = true;
        float time = 0;

        while (time <= 1)
        {
            Vector3 desiredPos = playerRef.position + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPos, time);

            time += 0.2f * Time.deltaTime;
            yield return null;
        }

        followPlayer = true;
    }
}
