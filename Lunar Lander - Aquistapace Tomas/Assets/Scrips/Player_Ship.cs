using System.Collections;
using UnityEngine;


public class Player_Ship : MonoBehaviour
{
    public delegate void NextLevel();
    public static NextLevel WinLevel;

    [Header("Stats")]
    [SerializeField] private float speedRotation = 2f;
    [SerializeField] private float forceImpulse = 2f;
    [SerializeField] private float gravityAffected = 1f;
    public float actualAltitude = 0f;

    [Header("Points")]
    [SerializeField] private int points = 0;
    [SerializeField] private int basePoints = 100;

    [Header("Fuel Properties")]
    [SerializeField] private float startingFuel = 500f;
    [SerializeField] private float actualFuel;
    [SerializeField] private float fuelConsuption = 2f;

    [Header("Lose Conditions")]
    [SerializeField] private float fallSpeed = 0f;
    [SerializeField] private float limitFallSpeed = 0f;
    [SerializeField] private float limitDegrees = 45f;
    [SerializeField] private float stopSeconds = 1f;

    [Header("Art Assets")]
    public ParticleSystem propellantFire;

    enum States
    {
        isFlying,
        crashed,
        landed
    }
    States actualState;

    Animator anim;
    Rigidbody2D rig;
    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;

        anim = transform.GetComponent<Animator>();
        rig = transform.GetComponent<Rigidbody2D>();
        points = 0;
        actualFuel = startingFuel;

        WinLevel += InitialiceShip;

        InitialiceShip();

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
        if (collision.transform.tag != "SafeZone" ||
            (transform.rotation.z > limitDegrees) || (transform.rotation.z < -limitDegrees) || (fallSpeed >= limitFallSpeed))
        {
            actualState = States.crashed;

            CrashAnimation();

            StartCoroutine(WaitAFewSeconds(false));
        }
        else
        {
            actualState = States.landed;

            points += collision.transform.GetComponent<SafeZone>().EarnPoints(basePoints);

            rig.simulated = false;

            StartCoroutine(WaitAFewSeconds(true));
        }
    }

    IEnumerator WaitAFewSeconds(bool state)
    {
        yield return new WaitForSeconds(stopSeconds);

        if (state) WinLevel();
        else InitialiceShip();
    }

    // -----------------

    void CrashAnimation()
    {
        anim.SetBool("Crash", true);
        rig.simulated = false;
    }

    public void InitialiceShip()
    {
        actualState = States.isFlying;

        transform.position = initialPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        actualAltitude = (int)transform.position.y;

        rig.simulated = true;
        rig.gravityScale = gravityAffected;
        rig.MoveRotation(0);

        rig.velocity = Vector2.zero;

        propellantFire.Stop();

        anim.SetBool("Crash", false);
    }
}
