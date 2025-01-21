using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator; // Make sure this is assigned in the Inspector.
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    private Vector2 currentMovement;
    private bool movementPressed;
    private bool runPressed;

    private PlayerInput input;

    void Awake()
    {
        input = new PlayerInput();

        // Movement input: capture when joystick is moved or released
        input.CharacterControls.Movement.performed += ctx => 
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };

        input.CharacterControls.Movement.canceled += ctx => 
        {
            currentMovement = Vector2.zero; // Stop movement when joystick is released
            movementPressed = false;
        };

        // Run input: detect if the run button is pressed
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Run.canceled += ctx => runPressed = false;
    }

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float speed = runPressed ? runSpeed : walkSpeed;

        // Set the correct animation parameters
        bool isWalking = movementPressed && !runPressed;
        bool isRunning = movementPressed && runPressed;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);

        // Move the character
        if (movementPressed)
        {
            Vector3 move = new Vector3(currentMovement.x, 0, currentMovement.y);
            transform.Translate(move * speed * Time.deltaTime, Space.World);
        }
    }

    void HandleRotation()
    {
        if (movementPressed)
        {
            Vector3 direction = new Vector3(currentMovement.x, 0, currentMovement.y);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotation
            }
        }
    }

    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}


// using UnityEngine;
// using UnityEngine.InputSystem;

// public class CharacterMovement : MonoBehaviour
// {
//     Animator animator;

//     int isWalkingHash;
//     int isRunningHash;

//     PlayerInput input;

//     Vector2 currentMovement;
//     bool movementPressed;
//     bool runPressed;

//     // private CharacterController characterController;
//     // public float Speed = 5f;

//     void Awake ()
//     {
//         input = new PlayerInput();

//         input.CharacterControls.Movement.performed += ctx => {
//             currentMovement = ctx.ReadValue<Vector2>();
//             movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
//         };

//         input.CharacterControls.Movement.canceled += ctx => {
//             currentMovement = Vector2.zero; // Reset movement when joystick is released
//             movementPressed = false; // Stop walking when joystick is released
//         };
        
//         input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
//     }
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         animator = GetComponent<Animator>();

//         isWalkingHash = Animator.StringToHash("isWalking");
//         isRunningHash = Animator.StringToHash("isRunning");

//         // characterController = GetComponent<CharacterController>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         handleMovement();
//         handleRotation();

//         // Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

//         // characterController.Move(move * Time.deltaTime * Speed);
//     }

//     void handleRotation()
//     {
//         if (movementPressed) // Only rotate when there's movement input
//         {
//         Vector3 currentPosition = transform.position; ;
//         Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
//         Vector3 positionToLookAt = currentPosition + newPosition;
//         transform.LookAt(positionToLookAt);
//         // Rotate smoothly
//             Vector3 direction = positionToLookAt - currentPosition;
//             if (direction != Vector3.zero)
//             {
//                 Quaternion targetRotation = Quaternion.LookRotation(direction);
//                 transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smoothing
//             }
//         }
//     }
    

//     void handleMovement () {
//         bool isRunning = animator.GetBool(isRunningHash);
//         bool isWalking = animator.GetBool(isWalkingHash);

//         if (movementPressed && !isWalking) {
//             animator.SetBool(isWalkingHash, true);
//         }

//         if (!movementPressed && isWalking) {
//             animator.SetBool(isWalkingHash, false);
//         }

//         if ((movementPressed && runPressed) && !isRunning) {
//             animator.SetBool(isRunningHash, true);
//         }

//         if ((!movementPressed && !runPressed) && isRunning) {
//             animator.SetBool(isRunningHash, false);
//         }

//     }
    

//     void OnEnable ()
//     {
//         input.CharacterControls.Enable();
//     }

//     void OnDisable ()
//     {
//         input.CharacterControls.Disable();
//     }
// }

