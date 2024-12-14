using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera fpsCam; // Oyuncunun görüş açısı
    public float range = 100f; // Mermi menzili
    public float baseDamage = 20f; // Temel hasar

    public void Shoot()
    {
        // Kameradan çıkan bir ray gönder
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Kameranın ortasından bir ışın
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range)) // Ray bir objeye çarptıysa
        {
            // Enemy script'ini kontrol et
            Enemy enemy = hit.collider.GetComponentInParent<Enemy>(); // Çarpılan objenin bir Enemy olup olmadığını kontrol et
            if (enemy != null)
            {
                // Bölgeye göre hasar uygula
                if (hit.collider.CompareTag("Head"))
                {
                    enemy.TakeDamage(baseDamage * 2.5f); // Kafaya vurduğunda 2.5 kat hasar
                }
                else if (hit.collider.CompareTag("Body"))
                {
                    enemy.TakeDamage(baseDamage); // Gövdeye vurduğunda normal hasar
                }
                else if (hit.collider.CompareTag("Limb"))
                {
                    enemy.TakeDamage(baseDamage * 0.5f); // Kol/bacak vurduğunda 0.5 kat hasar
                }
            }
        }
    }
}