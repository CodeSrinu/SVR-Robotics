using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private EnemyData data;
    private Transform target;
    private Health health;
    private Health targetHealth;
    private NavMeshAgent agent;
    [SerializeField] private EnemyDiedChannel enemyDiedChannel;

    [SerializeField] private float pathCheckCooldown = 0.2f;
    private float pathCheckTimer = 0;
    private float attackTimer = 0;
    private bool isInitialized = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        pathCheckTimer = pathCheckCooldown;
    }

    private void OnEnable()
    {
        health.Dead += OnEnemyDeath;
        
    }
    private void OnDisable()
    {
        health.Dead -= OnEnemyDeath;
    }

    public void Init(EnemyData enemyData, Transform target)
    {
        data = enemyData;
        this.target = target;
        agent.speed = data.moveSpeed;
        agent.stoppingDistance = data.contactRange;
        health.Initialize(enemyData.maxHealth);
        target.TryGetComponent<Health>(out targetHealth);
        isInitialized = true;
        attackTimer = data.damageInterval;
    }

    private void Update()
    {
        if (!isInitialized) return;

        pathCheckTimer -= Time.deltaTime;
        if(pathCheckTimer < 0)
        {
            Chase();
            pathCheckTimer = pathCheckCooldown;
        }

        attackTimer -= Time.deltaTime;
        bool isInRange = Vector3.Distance(target.position, transform.position) < data.contactRange; 
        if(attackTimer <= 0 && isInRange)
        {
            Attack();
            attackTimer = data.damageInterval;
        }
    }

    private void Chase()
    {
        agent.SetDestination(target.position);
    }

    private void Attack()
    {
        targetHealth.TakeDamage(data.contactDamage);
    }

    private void OnEnemyDeath()
    {
        enemyDiedChannel.EnemyDied(data);
        Destroy(gameObject);
    }
}
