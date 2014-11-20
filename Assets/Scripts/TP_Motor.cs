using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour
{
	public static TP_Motor Instance;
	public float ForwardSpeed = 20f;
	public float BackwardSpeed = 15f;
	public float StrafingSpeed = 20f;
	public float SlideSpeed = 10f;
	public float JumpSpeed = 10f;
	public float Gravity = 21f;
	public float TerminalVelocity = 20f;
	public float SlideThreshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;
	private Vector3 slideDirection;
	
	public Vector3 MoveVector { get; set; }

	public float VerticalVelocity { get; set; }

	public float CurrentPowerModifier { get; set; }
	
	void Awake()
	{
		Instance = this;
	}
	
	public void UpdateMotor()
	{
		SnapAlign();
		ProcessMotion();
	}
	
	void ProcessMotion()
	{
		// Transform MoveVector into world-space relative to character location.
		MoveVector = transform.TransformDirection(MoveVector);
		if (MoveVector.sqrMagnitude > 1)
			MoveVector = Vector3.Normalize(MoveVector);
		// Only applies it if applicable.
		ApplySlide();
		MoveVector *= MoveSpeed();
		
		// Reapply VerticalVelocity to MoveVector.y.
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
		
		ApplyGravity();
		
		// Move the character in world-space.
		TP_Controller.CharController.Move(MoveVector * Time.deltaTime);
	}
	
	void ApplyGravity()
	{
		if (MoveVector.y > - TerminalVelocity)
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);
		if (TP_Controller.CharController.isGrounded && MoveVector.y < -1)
			MoveVector = new Vector3(MoveVector.x, -1, MoveVector.z);
	}
	
	void ApplySlide()
	{
		if (!TP_Controller.CharController.isGrounded)
			return;
		slideDirection = Vector3.zero;
		
		// Check to see if the Ray cast downwards hit something.
		// It's cast from one unit up to avoid going past ground into infinity.
		RaycastHit hitInfo;
		if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo))
		{
			if (hitInfo.normal.y < SlideThreshold)
				slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z); 
		}
		
		// Defines MoveVector based on how strong the slide vector is.
		if (slideDirection.sqrMagnitude < MaxControllableSlideMagnitude)
			MoveVector += slideDirection;
		else
			MoveVector = slideDirection;
	}
	
	public void Jump()
	{
		if (TP_Controller.CharController.isGrounded)
			VerticalVelocity = JumpSpeed;
	}

	// Snap aligns character with camera.
	void SnapAlign()
	{
		// If the character is moving at all.
		if (MoveVector.x != 0 || MoveVector.z != 0)
		{
			// This will take the character XZ and camera Y,
			// because character always moves towards where camera is pointing.
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
				Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}
	
	float MoveSpeed()
	{
		var moveSpeed = 0f;
		switch (TP_Animator.Instance.MoveDirection)
		{
		case TP_Animator.Direction.Stationary:
			moveSpeed = 0f;
			break;
		case TP_Animator.Direction.Forward:
			moveSpeed = ForwardSpeed;
			break;
		case TP_Animator.Direction.Backward:
			moveSpeed = BackwardSpeed;
			break;
		case TP_Animator.Direction.Left:
			moveSpeed = StrafingSpeed;
			break;
		case TP_Animator.Direction.Right:
			moveSpeed = StrafingSpeed;
			break;
		case TP_Animator.Direction.LeftForward:
			moveSpeed = ForwardSpeed;
			break;
		case TP_Animator.Direction.LeftBackward:
			moveSpeed = BackwardSpeed;
			break;
		case TP_Animator.Direction.RightForward:
			moveSpeed = ForwardSpeed;
			break;
		case TP_Animator.Direction.RightBackward:
			moveSpeed = BackwardSpeed;
			break;
		}
		// Sliding patch.
		if (slideDirection.sqrMagnitude > 0)
			moveSpeed = SlideSpeed;
		
		moveSpeed *= CurrentPowerModifier;
		return moveSpeed;
	}
}