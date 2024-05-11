using UnityEngine;
using UnityEngine.UI;
using CC.Core.Data.Dynamic;

public class StaminaBar : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public Image staminaBar; // StaminaSliderBar image ntar di drag kesini di inspector

    private void Update()
    {
        float currentStamina = playerStats.GetInstanceValue(mainStat.Stamina);
        float maxStamina = playerStats.GetValue(mainStat.MaxStamina);
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
