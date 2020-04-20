using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlanetScript : MonoBehaviour {

    public float gravity = -12;

    public void Attract(Transform playerTransform, CharacterController cc)
    {
        Vector3 gravityUp = (playerTransform.position - transform.position).normalized;
        Vector3 localUp = playerTransform.up;

        //cc.Move(gravityUp * gravity* Time.deltaTime);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * playerTransform.rotation;
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, 50f * Time.deltaTime);
    }

    ///// <summary>
    ///// Apply a Vector3 to the position of the player relative to the plant
    ///// </summary>
    ///// <param name="playerTransform"></param>
    ///// <param name="movementFromPlayer">distance from player location</param>
    ///// <param name="speed">Speed used for movement</param>
    ///// <returns></returns>
    //public Vector3 GetMotionRelativeToPlanet(Transform playerTransform, Vector3 movementFromPlayer, float speed)
    //{
    //    var direction = Player.instance.hmdTransform.TransformDirection(movementFromPlayer);
    //    var movement = speed * Time.deltaTime * (Vector3.Distance(this.transform.position, this.transform.position) ) *
    //                (Vector3.ProjectOnPlane(direction, this.transform.TransformDirection(transform.position)) +
    //                    (new Vector3(0,movementFromPlayer.y,0) / speed));
    //    return movement;
    //}
}
