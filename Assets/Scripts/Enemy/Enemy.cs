using System;
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
    private bool isReleased;
    private Action<Enemy> OnRelease;
    private float damageMult = 1;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.Dead += OnEnemyDeath;
        
    }
    private void OnDisable()
    {
        health.Dead -= OnEnemyDeath;
    }

    public void Init(EnemyData enemyData, Transform target,Vector3 startPos, Action<Enemy> ReleaseAction, DifficultyScale scale)
    {
        data = enemyData;
        this.target = target;
        target.TryGetComponent<Health>(out targetHealth);
        transform.position = startPos;
        damageMult = scale.damage;
        agent.speed = data.moveSpeed * scale.speed;
        agent.stoppingDistance = data.contactRange;
        agent.enabled = true;
        GetComponent<MeshRenderer>().material = enemyData.mat;

        health.Initialize(enemyData.maxHealth * scale.health);
        OnRelease = ReleaseAction;
        attackTimer = data.damageInterval;
        pathCheckTimer = UnityEngine.Random.Range(0f, pathCheckCooldown);
        isInitialized = true;
        isReleased = false;
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
        targetHealth.TakeDamage(data.contactDamage * damageMult);
    }

    private void OnEnemyDeath()
    {
        if (isReleased) return;


        enemyDiedChannel.EnemyDied(data);
        ReleaseEnemy();
    }

    private void ReleaseEnemy()
    {
        isInitialized = false;
        agent.enabled = false;
        isReleased = true;
        OnRelease?.Invoke(this);
    }

}
