using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Enemy>())
        {
            var enemy = other.transform.GetComponent<Enemy>();
            enemy.TakeDamage(25);
        }
        Destroy(gameObject);

    }
}
