using UnityEngine;

public class CrouchTuto : MonoBehaviour
{
    public GameObject tutoText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            tutoText.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            tutoText.SetActive(false);
    }
}
