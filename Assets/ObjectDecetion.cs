using UnityEngine;

public class ObjectDecetion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<Enemy>())
        {
            var enemy = other.transform.GetComponent<Enemy>();
            enemy.TakeDamage(25);


        }

    }
}
