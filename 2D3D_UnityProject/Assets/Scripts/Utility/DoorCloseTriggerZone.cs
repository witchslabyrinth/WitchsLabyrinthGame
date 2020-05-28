using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloseTriggerZone : MonoBehaviour
{
    [SerializeField]
    private DoubleSlidingDoors toClose;
    [SerializeField]
    private GameObject HouseExitInvisibleWall;

    /// <summary>
    /// Enables actor swapping/commands when player passes through
    /// </summary>
    [SerializeField]
    private bool enableSwapping;

    private Actor player => PlayerController.Instance.GetPlayer();
    private Actor friend => PlayerController.Instance.GetFriend();

    //This aids in determining where the uncontrolled actor will be teleported once the player enters the house.
    private float UnitsBeside = -1;

    private void OnTriggerEnter(Collider other)
    {
        // Make sure friend actor follows player through doors
        if (other.gameObject == player.gameObject)
            MoveBesideOtherActor(friend, player);
        else
            return;

        // Close doors behind player
        toClose.Close();

        // Allow player to control friend actor
        if(enableSwapping) {
            PlayerController.Instance.canSwap = true;
            PlayerController.Instance.canCommand = true;
        }
        
        //Activate a trigger zone that will detect if the player exits the house while the doors are closing. If they do, the doors will open again. This will prevent the player from getting stuck outside.
        HouseExitInvisibleWall.SetActive(true);
        //Deactivate this trigger zone so we don't try to close the already closed doors if the player walks here again.
        gameObject.SetActive(false);
    }

    //This function is used to teleport one actor beside the other. This will ensure the other doesn't get stuck outside.
    private void MoveBesideOtherActor(Actor actorToMove, Actor actorToMoveTo)
    {
        //Oliver and the cat are not the same height, meaning we need to adjust the new y position to teleport properly.
        //If we don't, Oliver would fall through the floor upon being teleported.
        float heightDifference = actorToMoveTo.GetComponent<CapsuleCollider>().height - actorToMove.GetComponent<CapsuleCollider>().height;

        //Now we teleport them to the new location.
        actorToMove.transform.position = new Vector3(actorToMoveTo.transform.position.x, actorToMoveTo.transform.position.y - heightDifference, actorToMoveTo.transform.position.z + UnitsBeside);
    }

}
