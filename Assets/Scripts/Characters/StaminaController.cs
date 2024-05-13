using UnityEngine;
using CC.Core.Data.Dynamic;
using CC.Characters;
using CC.Characters.States;
using System.Collections;

public class StaminaController : MonoBehaviour
{
    public PlayerControllerStatesMachine playerController;
    public float staminaIncreaseRate = 5f;
    public float staminaRegenDelay = 3f; // 3 detik delay irl
    private float currentStamina;
    private bool isRegenerating;
    public bool CanBlock { get; private set; }
    public float blockStaminaReq = 10f; //blockStaminaRequirement
    PlayerStatsSO _statsSO;

    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public void SetStats(PlayerStatsSO statsSO)
    {
        _statsSO = statsSO;
        Init();
    }


    private void Init()
    {
        currentStamina = _statsSO.GetInstanceValue(mainStat.Stamina);
    }


    private void Update()
    {
        if (_statsSO == null) return;

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
        _statsSO.SetInstanceValue(mainStat.Stamina, currentStamina);
    }

    private IEnumerator RegenerateStamina()
    {
        isRegenerating = true;
        yield return new WaitForSeconds(staminaRegenDelay);

        while (currentStamina < _statsSO.GetValue(mainStat.Stamina))
        {
            // Stop regen stamina kalau player mulai sprinting, dashing, atau blocking lagi pas Stamina lagi diposisi regenerasi
            var currentState = playerController.GetCurrentState();
            if (currentState is PlayerSprintingState || currentState is PlayerDashingState || currentState is PlayerBlockingState)

            {
                isRegenerating = false;
                yield break;
            }

            currentStamina += staminaIncreaseRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, _statsSO.GetValue(mainStat.Stamina));
            _statsSO.SetInstanceValue(mainStat.Stamina, currentStamina);
            yield return null;
        }

        isRegenerating = false;
    }
}
