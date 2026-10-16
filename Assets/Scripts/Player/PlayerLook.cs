using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private float sensitivity = 1.0f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    private float pitch;

    public void Look(Vector2 delta)
    {
        transform.Rotate(Vector3.up * delta.x * sensitivity);
        pitch -= delta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        camTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
