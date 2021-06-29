using UnityEngine;
using TMPro;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private int multiplyValue;
    [SerializeField] private TextMeshProUGUI value;

    private void Start()
    {
        value.text = "x" + multiplyValue.ToString();
    }

    public int EarnPoints(int value)
    {
        return value * multiplyValue;
    }
}
