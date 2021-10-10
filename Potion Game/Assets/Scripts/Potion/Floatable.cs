using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class Floatable : MonoBehaviour, IMoverController
{
    public PhysicsMover Mover;
    
    public Vector3 TranslationAxis = Vector3.right;
    public float TranslationPeriod = 10;
    public float TranslationSpeed = 1;
    public Vector3 RotationAxis = Vector3.up;
    public float RotSpeed = 10;
    public Vector3 OscillationAxis = Vector3.zero;
    public float OscillationPeriod = 10;
    public float OscillationSpeed = 10;
    public bool Floating;
    public float StartStopLinearInterpolant = 0.2f;


    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private float _floatingTime =0;

    private void Start()
    {
        _originalPosition = Mover.Rigidbody.position;
        _originalRotation = Mover.Rigidbody.rotation;

        Mover.MoverController = this;
    }

    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        if (Floating)
        {
            _floatingTime += Time.deltaTime;
            goalPosition = Vector3.Lerp(transform.position,(_originalPosition + (TranslationAxis.normalized * Mathf.Sin(_floatingTime * TranslationSpeed) * TranslationPeriod)), StartStopLinearInterpolant);
            Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(_floatingTime * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(RotationAxis * RotSpeed * _floatingTime) * targetRotForOscillation;
            
        }
        else
        {
            _floatingTime = 0;
            goalPosition = Vector3.Lerp(transform.position,_originalPosition, StartStopLinearInterpolant);
            goalRotation = Quaternion.Lerp(transform.rotation, _originalRotation, StartStopLinearInterpolant);
        }
        
    }

}
