using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

[System.Serializable]
public class WeaponSlot
{
    public GameObject weaponModel;
    public Transform leftHandIK;
}

public class PlayerShoot : MonoBehaviour
{
    Tps_Inputs tpsInput;
    PlayerMove playerMove;
    PlayerStats playerStats;
    [SerializeField] Animator anim;

    [Header("Set Weapon Type")]
    public int weaponID = 0;

    [Header("HUD Elements")]
    public GameObject aimPanel;
    public GameObject scopePanel;
    public Image crosshair; // why is it not on the Unity editor...? (if I fix this, put here the Image_Crosshair gameObject) (It appears the type Image cannot appear in the editor, why...?)
    public Sprite pistolCrossHair;
    public Sprite shotgunCrossHair;

    [Header("Projectiles Effects")]
    public Rigidbody bulletTrail;
    public GameObject bulletDecal;
    public GameObject bulletImpact;

    [Header("Spawn Points")]
    public Transform gunSpawnPoint;
    public Transform pistolSpawn;
    public Transform shotgunSpawn;
    public Transform rifleSpawn;
    public Transform aimDebugObject;

    [Header("Damage")]
    // Pistol Damage
    public float pistolHeadDamage = 32;
    public float pistolBodyDamage = 25;
    public float pistolLegDamage = 15;

    // Shotgun Damage
    public float shotHeadDamage = 35;
    public float shotBodyDamage = 28;
    public float shotLegDamage = 24;

    // Rifle Damage
    public float rifleHeadDamage = 100;
    public float rifleBodyDamage = 50;
    public float riflelLegDamage = 35;

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

    [Header("Weapon Switching")]
    public WeaponSlot[] _weapons;
    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public Rig rig;
    public MultiAimConstraint bodyAim;
    public Vector3 pistolOffset;
    public Vector3 longGunOffset;

    [Header("Found Guns")]
    public bool hasRifle;
    public bool hasShotGun;

    [Header("Ammo")]
    public int[] weaponsAmmoMax = { 12, 3, 6 };
    public int[] weaponsAmmoCurrent = { 12, 3, 6 };
    
    private void Awake()
    {
        tpsInput = new Tps_Inputs();
        playerMove = GetComponent<PlayerMove>();
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        EquipWeapon(0);
    }

    public void OnEnable() => tpsInput.Enable();

    public void OnDisable() => tpsInput.Disable();

    private void Update()
    {
        aimPanel.SetActive(playerMove.isAiming ? true : false);
        scopePanel.SetActive(playerMove.isAiming && playerMove.isScoped? true : false);

        rig.weight = playerMove.isAiming ? 1 : 0;

        if (gunCoolDown > 0) gunCoolDown -= Time.deltaTime;
        else gunCoolDown = 0;

        if (tpsInput.Player.Weapon1.triggered)
        {
            weaponID = 0;
            EquipWeapon(weaponID);
        }
        if (tpsInput.Player.Weapon2.triggered && hasRifle)
        {
            weaponID = 1;
            EquipWeapon(weaponID);
        }
        if (tpsInput.Player.Weapon3.triggered && hasShotGun)
        {
            weaponID = 2;
            EquipWeapon(weaponID);
        }
            

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if(tpsInput.Player.Fire.triggered && playerMove.isAiming && gunCoolDown <= 0)
        {
            switch(weaponID)
            {
                case 0:
                    gunCoolDown = pistolSpeed;
                    FirePistol();
                    break;
                case 1:
                    gunCoolDown = rifleSpeed;
                    FireRifle();
                    break;
                case 2:
                    gunCoolDown = shotSpeed;
                    FireShotgun();
                    break;
            }
        }

        if(tpsInput.Player.Reload.triggered)
        {
            if(weaponsAmmoCurrent[weaponID] != weaponsAmmoMax[weaponID])
            {
                SoundManager.Instance.PlaySound2D("WeaponReload");
                weaponsAmmoCurrent[weaponID] = weaponsAmmoMax[weaponID];
                playerStats.AmmoUpdate();
            }
        }
            

        if(Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, aimCollisionMask ))
        {
            aimDebugObject.position = rayCastHit.point;
            gunSpawnPoint.LookAt(rayCastHit.point);
            pistolSpawn.LookAt(rayCastHit.point);
            rifleSpawn.LookAt(rayCastHit.point);
            shotgunSpawn.LookAt(rayCastHit.point);
        }
    }

    void EquipWeapon(int id)
    {
        SoundManager.Instance.PlaySound2D("WeaponEquip");
        playerStats.AmmoUpdate();
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].weaponModel.SetActive(i == id);
        }

        var slot = _weapons[id];
        var data = leftHandConstraint.data;
        data.target = slot.leftHandIK;

        rigBuilder.Build();
    }

    void FirePistol()
    {
        if (weaponsAmmoCurrent[0] > 0)
        {
            SoundManager.Instance.PlaySound2D("PistolFire");
            weaponsAmmoCurrent[0]--;
            playerStats.AmmoUpdate();

            anim.SetTrigger("Pistol");
            Rigidbody _trail;
            _trail = Instantiate(bulletTrail, pistolSpawn.position,
                pistolSpawn.rotation) as Rigidbody;
            _trail.AddForce(pistolSpawn.forward * 300, ForceMode.Impulse);

            if (Physics.Raycast(pistolSpawn.position, pistolSpawn.forward,
                out RaycastHit hit, pistolDistance))
            {
                WeaponHit(hit, (int)pistolHeadDamage, (int)pistolBodyDamage, (int)pistolLegDamage);
            }
        }
        else
        {
            playerStats.currentDuration = playerStats.textDuration;
        }

    }

    void FireRifle()
    {
        if (weaponsAmmoCurrent[1] > 0)
        {
            SoundManager.Instance.PlaySound2D("Rifle");
            weaponsAmmoCurrent[1]--;
            playerStats.AmmoUpdate();

            Rigidbody _trail;
            _trail = Instantiate(bulletTrail, rifleSpawn.position,
                rifleSpawn.rotation) as Rigidbody;
            _trail.AddForce(rifleSpawn.forward * 300, ForceMode.Impulse);

            if (Physics.Raycast(rifleSpawn.position, rifleSpawn.forward,
                out RaycastHit hit, pistolDistance))
            {
                WeaponHit(hit, (int)rifleHeadDamage, (int)rifleBodyDamage, (int)riflelLegDamage);
            }
        }
        else
        {
            playerStats.currentDuration = playerStats.textDuration;
        }
    }

    void FireShotgun()
    {
        if (weaponsAmmoCurrent[2] > 0)
        {
            SoundManager.Instance.PlaySound2D("ShotGun");
            weaponsAmmoCurrent[2]--;
            playerStats.AmmoUpdate();

            for (int i = 0; i < shotAmount; i++)
            {
                Vector3 shotAngle = GetShotDirection();
                Rigidbody _trail;
                _trail = Instantiate(bulletTrail, shotgunSpawn.position,
                    shotgunSpawn.rotation) as Rigidbody;
                _trail.AddForce(shotAngle * 300, ForceMode.Impulse);
                if (Physics.Raycast(shotgunSpawn.position, shotAngle,
                    out RaycastHit hit, pistolDistance))
                {
                    WeaponHit(hit, (int)shotHeadDamage, (int)shotBodyDamage, (int)shotLegDamage);
                }
            }
        }
        else
        {
            playerStats.currentDuration = playerStats.textDuration;
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
    void WeaponHit(RaycastHit hit, int headDmg, int bodyDmg, int legDmg)
    {
        //Debug.Log("You just hit " + hit.collider.name);

        if(hit.transform.gameObject.tag == "ground")
        {
            Instantiate(bulletDecal, hit.point, Quaternion.LookRotation(hit.normal));
            Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));
        }
        if(hit.collider.tag == "Enemy")
        {
            ZombieHitZone zone = hit.collider.GetComponent<ZombieHitZone>();
            if(zone != null)
            {
                zone.ZombieShoot(headDmg, bodyDmg, legDmg);
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 _offset = bodyAim.data.offset;
        if(playerMove.isAiming)
        {
            if (weaponID == 0)
                _offset = pistolOffset;
            else
                _offset = longGunOffset;
        }
        bodyAim.data.offset = _offset;
    }

    public void FoundRifle()
    {
        hasRifle = true;
        weaponID = 1;
        EquipWeapon(weaponID);
    }

    public void FoundShotGun()
    {
        hasShotGun = true;
        weaponID = 2;
        EquipWeapon(weaponID);
    }
}
