using UnityEngine;

public static class Helper
{
	public struct ClipPlanePoints
	{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}
	
	public static ClipPlanePoints ClipPlaneAtNear(Vector3 pos)
	{
		var clipPlanePoints = new ClipPlanePoints();
		
		if (Camera.main == null)
			return clipPlanePoints;
		
		var _transform = Camera.main.transform;
		// Convert to Rad for Tan function.
		var halfFOV = (Camera.main.fieldOfView / 2) * Mathf.Deg2Rad;
		var _aspect = Camera.main.aspect;
		var _distance = Camera.main.nearClipPlane;
		var _height = _distance * Mathf.Tan(halfFOV);
		var _width = _height * _aspect;
		
		clipPlanePoints.LowerRight = pos + _transform.right * _width;
		clipPlanePoints.LowerRight -= _transform.up * _height;
		clipPlanePoints.LowerRight += _transform.forward * _distance;
		
		clipPlanePoints.LowerLeft = pos - _transform.right * _width;
		clipPlanePoints.LowerLeft -= _transform.up * _height;
		clipPlanePoints.LowerLeft += _transform.forward * _distance;
		
		clipPlanePoints.UpperRight = pos + _transform.right * _width;
		clipPlanePoints.UpperRight += _transform.up * _height;
		clipPlanePoints.UpperRight += _transform.forward * _distance;
		
		clipPlanePoints.UpperLeft = pos - _transform.right * _width;
		clipPlanePoints.UpperLeft += _transform.up * _height;
		clipPlanePoints.UpperLeft += _transform.forward * _distance;
		
		return clipPlanePoints;
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		// Do While because it runs at least once.
		do
		{
			if (angle < -360)
				angle += 360;
			if (angle > 360)
				angle -= 360;
		} while (angle < -360 || angle > 360);
		return Mathf.Clamp(angle, min, max);
	}

	// Checks if player is in range of game object based on range and object's bounding box.
	public static bool InRange(float extent, float minR, float maxR, float sqrD)
	{
		float range = maxR - minR;
		return (sqrD - extent <= range);
	}
}