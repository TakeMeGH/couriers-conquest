using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    [SerializeField] CanvasGroup _HUD;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button mainMenuButton;
    public Button resumeButton;

    private void OnEnable()
    {
        _inputReader.PausePerformed += OnPausePressed;
    }

    private void OnDisable()
    {
        _inputReader.PausePerformed -= OnPausePressed;
    }

    private void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
        resumeButton.onClick.AddListener(Resume);
    }
    void OnPausePressed()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else
        {
            StartCoroutine(Pause());
        }

    }

    void Resume()
    {
        Time.timeScale = 1f;
        _inputReader.EnableGameplayInput();
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        _HUD.alpha = 1f;
    }

    IEnumerator Pause()
    {
        _HUD.alpha = 0f;
        _inputReader.EnableInventoryUIInput();
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0f;

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
