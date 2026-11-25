using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTriggerScript : MonoBehaviour
{
    bool isTriggered = false;
    PlayerMove playerMove;
    PlayerShoot playerShoot;
    PlayerStats playerStats;
    CinemachineBrain cineBrain;
    public GameObject[] cameras;
    public int cutsceneDuration;
    public int cutsceneID;
    public GameObject[] zombieGroup;
    public GameObject secret;


    private void Awake()
    {
        playerMove = FindFirstObjectByType<PlayerMove>();
        playerShoot = FindFirstObjectByType<PlayerShoot>();
        playerStats = FindFirstObjectByType<PlayerStats>();
        cineBrain = FindFirstObjectByType<CinemachineBrain>();
    }

    private void Start()
    {
        foreach (GameObject go in zombieGroup)
            go.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            playerStats.HealthUpdate(10, true);
            switch (cutsceneID)
            {
                case 1:
                    StartCoroutine(CutsceneOne());
                    break;
                case 2:
                    StartCoroutine(CutsceneTwo());
                    break;
                case 3:
                    StartCoroutine(CutsceneThree());
                    break;
                case 4:
                    StartCoroutine(CutsceneFour());
                    break;
                case 5:
                    StartCoroutine(CutsceneFive());
                    break;
                case 6:
                    StartCoroutine(CutsceneSix());
                    break;
            }
        }
    }

    IEnumerator CutsceneOne()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        cineBrain.DefaultBlend.Time = 0.8f;
        cameras[0].SetActive(true);

        yield return new WaitForSeconds(0.25f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn");
        foreach (GameObject go in zombieGroup)
            go.SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[0].SetActive(false);

        yield return new WaitForSeconds(0.75f);

        cineBrain.DefaultBlend.Time = 0.25f;
        playerShoot.OnEnable();
        playerMove.OnEnable();
        secret.SetActive(true);
        Destroy(gameObject, 0.1f);
    }

    IEnumerator CutsceneTwo()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        cineBrain.DefaultBlend.Time = 2f;
        cameras[1].SetActive(true);

        yield return new WaitForSeconds(4f);

        cineBrain.DefaultBlend.Time = 0.8f;
        cameras[1].SetActive(false);
        cameras[0].SetActive(true);
        SoundManager.Instance.PlaySound2D("ZombieSpawn");
        foreach (GameObject go in zombieGroup)
            go.SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[0].SetActive(false);

        yield return new WaitForSeconds(0.75f);

        cineBrain.DefaultBlend.Time = 0.25f;
        playerShoot.OnEnable();
        playerMove.OnEnable();
        Destroy(gameObject, 0.1f);
    }

    IEnumerator CutsceneThree()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        cineBrain.DefaultBlend.Time = 1.5f;
        cameras[0].SetActive(true);

        yield return new WaitForSeconds(0.75f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn");
        zombieGroup[0].SetActive(true);
        zombieGroup[1].SetActive(true);
        zombieGroup[2].SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[0].SetActive(false);
        cameras[1].SetActive(true);

        yield return new WaitForSeconds(0.75f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn");
        zombieGroup[3].SetActive(true);
        zombieGroup[4].SetActive(true);
        zombieGroup[5].SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[1].SetActive(false);

        yield return new WaitForSeconds(1.95f);

        cineBrain.DefaultBlend.Time = 0.25f;
        playerShoot.OnEnable();
        playerMove.OnEnable();
        Destroy(gameObject, 0.1f);
    }

    IEnumerator CutsceneFour()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        cineBrain.DefaultBlend.Time = 2.25f;
        cameras[0].SetActive(true);

        yield return new WaitForSeconds(2.5f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn1");
        zombieGroup[0].SetActive(true);

        yield return new WaitForSeconds(2.4f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn2");
        zombieGroup[1].SetActive(true);

        yield return new WaitForSeconds(2.3f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn3");
        zombieGroup[2].SetActive(true);

        yield return new WaitForSeconds(2.2f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn4");
        zombieGroup[3].SetActive(true);

        yield return new WaitForSeconds(2.1f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn5");
        zombieGroup[4].SetActive(true);

        yield return new WaitForSeconds(2f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn6");
        zombieGroup[5].SetActive(true);

        yield return new WaitForSeconds(1.9f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn7");
        zombieGroup[6].SetActive(true);

        yield return new WaitForSeconds(1.8f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn8");
        zombieGroup[7].SetActive(true);

        yield return new WaitForSeconds(1.7f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn9");
        zombieGroup[8].SetActive(true);

        yield return new WaitForSeconds(1.6f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn10");
        zombieGroup[9].SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn11");
        zombieGroup[10].SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[0].SetActive(false);

        yield return new WaitForSeconds(0.95f);

        cineBrain.DefaultBlend.Time = 0.25f;
        playerShoot.OnEnable();
        playerMove.OnEnable();
        Destroy(gameObject, 0.1f);
    }

    IEnumerator CutsceneFive()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        cineBrain.DefaultBlend.Time = 0.8f;
        cameras[0].SetActive(true);

        yield return new WaitForSeconds(0.5f);

        SoundManager.Instance.PlaySound2D("ZombieSpawn12");
        zombieGroup[0].SetActive(true);

        yield return new WaitForSeconds(cutsceneDuration);

        cameras[0].SetActive(false);

        yield return new WaitForSeconds(1f);

        cineBrain.DefaultBlend.Time = 0.25f;
        playerShoot.OnEnable();
        playerMove.OnEnable();
    }

    IEnumerator CutsceneSix()
    {
        playerMove.OnDisable();
        playerShoot.OnDisable();
        SoundManager.Instance.PlaySound2D("Won");

        yield return new WaitForSeconds(cutsceneDuration - 0.1f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene("Won");
    }
}
