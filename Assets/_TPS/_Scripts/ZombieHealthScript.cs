using UnityEngine;

public class ZombieHealthScript : MonoBehaviour
{
    [Header("Zombie Health Amount")]
    public int zombieHealth = 10;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject goalTrigger;

    [Header("States")]
    public bool isStunned;
    public bool isDead;
    public bool isStanding;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int _damage, HitZoneArea area)
    {
        if (isDead) return;
        zombieHealth -= _damage;

        if(zombieHealth > 0)
        {
            switch (area)
            {
                case HitZoneArea.Head:
                    _anim.SetTrigger("Head");
                    break;
                case HitZoneArea.Body:
                    _anim.SetTrigger("Body");
                    break;
                case HitZoneArea.Legs:
                    _anim.SetBool("Standing", false);
                    _anim.SetTrigger("Leg");
                    break;
            }
        }
        else
        {
            isDead = true;
            _anim.SetTrigger("DeathGround");

            if (gameObject.name == "ZombieBoss")
                goalTrigger.SetActive(true);
            Destroy(gameObject, 3f);
        }
    }
}
