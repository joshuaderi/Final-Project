using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Makes sure that the CharacterController Component is added to what ever the script is attached to
[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
	
	// Variables to control speeds for movements
	public float speed = 6.0f;
	public float jumpSpeed = 8.0f;
	public float rotateSpeed = 5.0f;
	public float gravity = 9.81f;
	
	// Used by Move() to move the player in a certain direction
	Vector3 moveDirection = Vector3.zero;
	
	// Used to select if Move() or SimpleMove() is used
	// Move() == 1
	// SimpleMove() == 0
	public int type;
	private int count;
	public Text countText;
	// Reference to controller used to move the player
	CharacterController cc;
	
	// Used to link the prefab to the player so it can be created
	public Rigidbody projectilePrefab;
	// Used to tell Instantiate where to create the projectile
	public Transform projectileSpawnPoint;
	// Used to tell the projectile how fast it should be fired
	public float fireSpeed = 10.0f;
	
	
	// Use this for initialization
	void Start () {
		// Grab a component and keep a reference to it
		cc = GetComponent<CharacterController>();
		
		count = 0;
		SetCountText ();
		
		// Check if the component exists
		if (!cc)
			Debug.Log("CharacterController does not exist.");
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
			
			count = count +50;
			
			SetCountText ();
		}
	}
	
	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
		// SimpleMove()
		if (type == 1)
		{
			// Rotate the transform the Script is attached to using to left and right keys
			transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
			
			// Translates the local coordinates to world coordinates
			Vector3 forward = transform.TransformDirection(Vector3.forward);
			
			// Uses the up and down arrow keys to come up with a speed to move the player
			float curSpeed = speed * Input.GetAxis("Vertical");
			
			// Apply the SimpleMove action using the direction and speed specified
			cc.SimpleMove(forward * curSpeed);
		}
		// Move()
		else if (type == 0)
		{
			// moveDirection = new Vector3(Input.GetAxis("Horizontal"), cc.velocity.y, Input.GetAxis("Vertical"));
			
			if (cc.isGrounded)
			{
				// Strafe movement
				// Use the left and right keys to move the player along the X-axis
				// Use the up and down keys to move the player along the Z-axis
				//moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
				
				// Use the up and down keys to move the player along the Z-axis
				moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
				
				// Rotate the transform the Script is attached to using to left and right keys
				transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
				
				// Translates the local coordinates to world coordinates
				moveDirection = transform.TransformDirection(moveDirection);
				
				// Make the speed more than 1
				moveDirection *= speed;
				
				// Apply a jumpSpeed when "Space" is pressed
				if (Input.GetButtonDown("Jump"))
					// Change the y direction for a jump
					moveDirection.y = jumpSpeed;
			}
			
			// Apply gravity in the y direction to move the player downwards 
			moveDirection.y -= gravity * Time.deltaTime;
			
			// Apply the Move action using the direction which had speed applied
			cc.Move(moveDirection * Time.deltaTime);
		}
		
		// Key Press Stuff
		if (Input.GetButtonDown("Fire1"))
		{
			Debug.Log("Pew Pew");
			
			// Check if the prefab was dragged into the variable exposed in the Inspector
			if (projectilePrefab)
			{
				// Create a new instance of the projectile using a prefab, position and rotation
				Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as Rigidbody;
				
				// Apply a force to the GameObject to make it move
				temp.AddForce(Vector3.forward * fireSpeed, ForceMode.Impulse);
			}
			else
				// Gracefully tells the user the prefab was not linked propery
				Debug.Log("No prefab found.");
		}
		
		// Check input using LeftControl and not using the axisName from the InputManager
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			
		}
		
	} // closes Update
}