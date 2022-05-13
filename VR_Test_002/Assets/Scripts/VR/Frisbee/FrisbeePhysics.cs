using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeePhysics : MonoBehaviour
{
    [SerializeField] private float RHO = 1.23f;
    [SerializeField] private float frisbeeArea = 0.0568f;
    [SerializeField] private float CL0 = 0.1f;
    [SerializeField] private float CLA = 1.4f;
    [SerializeField] private float CD0 = 0.08f;
    [SerializeField] private float CDA = 2.72f;
    [SerializeField] private float alpha0 = -4f;
    [SerializeField] private float factorY = 1f;
    [SerializeField] private float factorX = 1f;
    
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    private void FixedUpdate()
    {
        float flatVelocity = (new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z)).magnitude;
        Vector3 flatFrisbeeDir = _rigidbody.velocity;
        flatFrisbeeDir.y = 0f;
        flatFrisbeeDir.Normalize();
        
        // Angle of attack
        float alpha = Vector3.Angle(_rigidbody.velocity, transform.up) - 90f;
        float c1 = CL0 + CLA * alpha * Mathf.PI / 180f;

        float cd = CD0 + CDA * Mathf.Pow((alpha - alpha0) * Mathf.PI / 180f, 2f);

        float deltaVY = (RHO * Mathf.Pow(flatVelocity, 2f) * frisbeeArea * c1) * Time.deltaTime * factorY;

        float deltaVX = (-RHO * Mathf.Pow(flatVelocity, 2f) * frisbeeArea * cd) * Time.deltaTime * factorX;
        
        _rigidbody.AddForce(0f, deltaVY, 0f);
        
        _rigidbody.AddForce(flatFrisbeeDir * deltaVX);
        
        

    }
}
