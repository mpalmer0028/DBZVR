using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
