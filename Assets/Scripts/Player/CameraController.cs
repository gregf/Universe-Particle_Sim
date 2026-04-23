using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 50f;
    [SerializeField] private float boostedSpeed = 100f;
    [SerializeField] private float verticalSpeed = 20f;

    [Header("Mouse Settings")]
    [SerializeField] private float lookSensitivity = 0.3f;
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;


    //keybind references
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isBoosting;
    private Vector3 verticalMovement = Vector3.zero;
    private float xRotation = 0f;        
    private float yRotation = 0f;    


    private Camera cam;
    private PlayerInputActions inputActions;

    private void Awake()
    {
        cam = Camera.main;

        Vector3 initialRotaion = transform.localEulerAngles;
        xRotation = initialRotaion.x;
        yRotation = initialRotaion.y;
    }

    private void OnEnable()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();

        //move
        inputActions.FlyCam.Move.performed += OnMove;
        inputActions.FlyCam.Move.canceled  += OnMove;

        //look
        inputActions.FlyCam.Look.performed += OnLook;
        inputActions.FlyCam.Look.canceled  += OnLook;

        //move up
        inputActions.FlyCam.MoveUp.performed += OnMoveUp;
        inputActions.FlyCam.MoveUp.canceled  += OnMoveUp;

        //move down
        inputActions.FlyCam.MoveDown.performed += OnMoveDown;
        inputActions.FlyCam.MoveDown.canceled  += OnMoveDown;

        //boost
        inputActions.FlyCam.Boost.performed += OnBoost;
        inputActions.FlyCam.Boost.canceled  += OnBoost;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnMoveUp(InputAction.CallbackContext context)
    {
        verticalMovement = context.performed ? Vector3.up : Vector3.zero;
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        verticalMovement = context.performed ? Vector3.down : Vector3.zero;
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        isBoosting = context.performed;
    }

    void Update()
    {
        LookAround();
        MoveCamera();
    }
    void MoveCamera()
    {
        float speed = isBoosting ? boostedSpeed : normalSpeed;

        Vector3 forwardMovement = MathLib.Normalize(transform.forward) * moveInput.y * speed * Time.deltaTime;
        Vector3 rightMovement = MathLib.Normalize(transform.right) * moveInput.x * speed * Time.deltaTime;

        transform.position += forwardMovement + rightMovement + (verticalMovement * verticalSpeed * Time.deltaTime);
    }
    void LookAround()
    {
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime * (invertX ? -1 : 1);
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime * (invertY ? -1 : 1);

        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        transform.localRotation = quaternion.Euler(xRotation, yRotation, 0f);
    }
}
