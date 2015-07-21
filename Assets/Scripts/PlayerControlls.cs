using UnityEngine;
using System.Collections;

public class PlayerControlls : MonoBehaviour {


	//Hallo Sebastian, dies ist ein Test. Kannst du das lesen?


	// Vererbung o.ä.
	public GameObject vrHead;


	// Anpassungen Speed Movement
	public float speedMovement;
	public float speedRotation;


	private int stateMovement;
	/* LEGENDE:
	 * 0 == Bewegung in freiem Raum (Graben)
	 * 1 == Bewegung in Gängen (Graben möglich)
	 * 2 == Bewegung im beschränktem Raum, Hohlraum (Graben beschränkt möglich)
	*/

	void Start () {

		//Initialisierung Movement
		stateMovement = 2;

		speedMovement = 2;
		speedRotation = 60;
	}

	void Update () {

	// BEWEGUNG in freiem Raum (Graben)
	if (stateMovement == 0) {}

	// BEWEGUNG in Gängen (Graben möglich)
	if (stateMovement == 1) {}

	// BEWEGUNG im beschränktem Raum, Hohlraum (Graben beschränkt möglich)
	if (stateMovement == 2) {
		
			// ZUM TESTEN (MOVEMENT) - Allgemein
			if (Input.GetKey (KeyCode.X)) {}
			vrHead.transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * speedRotation * Time.deltaTime);
			transform.Rotate (Vector3.up * Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime);

			vrHead.transform.Rotate(Vector3.forward * vrHead.transform.rotation.eulerAngles.z * -1);

			transform.Translate (Vector3.right * Input.GetAxis ("MoveX") * speedMovement * Time.deltaTime);
			transform.Translate (Vector3.forward * Input.GetAxis ("MoveY") * speedMovement * Time.deltaTime);




				
			
			// ZUM TESTEN (MOVEMENT) - VRBrille
			//	transform.Translate (Vector3.forward * Inpus.GetAxis ("Vertical") * speedMovement * Time.deltaTime);
	}
	
	
	// GRABEN (Vorlage)
	/* 
	if (Input.GetButtonDown ("Joystick1Button16")) 
		{
			this.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward);
		}
	*/	






	}
}
