using UnityEngine;
using Unity.Cinemachine;

public class PlayerMove : MonoBehaviour
{
    Tps_Inputs tpsInputs;
    CharacterController cc;

    [Header("Movement Controls")]
    public float moveSpeed = 3;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Rotation and Aiming")]
    public Transform cameraFollow;
    [SerializeField] private float xRotate, yRotate;
    public float mouseSensitivity = 0.25f;
    public float yMin, yMax;

    private void Awake()
    {
        tpsInputs = new Tps_Inputs();
        cc = GetComponent<CharacterController>();
    }

    private void OnEnable() => tpsInputs.Enable();
    private void OnDisable() => tpsInputs.Disable();

    private void Update()
    {
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
    }

    void HandleAiming()
    {

    }
}
