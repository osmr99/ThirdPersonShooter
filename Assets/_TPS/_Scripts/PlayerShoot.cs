using UnityEditor.UIElements;
using Unity.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerShoot : MonoBehaviour
{
    Tps_Inputs tpsInput;
    PlayerMove playerMove;

    [Header("Set Weapon Type")]
    public int weaponID = 1;

    [Header("HUD Elements")]
    public GameObject aimPanel;
    public GameObject scopePanel;
    public Image crosshair; // why is it not on the Unity editor...? (if I fix this, put here the Image_Crosshair gameObject)
    public Sprite pistolCrossHair;
    public Sprite shotgunCrossHair;

    [Header("Projectiles Effects")]
    public Rigidbody bulletTrail;
    public GameObject bulletDecal;
    public GameObject bulletImpact;

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

    [Header("Shot Amount")]
    public int shotAmount = 7;
    public float randomAccuracy = 5;
    
    private void Awake()
    {
        tpsInput = new Tps_Inputs();
        playerMove = GetComponent<PlayerMove>();
    }

    private void OnEnable() => tpsInput.Enable();

    private void OnDisable() => tpsInput.Disable();

    private void Update()
    {
        aimPanel.SetActive(playerMove.isAiming ? true : false);

        if (gunCoolDown > 0) gunCoolDown -= Time.deltaTime;
        else gunCoolDown = 0;

        if (tpsInput.Player.Weapon1.triggered)
            weaponID = 1;
        if (tpsInput.Player.Weapon2.triggered)
            weaponID = 2;
        if (tpsInput.Player.Weapon3.triggered)
            weaponID = 3;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(tpsInput.Player.Fire.triggered && playerMove.isAiming && gunCoolDown <= 0)
        {
            switch(weaponID)
            {
                case 1:
                    gunCoolDown = pistolSpeed;
                    FirePistol();
                    break;
                case 2:
                    gunCoolDown = rifleSpeed;
                    FireRifle();
                    break;
                case 3:
                    gunCoolDown = shotSpeed;
                    FireShotgun();
                    break;
            }
        }
            

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

    void FireRifle()
    {
        SoundManager.Instance.PlaySound2D("Rifle");

        Rigidbody _trail;
        _trail = Instantiate(bulletTrail, gunSpawnPoint.position,
            gunSpawnPoint.rotation) as Rigidbody;
        _trail.AddForce(gunSpawnPoint.forward * 300, ForceMode.Impulse);

        if (Physics.Raycast(gunSpawnPoint.position, gunSpawnPoint.forward,
            out RaycastHit hit, pistolDistance))
        {
            WeaponHit(hit);
        }
    }

    void FireShotgun()
    {
        SoundManager.Instance.PlaySound2D("ShotGun");
        for(int i = 0; i < shotAmount; i++)
        {
            Rigidbody _trail;
            _trail = Instantiate(bulletTrail, gunSpawnPoint.position,
                gunSpawnPoint.rotation) as Rigidbody;
            _trail.AddForce(gunSpawnPoint.forward * 300, ForceMode.Impulse);

            if (Physics.Raycast(gunSpawnPoint.position, GetShotDirection(),
                out RaycastHit hit, pistolDistance))
            {
                WeaponHit(hit);
            }
        }
    }

    Vector3 GetShotDirection()
    {
        Vector3 targetPos = gunSpawnPoint.position + gunSpawnPoint.forward * 30;

        targetPos = new Vector3(targetPos.x + Random.Range(-randomAccuracy, randomAccuracy),
            targetPos.y + Random.Range(-randomAccuracy, randomAccuracy),
            targetPos.z + Random.Range(-randomAccuracy, randomAccuracy));

        Vector3 _direction = targetPos - gunSpawnPoint.position;
        return _direction.normalized;
    }
    void WeaponHit(RaycastHit hit)
    {
        Debug.Log("You just hit " + hit.collider.name);

        if(hit.transform.gameObject.tag == "ground")
        {
            Instantiate(bulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
