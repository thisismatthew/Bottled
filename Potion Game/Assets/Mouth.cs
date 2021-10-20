using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    private Renderer _rend;
    private int _blinkFrame = 9;
    private bool _idleBreath = false;
    private bool _mouthScream = false;
    private bool _mouthRun = false;
    private int lastProc = 1;
    // Start is called before the first frame update
    void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    void Start()
    {
        StartCoroutine(BreathLoop());
        StartCoroutine(ScreamLoop());
        StartCoroutine(RunLoop());
    }

    private IEnumerator BreathLoop()
    {
        float speed = 2f;

        while (gameObject.activeSelf)
        {
            int proc = Random.Range(2, 4);
            yield return new WaitUntil(() => _idleBreath == true);
            if (proc == 3 && lastProc !=3)
            {
                for (int j = _blinkFrame - 5 - 1; j >= 0; j--)
                {
                    yield return new WaitForSeconds(speed * Time.deltaTime);
                    SetFrame(j);
                }
            }

            yield return new WaitUntil(() => _idleBreath == false);

            if (proc == 3 && lastProc != 3)
            {
                for (int i = 0; i < _blinkFrame - 5; i++)
                {
                    yield return new WaitForSeconds(speed * Time.deltaTime);
                    SetFrame(i);
                }
            }
            if (lastProc == 3)
            {
                lastProc = 1;
            }
            else
            {
                lastProc = proc;
            }
        }
    }

    private IEnumerator RunLoop()
    {
        float speed = 2f;

        while (gameObject.activeSelf)
        {
            yield return new WaitUntil(() => _mouthRun == true);

            for (int i = 0; i < _blinkFrame - 2; i++)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetFrame(i);
            }
            yield return new WaitUntil(() => _mouthRun == false);
            for (int j = _blinkFrame - 2 - 1; j >= 0; j--)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetFrame(j);
            }
        }
    }

    private IEnumerator ScreamLoop()
    {
        float speed = 2f;

        while (gameObject.activeSelf)
        {
            yield return new WaitUntil(() => _mouthScream == true);
            for (int i = 2; i < _blinkFrame; i++)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                SetFrame(i);
            }

            yield return new WaitForSeconds(10f);
        }
    }

    private void SetFrame(int index)
    {
        Vector2 offset = new Vector2(index * (1f / _blinkFrame), 1);
        _rend.material.SetVector("_MouthPosition", offset);
    }

    public void Breath()
    {
        _idleBreath = !_idleBreath;
    }

    public void Scream()
    {
        _mouthScream = !_mouthScream;
    }

    public void RunBreath()
    {
        _mouthRun = !_mouthRun;
    }
}
