using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectableLightNode : MonoBehaviour
{
    //A proper ReflectableLightNode also creates its own reflection, so it is both a light node and a light head.
    //Implementing it this way is better than writing separate code for light heads and light nodes, since the two would be virtually identical, except for one extra function for light nodes.
    ReflectableLightHead lightHead;

    // Start is called before the first frame update
    void Start()
    {
        lightHead = GetComponent<ReflectableLightHead>();
    }

    public void RemoveThisReflection()
    {
        //All nodes should also be heads, but we'll check just to be safe. If it isn't, then we can simply destroy this object, since it doesn't have any reflections.
        if (lightHead == null) {
            Destroy(gameObject);
            return;
        }
        //Given that the block above will not be true under normal circumstances, we check whether this node has any reflections. We need to destroy all child reflections of this one so we don't leave a bunch of floating light beams in the room.
        if (lightHead.immediateChild != null)
        {
            //Since each child is a node that keeps track of its immediate child, this will have the effect of removing all reflections that stem from this one.
            lightHead.immediateChild.GetComponent<ReflectableLightNode>().RemoveThisReflection();
        }
        //Since we've destroyed all child reflections, we can safely destroy this reflection.
        Destroy(gameObject);
    }
}
