using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class Pickupable : MonoBehaviour
{
    private bool _moving = false;
    private Vector3 _target;
    private Rigidbody _rb;
    private Quaternion _normalRotation;
    public float ArcHeight = 5f;
    public float MoveSpeed = 10f;
    public float PickupAccuracy = 0.2f;
    public bool DisablePickup = false;
    public bool RotateToDefault;
    private float _rotationAccuracy = 2f;



    public Ingredient IngredientName;
    //just let the player know that this object can be picked up.
    //this will need refactoring if there are multiple interactables in range of the player. 
    void Start()
    {
        gameObject.tag = "PickUpable";
        _rb = GetComponent<Rigidbody>();
        _normalRotation = transform.rotation;
    }

    private void Update()
    {
        if (_moving) MoveToTarget(_target);
        if (RotateToDefault)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _normalRotation, 0.1f);
            if (Quaternion.Angle(transform.rotation,_normalRotation)< _rotationAccuracy)
            {
                RotateToDefault = false;
            }
        }
    }
    private void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player" && !DisablePickup && player.GetComponent<MainCharacterController>().Interactable == null) player.GetComponent<MainCharacterController>().Interactable = this.gameObject;
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {
            if (!player.GetComponent<MainCharacterController>().IsHolding)
            {
                player.GetComponent<MainCharacterController>().Interactable = null;
            }
        }
    }

    public void MoveToTarget(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < PickupAccuracy)
        {
            Debug.Log("finished moving pickupable");
            _moving = false;
            return;
        }
        else
        {
            _moving = true;
            _target = target;
        }
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * MoveSpeed);
        Debug.Log("test");
        transform.rotation = Quaternion.Lerp(transform.rotation, _normalRotation, 0.1f);
    }

    public void ThrowToTarget(Vector3 target)
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = CalculateLaunchVelocity(target);
    }

    public Vector3 CalculateLaunchVelocity(Vector3 target)
    {
        float displacementY = target.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * ArcHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * ArcHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - ArcHeight) / Physics.gravity.y));

        return velocityXZ + velocityY;
    }

    

    

}

