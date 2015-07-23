using UnityEngine;
using System.Collections;

public class VRCursor : MonoBehaviour {


    GameObject cursor = null;

    public float cursorDistance = 2f;

	public static VRCursor instance;
	
	///////////////////////////////////////////////////////////////////	

	public GameObject player;
	public WeaponBehavior weapon;

	GameObject currentTarget;
	GameObject nearestObjectRoom;
	GameObject nearestObjectMowl;
	GameObject lastInstance;
	GameObject[] bigRooms;
	GameObject[] rooms;

	float wallDistance;
	float cubeDistance;
	float playerRoomDistance;
	float playerDamage = 10f;
	
	bool enemyBool = false;
	bool isRoomWall = false;
	PlayerBehavior playerBehavior;
	RaycastHit rayHitInfo;
	RaycastHit layerInfo;
	int layerMask = 1 << 9;

	/////////////////////////////////////////////////////////////////// 

	void Awake()
	{
		instance = this;
		if (!cursor) CreateCursor ();
	}

	/// <summary>
	/// Attaches an icon to the cursor.
	/// </summary>
	/// <param name="prefab">The icon prefab</param>
	public void AttachIcon(GameObject prefab)
	{
		GameObject attachment = (GameObject)Instantiate (prefab);
		attachment.transform.parent = cursor.transform;
		attachment.transform.localPosition = Vector3.zero;
		attachment.transform.localRotation = Quaternion.identity;
	}

    public float cursorSize = 0.05f;
    /// <summary>
    /// Creates a cursor for the camera
    /// </summary>
    void CreateCursor()
    {
		cursor = (GameObject)Instantiate (cursorPrefab);//GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cursor.name = "Cursor";
        cursor.transform.localScale = new Vector3(cursorSize, cursorSize, cursorSize);
        cursor.transform.parent = transform;
        if (cursor.GetComponent<Collider>())
        {
            Destroy(cursor.GetComponent<Collider>());
        }
        UIController.Instance.reference = gameObject;
    }

	void Start()
	{
		rooms = GameObject.FindGameObjectsWithTag ("Tunnel");
		bigRooms = GameObject.FindGameObjectsWithTag ("bigRoom");
	}
	
	// Update is called once per frame
	void Update () {
        if (!cursor) CreateCursor();
        PositionCursor();
        CheckCollision();
		ShootEnemy ();
		Digging ();
		FindRoom ();

	}

    
    /// <summary>
    /// Positions the cursor in the center of the left and right camera
    /// </summary>
    void PositionCursor(){
        /*origin = (leftCam.transform.position + rightCam.transform.position) * 0.5f;
        Quaternion rotation = Quaternion.Lerp(leftCam.transform.rotation, rightCam.transform.rotation, 0.5f);
        cursor.transform.rotation = rotation;
        cursor.transform.position = origin;
        cursor.transform.Translate(new Vector3(0, 0, cursorDistance), Space.Self);*/
        cursor.transform.localPosition = new Vector3(0,0, cursorDistance);
    }

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////

	void ShootEnemy()
	{
		if (enemyBool == true && weapon.GetShoot()== false) 
		{
			currentTarget.GetComponent<EnemyBehavior>().DealDamage(weapon.DoShoot());
		}
	}

	void Digging(){

		GameObject[] mowlrooms = GameObject.FindGameObjectsWithTag ("Maulwurftunnel");
		wallDistance = Vector3.Distance (rayHitInfo.collider.gameObject.transform.position, player.transform.position);
		bool isBlockMowl = false;
		bool isBlockRoom = false;
		currentTarget = rayHitInfo.collider.transform.parent.gameObject;
		//Debug.Log(wallDistance +" "+ rayHitInfo.collider.gameObject.tag+" "+currentTarget+" "+isBlockRoom+" "+cubeDistance+" "+isRoomWall);



		switch (rayHitInfo.collider.gameObject.tag) 
		{
		case "Links":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log(cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2 && nearestObjectRoom.transform.position.z < currentTarget.transform.position.z)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}
//
			for(int i = 0; i < mowlrooms.Length; i++)
			{
				nearestObjectMowl = mowlrooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
				if(cubeDistance == 2 && nearestObjectMowl.transform.position.z < currentTarget.transform.position.z)
				{
					isBlockMowl = true;
					//nearestObjectMowl = mowlrooms[i].gameObject;
					break;
				}
			}

			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				if(isBlockRoom == true)
				{
					nearestObjectRoom.transform.GetChild(5).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockRoom = false;
				}
				else if(isBlockMowl == true)
				{
					nearestObjectMowl.transform.GetChild(5).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockMowl = false;
				}
				else
				{
					GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z - 2), Quaternion.identity);
					newTunnel.transform.GetChild(5).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					lastInstance = newTunnel;
				}
			}
			break;
		case "Hinten":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log("for geht "+cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2  && nearestObjectRoom.transform.position.x > currentTarget.transform.position.x)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}

			for(int i = 0; i < mowlrooms.Length; i++)
			{
				nearestObjectMowl = mowlrooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
				if(cubeDistance == 2 && nearestObjectMowl.transform.position.x > currentTarget.transform.position.x)
				{
					isBlockMowl = true;
					//nearestObjectMowl = mowlrooms[i].gameObject;
					break;
				}
			}
			
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				if(isBlockRoom == true)
				{
					nearestObjectRoom.transform.GetChild(2).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockRoom = false;
				}
				else if(isBlockMowl == true)
				{
					nearestObjectMowl.transform.GetChild(2).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockMowl = false;
				}
				else
				{
					GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x + 2, currentTarget.transform.position.y, currentTarget.transform.position.z), Quaternion.identity);
					newTunnel.transform.GetChild(2).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					lastInstance = newTunnel;
				}
			}
			break;
		case "Vorne":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log("for geht "+cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2  && nearestObjectRoom.transform.position.x < currentTarget.transform.position.x)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}
			
			for(int i = 0; i < mowlrooms.Length; i++)
			{
				nearestObjectMowl = mowlrooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
				if(cubeDistance == 2 && nearestObjectMowl.transform.position.x < currentTarget.transform.position.x)
				{
					isBlockMowl = true;
					//nearestObjectMowl = mowlrooms[i].gameObject;
					break;
				}
			}
			
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				if(isBlockRoom == true)
				{
					nearestObjectRoom.transform.GetChild(1).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockRoom = false;
				}
				else if(isBlockMowl == true)
				{
					nearestObjectMowl.transform.GetChild(1).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockMowl = false;
				}
				else
				{
					GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x - 2, currentTarget.transform.position.y, currentTarget.transform.position.z), Quaternion.identity);
					newTunnel.transform.GetChild(1).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					lastInstance = newTunnel;
				}
			}
			break;
					//GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x - 2, currentTarget.transform.position.y, currentTarget.transform.position.z), Quaternion.identity);
					//newTunnel.transform.GetChild(1).gameObject.SetActive(false);
			
		case "Rechts":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log("for geht "+cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2 && nearestObjectRoom.transform.position.z > currentTarget.transform.position.z)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}
			
			for(int i = 0; i < mowlrooms.Length; i++)
			{
				nearestObjectMowl = mowlrooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
				if(cubeDistance == 2 && nearestObjectMowl.transform.position.z > currentTarget.transform.position.z)
				{
					isBlockMowl = true;
					//nearestObjectMowl = mowlrooms[i].gameObject;
					break;
				}
			}
			
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				if(isBlockRoom == true)
				{
					nearestObjectRoom.transform.GetChild(4).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockRoom = false;
				}
				else if(isBlockMowl == true)
				{
					nearestObjectMowl.transform.GetChild(4).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockMowl = false;
				}
				else
				{
					GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z + 2), Quaternion.identity);
					newTunnel.transform.GetChild(4).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					lastInstance = newTunnel;
				}
			}
			break;
		case "Oben":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log("for geht "+cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2 && nearestObjectRoom.transform.position.y > currentTarget.transform.position.y)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}
			
			for(int i = 0; i < mowlrooms.Length; i++)
			{
				nearestObjectMowl = mowlrooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
				if(cubeDistance == 2 && nearestObjectMowl.transform.position.y > currentTarget.transform.position.y)
				{
					isBlockMowl = true;
					//nearestObjectMowl = mowlrooms[i].gameObject;
					break;
				}
			}
			
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				if(isBlockRoom == true)
				{
					nearestObjectRoom.transform.GetChild(0).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockRoom = false;
				}
				else if(isBlockMowl == true)
				{
					nearestObjectMowl.transform.GetChild(0).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					isBlockMowl = false;
				}
				else
				{
					GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 2, currentTarget.transform.position.z), Quaternion.identity);
					newTunnel.transform.GetChild(0).gameObject.SetActive(false);
					rayHitInfo.collider.gameObject.SetActive(false);
					lastInstance = newTunnel;
				}
			}
			break;
		case "Unten":
			for(int i = 0; i < rooms.Length; i++)
			{
				nearestObjectRoom = rooms[i].gameObject;
				cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectRoom.transform.position);//rooms[i].transform.position);
				//Debug.Log("for geht "+cubeDistance+" "+currentTarget+" "+nearestObjectRoom);
				if(cubeDistance == 2 && nearestObjectRoom.transform.position.y < currentTarget.transform.position.y)
				{
					isBlockRoom = true;
					//nearestObjectRoom = rooms[i].gameObject;
					break;
				}
			}
				
				for(int i = 0; i < mowlrooms.Length; i++)
				{
					nearestObjectMowl = mowlrooms[i].gameObject;
					cubeDistance = Vector3.Distance (currentTarget.transform.position, nearestObjectMowl.transform.position);//mowlrooms[i].transform.position);
					if(cubeDistance == 2 && nearestObjectMowl.transform.position.y < currentTarget.transform.position.y)
					{
						isBlockMowl = true;
						//nearestObjectMowl = mowlrooms[i].gameObject;
						break;
					}
				}
				
				if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
				{
					if(isBlockRoom == true)
					{
						nearestObjectRoom.transform.GetChild(3).gameObject.SetActive(false);
						rayHitInfo.collider.gameObject.SetActive(false);
						isBlockRoom = false;

					}
					else if(isBlockMowl == true)
					{
						nearestObjectMowl.transform.GetChild(3).gameObject.SetActive(false);
						rayHitInfo.collider.gameObject.SetActive(false);
						isBlockMowl = false;
					}
					else
					{
						GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y - 2, currentTarget.transform.position.z), Quaternion.identity);
						newTunnel.transform.GetChild(3).gameObject.SetActive(false);
						rayHitInfo.collider.gameObject.SetActive(false);
						lastInstance = newTunnel;
					}
				}
				break;
		}
		if (isRoomWall == true && wallDistance <= 1.4f && Input.GetButtonDown ("digging")) 
		{
			lastInstance.SetActive(false);
			rayHitInfo.collider.gameObject.SetActive(false);
			isRoomWall = false;
		}
	}

	void FindRoom()
	{
		int c = 0;
		GameObject nextRoom = bigRooms[c];
		playerRoomDistance = Vector3.Distance (player.transform.position, nextRoom.transform.position);
		Debug.Log (playerRoomDistance);
		if (playerRoomDistance <= 3.3)
		{
			c = c+1;
		}
		if (c > 2)
		{
			c = 0;
		}
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public GameObject cursorPrefab;

	void ShowCursor(bool value)
	{
		cursor.GetComponent<Renderer>().enabled = value;
	}

	float selectionTimer = 0;

	SelectableObject lastSelectedSo = null;

    void CheckCollision()
    {
		ShowCursor (true);
        RaycastHit hitInfo;
        MenuIcon.selectedItem = null;


        if (Physics.Raycast(transform.position, cursor.transform.position - transform.position, out hitInfo)){
			rayHitInfo = hitInfo;

			if (hitInfo.collider.gameObject.tag == "Screen"){
				ShowCursor(false);
			}
            //Test if the collided object is selectable
            SelectableObject so = hitInfo.collider.gameObject.GetComponent<SelectableObject>();
			if (so == null && lastSelectedSo != null && lastSelectedSo.progressBar)
			{
				lastSelectedSo.progressBar.SetActive(false);
			}

            if (so && !UIController.Instance.menu)
            {
				selectionTimer += Time.deltaTime;
				if (so.selectionTime != 0)
				{
					so.SetProgress(selectionTimer / so.selectionTime);
				}
				if (selectionTimer >= so.selectionTime)
				{
					so.SelectionEvent();
					if (so.selectionTime != 0) so.progressBar.SetActive(false);
                	//Inform the ui controller that a menu has been selected
                	UIController.Instance.SetMenu(so.menuPrefab.gameObject);
				}
            }
            //If it is not a selectable object, test if it is a menu icon
            else
            {
				selectionTimer = 0;
                MenuIcon mi = hitInfo.collider.gameObject.GetComponent<MenuIcon>();
                if (mi)
                {
                    MenuIcon.selectedItem = mi;
                }
            }



			///////////////////////////////////////////////////////////////////////////////////////////////////////////

			if(hitInfo.collider.gameObject.tag == "enemy" && Input.GetButtonDown("Jump"))
			{
				enemyBool = true;
				currentTarget = hitInfo.collider.gameObject;
			}
			else
			{
				enemyBool = false;
			}

			if(hitInfo.collider.gameObject.tag == "Raumwand")
			{
				isRoomWall = true;
			}
			else
			{
				isRoomWall = false;
			}

			//Debug.Log(currentTarget);
			///////////////////////////////////////////////////////////////////////////////////////////////////////////
			lastSelectedSo = so;
        }
		else if (lastSelectedSo)
		{
			selectionTimer = 0;
			if (lastSelectedSo.progressBar) lastSelectedSo.progressBar.SetActive(false);
		}
    }
}
