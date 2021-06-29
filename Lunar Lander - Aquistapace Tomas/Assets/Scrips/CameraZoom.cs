using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform playerRef;
    public Vector3 offset;
    public Vector3 zoomLimit;
    [Range(0,1)] public float smoothSpeed = 0.125f;
    public float limitAltitude = -35f;
    
    
    Vector3 initialPos;
    bool enableZoom = false;
    bool followPlayer = false;

    void Start()
    {
        initialPos = transform.position;
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
            float altitude = playerRef.position.y + limitAltitude;

            Vector3 zPosition = offset + zoomLimit;
            zPosition.z = Mathf.Clamp(zPosition.z - altitude, offset.z, zoomLimit.z);

            Vector3 desiredPos = playerRef.position + zPosition;

            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, initialPos, smoothSpeed);
        }
    }

    void Zoom()
    {
        if(playerRef.transform.position.y <= 0 && enableZoom == false)
        {
            enableZoom = true;
            followPlayer = true;
        }
        else if(playerRef.transform.position.y > 0 && enableZoom == true)
        {
            enableZoom = false;
            followPlayer = false;
        }
    }
}
