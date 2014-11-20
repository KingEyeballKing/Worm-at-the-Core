using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour
{
	public static CharacterController CharController;
	public static TP_Controller Instance;
	
	void Awake()
	{
		// Assign references to static fields.
		CharController = GetComponent("CharacterController") as CharacterController;
		Instance = this;
		
		// Initial camera check.
		TP_Camera.MainCameraManager();
	}
	
	void Update()
	{
		if (Camera.main == null)
			return;
		// GetLocomoyionInput Has to be called before UpdateMotor.
		GetLocomotionInput();
		HandleActionInput();
		TP_Motor.Instance.UpdateMotor();
	}
	
	void GetLocomotionInput()
	{
		float verticalAxis = Input.GetAxis("Vertical");
		float horizontalAxis = Input.GetAxis("Horizontal");
		var deadZone = 0.1f;
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		
		// Zero out (reset) move vector ever frame so motion isn't additive.
		TP_Motor.Instance.MoveVector = Vector3.zero;
		
		// Verify that motion is outside dead space, if so apply axis input to MoveVector.
		if (verticalAxis > deadZone || verticalAxis < -deadZone)
			TP_Motor.Instance.MoveVector += new Vector3(0, 0, verticalAxis);
		if (horizontalAxis > deadZone || horizontalAxis < -deadZone)
			TP_Motor.Instance.MoveVector += new Vector3(horizontalAxis, 0, 0);
		
		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}
	
	void HandleActionInput()
	{
		if (Input.GetButton("Jump"))
		{
			Jump();
		}
	}

	// Is its own method to enable more complex actions independently.
	void Jump()
	{
		TP_Motor.Instance.Jump();
	}
}