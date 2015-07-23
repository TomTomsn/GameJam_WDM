﻿using UnityEngine;
using System.Collections;

public class VRCursor : MonoBehaviour {


    GameObject cursor = null;

    public float cursorDistance = 2f;

	public static VRCursor instance;
	
	///////////////////////////////////////////////////////////////////	

	public GameObject player;
	GameObject currentTarget;

	float wallDistance;
	float playerDamage = 10f;
	
	bool enemyBool = false;
	PlayerBehavior playerBehavior;
	RaycastHit rayHitInfo;


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
	
	// Update is called once per frame
	void Update () {
        if (!cursor) CreateCursor();
        PositionCursor();
        CheckCollision();
		ShootEnemy ();
		DetectWall ();
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
		if (enemyBool == true) 
		{
			currentTarget.GetComponent<Renderer>().material.color = Color.black;
			EnemyBehavior enemyHealth = currentTarget.GetComponent<EnemyBehavior>();
			enemyHealth.DealDamage(-playerDamage);
			currentTarget.GetComponent<Renderer>().material.color = Color.white;
		}
	}

	void DetectWall(){
		
		//Debug.Log (rayHitInfo.collider.gameObject.tag);
		
		wallDistance = Vector3.Distance (rayHitInfo.collider.gameObject.transform.position, player.transform.position);
		currentTarget = rayHitInfo.collider.transform.parent.gameObject;
//		Debug.Log(wallDistance +" "+ rayHitInfo.collider.gameObject.tag+" "+currentTarget);
		
		switch (rayHitInfo.collider.gameObject.tag) 
		{
		case "Links":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z - 2), Quaternion.identity);
				newTunnel.transform.GetChild(5).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
		case "Hinten":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x + 2, currentTarget.transform.position.y, currentTarget.transform.position.z), Quaternion.identity);
				newTunnel.transform.GetChild(2).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
		case "Vorne":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x - 2, currentTarget.transform.position.y, currentTarget.transform.position.z), Quaternion.identity);
				newTunnel.transform.GetChild(1).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
		case "Rechts":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y, currentTarget.transform.position.z + 2), Quaternion.identity);
				newTunnel.transform.GetChild(4).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
		case "Oben":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y + 2, currentTarget.transform.position.z), Quaternion.identity);
				newTunnel.transform.GetChild(0).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
		case "Unten":
			if(wallDistance <= 0.7f && Input.GetButtonDown("digging"))
			{
				GameObject newTunnel = (GameObject)Instantiate(Resources.Load("Tunnel_Maulwurf"), new Vector3(currentTarget.transform.position.x, currentTarget.transform.position.y - 2, currentTarget.transform.position.z), Quaternion.identity);
				newTunnel.transform.GetChild(3).gameObject.SetActive(false);
				rayHitInfo.collider.gameObject.SetActive(false);
			}
			break;
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
