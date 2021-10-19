using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fillable : MonoBehaviour
{
    public Renderer _rend;
    private float rotateAmount = 0;
    public ParticleSystem _spillCup;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 box = transform.GetComponentInParent<BoxCollider>().transform.up;
        Vector3 pointNormal = NormalizePrecise(new Vector3(1.3f*box.x, 1.3f*box.y, 1.3f*box.z));
        _rend.material.SetVector("_planeNormal", pointNormal);
        Vector3 boxCol = transform.GetComponentInParent<BoxCollider>().transform.eulerAngles;
        float currentRotate = 180 - boxCol.y;
        rotateAmount = Mathf.Lerp(rotateAmount, currentRotate, 0.1f);
        _rend.material.SetFloat("_Rotate", rotateAmount);
        _rend.material.SetFloat("_Height", Mathf.Clamp(-0.5f - Mathf.Abs(1.3f * box.x) + Mathf.Abs(1.3f * box.z) / 15, -0.85f, -0.5f));
        //Debug.Log("Rotation is " + (transform.eulerAngles.x-boxCol.x) + " " + (transform.eulerAngles.z - boxCol.z));
        //Debug.Log("NORM is " + box.y);
        if (box.y<0)
        {
            Animator _cupAnim = transform.GetComponent<Animator>();
            _cupAnim.SetBool("CupTipped", true);

        }
        else if(box.y == 1)
        {
            _spillCup.Stop();
        }
    }

    Vector3 NormalizePrecise(Vector3 v)
    {
        float mag = v.magnitude;

        if (mag == 0) return Vector3.zero;

        return (v / mag);

    }

    void CupSpillLiquid()
    {
        _spillCup.Play();
    }
    void CupHideLiquid()
    {
        _rend.enabled = false;
    }
    void CupFillLiquid()
    {
        _rend.enabled = true;
    }
}
