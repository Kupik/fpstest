using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    public float move_speed { get; private set; } = 8f;

    public float runing_speed { get; private set; } = 14f;
    public float jumpHeight { get; private set; } = 2f;
    public float gravity { get; private set; } = -9.81f;

    private Vector3 velocity;


    private PlayerInput playerInput;
    private InputAction move;
    public bool isRun { get; private set; } = false;
    public bool isMove { get; private set; } = false;


    public Transform main_transform_camera { get; private set; }
    public bool isGrounded { get; private set; }

    public InputAction Jumping { get; private set; }
    public InputAction Runing { get; private set; }






    public void Awake()
    {
        controller = GetComponent<CharacterController>();

        playerInput = GetComponent<PlayerInput>();
        main_transform_camera = Camera.main.transform;

        Jumping = playerInput.actions["Jump"]; // jump din gameplay new input
        Runing = playerInput.actions["Shift"];

        Runing.started += ctx => isRun = true;
        Runing.canceled += ctx => isRun = false;

    }


    public void OnEnable()
    {
        Jumping.performed += Jump;


        Runing.Enable();
    }

    public void OnDisable()
    {
        Jumping.performed -= Jump;


        Runing.Disable();
    }


    private void Update()
    {

        // if ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Mișcare
        Vector2 moveInput = playerInput.actions["Move"].ReadValue<Vector2>(); //obtin miscarea de la player input
        isMove = moveInput.magnitude > 0.1f;


        if (main_transform_camera == null)
        {
            main_transform_camera = Camera.main?.transform;
            if (main_transform_camera == null)
            {
                Debug.LogError("Camera principală nu a fost găsită!");
                return;
            }
        }

        if (isMove)
        {

            Vector3 camera_forward = main_transform_camera.forward;
            Vector3 camera_right = main_transform_camera.right;

            // resetez y pentru a nu influenta miscarea
            camera_forward.y = 0f;
            camera_right.y = 0f;

            // normalizare a camerei
            camera_forward.Normalize();
            camera_right.Normalize();

            // calcul de directie a camerei
            Vector3 move_direction = (camera_forward * moveInput.y + camera_right * moveInput.x).normalized;

            float current_speed = isRun ? runing_speed : move_speed;
            // aplic miscarea
            controller.Move(move_direction * current_speed * Time.deltaTime);

            // roteste personajul in directia miscari
            Rotate(move_direction);
        }
        else
        {
            // miscarea va fii 0 cind nu va nici un input activ
            controller.Move(Vector3.zero);
        }

        //gravitatia
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

 
    }

    private void Rotate(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
        {

            Quaternion target_rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * 10);
        }
    }


    // functia jump care o apelez
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }


    public void Run(InputAction.CallbackContext context)
    {
        isRun = context.phase == InputActionPhase.Performed;
    }


}