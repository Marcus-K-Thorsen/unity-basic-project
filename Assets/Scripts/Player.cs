using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask playerMask;
    
    private bool _jumpKeyWasPressed;
    private float _horizontalInput;
    private Rigidbody _rigidbodyComponent;
    private int _superJumpRemaining;
    

    // Start is called before the first frame update
    void Start()
    {
        _rigidbodyComponent = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if space key is pressed down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpKeyWasPressed = true;
        }

        _horizontalInput = Input.GetAxis("Horizontal") * 2;
    }

    // FixedUpdate is called once every physic update
    private void FixedUpdate()
    {
        _rigidbodyComponent.velocity = new Vector3(_horizontalInput, _rigidbodyComponent.velocity.y, 0);
        
        int amountOfOverlappingSpheres = Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length;
        // If there are no overlapping then the GroundCheckTransform, that is within the Player, must not be touching anything, and we must be in the air
        if (amountOfOverlappingSpheres == 0)
        {
            return;
        }
        
        if (_jumpKeyWasPressed)
        {
            float jumpPower = 6f;
            if (_superJumpRemaining > 0)
            {
                jumpPower *= 1.5f;
                _superJumpRemaining--;
            }
            _rigidbodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            _jumpKeyWasPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
            _superJumpRemaining++;
        }
    }
}
