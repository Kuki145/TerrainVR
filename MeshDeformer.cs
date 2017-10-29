using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour {
	Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices, vertexVelocities;

	void Start() {
		Generate ();

	}
	void Generate(){
		deformingMesh = GetComponent<MeshFilter> ().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		vertexVelocities = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices [i] = originalVertices [i];
		}
	}

	public void addDeformingForce(Vector3 point, float force){
		Debug.DrawLine (Camera.main.transform.position, point);
		point = transform.InverseTransformPoint (point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex (i, point, force);
		}
	}
	void AddForceToVertex( int i, Vector3 point, float force){
		Vector3 pointToVertex = displacedVertices [i] - point;
		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities [i] += pointToVertex.normalized * velocity;
	}

	void Update(){
		if (deformingMesh != GetComponent<MeshFilter> ().mesh)
			Generate ();
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex (i);	
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals ();
	}

	void UpdateVertex( int i){
		Vector3 velocity = vertexVelocities [i];
		displacedVertices [i] += velocity * Time.deltaTime;
		vertexVelocities [i] = Vector3.zero;
	}
}
