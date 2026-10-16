using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8;
    [SerializeField] private float acceleration = 30;
    [SerializeField] private float gravity = -20f;

    private Vector3 moveDir;
    private CharacterController characterController;
    private Vector3 moveVelocity;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        moveVelocity = Vector3.MoveTowards(moveVelocity, moveSpeed * moveDir, acceleration * Time.deltaTime);
        if(characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = moveVelocity + Vector3.up * verticalVelocity;

        characterController.Move(velocity * Time.deltaTime); 
    }

    public void SetDirection(Vector3 dir)
    {
        moveDir = dir;
    }
}
