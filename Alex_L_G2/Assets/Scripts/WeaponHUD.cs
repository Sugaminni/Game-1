using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponHUD : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI controlsText;
    public Image healthFill;

    [Header("Auto-find (optional)")]
    public WeaponSwitcher switcher;
    public PlayerHealth playerHealth;

    string lastWeaponName;

    // Initialize references
    void Awake()
    {
        if (!switcher)     switcher     = FindFirstObjectByType<WeaponSwitcher>();
        if (!playerHealth) playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (controlsText)
        {
            controlsText.text = "WASD Move  |  Mouse Look  |  Mouse1 Fire\nQ/E or Wheel Switch  |  1–3 Equip  |  Walk into pickups";
        }
    }

    // Makes sure HUD is up to date
    void Update()
    {
        // Health
        if (playerHealth && healthFill)
        {
            float pct = Mathf.InverseLerp(0f, playerHealth.maxHealth, playerHealth.currentHealth);
            healthFill.fillAmount = pct;
        }

        // Weapon name 
        if (switcher && weaponText)
        {
            var w = switcher.Current();
            string display = w ? CleanName(w.name) : "None";
            if (display != lastWeaponName)
            {
                weaponText.text = "Weapon: " + display;
                lastWeaponName = display;
            }
        }
    }

    // Cleans up weapon name for display
    static string CleanName(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return "None";
        return raw.Replace("(Clone)", "").Trim();
    }
}
