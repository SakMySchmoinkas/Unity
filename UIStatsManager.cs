using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatsManager : MonoBehaviour
{
    public Image healthBar;
    public Image staminaBar;
    public Image XPBar;

    public TextMeshProUGUI healthCount;
    public TextMeshProUGUI staminaCount;
    public TextMeshProUGUI XPCount;

    public player playerScript; // Assuming Player is the correct type

    // Start is called before the first frame update
    void Start()
    {
        playerScript = FindObjectOfType<player>(); // Assuming Player is the correct type
    }

    // Update is called once per frame
    void Update()
{
    // Check if playerScript is not null
    if (playerScript != null)
    {
        // Update fill amounts for progress bars
        healthBar.fillAmount = playerScript.health / playerScript.maxHealth;
        staminaBar.fillAmount = playerScript.stamina / playerScript.maxStamina;
        XPBar.fillAmount = playerScript.currentXP / playerScript.maxXP;

        // Update text values (rounded to remove decimals)
        healthCount.text = $"{Mathf.Round(playerScript.health)} / {Mathf.Round(playerScript.maxHealth)}";
        staminaCount.text = $"{Mathf.Round(playerScript.stamina)} / {Mathf.Round(playerScript.maxStamina)}";
        XPCount.text = $"{Mathf.Round(playerScript.currentXP)} / {Mathf.Round(playerScript.maxXP)}";
    }
    else
    {
        Debug.LogWarning("Player script not found.");
    }
}

}
