using UnityEngine;

public enum HitZoneArea { Body, Head, Legs}

public class ZombieHitZone : MonoBehaviour
{
    public HitZoneArea area = HitZoneArea.Body;
    public ZombieHealthScript healthScript;

    public void ZombieShoot(int headDmg, int bodyDmg, int legDmg)
    {
        int _damage = 0;
        switch(area)
        {
            case HitZoneArea.Head:
                _damage = headDmg;
                break;
            case HitZoneArea.Body:
                _damage = bodyDmg;
                break;
            case HitZoneArea.Legs:
                _damage = legDmg;
                break;
        }
        healthScript.TakeDamage(_damage, area);
    }
}
