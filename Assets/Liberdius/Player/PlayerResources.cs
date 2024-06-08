using UnityEngine;

[System.Serializable]
public class PlayerResource
{
    public float value;
    public float maxValue;
    public float rechargeRate;

    public PlayerResource(float maxValue, float rechargeRate)
    {
        this.maxValue = maxValue;
        this.rechargeRate = rechargeRate;
        this.value = maxValue;
    }

    public void Recharge(float deltaTime)
    {
        if (value < maxValue)
        {
            value += rechargeRate * deltaTime;
            value = Mathf.Min(value, maxValue);
        }
    }

    public bool Use(float amount)
    {
        if (value >= amount)
        {
            value -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
