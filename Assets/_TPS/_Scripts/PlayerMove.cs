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

    [Header("Movement Controls")]
    public float moveSpeed = 3;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Rotation and Aiming")]
    public Transform cameraFollow;
    [SerializeField] private float xRotate, yRotate;
    public float mouseSensitivity = 0.25f;
    public float yMin, yMax;

    [Header("Booleans")]
    public bool isZoomedIn;
    public bool isAiming;
    public bool isScoped;
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

    private void OnEnable() => tpsInputs.Enable();
    private void OnDisable() => tpsInputs.Disable();

    private void Update()
    {
        isRunning = tpsInputs.Player.Run.IsPressed();
        if (tpsInputs.Player.Crouch.triggered)
            isCrouching = !isCrouching;

        isAiming = tpsInputs.Player.Aim.IsPressed();

        HandleInput();
        HandleMovement();
        HandleAiming();
    }

    private void LateUpdate()
    {
        cameraFollow.localEulerAngles = new Vector3(yRotate, 
            cameraFollow.localEulerAngles.y, cameraFollow.localEulerAngles.z);
        transform.rotation = Quaternion.Euler(0, xRotate, 0);
    }

    void HandleInput()
    {
        moveInput = tpsInputs.Player.Move.ReadValue<Vector2>();
        aimInput = tpsInputs.Player.Camera.ReadValue<Vector2>();


        xRotate += aimInput.x * mouseSensitivity;
        yRotate = Mathf.Clamp(yRotate - aimInput.y * mouseSensitivity, yMin, yMax);
    }

    void HandleMovement()
    {
        Vector3 moveDirection = (transform.forward * moveInput.y 
            + transform.right * moveInput.x) * moveSpeed;
        cc.Move(moveDirection * Time.deltaTime);

        // Player Animations

        _anim.SetBool("Crouch", isCrouching);
        _anim.SetFloat("XDirection", moveInput.x, 0.1f, Time.deltaTime);
        _anim.SetFloat("ZDirection", moveInput.y, 0.1f, Time.deltaTime);
    }

    void HandleAiming()
    {
        moveCamera.Priority = isAiming ? 0 : 1;
        aimCamera.Priority = isAiming ? 1 : -1;
        scopeCamera.Priority = isAiming && isScoped ? 1 : 0;
    }
}
