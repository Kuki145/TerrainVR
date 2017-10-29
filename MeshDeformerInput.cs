using UnityEngine;

public class MeshDeformerInput : MonoBehaviour {
	private float force = 100f, forceOffset = 0.01f;
	private MeshDeformer deformer;
	private Vector3 point, originalPosition, currentPosition;
	private bool deform = false;

	private Vector3[] points;
	private int numOfPoints = 0;
	private int iterator = 0;

	public float FORCE_MULTIPLIER = 100000000f;

	[SerializeField]
	private Leap.Unity.PinchDetector[] pinchDetectors;

	void Awake() {
		points = new Vector3[64];
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

	void HandlePinchInput (Leap.Unity.PinchDetector detector) {
		Debug.Log ("Hit something");
		RaycastHit hit;

		if (Physics.Raycast (detector.Position, Vector3.down, out hit, .5f)) {
			deformer = hit.collider.GetComponent<MeshDeformer> ();
		} else if (Physics.Raycast (detector.Position, Vector3.down, out hit, .5f)) {
			deformer = hit.collider.GetComponent<MeshDeformer> ();
		}

		if (deformer) {
			deform = true;
			originalPosition = detector.Position;
			point = hit.point;
		}
	}

	void PinchMove (Leap.Unity.PinchDetector detector) {
		if (deform) {
			currentPosition = detector.Position;
			Vector3 pointOfAttack = point;
			float dist = (float)currentPosition.y - (float)originalPosition.y;
			Debug.Log ("Current position = " + currentPosition.y);
			Debug.Log ("Original position = " + originalPosition.y);
			if ( Mathf.Abs(dist) > .001f ) {
				force = FORCE_MULTIPLIER * dist;

				if (force > 0) {
					pointOfAttack += Vector3.down * forceOffset;
					if((++iterator%20)==1)
					points [numOfPoints++] = pointOfAttack;
					Debug.Log ("NumOfPoints = "+numOfPoints.ToString());

				} else if (force < 0) {
					pointOfAttack += Vector3.up * forceOffset;
					force *= -1;
				}



				for (int i = numOfPoints-1; i <= 0; i--) {
					if(iterator%20 == 1)
						deformer.addDeformingForce (points[i], force/=1.1f);
					else
						deformer.addDeformingForce (points[i], force);
				}

			}

			originalPosition = currentPosition;
		}
	}

}
