using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public delegate void RotateArrow(float angle);
    public static RotateArrow RotateFuelArrow;
    public static RotateArrow RotateSpeedArrow;

    [SerializeField] private Transform player;

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI pointsText;

    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] private Image rotationImage;
    [SerializeField] private Image fuelArrowImage;
    [SerializeField] private Image speedArrowImage;

    [SerializeField] private TextMeshProUGUI horizontalText;
    [SerializeField] private TextMeshProUGUI verticalText;

    float initialAngle;
    float initialSpeedAngle;

    void Start()
    {
        initialAngle = fuelArrowImage.rectTransform.rotation.eulerAngles.z;
        initialSpeedAngle = speedArrowImage.rectTransform.rotation.eulerAngles.z;

        fuelText.text = player.GetComponent<Player_Ship>().actualFuel.ToString("0.00");
        speedText.text = player.GetComponent<Player_Ship>().fallSpeed.ToString("0.00");
    }

    private void OnEnable()
    {
        RotateFuelArrow += MoveFuelArrow;
        RotateSpeedArrow += MoveSpeedArrow;
    }

    private void OnDisable()
    {
        RotateFuelArrow -= MoveFuelArrow;
        RotateSpeedArrow -= MoveSpeedArrow;
    }

    void Update()
    {
        levelText.text = LevelManager.instance.actualLevel.ToString();

        pointsText.text = player.GetComponent<Player_Ship>().points.ToString();

        rotationImage.transform.rotation = player.rotation;
    }

    void MoveFuelArrow(float angle)
    {
        angle = (initialAngle / angle) * 5f * Time.deltaTime;

        fuelArrowImage.rectTransform.Rotate(new Vector3(0, 0, angle));
        if (player.GetComponent<Player_Ship>().actualFuel <= 0)
            fuelText.text = "Empty";
        else
            fuelText.text = player.GetComponent<Player_Ship>().actualFuel.ToString("0.00");
    }

    void MoveSpeedArrow(float angle)
    {
        angle = initialSpeedAngle - angle * 3f;

        if (angle < -(initialSpeedAngle - 20f)) angle = -(initialSpeedAngle - 20f);

        speedArrowImage.rectTransform.eulerAngles = new Vector3(0, 0, angle);

        speedText.text = player.GetComponent<Player_Ship>().fallSpeed.ToString("0.00");
        horizontalText.text = Mathf.Abs(player.GetComponent<Player_Ship>().shipSpeed.x).ToString("0.00");
        verticalText.text = Mathf.Abs(player.GetComponent<Player_Ship>().shipSpeed.y).ToString("0.00");
    }
}
