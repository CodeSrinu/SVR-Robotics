using UnityEngine;

public class Player : MonoBehaviour
{
    private InputReader inputReader;
    private PlayerMover mover;
    private PlayerLook look;

    private void Start()
    {
        inputReader = GetComponent<InputReader>();
        mover = GetComponent<PlayerMover>();
        look = GetComponent<PlayerLook>();

    }

    private void Update()
    {
        look.Look(inputReader.LookDir);

        Vector2 input = inputReader.MoveDir;
        Vector3 dir = transform.right * input.x + transform.forward * input.y;
        dir = Vector3.ClampMagnitude(dir, 1f); 
        mover.SetDirection(dir);
    }
}
