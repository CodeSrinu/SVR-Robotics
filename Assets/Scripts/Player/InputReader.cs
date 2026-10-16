using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference lookActionRef;
    [SerializeField] private InputActionReference fireActionRef;


    public Vector2 MoveDir => moveActionRef.action.ReadValue<Vector2>();
    public Vector2 LookDir => lookActionRef.action.ReadValue<Vector2>();
    public bool IsFiring {get; private set;}
    private void OnEnable()
    {
        moveActionRef.action.Enable();
        lookActionRef.action.Enable();
        fireActionRef.action.Enable();

        fireActionRef.action.started += OnFireStarted;
        fireActionRef.action.canceled += OnFireCanceled;


    }

    private void OnDisable()
    {
        moveActionRef.action.Disable();
        lookActionRef.action.Disable();
        fireActionRef.action.Disable();

        fireActionRef.action.started -= OnFireStarted;
        fireActionRef.action.canceled -= OnFireCanceled;
    }

    private void OnFireStarted(InputAction.CallbackContext obj)
    {
        IsFiring = true;
    }

    private void OnFireCanceled(InputAction.CallbackContext obj)
    {
        IsFiring = false;
    }


}
