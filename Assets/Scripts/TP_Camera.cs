using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour
{
	public static TP_Camera Instance;
	public Transform TargetLookAt;
	public float Distance = 15.0f;
	public float DistanceMin = 5.0f;
	public float DistanceMax = 75.0f;
	public float DistanceSmooth = 0.05f;
	public float DistanceResumeSmooth = 0.5f;
	public float X_MouseSensitivy = 5.0f;
	public float Y_MouseSensitivy = 5.0f;
	public float Wheel_MouseSensitivy = 15.0f;
	public float X_Smooth = 0.05f;
	public float Y_Smooth = 0.1f;
	public float Y_MinLimit = -40.0f;
	public float Y_MaxLimit = 80.0f;
	public float OcclusionDistanceStep = 0.5f;
	public int MaxOcclusionChecks = 10;
	private float mouseX = 0.0f;
	private float mouseY = 0.0f;
	private float velX = 0.0f;
	private float velY = 0.0f;
	private float velZ = 0.0f;
	private float velDistance = 0.0f;
	private float startDistance = 0.0f;
	private Vector3 position = Vector3.zero;
	private Vector3 targetPosition = Vector3.zero;
	private float targetDistance = 0.0f;
	private float distanceSmooth = 0f;
	private float preOccludedDistance = 0f;
	
	void Awake()
	{
		Instance = this;
	}
	
	void Start()
	{
		// Validate Distance.
		Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
		startDistance = Distance;
		Reset();
	}
	
	void LateUpdate()
	{
		if (TargetLookAt == null)
			return;
		
		HandlePlayerInput();
		
		// Deal with occlusion.
		var count = 0;
		do
		{
			CalculateTargetPosition();
			count++;
		} while (CheckIfOccluded(count));
		
		UpdatePosition();
	}
	
	void HandlePlayerInput()
	{
		var deadZone = 0.01f;
		float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

		// Only orbit camera if Right Mouse Button is down.
		if (Input.GetMouseButton(1))
		{
			mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivy;
			mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivy;
		}
		
		// Clamp Mouse Y rotation to avoid Gimbal lock.
		mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);
		
		if (mouseWheelInput < -deadZone || mouseWheelInput > deadZone)
		{
			targetDistance = Mathf.Clamp(Distance - mouseWheelInput * Wheel_MouseSensitivy, DistanceMin, DistanceMax);
			preOccludedDistance = targetDistance;
			distanceSmooth = DistanceSmooth;
		}
	}

	void CalculateTargetPosition()
	{
		ResetTargetDistance();
		// Evaluate Distance.
		Distance = Mathf.SmoothDamp(Distance, targetDistance, ref velDistance, distanceSmooth);
		// Calculate target position.
		targetPosition = CalculatePosition(mouseY, mouseX, Distance);
	}
	
	Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
		return TargetLookAt.position + rotation * direction;
	}
	
	bool CheckIfOccluded(int count)
	{
		var isOccluded = false;
		float nearestDistance = CheckCameraPoints(TargetLookAt.position, targetPosition);
		
		if (nearestDistance != -1)
		{
			if (count < MaxOcclusionChecks)
			{
				isOccluded = true;
				Distance -= OcclusionDistanceStep;
				// May need adjusting based on character.
				if (Distance <= 0.25f)
					Distance = 0.25f;
			} else
				// The 0.25f  is added to avoid camera going through ground.
				Distance = nearestDistance + Camera.main.nearClipPlane + 0.25f;
			
			targetDistance = Distance;
			distanceSmooth = DistanceResumeSmooth;
		}
		return isOccluded;
	}
	
	float CheckCameraPoints(Vector3 fromVector, Vector3 toVector)
	{
		var nearestDistance = -1f;
		RaycastHit hitInfo;
		Helper.ClipPlanePoints clipPlanePoints = Helper.ClipPlaneAtNear(toVector);
		
		// Draw lines in the editor to make it easier to visualize.
		Debug.DrawLine(fromVector, toVector + transform.forward * -camera.nearClipPlane, Color.red);
		Debug.DrawLine(fromVector, clipPlanePoints.UpperLeft);
		Debug.DrawLine(fromVector, clipPlanePoints.LowerLeft);
		Debug.DrawLine(fromVector, clipPlanePoints.UpperRight);
		Debug.DrawLine(fromVector, clipPlanePoints.UpperRight);
		Debug.DrawLine(clipPlanePoints.UpperLeft, clipPlanePoints.UpperRight);
		Debug.DrawLine(clipPlanePoints.UpperRight, clipPlanePoints.LowerRight);
		Debug.DrawLine(clipPlanePoints.LowerRight, clipPlanePoints.LowerLeft);
		Debug.DrawLine(clipPlanePoints.LowerLeft, clipPlanePoints.UpperLeft);
		
		if (Physics.Linecast(fromVector, clipPlanePoints.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
			nearestDistance = hitInfo.distance;
		
		return nearestDistance;
	}
	
	void ResetTargetDistance()
	{
		if (targetDistance < preOccludedDistance)
		{
			Vector3 pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);
			float nearestDistance = CheckCameraPoints(TargetLookAt.position, pos);
			
			if (nearestDistance == -1 || nearestDistance > preOccludedDistance)
			{
				targetDistance = preOccludedDistance;
			}
		}
	}
	
	void UpdatePosition()
	{
		// Smooth each indivisual axis positions.
		var posX = Mathf.SmoothDamp(position.x, targetPosition.x, ref velX, X_Smooth);
		var posY = Mathf.SmoothDamp(position.y, targetPosition.y, ref velY, Y_Smooth);
		var posZ = Mathf.SmoothDamp(position.z, targetPosition.z, ref velZ, X_Smooth);
		position = new Vector3(posX, posY, posZ);
		transform.position = position;
		transform.LookAt(TargetLookAt);
	}
	
	public void Reset()
	{
		mouseX = 0;
		mouseY = 10;
		Camera.main.nearClipPlane = 0.1f;
		Distance = startDistance;
		targetDistance = Distance;
		preOccludedDistance = Distance;
	}
	
	// Uses existing main camera or creates one and assigns a target look at object to it.
	public static void MainCameraManager()
	{
		GameObject tempCamera;
		GameObject targetLookAt;
		TP_Camera myCamera;
		
		if (Camera.main != null)
			tempCamera = Camera.main.gameObject;
		// Create a main camera if scene doesn't contain one.
		else
		{
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent("Camera");
			tempCamera.tag = "MainCamera";
		}
		
		if (tempCamera.GetComponent("TP_Camera") as TP_Camera == null)
			// Add TP_Camera class to main camera
			tempCamera.AddComponent("TP_Camera");
		myCamera = tempCamera.GetComponent("TP_Camera") as TP_Camera;
		targetLookAt = GameObject.Find("targetLookAt") as GameObject;

		// If there isn't a target look at game object, make one.
		if (targetLookAt == null)
		{
			targetLookAt = new GameObject("targetLookAt");
			targetLookAt.transform.position = Vector3.zero;
			targetLookAt.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
		}
		myCamera.TargetLookAt = targetLookAt.transform;
	}
}
