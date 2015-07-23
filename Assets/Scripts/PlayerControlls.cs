using UnityEngine;
using System.Collections;
using System;

public class PlayerControlls : MonoBehaviour {



	// Vererbung o.ä.
	public GameObject vrHead;
	public Rigidbody thiBody;
	public Collider state0Collider;
	public Collider state2Collider;


	// Anpassungen Speed Movement
	private float speedMovement;
	private float speedGraben;
	private float speedRotation;

	// Allgemeine StatusVariablen
	private int stateMovement;

	private bool spezielleBewegung;
	private int indexBewegung;

	// Spezielle Bewegung
	private float speedSpBewegung;

	private float rotationNull;
	private GameObject groundPlate;

	private float movePlateX;
	private float movePlateY;
	private float movePlateZ;


	/* LEGENDE:
	 * 0 == Bewegung in freiem Raum (Graben)
	 * 1 == Bewegung in Gängen (Graben möglich)
	 * 2 == Bewegung im beschränktem Raum, Hohlraum (Graben beschränkt möglich)
	*/







// START INITIALISIERUNG ---------------------------------------------------------------
	void Start () {

		//Initialisierung
		spezielleBewegung = false;
		indexBewegung = -1;

// Settings -----------------------------------------------------------------------------
		speedMovement = 2;
		speedGraben = 1;
		speedRotation = 60;
		speedSpBewegung = 1.0f;

// TEST / DEBUG -------------------------------------------------------------------------
		/* ACHTUNG ACHTUNG ACHTUNG ACHTUNG ACHTUNG ACHTUNG */
		setState (0);   //ANFANGSZUSTAND MOVEMENT

	} //ENDE Start()







// UPDATE -------------------------------------------------------------------------------
	void Update () {

// BEWEGUNG -----------------------------------------------------------------------------
	// BEWEGUNG in freiem Raum (Graben)
	if (stateMovement == 0 && !spezielleBewegung) {
		//Abfrage nach "Graben"
			transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * speedRotation * Time.deltaTime);
			transform.Rotate (Vector3.up * Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime);
			transform.Rotate(Vector3.forward * vrHead.transform.rotation.eulerAngles.z * -1);


			// DEBUG MODE, fliegen/graben
			if (Input.GetKey (KeyCode.X)) {
				// ZUM TESTEN -> Rotation Kopf
				transform.Translate (Vector3.forward * speedGraben * Time.deltaTime);
				// DO GRABEN + Tunnel erstellen
			}
		} //Ende stateMovement == 0

	// BEWEGUNG in Gängen (Graben möglich)
		if (stateMovement == 1 && !spezielleBewegung) {} //Ende stateMovement == 1

	// BEWEGUNG im beschränktem Raum, Hohlraum (Graben beschränkt möglich)
		if (stateMovement == 2 && !spezielleBewegung) {
		
			// ZUM TESTEN -> Rotation Kopf
			vrHead.transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * speedRotation * Time.deltaTime);
			transform.Rotate (Vector3.up * Input.GetAxis("Horizontal") * speedRotation * Time.deltaTime);
			vrHead.transform.Rotate(Vector3.forward * vrHead.transform.rotation.eulerAngles.z * -1);

			// Movement mit Tastatur
			transform.Translate (Vector3.right * Input.GetAxis ("MoveX") * speedMovement * Time.deltaTime);
			transform.Translate (Vector3.forward * Input.GetAxis ("MoveY") * speedMovement * Time.deltaTime);

			// ZUM TESTEN (MOVEMENT) - VRBrille
			//	transform.Translate (Vector3.forward * Inpus.GetAxis ("Vertical") * speedMovement * Time.deltaTime);
		} //Ende stateMovement == 2
	
	
// GRABEN -----------------------------------------------------------------------------


// SPEZIELLE BEWEGUNGEN -----------------------------------------------------------------------------

		/* LEGENDE ----------
		 * -1 = NOTHING / null
		 * 0  = enterRaum (Drehung und Verschiebung
		 * 
		 */
		if(spezielleBewegung)
		{
			float xA = groundPlate.transform.position.x - (groundPlate.transform.localScale.x/2);
			float yA = groundPlate.transform.position.y - (groundPlate.transform.localScale.y/2);
			float zA = groundPlate.transform.position.z - (groundPlate.transform.localScale.z/2);


			if (indexBewegung == 0)
			{

				if (!(transform.rotation.eulerAngles.x < 359 && transform.rotation.eulerAngles.x < 1) 
				    || !(((transform.position.y - (gameObject.GetComponent<BoxCollider>().size.y/2)) > (yA + groundPlate.transform.localScale.y + 0.08f)) && ((transform.position.y - (gameObject.GetComponent<BoxCollider>().size.y/2)) < (yA + groundPlate.transform.localScale.y + 0.10f)))
				    || !((transform.position.x - (gameObject.GetComponent<BoxCollider>().size.x/2)) > xA && (transform.position.x + (gameObject.GetComponent<BoxCollider>().size.x/2)) < (xA + groundPlate.transform.localScale.x))
				    || !((transform.position.z - (gameObject.GetComponent<BoxCollider>().size.z/2)) > zA && (transform.position.z + (gameObject.GetComponent<BoxCollider>().size.z/2)) < (zA + groundPlate.transform.localScale.z)))  //ALLGEMEINE ABFRAGE!!
				{
					// Y Behandlung
					if(!(((transform.position.y - (gameObject.GetComponent<BoxCollider>().size.y/2)) > (yA + groundPlate.transform.localScale.y + 0.08f)) && ((transform.position.y - (gameObject.GetComponent<BoxCollider>().size.y/2)) < (yA + groundPlate.transform.localScale.y + 0.10f))))
					{
						transform.Translate(Vector3.up * movePlateY * Time.deltaTime);
					}

					// X,Z Behandlung
					if(!((transform.position.x - (gameObject.GetComponent<BoxCollider>().size.x/2)) > xA && (transform.position.x + (gameObject.GetComponent<BoxCollider>().size.x/2)) < (xA + groundPlate.transform.localScale.x)))
					{
						if (movePlateX>0)
							transform.Translate(Vector3.forward * movePlateX * Time.deltaTime);
						else
							transform.Translate(Vector3.back * movePlateX * Time.deltaTime);
					}

					if(!((transform.position.z - (gameObject.GetComponent<BoxCollider>().size.z/2)) > zA && (transform.position.z + (gameObject.GetComponent<BoxCollider>().size.z/2)) < (zA + groundPlate.transform.localScale.z)))
					{
						if (movePlateZ>0)
							transform.Translate(Vector3.forward * movePlateZ * Time.deltaTime);
						else
							transform.Translate(Vector3.back * movePlateZ * Time.deltaTime);
					}

					// Rotation
					if(!(transform.rotation.eulerAngles.x < 359 && transform.rotation.eulerAngles.x < 1))
					{
						transform.Rotate(Vector3.right * rotationNull * Time.deltaTime);
					}
				}
				else
				{
					// Collider aktivieren
					groundPlate.GetComponent<Collider>().enabled = true;
					setState (2);

					spezielleBewegung = false;
					indexBewegung = -1;
				}
			} //Ende if (indexBewegung == 0)

		} //Ende if(spezielleBewegung)

	} // ENDE Update()








	
// Unterscheidung zwischen Movement-Modus -----------------------------------------------------------
	public void setState(int a) {
		stateMovement = a;

		if (a == 0) {
			Debug.Log("setState 0 wird aktiviert");
			thiBody.useGravity = false;
			state0Collider.enabled = true;
			state2Collider.enabled = false;
		}

		if (a == 1) {Debug.Log ("setState 1 wird aktiviert");}

		if (a == 2) {
			Debug.Log ("setState 2 wird aktiviert");
			thiBody.useGravity = true;
			//transform.Translate (Vector3.up * state2Collider.transform.position.y);   //Player nach oben schubsen, damit er nicht hängen bleibt
			state0Collider.enabled = false;
			state2Collider.enabled = true;
		}


	} //Ende setState(..)






// TRIGGER -----------------------------------------------------------------------------
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.tag == "State0") {

			groundPlate = collision.transform.parent.gameObject;

			// Richtung zur GroundPlate ermitteln
			if (groundPlate.transform.position.x > transform.position.x)
				movePlateX = speedMovement;
			else
				movePlateX = -speedMovement;

			if (groundPlate.transform.position.z > transform.position.z)
				movePlateZ = speedMovement;
			else
				movePlateZ = -speedMovement;

			movePlateY = ((groundPlate.transform.position.y + (groundPlate.transform.localScale.y/2)) - transform.position.y) + (gameObject.GetComponent<BoxCollider>().size.y/2);

			// Berechnung Rotation
			float rotationX = gameObject.transform.rotation.eulerAngles.x;

			if ( (rotationX > 90 && rotationX < 270) || rotationX == 270 || rotationX == 90)
			{
				rotationNull = 180-rotationX;
			}
			else 
			{
				if (rotationX > 270)
				{
					rotationNull = 360-rotationX;
				}
				else {
					rotationNull = -rotationX;
				}
			}
				
			// Debug Log
			//Debug.Log ("---------");
			//Debug.Log ("closestPoint: "+closestPoint);
			//Debug.Log ("playerPoint: "+gameObject.transform.position);
			//Debug.Log ("distancePoint: "+distancePoint);
			//Debug.Log ("distance: "+distance);
			//Debug.Log ("");
			//Debug.Log ("RotationX: "+rotationX);
			//Debug.Log ("RotationNull: "+rotationNull);
			//Debug.Log ("");
			//Debug.Log ("---------");

			// Aktivieren von speziellenBewegungen im Update
			spezielleBewegung = true;
			indexBewegung = 0;

		} //Ende if (collision.gameObject.tag == "State0")

	} //Ende void OnTriggerEnter(Collider other)







// COLLISION ---------------------------------------------------------------------------

	void OnCollisionEnter(Collision collision)
	{


		/*if (collision.gameObject.tag != "GroundPlate") {
			statusCollision = true;
			Debug.Log ("status Collision = true"); 
		}*/
	} //Ende void OnCollisionEnter(Collision collision)

	void OnCollisionExit(Collision collision)
	{ 
		if (collision.gameObject.tag == "GroundPlate") {   
			setState(0);
		}
	} //Ende void OnCollisionExit(Collision collision)

}
