using System.Collections;
using UnityEngine;

public class Player_Ship : MonoBehaviour
{
    public delegate void NextLevel();
    public static NextLevel WinLevel;
    public delegate void EndGame();
    public static EndGame FinishMission;

    [Header("Stats")]
    [SerializeField] private float speedRotation = 2f;
    [SerializeField] private float forceImpulse = 2f;
    [SerializeField] private float gravityAffected = 1f;
    public float actualAltitude = 0f;

    [Header("Points")]
    public int points = 0;
    [SerializeField] private int basePoints = 100;

    [Header("Fuel Properties")]
    [SerializeField] private float startingFuel = 500f;
    public float actualFuel;
    [SerializeField] private float fuelConsuption = 2f;

    [Header("Lose Conditions")]
    public float fallSpeed = 0f;
    [HideInInspector] public Vector2 shipSpeed;
    [SerializeField] private float limitFallSpeed = 0f;
    [SerializeField] private float limitDegrees = 45f;
    [SerializeField] private float stopSeconds = 1f;

    [Header("Art Assets")]
    public ParticleSystem propellantFire;

    enum States
    {
        isFlying,
        crashed,
        landed,
        finishMission
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

        InitialiceShip();

        propellantFire.Stop();
    }

    private void OnEnable()
    {
        WinLevel += InitialiceShip;
    }

    private void OnDisable()
    {
        WinLevel -= InitialiceShip;
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

        if (transform.eulerAngles.z <= 90.0f || transform.eulerAngles.z >= 270)
        {
            transform.Rotate(new Vector3(0, 0, -horizontal), speedRotation * Time.deltaTime);
        }
        if (transform.eulerAngles.z > 90.0f && transform.eulerAngles.z < 180.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
        }
        if (transform.eulerAngles.z < 270.0f && transform.eulerAngles.z > 180.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
        }
    }

    void ForceImpulse()
    {
        if ((Input.GetKey("space") || Input.GetKey(KeyCode.UpArrow)) && actualFuel > 0)
        {
            rig.AddForce(transform.up * forceImpulse, ForceMode2D.Force);

            actualFuel -= fuelConsuption * Time.deltaTime;

            UI_Manager.RotateFuelArrow?.Invoke(startingFuel);

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

        shipSpeed = rig.velocity;

        fallSpeed = shipSpeed.magnitude * 10f;

        UI_Manager.RotateSpeedArrow?.Invoke(fallSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "SafeZone" ||
            (transform.rotation.z > limitDegrees) || (transform.rotation.z < -limitDegrees) || (fallSpeed >= limitFallSpeed))
        {
            actualState = States.crashed;

            CrashAnimation();

            if (actualFuel <= 0)
            {
                actualState = States.finishMission;
                StartCoroutine(WaitAFewSeconds("Finish"));
            }
            else
            {
                StartCoroutine(WaitAFewSeconds("Crash"));
            }
        }
        else
        {
            actualState = States.landed;

            points += collision.transform.GetComponent<SafeZone>().EarnPoints(basePoints);

            rig.simulated = false;

            if (actualFuel <= 0)
            {
                actualState = States.finishMission;
                StartCoroutine(WaitAFewSeconds("Finish"));
            }
            else
            {
                StartCoroutine(WaitAFewSeconds("Win"));
            }
        }
    }

    IEnumerator WaitAFewSeconds(string situation)
    {
        yield return new WaitForSeconds(stopSeconds);

        switch (situation)
        {
            case "Crash":
                InitialiceShip();
                break;
            case "Win":
                WinLevel();
                break;
            case "Finish":
                FinishMission();
                break;
        }
    }

    // -----------------

    void CrashAnimation()
    {
        anim.SetBool("Crash", true);
        rig.simulated = false;
    }

    void InitialiceShip()
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
