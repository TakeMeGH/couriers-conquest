using System.Collections;
using System.Collections.Generic;
using CC.Characters;
using UnityEngine.Events;
using CC.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIPlayerDied : MonoBehaviour
{
    [SerializeField] PlayerControllerStatesMachine  _playerController;
    public Button mainMenuButton;
    public Button respawnButton;
    public Button loadGameButton;
    public GameObject gameOverUI;
    public GameObject blankPanel;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(MainMenu);
        respawnButton.onClick.AddListener(Respawn);
        loadGameButton.onClick.AddListener(LoadGame);

        if (_playerController._onPlayerDead != null)
        {
            Debug.Log("Ini ada isinya");
            _playerController._onPlayerDead.OnEventRaised += gameOver;
        }
        else
            Debug.LogError("VoidEventChannelSO _onPlayerDead is not assigned.");
    }

    private void OnDestroy()
    {
        if (_playerController._onPlayerDead == null)
            Debug.Log("Died");
            _playerController._onPlayerDead.OnEventRaised -= gameOver;
    }

    public void gameOver()
    {
        Debug.Log("Player Already Dead");
        gameOverUI.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Respawn()
    {
        
    }

    public void LoadGame()
    {
        blankPanel.SetActive(true);
    }
}
