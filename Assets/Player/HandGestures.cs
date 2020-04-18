using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandGestures : MonoBehaviour
{
    public SteamVR_Action_Skeleton skeletonInputLeft;
    public SteamVR_Action_Skeleton skeletonInputRight;
    public SteamVR_Action_Pose poseInput;
    public SteamVR_Action_Single triggerInput;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject attackSpawner;
    public GameObject blastLarge;
    public float minDistanceBetweenHands;

    public float x_spawnerRotationOffset = 45;
    public float y_spawnerRotationOffset;
    public float z_spawnerRotationOffset;

    public float x_spawnerPositionOffset;
    public float y_spawnerPositionOffset = -0.2f;
    public float z_spawnerPositionOffset;

    private GameObject spawner;
    private GameObject powerball;
    private bool spawnerInScene;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var leftPosition = leftHand.transform.position;
        var rightPosition = rightHand.transform.position;
        Vector3 spawnerPosition = Vector3.forward;
        Quaternion direction = Quaternion.identity;
        if (Vector3.Distance(leftPosition, rightPosition) < minDistanceBetweenHands)
        {
            //Debug.Log(poseInput[SteamVR_Input_Sources.RightHand].localRotation);

            //direction = Quaternion.Lerp(
            //    poseInput[SteamVR_Input_Sources.RightHand].localRotation,
            //    poseInput[SteamVR_Input_Sources.LeftHand].localRotation,
            //    0.5f);
            //direction = Quaternion.Lerp(
            //    skeletonInputLeft.localRotation,
            //    skeletonInputRight.localRotation,
            //    0.5f);

            //direction = Quaternion.Lerp(
            //    skeletonInputLeft.boneRotations[SteamVR_Skeleton_JointIndexes.root],
            //    skeletonInputRight.boneRotations[SteamVR_Skeleton_JointIndexes.root],
            //    0.5f) *
            //    Quaternion.Euler(
            //        x_spawnerRotationOffset,
            //        y_spawnerRotationOffset,
            //        z_spawnerRotationOffset
            //    );

            //direction = Quaternion.Lerp(
            //    leftHand.transform.rotation,
            //    rightHand.transform.rotation,
            //    0.5f) *
            //    Quaternion.Euler(
            //        x_spawnerRotationOffset,
            //        y_spawnerRotationOffset,
            //        z_spawnerRotationOffset
            //    );
            spawnerPosition = Vector3.Lerp(leftPosition, rightPosition, 0.5f);

            //Debug.Log(transform.position);
            //Debug.Log(spawnerPosition);

            direction = Quaternion.FromToRotation(transform.position, spawnerPosition);
            direction = Player.instance.hmdTransform.transform.rotation;

            //Debug.Log(direction);
            //var offsetPosition = RotatePointAroundPivot(new Vector3(x_spawnerPositionOffset, y_spawnerPositionOffset, z_spawnerPositionOffset) + spawnerPosition, transform.position, transform.rotation);
            //spawnerPosition = RotatePointAroundPivot(new Vector3(x_spawnerPositionOffset, y_spawnerPositionOffset, z_spawnerPositionOffset)+spawnerPosition, spawnerPosition, direction);
            if (spawner != null)
            {
                spawner.transform.position = spawnerPosition;
                spawner.transform.rotation = direction;
            }
            else
            {
                spawner = Instantiate(attackSpawner, spawnerPosition, direction);
            }
            //spawner.transform.RotateAround(spawner.transform.position, spawner.transform.up, Time.deltaTime);
        }
        else
        {
            if (spawner != null)
            {
                Destroy(spawner);
                spawner = null;
            }
        }

        //Both Hand Blast
        var leftPull = triggerInput[SteamVR_Input_Sources.LeftHand].axis;
        var rightPull = triggerInput[SteamVR_Input_Sources.RightHand].axis;

        if ((leftPull > .3f || rightPull > .3f) && spawner != null) {
            if (powerball == null)
            {
                this.StartPowerBall(spawnerPosition, direction, leftPull + rightPull);
            }
        }
        else if((leftPull < .3f || rightPull < .3f) && powerball != null)
        {            
            powerball.GetComponent<PowerBall>().Fire();
            Destroy(powerball, 60);
            powerball = null;
        }
        
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = angles * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
    private void StartPowerBall(Vector3 spawnerPosition, Quaternion direction, float pull)
    {
        if (powerball == null)
        {
            powerball = Instantiate(blastLarge, spawnerPosition, direction, this.spawner.transform);
            //powerball.transform.localScale = powerball.transform.localScale * pull * 20;
        }        
    }
}
