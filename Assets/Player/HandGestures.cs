using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public struct HandTransform
{
    public Vector3 leftPosition;
    public Quaternion leftRotation;
    public Vector3 rightPosition;
    public Quaternion rightRotation;
}
public class HandGestures : MonoBehaviour
{
    public SteamVR_Action_Skeleton skeletonInputLeft;
    public SteamVR_Action_Skeleton skeletonInputRight;
    public SteamVR_Action_Pose poseInput;
    public SteamVR_Action_Single triggerInput;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject testCube;
    public GameObject attackSpawner;
    public GameObject blastLarge;
    public GameObject disk;

    /// <summary>
    /// How many samples to compare for punch
    /// </summary>
    public int PuncheResolution = 20;

    /// <summary>
    /// How many updates to skip before sampling a position again
    /// </summary>
    public int PuncheSampleLength = 30;

    /// <summary>
    /// How fast a punch has to go to be registered
    /// </summary>
    public float PuncheThreshold = .5f;

    /// <summary>
    /// How close to the current looking direction the punch must be
    /// </summary>
    public float AcceptablePunchAngle = 45;

    public float minDistanceBetweenHands;


    public float x_spawnerRotationOffset = 45;
    public float y_spawnerRotationOffset;
    public float z_spawnerRotationOffset;

    public float x_spawnerPositionOffset;
    public float y_spawnerPositionOffset = -0.2f;
    public float z_spawnerPositionOffset;
    public GameObject BodyCollider;

    private GameObject spawner;
    private GameObject powerball;    
    private bool spawnerInScene;

    private int punchSampleI = 0;
    private float punchMagnitudeL = 0;
    private float punchMagnitudeR = 0;

    private LinkedList<HandTransform> recentPositions = new LinkedList<HandTransform>();

    private PunchSpawner punchSpawnerL;
    private PunchSpawner punchSpawnerR;

    private GameObject diskL;
    private GameObject diskR;


    // Start is called before the first frame update
    void Start()
    {
        punchSpawnerL = leftHand.GetComponent<PunchSpawner>();
        punchSpawnerR = rightHand.GetComponent<PunchSpawner>();
        //if(ps == null || vr_movement == null)
        //{
        //    //must have script refs
        //    throw new System.Exception();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //var playerPos = Player.instance.hmdTransform.transform.position;
        var playerPos = transform.position;
        var leftPosition = leftHand.transform.position;
        var rightPosition = rightHand.transform.position;
        var leftRotation = leftHand.transform.rotation;
        var rightRotation = rightHand.transform.rotation;
        Vector3 spawnerPosition = Vector3.forward;
        Quaternion direction = Quaternion.identity;
        var leftPull = triggerInput[SteamVR_Input_Sources.LeftHand].axis;
        var rightPull = triggerInput[SteamVR_Input_Sources.RightHand].axis;

        #region One Hand Attack
        if (Vector3.Distance(leftPosition, rightPosition) >= minDistanceBetweenHands)
        {
            if (leftPull > .3f)
            {
                if (diskL == null)
                {
                    diskL = Instantiate(disk, leftPosition, Quaternion.identity, leftHand.transform);
                }
            }
            else
            {
                if (diskL != null)
                {

                    diskL.GetComponent<DiskScript>().Fire();
                    diskL = null;

                }
            }
        }
        else
        {

        }
        #endregion

        #region Two Hand Attacks
        if (Vector3.Distance(leftPosition, rightPosition) < minDistanceBetweenHands)
        {
            //Debug.Log(poseInput[SteamVR_Input_Sources.RightHand].localRotation);

            spawnerPosition = Vector3.Lerp(leftPosition, rightPosition, 0.5f);

            
            var offset = (Quaternion.FromToRotation(spawnerPosition, Player.instance.hmdTransform.transform.position) * Quaternion.Euler(x_spawnerRotationOffset,
                    y_spawnerRotationOffset,
                    z_spawnerRotationOffset)) *
                new Vector3(x_spawnerPositionOffset, y_spawnerPositionOffset, z_spawnerPositionOffset);

            spawnerPosition += offset;

            
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
        

        if ((leftPull > .3f || rightPull > .3f) && spawner != null) {
            if (powerball == null)
            {
                this.StartPowerBall(direction);
            }
        }
        else if((leftPull < .3f || rightPull < .3f) && powerball != null)
        {            
            powerball.GetComponent<PowerBall>().Fire();
            powerball = null;
        }
        #endregion

        #region Punching
        
        if(punchSampleI < PuncheSampleLength)
        {
            punchSampleI++;            
        }
        else
        {
            recentPositions.AddFirst(new HandTransform {
                leftPosition = leftPosition - playerPos, rightPosition = rightPosition - playerPos,
                leftRotation = leftRotation, rightRotation = rightRotation
            });
            if (recentPositions.Count > this.PuncheResolution)
            {
                recentPositions.RemoveLast();
            }
            
            var posNode = recentPositions.First;
            punchMagnitudeL = 0;
            punchMagnitudeR = 0;
            var punchDirectionL = new List<Quaternion>();
            var punchDirectionR = new List<Quaternion>();
            var handMovingAwayFromPlayerL = false;
            var handMovingAwayFromPlayerR = false;
            while (posNode != null)
            {
                if(posNode.Previous != null)
                {
                    punchMagnitudeL += Vector3.Distance(posNode.Previous.Value.leftPosition, posNode.Value.leftPosition);
                    punchMagnitudeR += Vector3.Distance(posNode.Previous.Value.rightPosition, posNode.Value.rightPosition);
                    //punchDirectionL.Add(Quaternion.FromToRotation(posNode.Previous.Value.leftPosition, posNode.Value.leftPosition));
                    //punchDirectionR.Add(Quaternion.FromToRotation(posNode.Previous.Value.rightPosition, posNode.Value.rightPosition));
                    handMovingAwayFromPlayerL = Vector3.Distance(posNode.Previous.Value.leftPosition, playerPos) < Vector3.Distance(posNode.Value.leftPosition, playerPos);
                    handMovingAwayFromPlayerR = Vector3.Distance(posNode.Previous.Value.rightPosition, playerPos) < Vector3.Distance(posNode.Value.rightPosition, playerPos);
                }                
                posNode = posNode.Next;
            }
            
            //var punchAvgL = CalcAvg(punchDirectionL);
            //var punchAvgR = CalcAvg(punchDirectionR);
            //var angleL = Quaternion.Angle(Player.instance.hmdTransform.transform.rotation, punchAvgL);
            //var angleR = Quaternion.Angle(Player.instance.hmdTransform.transform.rotation, punchAvgR);
            if ((punchMagnitudeL > PuncheThreshold && handMovingAwayFromPlayerL) || 
                (punchMagnitudeR > PuncheThreshold && handMovingAwayFromPlayerR))
            {
                //Instantiate(testCube, recentPositions.Last.Value.leftPosition, recentPositions.Last.Value.leftRotation);
                recentPositions.Clear();
                Debug.Log("Punch L:" + punchMagnitudeL + " R:" + punchMagnitudeR);
                if(punchMagnitudeL > PuncheThreshold && handMovingAwayFromPlayerL)
                {
                    punchSpawnerL.Punch();
                }

                if (punchMagnitudeR > PuncheThreshold && handMovingAwayFromPlayerR)
                {
                    punchSpawnerR.Punch();
                }
                //Debug.Log("PunchRo L:" + angleL + " R:" + angleR);
            }

            punchSampleI = 0;
        }

        #endregion

    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = angles * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    private void StartPowerBall(Quaternion direction)
    {
        if (powerball == null)
        {
            var powerBallSpawnerTransform = this.spawner.transform.Find("PowerBallSpawner");
            powerball = Instantiate(blastLarge, powerBallSpawnerTransform.position, direction, powerBallSpawnerTransform);
            //powerball.transform.localScale = powerball.transform.localScale * pull * 20;
        }        
    }
    private Quaternion CalcAvg(List<Quaternion> rotationlist)
    {
        if (rotationlist.Count == 0)
            throw new ArgumentException();

        var final = rotationlist[0];
        var skipFirst = true;
        foreach (var q in rotationlist)
        {
            if (skipFirst)
            {
                skipFirst = false;
            }
            else
            {
                final = Quaternion.Lerp(final, q, .5f).normalized;
            }
        }
        
        return final;
    }
}
