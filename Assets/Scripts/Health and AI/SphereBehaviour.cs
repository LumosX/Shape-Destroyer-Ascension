using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SphereBehaviour : MonoBehaviour {
    
    private NavMeshAgent agent;
    private Transform target;

    public GameObject SphereShotPrefab;
    public float FireDelay = 2.0f;

    private float nextFireTime;

    void Start() {
        agent = GetComponent<NavMeshAgent>();

        target = GameObject.Find("Player/Capsule").transform;
        agent.SetDestination(target.position);
    }

    // Spheres go for the player alone.
    void Update() {
        // Slow them down once they get near, to make things a little easier
        var distToTarget = (transform.position - target.position).sqrMagnitude;
        agent.speed = distToTarget > 100 * 100 ? 40 : 15;
        
        transform.LookAt(target);

        // Stop if close.
        if (distToTarget < 50 * 50 && !agent.isStopped) {
            agent.isStopped = true;
            nextFireTime = Time.time + FireDelay;
        }

        if (agent.isStopped && distToTarget > 50 * 50) {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return;
        }
        
        // Start shooting.
        if (agent.isStopped) {
            if (Time.time > nextFireTime) {
                var firingPos = transform.Find("Visuals/LaserFocus/Start");
                nextFireTime = Time.time + FireDelay;
                var shot = Instantiate(SphereShotPrefab, firingPos.position, Quaternion.identity);
                shot.transform.LookAt(target.position);
            }
        }
        

    }
}
