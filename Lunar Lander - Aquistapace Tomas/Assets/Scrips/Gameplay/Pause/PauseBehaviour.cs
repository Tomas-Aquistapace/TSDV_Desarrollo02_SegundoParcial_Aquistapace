using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseButton;

    public void ActivatePause()
    {
        pauseScreen.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;
    }

    public void DisablePause()
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;
    }

    public void RestartAndDisablePause(string scene)
    {
        pauseScreen.SetActive(false);
        pauseButton.SetActive(true);

        Time.timeScale = 1f;

        SceneManager.LoadScene(scene);
    }
}
