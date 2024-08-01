using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance { get; set; }
    public TextMeshProUGUI ammoDisplay;
    public Slider throwForceSlider;
    public TextMeshProUGUI chargeTimeDisplay;  // New field for displaying charge time

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateAmmoDisplay(Weapon weapon)
    {
        ammoDisplay.text = $"{weapon.bulletsLeft}/{weapon.accumulatedBullets}";
    }

    public void UpdateThrowForceSlider(float value)
    {
        if (throwForceSlider != null)
        {
            throwForceSlider.value = value;
            throwForceSlider.maxValue = 6.0f; // Ensure max value is set to 6
        }

        if (chargeTimeDisplay != null)
        {
            chargeTimeDisplay.text = $"{value:F1}";  // Display charge time as a number
        }
    }
}
