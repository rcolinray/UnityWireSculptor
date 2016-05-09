using UnityEngine;

/// <summary>
/// Rotates a GameObject
/// </summary>
public class Rotator : MonoBehaviour {

	#region Inspector Fields
	
	[Tooltip("The rotation speed in degrees per second")]
	public Vector3 speed;
	
	#endregion
	
	void Update() {
		transform.Rotate(speed * Time.deltaTime);
	}
}
