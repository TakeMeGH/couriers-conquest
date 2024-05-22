using UnityEngine;
using CC.Core.Data.Dynamic;
using CC.Characters;
using CC.Characters.States;
using System.Collections;
using CC.StateMachine;
using System;
using System.Collections.Generic;

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
    private Coroutine _regenerateCourutine;

    List<Type> activeStates = new List<Type> {
        typeof(PlayerSprintingState),
        typeof(PlayerDashingState),
        typeof(PlayerBlockingState),
        typeof(PlayerClimbState),
    };


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
        if (_regenerateCourutine == null)
        {
            _regenerateCourutine = StartCoroutine(RegenerateStamina());
        }
    }

    public void DecreaseStaminaByAmount(float amount)
    {
        if (_regenerateCourutine != null)
        {
            StopCoroutine(_regenerateCourutine);
            _regenerateCourutine = null;

        }

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
            // Stop regen stamina kalau player berada di state activeStates
            var currentState = playerController.GetCurrentState();
            foreach (var state in activeStates)
            {
                if (currentState.GetType() == state)
                {
                    isRegenerating = false;
                    yield break;
                }
            }
            currentStamina += staminaIncreaseRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, _statsSO.GetValue(mainStat.Stamina));
            _statsSO.SetInstanceValue(mainStat.Stamina, currentStamina);
            yield return null;
        }

        isRegenerating = false;
    }
}
