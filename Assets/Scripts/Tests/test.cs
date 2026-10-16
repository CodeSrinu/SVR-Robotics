using UnityEngine;

public class test : MonoBehaviour
{
    public Health health;
    public GameObject projectilePrefab;
    public Camera cam;
    public  EnemyData enemyData;
    public Enemy[] enemies;
    public Player player;

    private void Start()
    {
        //health.Damaged += OnDamaged;
        //health.Changed += OnHealthChanged;
        //health.Dead += OnDeath;
    }

    private void OnDeath()
    {
        Debug.Log("Died");
    }

    private void OnHealthChanged(float ch, float mh)
    {
        Debug.Log($"Health decreased to {ch} from max health {mh}");
    }

    private void OnDamaged(float dmg)
    {
        Debug.Log($"Damage Taken: {dmg}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Init(enemyData, player.transform);
            }
        }
    }
}
