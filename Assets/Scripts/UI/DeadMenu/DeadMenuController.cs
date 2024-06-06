using System;
using CC.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CC.UI
{
    public class DeadMenuController : MonoBehaviour
    {
        [SerializeField] GameObject _deadMenu;
        [SerializeField] Button _continueButton;
        [SerializeField] Button _exitButton;
        [SerializeField] VoidEventChannelSO _onPlayerDeath;
        [SerializeField] VoidEventChannelSO _onItemDestroyed;
        [SerializeField] InputReader _inputReader;


        private void OnEnable()
        {
            _onPlayerDeath.OnEventRaised += OnPlayerDeath;
            _onItemDestroyed.OnEventRaised += OnPlayerDeath;
        }
        private void OnDisable()
        {
            _onPlayerDeath.OnEventRaised -= OnPlayerDeath;
            _onItemDestroyed.OnEventRaised -= OnPlayerDeath;
        }

        private void Start()
        {
            _continueButton.onClick.AddListener(OnContinue);
            _exitButton.onClick.AddListener(OnExit);

        }

        void OnPlayerDeath()
        {
            Debug.Log("PLAYER DEATH");
            _deadMenu.SetActive(true);
            _inputReader.EnableInventoryUIInput();
        }

        private void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void OnContinue()
        {
            // TODO
        }
    }
}
