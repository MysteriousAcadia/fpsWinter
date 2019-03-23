using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMotor)) ]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]private float lookSensitivity = 9f;

    [SerializeField] private float thrusterForce = 1000f;

    [SerializeField] private float thrusterFuelBurnSpeed = 1f;

    [SerializeField] private float thrusterFuelRegenSpeed = 0.3f;

    [SerializeField] private float thrusterFuelAmount = 1f;

    [Header("Spring Settings")]
    //[SerializeField] private JointDriveMode jointMode=JointDriveMode.Position;
    [SerializeField] private float jointSpring=20f;
    [SerializeField] private float jointMaxForce=40f;


    //caching components
    private Animator animator;
    private PlayerMotor motor;
    private ConfigurableJoint joint;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        setJointSettings(jointSpring);
        

    }


    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }



    private void Update()
    {  
        if(PauseMenu.isOn){
            if(Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            }
            motor.Move(Vector3.zero);
            motor.Rotate(Vector3.zero);
            motor.RotateCamera(0f);
            return;
        }
        if(Cursor.lockState != CursorLockMode.Locked){
            Cursor.lockState = CursorLockMode.Locked;
        }
        //player Movement
        float _xmove = Input.GetAxis("Horizontal");
        float _zmove = Input.GetAxis("Vertical");

        Vector3 _moveHorizontal = transform.right * _xmove;
        Vector3 _moveVertical = transform.forward * _zmove;
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * speed;

        //Animate Movement
        animator.SetFloat("Blend", _zmove);

        motor.Move(_velocity);


        //Player Rotation
        float _yRot = Input.GetAxisRaw("Mouse X");
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * 3;
        motor.Rotate(_rotation);

        //Camera Rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");
        float _cameraRotation = _xRot * lookSensitivity;
        motor.RotateCamera(_cameraRotation);

        //Applying thruster force
        Vector3 _thrusterForce = Vector3.zero;
        if(Input.GetButton("Jump") && thrusterFuelAmount>0f){

            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
            _thrusterForce = Vector3.up * thrusterForce;
            setJointSettings(0f);
        }

       
        else{

            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            setJointSettings(jointSpring);
        }
        motor.applyThrusterForce(_thrusterForce);
        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);


    }
    private void setJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
           // mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
