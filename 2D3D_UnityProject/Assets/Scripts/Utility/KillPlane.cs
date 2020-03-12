using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Actor player = PlayerController.Instance.GetPlayer();
        Actor friend = PlayerController.Instance.GetFriend();

        // If the player falls out of bounds, respawn them at their starting position
        if(other.gameObject == player.gameObject) {
            player.transform.position = player.spawnPosition;
        }
        // If the friend falls out of bounds, respawn them at the player's position
        else if(other.gameObject == friend.gameObject) {
            friend.transform.position = player.transform.position;
        }
    }
}
