using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerResource mana;

    void Awake()
    {
        mana = new PlayerResource(100.0f, 2.5f);
    }

    void Update()
    {
        mana.Recharge(Time.deltaTime);
    }
}
