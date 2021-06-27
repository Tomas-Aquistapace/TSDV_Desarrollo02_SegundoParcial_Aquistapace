using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ship : MonoBehaviour
{
    [Header("Stats")]
    public float speedRotation = 2f;
    public float forceImpulse = 2f;
    public float actualAltitude = 0f;
    public float gravityAffected = 1f;

    [Header("Fuel Properties")]
    public float startingFuel = 500f;
    public float actualFuel;
    public float fuelConsuption = 2f;

    [Header("Lose Conditions")]
    public float fallSpeed = 0f;
    public float limitFallSpeed = 0f;
    public float limitDegrees = 45f;

    [Header("Art Assets")]
    public ParticleSystem propellantFire;

    enum States
    {
        isFlying,
        crashed,
        landed
    }
    States actualState;
    Rigidbody2D rig;

    private void Start()
    {
        actualState = States.isFlying;

        fallSpeed = 0;
        actualFuel = startingFuel;
        actualAltitude = (int)transform.position.y;

        rig = transform.GetComponent<Rigidbody2D>();

        rig.gravityScale = gravityAffected;

        propellantFire.Stop();
    }

    private void Update()
    {
        if(actualState == States.isFlying)
        {
            InputRotation();
            ForceImpulse();
            UpdateAltitude();
        }
        else
        {
            propellantFire.Stop();
        }
    }

    void InputRotation()
    {
        float horizontal = Input.GetAxis("Horizontal");

        transform.Rotate(new Vector3(0, 0, -horizontal), speedRotation * Time.deltaTime);
    }

    void ForceImpulse()
    {
        if (Input.GetKey("space") && actualFuel > 0)
        {
            rig.AddForce(transform.up * forceImpulse, ForceMode2D.Force);

            actualFuel -= fuelConsuption * Time.deltaTime;
            
            if (actualFuel < 0)
                actualFuel = 0;

            propellantFire.Play();
        }
        else
        {
            propellantFire.Stop();
        }
    }

    void UpdateAltitude()
    {
        actualAltitude = transform.position.y;

        Vector3 vel = rig.velocity;

        fallSpeed = vel.magnitude * 10f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((transform.rotation.z > limitDegrees) || (transform.rotation.z < -limitDegrees) || (fallSpeed >= limitFallSpeed))
        {
            actualState = States.crashed;
        }
        else
        {
            actualState = States.landed;
        }
    }
}
