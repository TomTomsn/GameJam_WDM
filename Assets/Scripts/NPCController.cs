using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour {

	// Allgemeine Variablen
	public string name;
	public GameObject player;
	public int idleTimer = 500;  		//Wert setzen um die Dauer zwischen Idle und Bewegung zu verändern.
	public float distanceAction = 5f;  //Wert setzen um Distanz für Aktionen zu verändern
	public float distanceMesh = 20f;	//Wert setzen um Distanz für aktivieren des MeshRenderers zu verändern.

	public Transform waypoint;
	protected Vector3 waypointPosition;
	protected Vector3 standartPosition;
	protected bool statusPosition = false; //false = STANDART , true = WAYPOINT

	public float moveSpeed = 2;
	public int retryAnzahl = 15;

	protected bool idle;
	protected bool moveIdle;
	protected bool isAction;
	protected int idleCounter;
	protected Vector3 waypointTarget;
	protected Vector3 waypointOld;
	protected Vector3 waypointCurrent;
	protected Vector3 waypointIdleNext;

	protected bool moveBack;
	protected int retryCount;



// START ---------------------------------------------------------------------------
	protected void Start () {
		idle = false;
		moveIdle = false;
		isAction = false;
		idleCounter = 0;

		waypointPosition = waypoint.transform.position;
		standartPosition = gameObject.transform.position;
	} //Ende void Start()



// IDLE -------------------------------------------------------------------------------
	protected void updateIdle()
	{
		idleCounter++;
//		Debug.Log ("updateIdle: " + idleCounter);
	} //Ende private void updateCount()



	protected void controlIdle()
	{
		if (idleCounter == idleTimer && !moveIdle) {
			idleCounter = 0;
			moveIdle = true;
			waypointOld = gameObject.transform.position;
			if(!statusPosition)
				waypointIdleNext = waypointPosition;
			else
				waypointIdleNext = standartPosition;
		}
		else {
			//Idle();    //Idle Animation
		}
	} //Ende protected virtual void controlIdle()



// KONTROLLE -------------------------------------------------------------------------------
	protected bool controlDistance(float f)
	{
		if(f <= distanceMesh)
		{
			enableRenderer(true);
			controlAction(f);
			return true;
		}
		else {
			enableRenderer(false);
			return false;
		}
	} //Ende protected bool controlDistance(float f)


	protected virtual void enableRenderer(bool a) 
	{	
		Renderer rend = gameObject.GetComponent<Renderer>();
		rend.enabled = a;
	} //Ende protected virtual void enableRenderer(bool a) 


	
	protected void controlAction (float f)
	{
		if (f <= distanceAction) {
			isAction = true;
			waypointOld = gameObject.transform.position;
		}
		else {
			isAction = false;
		}

	} //Ende protected void OnBecameVisible (float f)



// MOVE ---------------------------------------------------------------------------------
	protected virtual void move(Vector3 v) 
	{
		//Überprüfen ob Zurücklaufen oder zum Ziel laufen
		if (moveBack) 
			waypointTarget = waypointOld;
		else
			waypointTarget = v;

		//Spieler anschauen, um auf ihn zu zu laufen
		gameObject.transform.LookAt (new Vector3(waypointTarget.x, gameObject.transform.position.y ,waypointTarget.z));

		//Auf Spieler zu bewegen
		gameObject.transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

		//Abfrage ob Zurücklaufen erfolgreich
		if (gameObject.transform.position.x >= waypointOld.x-0.25 && gameObject.transform.position.x <= waypointOld.x+0.25f
		    && gameObject.transform.position.z >= waypointOld.z-0.25 && gameObject.transform.position.z <= waypointOld.z+0.25f
		    && moveBack) {
			moveBack = false;

		//Idle Reset, damit nicht sofort neu losgelaufen wird
			idleCounter = 0;
			moveIdle = false;
		} //Ende if (gameObject.transform.position.x >= waypointOld.x-0.25 && gameObject.transform.position.x <= waypointOld.x+0.25f && ...

		//Überprüfung, ob man Anzahl an Versuchen überschritten
		if (retryCount >= retryAnzahl) 
		{
			moveBack = true;
			retryCount = 0;
		} //Ende if (retryCount >= retryAnzahl) 
	} //Ende protected virtual void move(Vector3 v)



// COLLISION ---------------------------------------------------------------------------
	protected void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "GroundPlate") {  
			retryCount++;
		}
	}

	protected void OnCollisionStay (Collision collision)
	{
		if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "GroundPlate") {  
			retryCount++;
		}
	}

	protected void onCollisionExit (Collision collision)
	{
		retryCount = 0;
	}



// OVERRITE ---------------------------------------------------------------------------	
	protected virtual void doAction() {}
	protected virtual void Update() {}
} //Ende public class NPCController : MonoBehaviour {