using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject CreditsPanel;
    public GameObject ControlsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void OpenCredits()
    {
        CreditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        CreditsPanel.SetActive(false);
    }

    public void OpenControls()
    {
        ControlsPanel.SetActive(true);
    }

    public void CloseControls()
    {
        ControlsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed!");
        Application.Quit();
    }
}