using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour, IHealthManager {

    public float MaxHP = 100;
    public float HPRegenRate = 1.5f;
    public float HPRegenDelay = 10;

    private float curHP;
    private float lastTimeDamageTaken;

    void Start() {
        curHP = MaxHP;
        lastTimeDamageTaken = 0;
    }

	void Update () {

        // Enemy deaths are easy: explode them
	    if (curHP <= 0) {
            



	    }

        // health regen
	    if (HPRegenDelay > 0 && Time.time > lastTimeDamageTaken + HPRegenDelay && curHP < MaxHP) {
	        curHP += HPRegenRate * Time.deltaTime;
	        if (curHP > MaxHP) curHP = MaxHP;
	    }
	}

    void OnGUI() {
        GUI.Label(new Rect(10, Screen.height - 30, 600, 20), "Health: " + curHP + "/" + MaxHP);
    }

    public void TakeDamage(float amount) {
        Debug.Log("player took damage");

        lastTimeDamageTaken = Time.time;
        curHP -= amount;
    }
}