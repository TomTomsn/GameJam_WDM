using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public float health = 100f;

	// Update is called once per frame
	void Update () {
	
		if(health <= 0){
			Destroy(this.gameObject);
		}
	}
}
