using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator credits;

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CallCredits()
    {
        credits.SetBool("Start",  true);
        credits.SetBool("Close",  false);
    }

    public void CloseCredits()
    {
        credits.SetBool("Start",  false);
        credits.SetBool("Close",  true);
    }
}
