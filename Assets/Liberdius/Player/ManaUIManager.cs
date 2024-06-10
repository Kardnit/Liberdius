using UnityEngine;
using UnityEngine.UI;

public class ManaUIManager : MonoBehaviour
{
    public Slider manaSlider;
    public PlayerStats playerStats;

    void Start()
    {
        if (playerStats != null && manaSlider != null)
        {
            manaSlider.maxValue = playerStats.mana.maxValue;
            manaSlider.value = playerStats.mana.value;
        }
    }

    void Update()
    {
        if (playerStats != null && manaSlider != null)
        {
            manaSlider.value = playerStats.mana.value;
        }
    }
}
