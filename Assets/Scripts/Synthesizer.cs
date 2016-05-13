using UnityEngine;

namespace WireSculptor
{

    [RequireComponent(typeof(LineRenderer))]
    class Synthesizer : MonoBehaviour {
        
        public Vector3 initialRotation;
        
        private float increment;
        private float phase;
        
        void Start() {
            
        }
        
        void OnAudioFilterRead(float[] data, int channels) {
            print("OnAudioFilterRead");
            // var rotation = transform.parent.rotation;      
            for (int i = 0; i < data.Length; i = i + channels) {
                
            }
        }
    }
    
}