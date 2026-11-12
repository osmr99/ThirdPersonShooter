using UnityEngine;

public class AreaTriggerScript : MonoBehaviour
{
    public GameObject[] zombieGroup;
    public bool isTriggered = false;

    private void Start()
    {
        foreach (GameObject go in zombieGroup)
            go.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !isTriggered)
        {
            isTriggered = true;
            foreach (GameObject go in zombieGroup)
                go.SetActive(true);
        }
    }
}
