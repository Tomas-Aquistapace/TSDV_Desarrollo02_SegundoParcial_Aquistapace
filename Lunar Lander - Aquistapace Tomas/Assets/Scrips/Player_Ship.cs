using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ship : MonoBehaviour
{
    [Header("Stats")]
    public float speedRotation = 2f;
    public float forceImpulse = 2f;
    public float actualAltitude = 0f;

    Rigidbody2D rig;

    private void Start()
    {
        actualAltitude = (int)transform.position.y;

        rig = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputRotation();
        ForceImpulse();
        UpdateAltitude();
    }

    void InputRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Rotate(new Vector3(0, 0, -horizontal), speedRotation * Time.deltaTime);
    }

    void ForceImpulse()
    {
        if (Input.GetKey("space"))
        {
            rig.AddForce(transform.up * forceImpulse, ForceMode2D.Force);
        }
    }

    void UpdateAltitude()
    {
        actualAltitude = transform.position.y;
    }
}
