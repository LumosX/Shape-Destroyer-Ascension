using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float Velocity = 10.0f;
    public float Damage = 5;
    public float TTL = 20;
    public float Inaccuracy = 0.01f;

    private float startTime;

	// Use this for initialization
	void Start () {
	    startTime = Time.time;

        // Modify own X/Y rotation to make bullets inaccurate
	    var xOff = Random.Range(-Inaccuracy, Inaccuracy);
	    var yOff = Random.Range(-Inaccuracy, Inaccuracy);
	    transform.localEulerAngles = transform.localEulerAngles + new Vector3(xOff, yOff, 0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition += transform.forward * Velocity * Time.deltaTime;

	    if (Time.time > startTime + TTL) Destroy(gameObject);
	}

    public void OnTriggerEnter(Collider col) {
        var target = col.transform.GetComponent<IHealthManager>();
        target?.TakeDamage(Damage);
        Destroy(gameObject);
    }
}
