using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TerrainDeformerInput : MonoBehaviour {
	private TerrainDeformer deformer;
	private bool deform = false;
	private Vector3 point;

	[SerializeField]
	private Leap.Unity.PinchDetector[] pinchDetectors;

	void Awake(){
		if (pinchDetectors.Length == 0) {
			Debug.LogWarning("No pinch detectors were specified!  PinchDraw can not draw any lines without PinchDetectors.");
		}
	}
		
	void Update() {
		for (int i = 0; i < pinchDetectors.Length; i++) {
			var detector = pinchDetectors [i];
			if (detector.DidStartHold) {
				Debug.Log ("Started pinch");
				HandlePinchInput (detector);
			}
			else if (detector.DidRelease) {
				Debug.Log ("Finished pinch");
				deform = false;
				deformer = null;
				numOfPoints = 0;
				iterator = 0;
			}
			else if (detector.IsHolding) {
				Debug.Log ("Holding pinch");
				PinchMove (detector);
			}
		}

	}

	void HandlePinchInput(Leap.Unity.PinchDetector detector){
		RaycastHit hit;

		if(Physics.Raycast(detector.Position, Vector3.down, out hit)){
			deformer = hit.collider.GetComponent<TerrainCollider>();
		}
		
		if(deformer){
			deform = true;
			point = hit.point;

		}
	}

	void PinchMove(Leap.Unity.PinchDetector detector){
		if (deform) {
			Vector3 pointOfAttack = detector.Position - Vector3.down * 0.1f;
		}
	}
}
