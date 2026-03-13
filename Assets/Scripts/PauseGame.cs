using MarkusSecundus.Utils.Behaviors.GameObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    private bool isPaused = false;

    [SerializeField] Slider _lookSensitivitySlider;

    PlayerController _player;
    Vector2 _ogLookSpeed;


    const string LookSpeedName = "LookSpeed";
    private void Start()
    {
        _player = TagSearchable.FindByTag<PlayerController>("Player");
        _ogLookSpeed = _player.Tweaks.TotalLookSpeed;
        _onSensitivityChanged(_lookSensitivitySlider.value = PlayerPrefs.GetFloat(LookSpeedName, 1f));
        _lookSensitivitySlider.onValueChanged.AddListener(_onSensitivityChanged);
        ResumeGame();
    }

    void _onSensitivityChanged(float newValue)
    {
        _player.Tweaks.TotalLookSpeed = _ogLookSpeed * newValue;
        PlayerPrefs.SetFloat(LookSpeedName, newValue);
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        PlayerPrefs.Save();
    }
    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void ResumeGame()
    {
        PlayerPrefs.Save();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        isPaused = false;
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);
    }
}
