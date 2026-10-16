using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform aim;
    [SerializeField] private float fireCooldown = 0.2f;
    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float maxAimDist = 100f;
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private Transform poolContainer;
    private float nextFireTime;
    private Pool<Projectile> pool;

    private void Awake()
    {
        pool = new Pool<Projectile>(projectile, poolContainer, 10);
    }

    private void Update()
    {
        if(inputReader.IsFiring && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 targetPoint;

        if (Physics.Raycast(aim.position, aim.forward, out RaycastHit hit, maxAimDist, aimMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = aim.position + maxAimDist * aim.forward;
        }

        Vector3 direction = (targetPoint - muzzle.position).normalized;

        Projectile projectileObj = pool.Get();
        
        projectileObj.Launch(muzzle.position, direction, projectileSpeed, damage, pool.Release);
    }
}
