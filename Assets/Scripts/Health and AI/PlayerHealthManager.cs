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

        // Take damage if out of "dome".
	    var curPos = transform.root.position;
        if (curPos.x < WorldBuilder.MIN_SPAWN_XN || curPos.x > WorldBuilder.MIN_SPAWN_XP ||
            curPos.z < WorldBuilder.MIN_SPAWN_ZN || curPos.z > WorldBuilder.MIN_SPAWN_ZP) TakeDamage(2.5f * Time.deltaTime);



        // just lose the game if you die
	    if (curHP <= 0) {
            GameController.PlayerInstance.LoseGame();
	    }

        // health regen
	    if (HPRegenDelay > 0 && Time.time > lastTimeDamageTaken + HPRegenDelay && curHP < MaxHP) {
	        curHP += HPRegenRate * Time.deltaTime;
	        if (curHP > MaxHP) curHP = MaxHP;
	    }
	}
    

    public void TakeDamage(float amount) {
        lastTimeDamageTaken = Time.time;
        curHP -= amount;
    }
}