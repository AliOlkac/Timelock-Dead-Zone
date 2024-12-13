using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public TextMeshProUGUI ammoText; // Mermi sayacı referansı
    private int maxAmmo = 50;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo; // Başlangıçta tam mermi
        UpdateAmmoText();
    }

    public void UseAmmo()
    {
        currentAmmo--;
        if (currentAmmo < 0) currentAmmo = 0;
        UpdateAmmoText();
    }

    public void ReloadAmmo()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    private void UpdateAmmoText()
    {
        ammoText.text = $" {currentAmmo} / {maxAmmo}";
    }
}
