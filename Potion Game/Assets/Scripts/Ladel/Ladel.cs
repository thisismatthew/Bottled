using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Ladel : MonoBehaviour
{
    private Transform _player;
    private Vector3 _startPosition;
    public bool _chaseStart = false;
    public float LiftHeight = 6;
    private bool _chasePlayer = false;
    private bool _returnToCauldron = false;
    private bool _returnToStart;
    public float Speed = 7;
    private Vector3 _target;

    public Renderer _ladle;
    public VisualEffect _sparkle;
    public VisualEffect _dip;
    public VisualEffect _handle;
    private void Start()
    {
        _startPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        EffectsOff();
    }

    public void StartChase()
    {
        _chaseStart = true;
        _target = _startPosition;
        _target.y += LiftHeight;
    }
    private void EffectsOn()
    {
        //_ladle.materials[1].SetFloat("_OnOff", 1);
        _sparkle.enabled = true;
        _sparkle.Play();
        _dip.enabled = true; ;
        _dip.Play();
        _handle.enabled = true;
        _handle.Play();
    }

    private void EffectsOff()
    {
        //ALSO THIS _ladle.materials[1].SetFloat("_OnOff", 0);
        _sparkle.enabled = false;
        _dip.enabled = false;
        _handle.enabled = false;
    }

    void Update()
    {
        if (_chaseStart || _chasePlayer || _returnToCauldron || _returnToStart)
            transform.position = Vector3.MoveTowards(transform.position, _target, Speed * Time.deltaTime);

        if (_chaseStart)
        {

            EffectsOn();
            //Debug.Log("EffectsAdded");
            if (Vector3.Distance(transform.position, _target) < 0.5)
            {
                _chaseStart = false;
                _chasePlayer = true;
            }
        }

        if (_chasePlayer)
        {
            _target = _player.position;
            _target.y += LiftHeight;
            if (Vector3.Distance(transform.position, _target) < 0.5)
            {
                _chasePlayer = false;
                _returnToCauldron = true;
            }
        }

        if (_returnToCauldron)
        {
            _target = _startPosition;
            _target.y += LiftHeight + 2;
            if (Vector3.Distance(transform.position, _target) < 0.5)
            {
                _returnToCauldron = false;
                _returnToStart = true;
            }
        }

        if (_returnToStart)
        {
            _target = _startPosition;
            if (Vector3.Distance(transform.position, _target) < 0.5)
            {
                EffectsOff();
                _returnToStart = false;
            }
        }

        
    }
}
