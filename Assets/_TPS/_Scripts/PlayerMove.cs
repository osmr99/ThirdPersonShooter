using UnityEngine;
using Unity.Cinemachine;

public class PlayerMove : MonoBehaviour
{
    Tps_Inputs tpsInputs;
    CharacterController cc;
    Animator _anim;

    [Header("Camera Controls")]
    public CinemachineCamera moveCamera;
    public CinemachineCamera aimCamera;
    public CinemachineCamera scopeCamera;
    [SerializeField] CinemachineThirdPersonFollow shoulderFollow;
    [SerializeField] private float shoulderTarget = 1;
    public float shoulderSwitchSpeed = 10;

    [Header("Movement Controls")]
    public float moveSpeed = 3;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Rotation and Aiming")]
    public Transform cameraFollow;
    public Transform standPos;
    public Transform crouchPos;
    public float crouchSpeed = 8;
    [SerializeField] private float xRotate, yRotate;
    public float mouseSensitivity = 0.25f;
    public float yMin, yMax;

    [Header("Player Physics")]
    public Vector3 playerVelocity;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float gravityForce = -10;
    public float groundForce = -2;

    [Header("Booleans")]
    public bool isZoomedIn;
    public bool isAiming;
    public bool isScoped;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isCrouching;

    private void Awake()
    {
        tpsInputs = new Tps_Inputs();
        cc = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

        // Set camera priorities
        moveCamera.Priority = 1;
        aimCamera.Priority = 0;
        scopeCamera.Priority = 0;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => tpsInputs.Enable();
    private void OnDisable() => tpsInputs.Disable();

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

        isRunning = tpsInputs.Player.Run.IsPressed();

        if (tpsInputs.Player.Crouch.triggered)
            isCrouching = !isCrouching;

        isAiming = tpsInputs.Player.Aim.IsPressed();
        if (isAiming) _anim.SetLayerWeight(1, 1);
        else _anim.SetLayerWeight(1, 0);

        HandleInput();
        HandleMovement();
        HandleAiming();
    }

    private void LateUpdate()
    {
        Transform marker = isCrouching ? crouchPos : standPos;
        cameraFollow.localPosition = Vector3.Lerp(cameraFollow.localPosition, 
            marker.localPosition, crouchSpeed * Time.deltaTime);
        cameraFollow.localRotation = Quaternion.Euler(yRotate, 0, 0);
        //cameraFollow.localEulerAngles = new Vector3(yRotate, 
            //cameraFollow.localEulerAngles.y, cameraFollow.localEulerAngles.z);
        transform.rotation = Quaternion.Euler(0, xRotate, 0);
    }

    void HandleInput()
    {
        moveInput = tpsInputs.Player.Move.ReadValue<Vector2>();
        aimInput = tpsInputs.Player.Camera.ReadValue<Vector2>();

        xRotate += aimInput.x * mouseSensitivity;
        yRotate = Mathf.Clamp(yRotate - aimInput.y * mouseSensitivity, yMin, yMax);

        if (tpsInputs.Player.ShoulderSwitch.triggered & isAiming)
            shoulderTarget = shoulderTarget > 0.5f ? 0 : 1;
    }

    void HandleMovement()
    {
        Vector3 moveDirection = (transform.forward * moveInput.y 
            + transform.right * moveInput.x) * moveSpeed;

        // Set gravity for the player
        if (!isGrounded) playerVelocity.y += gravityForce * Time.deltaTime;
        else if (playerVelocity.y < 0) playerVelocity.y = 0; //gravityForce;

        moveDirection.y = playerVelocity.y;
        cc.Move(moveDirection * Time.deltaTime);

        // Player Animations
        _anim.SetBool("Crouch", isCrouching);
        _anim.SetFloat("XDirection", moveInput.x, 0.1f, Time.deltaTime);
        _anim.SetFloat("ZDirection", moveInput.y, 0.1f, Time.deltaTime);
    }

    void HandleAiming()
    {
        shoulderFollow.CameraSide = Mathf.MoveTowards(shoulderFollow.CameraSide,
            shoulderTarget, shoulderSwitchSpeed * Time.deltaTime);
        moveCamera.Priority = isAiming ? 0 : 1;
        aimCamera.Priority = isAiming ? 1 : -1;
        scopeCamera.Priority = isAiming && isScoped ? 1 : 0;
    }
}
