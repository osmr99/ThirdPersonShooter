using UnityEngine;
using TMPro;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    PlayerShoot playerShoot;
    PlayerMove playerMove;
    public int maxHealth;
    public int currentHealth;
    public TMP_Text healthText;
    public TMP_Text ammoText;
    public GameObject noAmmoText;
    public float textDuration;
    public float currentDuration;
    public float invTime;
    public float currentInv;

    private void Awake()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerMove = GetComponent<PlayerMove>();
    }

    private void Start()
    {
        HealthUpdate(0, true);
        AmmoUpdate();
    }

    private void Update()
    {
        if(currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            noAmmoText.SetActive(true);
        }
        else
        {
            noAmmoText.SetActive(false);
        }

        if(currentInv > 0)
            currentInv -= Time.deltaTime;
            
    }

    public void HealthUpdate(int num, bool isHeal)
    {
        if (isHeal)
            currentHealth += num;
        else
            currentHealth -= num;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        if (currentHealth < 0)
            currentHealth = 0;

        if (currentHealth > maxHealth / 1.3f)
            healthText.color = Color.green;
        if (currentHealth <= maxHealth / 1.3f)
            healthText.color = Color.green + Color.yellow;
        if (currentHealth <= maxHealth / 2)
            healthText.color = Color.yellow;
        if (currentHealth <= maxHealth / 4)
            healthText.color = Color.yellow + Color.red;
        if (currentHealth <= maxHealth / 5)
            healthText.color = Color.red;
        if(currentHealth <= 0)
        {
            playerMove.OnDisable();
            playerShoot.OnDisable();
            StartCoroutine(GameOver());
        }
        healthText.text = currentHealth + " / " + maxHealth;
    }

    public void AmmoUpdate()
    {
        switch (playerShoot.weaponID)
        {
            case 0:
                if (playerShoot.weaponsAmmoCurrent[0] > 0)
                    AmmoText(playerShoot.weaponID, true);
                else
                    AmmoText(playerShoot.weaponID, false);
                break;
            case 1:
                if (playerShoot.weaponsAmmoCurrent[1] > 0)
                    AmmoText(playerShoot.weaponID, true);
                else
                    AmmoText(playerShoot.weaponID, false);
                break;
            case 2:
                if (playerShoot.weaponsAmmoCurrent[2] > 0)
                    AmmoText(playerShoot.weaponID, true);
                else
                    AmmoText(playerShoot.weaponID, false);
                break;
        }
    }

    void AmmoText(int weaponID, bool hasAmmo)
    {
        if (hasAmmo)
            ammoText.color = Color.white;
        else
            ammoText.color = Color.red;
        ammoText.text = playerShoot.weaponsAmmoCurrent[weaponID] +
                    " / " + playerShoot.weaponsAmmoMax[weaponID];

    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.9f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Dead");
    }

}
