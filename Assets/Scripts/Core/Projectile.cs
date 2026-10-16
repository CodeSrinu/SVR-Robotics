using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 5f;
    private float damage;
    private Rigidbody rb;
    private float expireTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if(Time.time >  expireTime) Release();

        expireTime -= Time.deltaTime;
    }

    public void Launch(Vector3 dir, float speed, float dmg)
    {
        transform.forward = dir;
        rb.velocity = dir * speed;
        damage = dmg;
        expireTime = Time.time + lifeSpan;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) return;

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
        Release();
    }

    private void Release()
    {
        Destroy(gameObject);
    }
}
