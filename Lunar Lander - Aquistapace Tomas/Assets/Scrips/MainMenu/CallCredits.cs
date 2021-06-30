using UnityEngine;

public class CallCredits : MonoBehaviour
{
    [SerializeField] private Animator credits;
    [SerializeField] private GameObject closeButton;

    public void CallCredit()
    {
        credits.SetBool("Start", true);
        credits.SetBool("Close", false);

        closeButton.SetActive(true);
    }

    public void CloseCredit()
    {
        credits.SetBool("Start", false);
        credits.SetBool("Close", true);

        closeButton.SetActive(false);
    }

}
