using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthManager : MonoBehaviour, IHealthManager {

    public float MaxHP = 100;
    public float HPRegenRate = 1.5f;
    public float HPRegenDelay = 10;
    public Vector2 MatsDroppedOnDeath = new Vector2(10, 20);

    private float curHP;
    private float lastTimeDamageTaken;

    private bool isDying = false;
    private float deathDelay = 5.1f;
    private float deathTime; // fuck coroutines
    private ParticleSystem explosion;

    void Start() {
        curHP = MaxHP;
        lastTimeDamageTaken = 0;

        explosion = GetComponentInChildren<ParticleSystem>();
    }

	void Update () {

	    if (curHP <= 0 && !isDying) {
	        isDying = true;
	        explosion.Play();
	        deathTime = Time.time + deathDelay;
            // kill colliders and AI behaviours
	        GetComponent<Collider>().enabled = false;
	        GetComponent<NavMeshAgent>().enabled = false;
	        GetComponent<Rigidbody>().isKinematic = true;
	        MonoBehaviour behaviour = GetComponent<CubeBehaviour>();
	        if (behaviour == null) behaviour = GetComponent<SphereBehaviour>();
	        if (behaviour != null) behaviour.enabled = false;

	        foreach (var renderer in GetComponentsInChildren<MeshRenderer>()) {
	            renderer.enabled = false;
	        }
	        GameController.EnemyKilled((int)Random.Range(MatsDroppedOnDeath.x, MatsDroppedOnDeath.y));
	    }

        // health regen
	    if (HPRegenDelay > 0 && Time.time > lastTimeDamageTaken + HPRegenDelay && curHP < MaxHP) {
	        curHP += HPRegenRate * Time.deltaTime;
	        if (curHP > MaxHP) curHP = MaxHP;
	    }


	    if (isDying && Time.time > deathTime) {
            Destroy(gameObject);
	    }
	}

    public void TakeDamage(float amount) {
        lastTimeDamageTaken = Time.time;
        curHP -= amount;
    }

}

public interface IHealthManager {
    void TakeDamage(float amount);
}