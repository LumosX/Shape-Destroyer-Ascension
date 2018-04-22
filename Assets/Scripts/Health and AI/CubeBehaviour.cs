using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubeBehaviour : MonoBehaviour {
    
    private NavMeshAgent agent;
    private Transform target;
    private LineRenderer lr;

    private float fireDelay = 5.0f;
    private float nextFireTime;
    private float alpha = 0.5f;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        lr = GetComponent<LineRenderer>();

        target = GameObject.Find("GeneratorTarget").transform;
        agent.SetDestination(target.position);
    }

    // Cubes go for the generator.
    void Update() {
        // Slow them down once they get near, to make things a little easier
        var distToTarget = (transform.position - target.position).sqrMagnitude;
        agent.speed = distToTarget > 100 * 100 ? 40 : 15;
        
        transform.LookAt(target);

        // Stop if close.
        if (distToTarget < 1000 && !agent.isStopped) {
            agent.isStopped = true;
            nextFireTime = Time.time + fireDelay;
        }

        if (agent.isStopped && distToTarget > 1000) {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return;
        }
        
        // Start shooting.
        if (agent.isStopped) {
            if (Time.time > nextFireTime) {
                var firingPos = transform.Find("Visuals/Laser Focus/Start").position + transform.forward * 0.2f;
                lr.positionCount = 2;
                lr.SetPositions(new [] { firingPos, target.position });
                alpha = 1f;
                lr.material = new Material(Shader.Find("Particles/Additive (Soft)"));
                nextFireTime = Time.time + fireDelay;
                GameController.GeneratorShot();
            }
            else {
                alpha -= Time.deltaTime * fireDelay * 0.03f;
                var start = Color.red;
                start.a = alpha;
                var end = Color.red;
                end.a = alpha;
                lr.startColor = start;
                lr.endColor = end;
            }
        }
        

    }
}
