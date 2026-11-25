using UnityEngine;

public class DontUncrouch : MonoBehaviour
{
    PlayerMove playerMove;

    private void Awake()
    {
        playerMove = FindFirstObjectByType<PlayerMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerMove.canUncrouch = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerMove.canUncrouch = true;
    }
}
