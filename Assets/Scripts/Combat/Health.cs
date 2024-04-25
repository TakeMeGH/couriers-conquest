using UnityEngine;
using CC.Core.Data.Dynamic;
using CC.Characters;

namespace CC.Combats
{
    public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageReduction = 0f; // 20% damage reduction
    [SerializeField] private float blockDamageReduction = 0f; // 50% additional reduction when blocking
    [SerializeField] private PlayerStatsSO playerStatsSO;
    [SerializeField] private PlayerControllerStatesMachine _playerController;

    private int health;
    private bool isBlocking;

    private void Start()
    {
        health = maxHealth;
        Debug.Log("Initial Player Health: " + health);
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void SetBlocking(bool isBlocking)
    {
        this.isBlocking = isBlocking;
    }

    public int DealDamage(int damage)
{  
    Debug.Log("Received Damage: " + damage);

    if (health == 0) { return 0; }

    float totalReduction = playerStatsSO.GetDamageReduction(); 

    if (isBlocking)
    {
        totalReduction += _playerController.CalculateTotalShieldValue() / 100f; // Modify this line
    }
    int calculatedDamage = Mathf.Min(Mathf.RoundToInt(damage * (1 - totalReduction)), health);
    health = Mathf.Max(health - calculatedDamage, 0);
    Debug.Log("Calculated Damage after Reduction: " + calculatedDamage);
    Debug.Log("Player Health after Damage: " + health);

    return calculatedDamage;
}




}

}
