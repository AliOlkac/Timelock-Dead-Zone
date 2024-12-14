using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navAgent;  // Düşmanın NavMesh üzerinde hareket etmesini sağlar.
    public Transform player;// Oyuncunun pozisyonunu takip etmek için referans.
    public LayerMask groundLayer, playerLayer;  // Zemin ve oyuncu katmanlarını ayırmak için.
    public float health; // Düşmanın toplam sağlığı.
    public float walkPointRange;  // Devriye noktalarının rastgele seçileceği mesafe.
    public float timeBetweenAttacks; // Saldırılar arasında bekleme süresi.
    public float sightRange; // Düşmanın oyuncuyu görebileceği mesafe.
    public float attackRange; // Düşmanın oyuncuya saldırabileceği mesafe.
    public int damage; // Saldırıda oyuncuya verilen hasar miktarı.
    public Animator animator; // Animasyonların kontrolü için referans.
    public ParticleSystem hitEffect; // Düşman hasar aldığında oynayacak partikül efekti.
    public AudioSource alertSound; // Düşmanın oyuncuyu fark ettiğinde çalacak ses efekti.
    
    
    private Vector3 walkPoint; // Devriye noktası.
    private bool walkPointSet; // Devriye noktası ayarlandı mı?
    private bool alreadyAttacked; // Saldırı yapıldı mı?
    private bool takeDamage;  // Düşman hasar alıyor mu?
    private bool playerInSightRange, playerInAttackRange; // Oyuncu görüş ve saldırı menzilinde mi?
    
 
    private void Awake()
    {
        animator = GetComponent<Animator>(); // Animasyon bileşenini alır.
        //player = GameObject.FindWithTag("Player").transform;
        navAgent = GetComponent<NavMeshAgent>(); // NavMeshAgent bileşenini alır.
        
    }
    
    
    void Start()
    {
        
    }

    private void Update()
    {
        CheckPlayerRange();
        
        
        
    }

    
    //Oyuncunun düşmanın görüş menzilinde olup olmadığını ve saldırı menzilinde olup olmadığını kontrol eder.
    private void CheckPlayerRange()
    {
        playerInSightRange = Vector3.Distance(transform.position, player.position) < sightRange;
        playerInAttackRange = Vector3.Distance(transform.position, player.position) < attackRange;
        if (!playerInSightRange && !playerInAttackRange)
        {
            ResetAttack();
            Patroling();
            print("Patroling now");
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ResetAttack();
            ChasePlayer();
            print("Chasing now");
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
            print("Attacking now");
        }
        else if (!playerInSightRange && takeDamage)
        {
            ChasePlayer();
            print("Chasing now");
        }
    }
    
    //Düşmanın belirli bir alanda devriye gezmek için kullanacağı fonksiyon.
    private void Patroling() // Düşmanın devriye noktaları arasında hareket etmesini sağlar.
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetFloat("Velocity", 0.2f);

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
        
    }
    
    //Rastgele bir walkPoint belirler.Zemin üzerinde geçerli bir nokta olduğundan emin olmak için Physics.Raycast kullanır. 
    private void SearchWalkPoint() // Düşmanın devriye noktası belirlemesini sağlar.
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

   private void ChasePlayer() // Oyuncuyu takip etmek için kullanılan fonksiyon.
{
    navAgent.SetDestination(player.position);
    animator.SetFloat("Velocity", 0.6f);
    navAgent.isStopped = false; // Düşmanın hareket etmesini sağlar.
}


   private void AttackPlayer() // Oyuncuya saldırmak için kullanılan fonksiyon.
{
    navAgent.isStopped = true; // Hareketi durdur

    if (!alreadyAttacked) // Düşmanın saldırı yapmadığından emin olur.
    {
        transform.LookAt(player.position); // Oyuncuya bak
        alreadyAttacked = true;
        animator.SetBool("Attack", true);
        Invoke(nameof(ResetAttack), timeBetweenAttacks);// Saldırılar arasında bekleme süresi.

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange)) // Düşmanın saldırı menzilinde bir şey var mı kontrol eder.
        {
            /*
                YOU CAN USE THIS TO GET THE PLAYER HUD AND CALL THE TAKE DAMAGE FUNCTION

            PlayerHUD playerHUD = hit.transform.GetComponent<PlayerHUD>();
            if (playerHUD != null)
            {
               playerHUD.takeDamage(damage);
            }
             */
        }
    }
}


    private void ResetAttack() // Saldırıyı sıfırlar.
    {
        alreadyAttacked = false;
        animator.SetBool("Attack", false);
    }

    public void TakeDamage(float damage) // Düşmanın hasar almasını sağlar.
    {
        health -= damage;
        hitEffect.Play();
        StartCoroutine(TakeDamageCoroutine());

        if (health <= 0)
        {
            StartCoroutine(DestroyEnemyCoroutine());
        }

        
    }
    


    private IEnumerator TakeDamageCoroutine() // Düşmanın hasar almasını kontrol eder.
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f);
        takeDamage = false;
    }

    public void PlayAlertSound()
    {
        if (!alertSound.isPlaying)
            alertSound.Play();
    }
    private void DestroyEnemy() // Düşmanı yok eder.
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()// Düşmanın yok
    {
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() // Gizmos kullanarak düşmanın saldırı ve görüş menzillerini gösterir.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
