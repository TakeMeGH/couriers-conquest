using UnityEngine;
using CC.Core.Data.Dynamic;
using CC.Characters;
using CC.Characters.States;
using System.Collections;

public class StaminaController : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public PlayerControllerStatesMachine playerController;
    public float staminaIncreaseRate = 5f;
    public float staminaRegenDelay = 3f; // 3 detik delay irl
    private float currentStamina;
    private bool isRegenerating;
    public bool CanBlock { get; private set; }
    public float blockStaminaReq = 10f; //blockStaminaRequirement

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    private void Start()
    {
        currentStamina = playerStats.GetInstanceValue(mainStat.Stamina);
    }

    private void Update()
    {
        HandleStamina();

        CanBlock = currentStamina >= blockStaminaReq; 
    }

    private void HandleStamina()
    {
        if (!isRegenerating)
        {
            StartCoroutine(RegenerateStamina());
        }
    }

    public void DecreaseStaminaByAmount(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Max(currentStamina, 0);
        playerStats.SetInstanceValue(mainStat.Stamina, currentStamina);
    }

    private IEnumerator RegenerateStamina()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(staminaRegenDelay);

        while (currentStamina < playerStats.GetValue(mainStat.MaxStamina))
        {
            // Stop regen stamina kalau player mulai sprinting, dashing, atau blocking lagi pas Stamina lagi diposisi regenerasi
            var currentState = playerController.GetCurrentState();
            if (currentState is PlayerSprintingState || currentState is PlayerDashingState || currentState is PlayerBlockingState)

            {
                isRegenerating = false;
                yield break;
            }

            currentStamina += staminaIncreaseRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, playerStats.GetValue(mainStat.MaxStamina));
            playerStats.SetInstanceValue(mainStat.Stamina, currentStamina);
            yield return null;
        }

        isRegenerating = false;
    }
}
