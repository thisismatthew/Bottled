using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private Renderer _rend;
    private int _blinkFrame = 6;
    float xx = 0;
    float yy = 0;
    private float _posModX = 0;
    private float _posModY = 0;
    // Start is called before the first frame update
    void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    void Start()
    {
        StartCoroutine(BlinkLoop());
        StartCoroutine(MoveLoop());
    }

    void Update()
    {
        float xpos = _rend.material.GetVector("_EyePosition").x;
        if (xpos > -0.02f && xpos<0.02f)
        {
            ShiftFrame();
        }
    }


    private IEnumerator BlinkLoop()
    {
        float speed = 0.003f;

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));

            for (int i = 0; i < _blinkFrame; i++)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetFrame(i);
            }

            yield return new WaitForSeconds(0.25f);
            
            for (int j=_blinkFrame-1;j>=0;j--)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetFrame(j);
            }
        }

    }

    private IEnumerator MoveLoop()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(5, 6));
            _posModX = Random.Range(-0.005f, 0.005f);
            _posModY = Random.Range(-0.01f, 0.015f);

            yield return new WaitForSeconds(2.5f);
            _posModX = 0;
            _posModY = 0;
        }

    }



    private void SetFrame (int index)
    {
        Vector2 offset = new Vector2(index * (1f / _blinkFrame)+ _posModX, 1+ _posModY);
        _rend.material.SetVector("_EyePosition", offset);
    }

    private void ShiftFrame()
    {
        Vector2 offset = new Vector2((_posModX), 1 + _posModY);
        _rend.material.SetVector("_EyePosition", offset);
    }
}
