using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public float currentHealth;
	private float maxHealth;

	public float currentPower;
	private float maxPower;

	private float powerPack;
	private float healthPack;

	void Start() {
		maxHealth = 100;
		maxPower = 30;

		currentHealth = maxHealth;
		currentPower = maxPower;

		powerPack = 20;
		healthPack = 20;

	}


	// Update is called once per frame
	void Update () {
	
		if(currentHealth <= 0){
			Destroy(this.gameObject);
		}
	}


	public void AddMaxHealth()
	{
		currentHealth += healthPack;
		maxHealth += healthPack;

		if (currentHealth > maxHealth)
			currentHealth = maxHealth;

	} //Ende public void AddMaxHealth()

	public void AddMaxPower()
	{
		currentPower += powerPack;
		maxPower += powerPack;

		if (currentPower > maxPower)
			currentPower = maxPower;
	} //Ende public void AddMaxPower()

	public void getDamage(float d)
	{
		Debug.Log ("Schaden erfasst");
	}



} //Ende public class PlayerBehavior
