using UnityEngine;
using System.Collections;

public class PlayerFeetAnimator : MonoBehaviour {

	Animator anim;
	public bool pause=false;

	private float move;
	private bool isStrafing = false;
	private bool isStrafingLeft = false;
	private Vector3 moveDirection;
	private Vector3 lookDirection;
	private float angleFeet;
	private float angleMove;
	private float angleLook;
	private float moveLookDot;
	private Vector3 moveLookCross;

	void Start(){
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate(){
		if(!pause){
			//Calculate the move direction of the player
			moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0.0f, Input.GetAxisRaw ("Vertical"));
			moveDirection = Vector3.Normalize (moveDirection);

			//Set animation parameter for moving
			move = moveDirection.sqrMagnitude;
			anim.SetFloat ("speed", move);

			//Calculate the look direction of the player
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
			lookDirection = new Vector3(cursorPosition.x - transform.position.x, 0.0f, cursorPosition.z - transform.position.z);
			lookDirection = Vector3.Normalize (lookDirection);

			//Define angleMove, the angle between the global z-axis and the move direction about the y-axis, positive counterclockwise
			if(moveDirection.x <= 0){
				angleMove = Vector3.Angle (new Vector3 (0, 0, 1), moveDirection);
			}
			else{
				angleMove = 360 - Vector3.Angle (new Vector3 (0, 0, 1), moveDirection);
			}
			if(lookDirection.x <= 0){
				angleLook = Vector3.Angle (new Vector3 (0, 0, 1), lookDirection);
			}
			else{
				angleLook = 360 - Vector3.Angle (new Vector3 (0, 0, 1), lookDirection);
			}

			//Define the Dot Product between the move and look directions.
			moveLookDot = Vector3.Dot (moveDirection, lookDirection);
			//Define the Cross Product between the move and look directions.
			moveLookCross = Vector3.Cross (moveDirection, lookDirection);

			//Logic to determine whether the player is strafing and what the local angle of the feet should be
			if(moveLookDot >= 0.5 && moveLookDot <= 1.0){
				isStrafing = false;
				angleFeet = angleMove - angleLook;
			}
			else if(moveLookDot > -0.5 && moveLookDot < 0.5){
				isStrafing = true;
				if(moveLookCross.y < 0){
					angleFeet = angleMove - angleLook + 90;
					isStrafingLeft = false;
				}
				else{
					angleFeet = angleMove - angleLook + 270;
					isStrafingLeft = true;
				}
			}
			else {
				isStrafing = false;
				angleFeet = angleMove - angleLook + 180;
			}

			//Set the local angle of the feet to angleFeet
			Vector3 TR = transform.localEulerAngles;
			TR.z = angleFeet;
			transform.localEulerAngles = TR;
			//Set animation bool parameter to trigger strafing
			anim.SetBool ("strafe", isStrafing);
			anim.SetBool ("strafeleft", isStrafingLeft);
		}
		else{
			//Set animation parameter for moving
			anim.SetFloat ("speed", 0f);
		}
	}

}
