using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;

    private Health health;
    private Canvas canvas;
    private Transform camTransform;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        health = GetComponentInParent<Health>();
        camTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        health.Changed += OnHealthChanged;
    }

    private void OnDisable()
    {
        health.Changed -= OnHealthChanged;
    }

    private void OnHealthChanged(float current, float max)
    {
        canvas.enabled = current < max;
        fill.fillAmount = current / max;
    }

    private void LateUpdate()
    {
        transform.LookAt(camTransform);
    }
}