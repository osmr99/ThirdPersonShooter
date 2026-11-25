using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class _OmarScript : MonoBehaviour
{
    PlayerShoot playerShoot;
    PlayerMove playerMove;
    public GameObject[] zombiesAlive;
    public GameObject swoonImage;
    public bool allowSwoon = false;
    bool canSwoon = true;

    private void Awake()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerMove = GetComponent<PlayerMove>();

    }
    private void Update()
    {
        //if (Keyboard.current.escapeKey.wasPressedThisFrame)
            //EditorApplication.ExitPlaymode();
            //Application.Quit();

        //if (Keyboard.current.qKey.wasPressedThisFrame)
            //EditorApplication.isPaused = !EditorApplication.isPaused;

        if (Keyboard.current.cKey.wasPressedThisFrame)
            SceneManager.LoadScene("Main");

        //if (Keyboard.current.zKey.wasPressedThisFrame)
        //playerShoot.hasRifle = !playerShoot.hasRifle;

        //if (Keyboard.current.xKey.wasPressedThisFrame)
        //playerShoot.hasShotGun = !playerShoot.hasShotGun;

        //if (Keyboard.current.leftAltKey.isPressed)
        //Cursor.lockState = CursorLockMode.None;
        //else
        //Cursor.lockState = CursorLockMode.Locked;

        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            if (allowSwoon && canSwoon)
                StartCoroutine(SWOON());
        }
    }



    public IEnumerator SWOON()
    {
        canSwoon = false;
        bool hitSomeone = false;
        playerMove.OnDisable();
        playerShoot.OnDisable();
        Time.timeScale = 0;
        swoonImage.SetActive(true);
        SoundManager.Instance.PlaySound2D("SWOON");
        
        yield return new WaitForSecondsRealtime(3f);

        swoonImage.SetActive(false);
        Time.timeScale = 1;
        foreach (GameObject zombie in zombiesAlive)
        {
            if (!zombie.IsDestroyed() && zombie.activeInHierarchy == true)
            {
                hitSomeone = true;
                var zombieHealth = zombie.GetComponent<ZombieHealthScript>();
                HitZoneArea pain = HitZoneArea.Body;
                zombieHealth.TakeDamage(999, pain);
            }
        }

        if(hitSomeone)
        {
            SoundManager.Instance.PlaySound2D("IMPACT");
            playerMove.OnEnable();
            playerShoot.OnEnable();

            yield return new WaitForSeconds(6.5f);

            canSwoon = true;
        }
        else
        {
            playerMove.OnEnable();
            playerShoot.OnEnable();

            yield return new WaitForSeconds(1f);

            canSwoon = true;
        }
            

        

        

        

    }
}
