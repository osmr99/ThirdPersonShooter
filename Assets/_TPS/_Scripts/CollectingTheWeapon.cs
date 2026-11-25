using System.Collections;
using UnityEngine;

public class CollectingTheWeapon : MonoBehaviour
{
    PlayerShoot playerShoot;
    bool isTriggered;
    public int weaponID;
    
    private void Awake()
    {
        playerShoot = FindFirstObjectByType<PlayerShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            switch (weaponID)
            {
                case 1:
                    playerShoot.FoundRifle();
                    break;
                case 2:
                    playerShoot.FoundShotGun();
                    break;
            }
            gameObject.SetActive(false);
        }
    }
}
