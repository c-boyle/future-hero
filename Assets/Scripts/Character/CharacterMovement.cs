using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform; 
    private float _movementSpeed;
    // private float _rotationSpeed; 


    // Start is called before the first frame update
    void Start()
    {   
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        // _rotationSpeed = 1f;
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

        Debug.Log(movement);

        // Check y direction in movement vector to know if we will...
        if (movement.x > 0) // ...move forward
        {
            tmpVelocity = Vector3.forward;
        }
        else if (movement.x < 0) // ...move backward
        {
            tmpVelocity = Vector3.back;
        }
        else // ...stop moving
        {
            tmpVelocity.x = 0;
        } 


        // Check x direction in movement vector to know if we will...
        if (movement.y < 0) // ...move forward
        {
            tmpVelocity += Vector3.right;
        }
        else if (movement.y > 0) // ...move backward
        {
            tmpVelocity += Vector3.left;
        }
        else // ...stop moving
        {
            tmpVelocity.y = 0;
        } 

        _rigidbody.velocity = tmpVelocity * _movementSpeed;
        //  _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        // _rigidbody.rotation += _transform.rotation;
    }
}
