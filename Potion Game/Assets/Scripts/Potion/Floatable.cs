using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

public class Floatable : MonoBehaviour, IMoverController
{
    public PhysicsMover Mover;
    public bool Floating = false;
    public float FloatTime = 15f;
    public Vector3 FloatTarget;
    public float FloatSpeed = 0.02f;
    public Vector3 RotationAxis = Vector3.up;
    public float RotSpeed = 10;
    public Vector3 OscillationAxis = Vector3.zero;
    public float OscillationPeriod = 10;
    public float OscillationSpeed = 10;

    private bool _descending = false;
    private float _floatTimeMax;
    private float _sinWaveTimer = 0;
    private float StartStopLinearInterpolant = 0.2f;
    private float RotationResetThreshold = 2f;
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;

    private void Start()
    {
        _originalPosition = Mover.Rigidbody.position;
        _originalRotation = Mover.Rigidbody.rotation;
        FloatTarget = FloatTarget + _originalPosition;
        _floatTimeMax = FloatTime;
        Mover.MoverController = this;
    }

   

    public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
    {
        if (Floating)
        {
            _descending = false;
            _sinWaveTimer += Time.deltaTime;
            FloatTime -= Time.deltaTime;
            goalPosition = Vector3.MoveTowards(transform.position,FloatTarget, FloatSpeed);
            Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(_sinWaveTimer * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(RotationAxis * RotSpeed * _sinWaveTimer) * targetRotForOscillation;
            if(FloatTime <= 0)
            {
                _descending = true;
                Floating = false;
                _sinWaveTimer = 0;
                FloatTime = _floatTimeMax;
            }
        }
        else if (_descending)
        {
            _sinWaveTimer += Time.deltaTime;
            goalPosition = Vector3.MoveTowards(transform.position, _originalPosition, FloatSpeed);
            Quaternion targetRotForOscillation = Quaternion.Euler(OscillationAxis.normalized * (Mathf.Sin(_sinWaveTimer * OscillationSpeed) * OscillationPeriod)) * _originalRotation;
            goalRotation = Quaternion.Euler(RotationAxis * RotSpeed * _sinWaveTimer) * targetRotForOscillation;
            if (Vector3.Distance(transform.position, _originalPosition) < 0.1f)
            {
                _sinWaveTimer = 0;
                _descending = false;
            }
        }
        else
        {
            goalPosition = Vector3.MoveTowards(transform.position, _originalPosition, FloatSpeed);
            goalRotation = Quaternion.Lerp(transform.rotation, _originalRotation, StartStopLinearInterpolant);
        }
    }

}
