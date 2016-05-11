using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace WireSculptor
{
    struct Sample {
        Vector3 position;
        float time;
    }
    
    struct Line {
        List<Sample> samples;
    }
    
    class Synthesizer : MonoBehaviour {
        
        private List<Line> lines;
        
        private int currentLine;
        
        public void BeginLine() {
            
        }
        
        public void AddSample(Vector3 position) {
            
        }
        
        public void EndLine() {
            
        }
    }
    
}