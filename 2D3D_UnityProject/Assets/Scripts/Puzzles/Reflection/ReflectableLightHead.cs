using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectableLightHead : MonoBehaviour
{
    //We need these two objects for this script to work as intended, but no other script needs to use them, thus we'll make them private.
    [SerializeField]
    private GameObject toGenerateOnReflect;
    [SerializeField]
    private GameObject myBeam;

    //We need the immediate child to be public for light nodes to work properly, but we absolutely do NOT want anyone setting it before running the game. It's intended function is to keep track of reflections created, which would be completely screwed up by setting this in the editor.
    //For this reason, we make it nonserialized, thus making such a mistake impossible.
    [System.NonSerialized]
    public GameObject immediateChild = null;

    //These are needed in order to calculate reflections and rays
    RaycastHit rcHit;
    Vector3 HitPosit = Vector3.zero;
    Vector3 PrevHitPosit = Vector3.zero;
    bool setPrevHitPosit = false;

    //Reflections are only supposed to be made when light hits a mirror. All objects considered mirrors should be on layer 9.
    const int LAYER_MASK_TO_TEST_ONLY_LAYER_9 = (1 << 9);

    private void FixedUpdate()
    {
        //If myBeam has an assigned object, perform one raycast with no distance limit to determine the beam. Regardless of where it hits, the beam will be shown at all times, so we need to test for all layers in this initial raycast.
        if(myBeam!=null && Physics.Raycast(transform.position, transform.forward, out rcHit, Mathf.Infinity))
        {
            //Set the beam's scale to the distance from the lightsource to the object it's shining on. This is necessary so the light appears to reach the full distance from the source to the object.
            Vector3 newScale = myBeam.transform.localScale;
            newScale.y = (rcHit.distance / 2);
            myBeam.transform.localScale = newScale;

            //Set the beam's position to the midpoint of the ray so it appears to begin from the lightsource and end on the object it's shining on.
            Vector3 newPosition = myBeam.transform.position;
            newPosition.x = transform.position.x + (rcHit.point.x - transform.position.x) / 2;
            newPosition.y = transform.position.y + (rcHit.point.y - transform.position.y) / 2;
            newPosition.z = transform.position.z + (rcHit.point.z - transform.position.z) / 2;
            myBeam.transform.position = newPosition;
        }
        //perform a raycast in front of this object from its origin, only detect collisions with layer 9 (the mirror layer)
        if (Physics.Raycast(transform.position, transform.forward, out rcHit, 10, LAYER_MASK_TO_TEST_ONLY_LAYER_9))
        {
            //Uncomment the line below to debug the raycast.
            //Debug.DrawRay(transform.position, transform.forward*10, Color.yellow);

            //Set the hit position to the position of the raycast collision.
            HitPosit = rcHit.point;
            //if we haven't already set the previous position, automatically set it. It's important for detecting when the mirror moves.
            if(!setPrevHitPosit)
            {
                PrevHitPosit = HitPosit;
                setPrevHitPosit = true;
            }
            //Otherwise, if the positions don't match on this frame, destroy all existing reflections and set the two positions equal
            else if (PrevHitPosit != HitPosit)
            {
                immediateChild.GetComponent<ReflectableLightNode>().RemoveThisReflection();
                immediateChild = null;
                PrevHitPosit = HitPosit;
            }
            //Since we're colliding with a mirror, create a reflection if one does not already exist.
            if (immediateChild == null) {
                immediateChild = Instantiate(toGenerateOnReflect, HitPosit, Quaternion.LookRotation(Vector3.Reflect(transform.forward,rcHit.normal)));//Quaternion.Euler(Vector3.Reflect(transform.forward, rcHit.normal)));
            }
        }
        //If no collision has been detected, then we're no longer hitting a mirror. So destroy the existing reflection.
        else
        {
            if (immediateChild != null)
            {
                PrevHitPosit = Vector3.zero;
                immediateChild.GetComponent<ReflectableLightNode>().RemoveThisReflection();
                immediateChild = null;
                //make a note that we haven't set a previous hit position so we don't run into any issues if we hit another mirror.
                setPrevHitPosit = false;
            }
        }
    }
}
