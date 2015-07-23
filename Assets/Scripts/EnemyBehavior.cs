using UnityEngine;
using System.Collections;

public class EnemyBehavior : NPCController {

	public float currentHealth = 20f;
	public float maxHealth = 20f;
	public float attackRange = 50f;
	public float doDamage = 15f;

	public bool isDead;
	 


// UPDATE --------------------------------------------------------------------------
	protected override void Update () {
		//Objekt Rot färben - DMG damit darstellen
		float distanceTemp = Vector3.Distance (player.transform.position, gameObject.transform.position);
		if (controlDistance(distanceTemp)) 
		{
			if(isAction)
			{
				if(distanceTemp <= attackRange)
				{
					doAction();
				}
				else
				{
						move(player.transform.position);
				}
			} //Ende if(isAction)
			else
			{
				controlIdle();
				if (moveIdle) 
				{
					if(gameObject.transform.position.x >= waypointIdleNext.x-0.25f && gameObject.transform.position.x <= waypointIdleNext.x+0.25f
					   && gameObject.transform.position.z >= waypointIdleNext.z-0.25f && gameObject.transform.position.z <= waypointIdleNext.z+0.25f)
					{
						moveIdle = false;
						if(statusPosition)
							statusPosition = false;
						else
							statusPosition = true;
					}
					else {
						move(waypointIdleNext);	
					}
				} //Ende if (moveIdle) 
				else {
					updateIdle();
				} //Ende else -> if (moveIdle) 
			}//Ende else -> if(isAction)
		} //Ende if (controlDistance)
	} //Ende void Update()



// ACTION ---------------------------------------------------------------------------
	protected override void doAction ()
	{
		player.GetComponent<PlayerBehavior>().getDamage(doDamage);
	}



// DAMAGE ---------------------------------------------------------------------------
	public void DealDamage(float adj)
	{
		currentHealth -= adj;

		if (currentHealth <= 0)
			isDead = true;

		if (currentHealth > maxHealth)
			currentHealth = maxHealth;

		if (isDead == true)
			Destroy (this.gameObject);

		Debug.Log (currentHealth);
	} //Ende void DealDamage(float adj)



} //Ende public class EnemyBehavior : NPCController 
