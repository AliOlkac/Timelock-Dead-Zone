using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; // Sağlık barı slider referansı
    private int maxHealth = 1000; // Maksimum sağlık
    private int currentHealth; // Mevcut sağlık

    void Start()
    {
        currentHealth = maxHealth; // Başlangıçta tam sağlık
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Hasar al
        if (currentHealth < 0) currentHealth = 0; // Sağlık sıfırın altına inmesin
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthSlider.value = (float)currentHealth / maxHealth; // Barı güncelle
    }
}
//