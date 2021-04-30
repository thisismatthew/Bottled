using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    MeshRenderer thePlane;
    Vector3 facing = Vector3.zero;

    //Material LiquidTop;
    [SerializeField] SkinnedMeshRenderer theBottle;
    // Start is called before the first frame update
    void Start()
    {
        thePlane = GetComponent<MeshRenderer>();
        //LiquidTop = thePlane.material;
    }

    // Update is called once per frame
    void Update()
    {
        //Quaternion(-0.707106829, 0, 0, 0.707106829)
        float wobbleX = theBottle.material.GetFloat("_WobbleX");
        float wobbleZ = theBottle.material.GetFloat("_WobbleZ");

        facing = new Vector3(-wobbleX, 0, -wobbleZ);
        transform.position = new Vector3(transform.position.x, transform.position.y - transform.localPosition.y + theBottle.material.GetFloat("_LiquidHeight"), transform.position.z);
        thePlane.transform.LookAt(thePlane.transform.position + Vector3.up - facing);
        theBottle.material.SetVector("_PlanePosition", new Vector4(thePlane.transform.position.x, thePlane.transform.position.y, thePlane.transform.position.z));
        theBottle.material.SetVector("_PlaneNormal", new Vector4(thePlane.transform.forward.x, thePlane.transform.forward.y, thePlane.transform.forward.z));
        Debug.Log(theBottle.material.GetVector("_PlanePosition") + " " + theBottle.material.GetVector("_PlaneNormal"));
    }
}
