using UnityEngine;
using System.Collections;

public class WeaponBehavior : MonoBehaviour {

	public float doDamage = 10f;
	public float timerShoot = 50f;

	private bool isShoot;
	private float counterShoot;




	// Use this for initialization
	void Start () {
		isShoot = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (isShoot) {
			if (counterShoot >= timerShoot)
			{
				isShoot = false;
				counterShoot = 0;
			}
			else {
				counterShoot ++;
			}
		}

	} //Ende void Update()



	public float DoShoot()
	{
		isShoot = true;
		return doDamage;
	} //Ende public float doShoot()



	public bool GetShoot()
	{
		return isShoot;
	} //Ende public bool getShoot()


} //Ende public class WeaponBehavior : MonoBehaviour
