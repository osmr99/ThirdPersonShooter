using UnityEngine;

public enum HitZoneArea { Body, Head, Legs}

public class ZombieHitZone : MonoBehaviour
{
    public HitZoneArea area = HitZoneArea.Body;
    public ZombieHealthScript healthScript;
}
