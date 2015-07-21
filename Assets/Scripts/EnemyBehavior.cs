using UnityEngine;
using System.Collections;

public class EnemyBehavior : PlayerBehavior {

	public float currentHealth = 100f;
	public float maxHealth = 100f;

	public bool isDead;

	void Update () {
		DealDamage (0);
	}

	public void DealDamage(float adj)
	{
		currentHealth += adj;

		if (currentHealth <= 0)
			isDead = true;

		if (currentHealth > maxHealth)
			currentHealth = maxHealth;

		if (isDead == true)
			Destroy (this.gameObject);
		Debug.Log (currentHealth);
	}
}
