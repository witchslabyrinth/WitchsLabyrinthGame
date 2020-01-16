using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectableLightHead : MonoBehaviour
{

    [SerializeField]
    private GameObject toGenerateOnReflect;

    GameObject immediateChild = null;

    RaycastHit rcHit;
    Vector3 HitPosit = Vector3.zero;
    Vector3 PrevHitPosit = Vector3.zero;
    bool setPrevHitPosit = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out rcHit, 10))
        {
            //Debug.DrawRay(transform.position, transform.forward*10, Color.yellow);

            HitPosit = rcHit.point;
            if(!setPrevHitPosit)
            {
                PrevHitPosit = HitPosit;
                setPrevHitPosit = true;
            }
            else if (PrevHitPosit != HitPosit)
            {
                immediateChild.GetComponent<ReflectableLightNode>().RemoveThisReflection();
                immediateChild = null;
                PrevHitPosit = HitPosit;
            }
            if (immediateChild == null) {
                immediateChild = Instantiate(toGenerateOnReflect, HitPosit, Quaternion.LookRotation(Vector3.Reflect(transform.forward,rcHit.normal)));//Quaternion.Euler(Vector3.Reflect(transform.forward, rcHit.normal)));
            }
            //Debug.Log("OBJ Detected!");
        }
        else
        {
            if (immediateChild != null)
            {
                PrevHitPosit = Vector3.zero;
                immediateChild.GetComponent<ReflectableLightNode>().RemoveThisReflection();
                immediateChild = null;
                setPrevHitPosit = false;
            }
        }
    }
}
