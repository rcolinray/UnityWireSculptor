using UnityEngine;

namespace WireSculptor {
	
	/// <summary>
	/// Allows the user to draw lines on the rotating sculpture.
	/// </summary>
	public class Sculptor : MonoBehaviour {
		
		#region Inspector Fields
		
		[Tooltip("The mouse button used to start drawing")]
		public int drawButton;
		
		[Tooltip("The period at which the mouse position is sampled")]
		public int samplePeriod;
		
		[Tooltip("When the user draws a line, it is added as a child of this object")]
		public GameObject sculpture;
		
		[Tooltip("Used to color the lines drawn by the user")]
		public Material lineMaterial;
		
		[Tooltip("Width of the lines drawn by the user")]
		public float lineWidth;
		
		#endregion
		
		/// The current line
		private GameObject line;
		
		/// Contains the data for the current line
		private LineRenderer lineRenderer;
		
		/// The number of vertices in the current line
		private int lineLength;
		
		/// The rotation of the sculpture when the user initiates a new drawing. Needed to
		/// correctly transform new vertices as they are added to the line
		private Vector3 initialRotation;
		
		/// When the current sample is 0, the mouse position is sampled, otherwise the frame is ignored.
		/// When the current sample equals the sample period, the current sample is reset to 0
		private int currentSample;
		
		void Update() {
			if (!Input.GetMouseButton(drawButton)) {
				return;
			}
			
			// On the first frame that the draw button is pressed, initialize a new line segment
			if (Input.GetMouseButtonDown(drawButton)) {
				currentSample = 0;
				line = new GameObject("Line");
				line.transform.parent = sculpture.transform;
				lineRenderer = line.AddComponent<LineRenderer>();
				// Don't use world space so the line rotates with the sculpture
				lineRenderer.useWorldSpace = false;
				lineRenderer.material = lineMaterial;
				lineRenderer.SetWidth(lineWidth, lineWidth);
				lineLength = 0;
				initialRotation = sculpture.transform.rotation.eulerAngles;
			}
			
			if (currentSample == 0) {
				// Transform screen coordinates of the mouse click to world coordinates
				var camera = Camera.main;
				var mousePos = Input.mousePosition;
				var screenPos = new Vector3(mousePos.x, mousePos.y, -camera.transform.position.z);
				var worldPos = camera.ScreenToWorldPoint(screenPos);
				
				// Rotate the world coordinates to counteract the rotation of the sculpture
				var rotation = Quaternion.Euler(initialRotation - sculpture.transform.rotation.eulerAngles);
				var finalPos = rotation * worldPos;

				Debug.DrawLine(Vector3.zero, worldPos);
				Debug.DrawLine(Vector3.zero, finalPos);

				// Add the new vertex
				var index = lineLength;
				lineLength += 1;
				lineRenderer.SetVertexCount(lineLength);
				lineRenderer.SetPosition(index, finalPos);
			}
			
			currentSample += 1;
			if (currentSample == samplePeriod) {
				currentSample = 0;
			}
		}
		
		public void OnClear() {
			foreach (Transform child in sculpture.transform) {
				if (child.gameObject.name == "Line") {
					GameObject.Destroy(child.gameObject);
				}
			}
		}
	}
	
}
