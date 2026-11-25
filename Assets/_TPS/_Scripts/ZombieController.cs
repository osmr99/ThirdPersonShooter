using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // When this script to a gameobject, it automatically adds this component!
public class ZombieController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] ZombieHealthScript healthScript;
    PlayerStats playerStats;

    [Header("Target")]
    private Transform _player;

    [Header("Movement")]
    public float walkSpeed = 0.8f;
    public float crawlSpeed = 0.3f;
    public float stopDistance = 2;

    [Header("Boolean States")]
    public bool isCrawling = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = FindFirstObjectByType<PlayerStats>();

    }

    private void Update()
    {
        if(healthScript.isDead)
        {
            StopAndDisableAgent();
            return;
        }

        isCrawling = !healthScript.isStanding;
        agent.speed = isCrawling ? crawlSpeed : walkSpeed;

        float _distance = Vector3.Distance(transform.position, _player.position);
        if (_distance < stopDistance)
        {
            if (_distance < stopDistance - 1 && playerStats.currentInv <= 0)
            {
                playerStats.HealthUpdate(3, false);
                playerStats.currentInv = playerStats.invTime;
            }
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(_player.position);
        }
    }

    void StopAndDisableAgent()
    {
        if(agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
    }
}
