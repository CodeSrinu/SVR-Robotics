using System.Collections.Generic;
using UnityEngine;

public class VfxManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactPrefab;
    [SerializeField] private Transform container;
    [SerializeField] private int initialCount = 10;

    private List<ParticleSystem> instances = new List<ParticleSystem>();

    private void Awake()
    {
        for (int i = 0; i < initialCount; i++)
            instances.Add(CreateInstance());
    }

    public void PlayImpact(Vector3 position)
    {
        ParticleSystem free = instances.Find(p => p!=null && !p.IsAlive());

        if (free == null)
        {
            free = CreateInstance();
            instances.Add(free);
        }

        free.transform.position = position;
        free.Play(true);
    }

    private ParticleSystem CreateInstance()
    {
        ParticleSystem p = Instantiate(impactPrefab, container);
        p.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        return p;
    }
}