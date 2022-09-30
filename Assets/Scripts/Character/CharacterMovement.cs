using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _bodyTransform, _cameraTransform; 
    private float _movementSpeed;
    private float _rotationSpeed;


    // Start is called before the first frame update
    void Start()
    {   
        // _rigidbody = GetComponent<Rigidbody>();
        // _bodyTransform = GetComponent<Transform>();
        _rotationSpeed = 3f;
        _movementSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Function that moves character
    public void Move(Vector2 movement)
    {   
        // Temporary Velocity variable that we'll set player velocity to later (for now until comfortable with impl.)
        Vector3 tmpVelocity = new Vector3(0,0,0);

        // Debug.Log(movement);

        // Check y direction in movement vector to know if we will...
        if (movement.x > 0) // ...move forward
        {
            tmpVelocity = _bodyTransform.forward;
        }
        else if (movement.x < 0) // ...move backward
        {
            tmpVelocity = _bodyTransform.forward * (-1);
        }
        else // ...stop moving
        {
            tmpVelocity.x = 0;
        } 


        // Check x direction in movement vector to know if we will...
        if (movement.y < 0) // ...move forward
        {
            tmpVelocity += _bodyTransform.right;
        }
        else if (movement.y > 0) // ...move backward
        {
            tmpVelocity += _bodyTransform.right * (-1);
        }
        else // ...stop moving
        {
            tmpVelocity.y = 0;
        } 

        _rigidbody.velocity = tmpVelocity * _movementSpeed;
        //  _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        // _rigidbody.rotation += _bodyTransform.rotation;
    }


    // Function that changes where the character is looking
    public void Look(Vector2 rotation)
    {
        //Debug.Log(rotation);
    
        Vector3 cameraRotation = _cameraTransform.rotation.eulerAngles;
        Vector3 playerRotation = _bodyTransform.rotation.eulerAngles;
    
        cameraRotation.x -= rotation.y * _rotationSpeed;
        playerRotation.y += rotation.x * _rotationSpeed;
        // playerRotation.x = 0;

        _cameraTransform.rotation = Quaternion.Euler(cameraRotation);
        _bodyTransform.rotation = Quaternion.Euler(playerRotation);
    }
}
