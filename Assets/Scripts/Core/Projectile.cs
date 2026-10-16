using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeSpan = 5f;
    private float damage;
    private Rigidbody rb;
    private float expireTime;
    private Action<Projectile> OnRelease;
    private bool isReleased;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Time.time > expireTime) OnRelease?.Invoke(this);
    }

    public void Launch(Vector3 pos, Vector3 dir, float speed, float dmg, Action<Projectile> action)
    {
        transform.position = pos;
        transform.forward = dir;
        rb.velocity = dir * speed;
        damage = dmg;
        OnRelease = action;
        expireTime = Time.time + lifeSpan;
        isReleased = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isReleased) return;

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
    }
}
