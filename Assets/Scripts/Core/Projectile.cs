using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 5f;
    private float damage;
    private Rigidbody rb;
    private float expireTime;
    private Action<Projectile> OnRelease;
    private bool isReleased;
    [SerializeField] private VfxManager vfxManager;   
    private TrailRenderer trail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Time.time > expireTime) OnRelease?.Invoke(this);
    }

    public void Launch(Vector3 pos, Vector3 dir, float speed, float dmg, Action<Projectile> action)
    {
        trail.Clear();
        trail.enabled = false;
        transform.position = pos;
        transform.forward = dir;
        rb.velocity = dir * speed;
        damage = dmg;
        OnRelease = action;
        expireTime = Time.time + lifeSpan;
        isReleased = false;
        trail.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isReleased) return;
        vfxManager.PlayImpact(other.transform.position);

        if (other.CompareTag("Player")) return;

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }
        ReleaseProjectile();
    }

    private void ReleaseProjectile()
    {
        if(isReleased) return;

        OnRelease?.Invoke(this);
        isReleased = true;
        trail.enabled = false;
    }
}
