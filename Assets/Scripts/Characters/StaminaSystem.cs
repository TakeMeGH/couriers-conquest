/*using UnityEngine;
using UnityEngine.UI;
using CC.Core.Data.Dynamic;

public class StaminaSystem : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public Slider staminaSliderBar;
    public float sprintStaminaCost = 10f; // Stamina cost per second of sprinting
    public float dashStaminaCost = 20f; // Stamina cost per second of dashing
    public float staminaRegenRate = 5f; // The rate at which stamina regenerates
    public float regenDelay = 3f; // The delay before stamina starts regenerating

    private float lastStaminaUseTime; // The last time the player used stamina
    private bool isSprinting; // Whether the player is currently sprinting

    // Reference to your InputReader
    public InputReader inputReader;

    private void OnEnable()
    {
        // Subscribe to the StartedSprinting and StopedSprinting events
        inputReader.StartedSprinting += OnStartedSprinting;
        inputReader.StopedSprinting += OnStopedSprinting;
    }

    private void OnDisable()
    {
        // Unsubscribe from the StartedSprinting and StopedSprinting events
        inputReader.StartedSprinting -= OnStartedSprinting;
        inputReader.StopedSprinting -= OnStopedSprinting;
    }

    private void Update()
    {
        // Update the stamina bar's fill amount to match the player's current stamina
        staminaSliderBar.value = playerStats.GetStatData().instanceValue[mainStat.Stamina] / playerStats.GetStatData().instanceValue[mainStat.MaxStamina];

        // Print the current stamina to the console
        Debug.Log("Current stamina: " + playerStats.GetStatData().instanceValue[mainStat.Stamina]);

        // Check if the player is sprinting
        if (isSprinting)
        {
            // Reduce the player's stamina based on the cost per second
            UseStamina(Time.deltaTime * sprintStaminaCost);
        }

        // Regenerate stamina after a delay
        else if (playerStats.GetStatData().instanceValue[mainStat.Stamina] < playerStats.GetStatData().instanceValue[mainStat.MaxStamina] && Time.time >= lastStaminaUseTime + regenDelay)
        {
            float newStamina = playerStats.GetStatData().instanceValue[mainStat.Stamina] + staminaRegenRate * Time.deltaTime;

            // Ensure the player's stamina doesn't go above the max
            newStamina = Mathf.Min(newStamina, playerStats.GetStatData().instanceValue[mainStat.MaxStamina]);

            playerStats.SetValue(mainStat.Stamina, newStamina);

            // Print a message to the console when stamina is regenerating
            Debug.Log("Stamina is regenerating...");
        }
    }

    // Called when the player starts sprinting
    private void OnStartedSprinting()
    {
        // Check if the player has enough stamina to sprint
        if (HasEnoughStamina(sprintStaminaCost))
        {
            isSprinting = true;
        }
        else
        {
            // Not enough stamina to sprint, play a sound or show a message
            Debug.Log("Not enough stamina to sprint!");
        }
    }

    // Called when the player stops sprinting
    private void OnStopedSprinting()
    {
        isSprinting = false;
    }

    // Check if the player has enough stamina for a certain action
    public bool HasEnoughStamina(float cost)
    {
        return playerStats.GetStatData().instanceValue[mainStat.Stamina] >= cost;
    }

    // Reduce the player's stamina by a certain amount
    public void UseStamina(float cost)
    {
        float newStamina = playerStats.GetStatData().instanceValue[mainStat.Stamina] - cost;

        // Ensure the player's stamina doesn't go below 0
        newStamina = Mathf.Max(newStamina, 0);

        playerStats.SetValue(mainStat.Stamina, newStamina);

        // Update the last stamina use time
        lastStaminaUseTime = Time.time;
    }
}
*/