using UnityEngine;

public class ZombieHealthScript : MonoBehaviour
{
    [Header("Zombie Health Amount")]
    public int zombieHealth = 10;
    [SerializeField] Animator _anim;

    [Header("States")]
    public bool isStunned;
    public bool isDead;

    public void TakeDamage(int _damage)
    {
        zombieHealth -= _damage;
    }
}
