using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float _speed = 5;
    [SerializeField] float _rotationSpeed = 125;
    [SerializeField] float jumpForce = 7;

    float distToGround;
    bool grounded;
    private Rigidbody characterRB;
    [SerializeField]  Animator anim;
    

    private void Start()
    {
        characterRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * _speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.back * Time.deltaTime * _speed);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed);
        }



        if (Input.GetKey(KeyCode.V))
        {
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.N))
        {
            transform.Rotate(0, -_rotationSpeed * Time.deltaTime, 0);
        }


        if (Input.GetKey(KeyCode.Z))
        {
            transform.localScale = Vector3.one * 0.5f;
        }

        if (Input.GetKey(KeyCode.C))
        {
            transform.localScale = Vector3.one;
        }

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.Play("Jump", 0);
            characterRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.Play("Dance", 0);
        }


    }

    private void FixedUpdate()
    {
        grounded = (Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f));
    }
}
