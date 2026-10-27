using Unity.Cinemachine;
using UnityEditor.UIElements;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    Tps_Inputs tpsInput;
    PlayerMove playerMove;

    [Header("Projectiles Effects")]
    public Rigidbody bulletTrail;
    public GameObject bulletDecal;

    [Header("Spawn Points")]
    public Transform gunSpawnPoint;
    public Transform aimDebugObject;

    [Header("Damage")]
    // Pistol Damage
    public float pistolHeadDamage = 30;
    public float pistolBodyDamage = 10;
    public float pistolLegDamage = 10;

    // Shotgun Damage
    public float shotHeadDamage = 30;
    public float shotBodyDamage = 10;
    public float shotLegDamage = 10;

    // Rifle Damage
    public float rifleHeadDamage = 30;
    public float rifleBodyDamage = 10;
    public float riflelLegDamage = 10;

    [Header("Layer Masks")]
    public LayerMask aimCollisionMask = new LayerMask();
    public LayerMask environment = new LayerMask();

    [Header("Ray Distance")]
    public float pistolDistance = 100;
    public float shotDistance = 15;
    public float rifleDistance = 999;

    [Header("Weapon Speed")]
    public float pistolSpeed = 0.25f;
    public float shotSpeed = 1f;
    public float rifleSpeed = 1f;
    [SerializeField] private float gunCoolDown;
    
    private void Awake()
    {
        tpsInput = new Tps_Inputs();
        playerMove = GetComponent<PlayerMove>();
    }

    private void OnEnable() => tpsInput.Enable();

    private void OnDisable() => tpsInput.Disable();

    private void Update()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(tpsInput.Player.Fire.triggered && playerMove.isAiming)
            FirePistol();
            

        if(Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, aimCollisionMask ))
        {
            aimDebugObject.position = rayCastHit.point;
            gunSpawnPoint.LookAt(rayCastHit.point);
        }
    }

    void FirePistol()
    {
        SoundManager.Instance.PlaySound2D("PistolFire");
        Rigidbody _trail;
        _trail = Instantiate(bulletTrail, gunSpawnPoint.position,
            gunSpawnPoint.rotation) as Rigidbody;
        _trail.AddForce(gunSpawnPoint.forward * 300, ForceMode.Impulse);

        if(Physics.Raycast(gunSpawnPoint.position, gunSpawnPoint.forward, 
            out RaycastHit hit, pistolDistance))
        {
            WeaponHit(hit);
        }
    }

    void WeaponHit(RaycastHit hit)
    {
        Debug.Log("You just hit " + hit.collider.name);

        if(hit.transform.gameObject.tag == "ground")
        {
            Instantiate(bulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
