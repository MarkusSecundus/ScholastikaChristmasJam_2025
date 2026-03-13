using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    private void Start()
    {
        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || (Gamepad.current?.startButton?.wasPressedThisFrame == true))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGameWoutCanvas()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);
    }
}
