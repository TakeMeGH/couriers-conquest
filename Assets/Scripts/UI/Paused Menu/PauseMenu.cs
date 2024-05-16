using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputReader _inputReader;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button mainMenuButton;
    public Button resumeButton;


    private void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
        resumeButton.onClick.AddListener(Resume);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    void Resume()
    {
        Time.timeScale = 1f;
        _inputReader.EnableGameplayInput();
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
    }

    IEnumerator Pause()
    {
        _inputReader.EnableInventoryUIInput();
        yield return null;
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        Time.timeScale = 0f;

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}