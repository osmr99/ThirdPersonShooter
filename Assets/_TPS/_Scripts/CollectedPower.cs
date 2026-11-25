using UnityEngine;

public class CollectedPower : MonoBehaviour
{
    public GameObject text;
    _OmarScript myScript;

    private void Awake()
    {
        myScript = FindFirstObjectByType<_OmarScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlaySound2D("POWER");
            text.SetActive(true);
            Destroy(text, 3f);
            myScript.allowSwoon = true;
            gameObject.SetActive(false);
        }
            
    }

}
