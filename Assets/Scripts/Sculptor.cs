using UnityEngine;

namespace WireSculptor {

    /// <summary>
    /// Allows the user to draw lines on the rotating sculpture.
    /// </summary>
    public class Sculptor : MonoBehaviour {

        #region Inspector Fields

        [Tooltip("Prefab used when drawing lines")]
        public GameObject linePrefab;

        [Tooltip("The mouse button used to start drawing")]
        public int drawButton;

        [Tooltip("The period at which the mouse position is sampled")]
        public int samplePeriod;

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

            print("Update");

            // On the first frame that the draw button is pressed, initialize a new line segment
            if (Input.GetMouseButtonDown(drawButton)) {
                currentSample = 0;
                lineLength = 0;
                initialRotation = transform.rotation.eulerAngles;

                line = Instantiate(linePrefab);
                line.transform.parent = transform;

                lineRenderer = line.GetComponent<LineRenderer>();

                var synth = line.GetComponent<Synthesizer>();
                synth.initialRotation = initialRotation;
            }

            if (currentSample == 0) {
                // Transform screen coordinates of the mouse click to world coordinates
                var camera = Camera.main;
                var mousePos = Input.mousePosition;
                var screenPos = new Vector3(mousePos.x, mousePos.y, -camera.transform.position.z);
                var worldPos = camera.ScreenToWorldPoint(screenPos);

                // Rotate the world coordinates to counteract the rotation of the sculpture
                var rotation = Quaternion.Euler(initialRotation - transform.rotation.eulerAngles);
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
            foreach (Transform child in transform) {
                if (child.gameObject.name == "Line(Clone)") {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }

}
