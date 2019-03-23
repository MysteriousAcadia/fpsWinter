using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {
    [SerializeField] private Camera cam;
    private Vector3 velocity;
    private Rigidbody rb;
    private Vector3 rotation;
    private float currentCameraLocationX = 0f;
    private float cameraRotationX;
    private Vector3 thrusterForce;
    [SerializeField] private float cameraRotationLimit = 85f;

	// Use this for initialization
	public void Start () {
        rb = GetComponent<Rigidbody>();
		
	}

    public void Move(Vector3 _velocity){
        velocity = _velocity;

    }
    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX){
        cameraRotationX = _cameraRotationX;

    }

    public void applyThrusterForce(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }
    // Update is called once per frame
    void FixedUpdate () {
        PerformMovement();
        PerformRotation();
		
	}
    public void PerformMovement(){
        if (velocity != Vector3.zero){
            rb.MovePosition(rb.position+velocity*Time.fixedDeltaTime);
        }

        if(thrusterForce!=Vector3.zero){
            rb.AddForce(thrusterForce*Time.fixedDeltaTime,ForceMode.Acceleration);
        }

    }
    public void PerformRotation(){
        rb.MoveRotation(rb.rotation*Quaternion.Euler(rotation));

        if(cam!= null){
            //set rotation and clamp it
            currentCameraLocationX -= cameraRotationX;
            currentCameraLocationX = Mathf.Clamp(currentCameraLocationX, -cameraRotationLimit, cameraRotationLimit);
            //Applying rotation to camera
            cam.transform.localEulerAngles = new Vector3(currentCameraLocationX, 0, 0);
        }
    }

   

}
