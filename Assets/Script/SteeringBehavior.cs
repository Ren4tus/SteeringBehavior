using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehavior : MonoBehaviour
{
    private static SteeringBehavior instance = null;
    private Camera mainCamera; // 메인 카메라
    [HideInInspector]
    public Vector3 targetPos; // 캐릭터의 이동 타겟 위치

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
            targetPos = new Vector3(0, 0, 0);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static SteeringBehavior Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Update()
    {
        // 마우스 입력을 받았 을 때
        if (Input.GetMouseButtonUp(0))
        {
            // 마우스로 찍은 위치의 좌표 값을 가져온다
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                targetPos = hit.point;
            }
        }
    }

    public Vector3 Seek(Vector3 TargetPos, Vehicle vehicle)
    {
        Vector3 DesiredVelocity = Vector3.Normalize(TargetPos - vehicle.transform.position)
                          * vehicle.MaxSpeed;


        return (DesiredVelocity - vehicle.rb.velocity);
    }

    public Vector3 Flee(Vector3 TargetPos, Vehicle vehicle)
    {
        // only flee if the target is within 'panic distance'.Work in distance
        //squared space.
        const double PanicDistanceSq = 8.0f;
        if (Vector3.Magnitude(vehicle.transform.position - targetPos) > PanicDistanceSq)
        {
            return new Vector3(0, 0, 0);
        }


        Vector3 DesiredVelocity = Vector3.Normalize(vehicle.transform.position - TargetPos)
                                  * vehicle.MaxSpeed;

        return (DesiredVelocity - vehicle.rb.velocity);
    }
    public Vector3 Arrive(Vector3 TargetPos, Vehicle vehicle)
    {
        Vector3 ToTarget = TargetPos - vehicle.transform.position;
        float dist = ToTarget.magnitude;

        if (dist > 0.0001f)
        {
            const float DecelerationTweaker = 0.6f;

            float speed = dist / DecelerationTweaker;

            speed = Mathf.Min(speed, vehicle.MaxSpeed);

            Vector3 DesiredVelocity = ToTarget * speed / dist;

            return (DesiredVelocity - vehicle.rb.velocity);
        }
        return Vector3.zero;
    }
    public Vector3 Pursuit(Vehicle evader, Vehicle vehicle)
    {
        //return Vector3.zero;
        Vector3 ToEvader = evader.transform.position - vehicle.transform.position;
        float RelativeHeading = Vector3.Dot(vehicle.rb.velocity, evader.rb.velocity);

        
        if ((Vector3.Dot(ToEvader, vehicle.rb.velocity) > 0) &&
       (RelativeHeading < -0.95))  //acos(0.95)=18 degs
        {
            return Seek(evader.transform.position, vehicle);
        }

        float LookAheadTime = ToEvader.magnitude /
                        (vehicle.MaxSpeed + evader.rb.velocity.magnitude);
        return Seek(evader.transform.position + evader.rb.velocity * LookAheadTime, vehicle);
    }
    public Vector3 Evade(Vehicle pursuer, Vehicle vehicle)
    {
        //return Vector3.zero;
        Vector3 ToPursuer = pursuer.transform.position - vehicle.transform.position;

        const double ThreatRange = 100.0;
        if (ToPursuer.magnitude > ThreatRange) return Vector3.zero ;

        float LookAheadTime = ToPursuer.magnitude /
                        (vehicle.MaxSpeed + pursuer.rb.velocity.magnitude);
        return Flee(pursuer.transform.position + pursuer.rb.velocity * LookAheadTime, vehicle);
    }
    public Vector3 Wander(Vehicle vehicle)
    {
        float JitterThisTimeSlice = vehicle.WanderJitter * Time.deltaTime;

        vehicle.WanderTarget += new Vector3(Random.Range(-1.0f,1.0f) * JitterThisTimeSlice,0,
                                    Random.Range(-1.0f, 1.0f) * JitterThisTimeSlice);

        //reproject this new vector back on to a unit circle
        vehicle.WanderTarget.Normalize();

        //increase the length of the vector to the same as the radius
        //of the wander circle
        vehicle.WanderTarget *= vehicle.WanderRadius;

        //move the target into a position WanderDist in front of the agent
        Vector3 target = vehicle.WanderTarget * 3.0f;

        Debug.DrawLine(vehicle.transform.position, vehicle.transform.position + target, Color.red);
        //and steer towards it
        return target;
    }
    public Vector3 OffsetPursuit(Vehicle leader, Vector3 offset, Vehicle vehicle)
    {

        ////calculate the offset's position in world space
        //Vector2D WorldOffsetPos = PointToWorldSpace(offset,
        //                                                leader->Heading(),
        //                                                leader->Side(),
        //                                                leader->Pos());

        GameObject WorldOffsetPos = new GameObject();
        WorldOffsetPos.transform.position = leader.transform.position;
        WorldOffsetPos.transform.rotation = leader.transform.rotation;
        WorldOffsetPos.transform.Translate(offset);

        Vector3 ToOffset = WorldOffsetPos.transform.position - vehicle.transform.position;
        //Vector3 newPoint = leader.transform.localPosition + offset;


        //Vector3 ToOffset = leader.transform.position + offset;

        //the lookahead time is propotional to the distance between the leader
        //and the pursuer; and is inversely proportional to the sum of both
        //agent's velocities
        float LookAheadTime = ToOffset.magnitude /
                              (vehicle.MaxSpeed + leader.rb.velocity.magnitude);
        //now Arrive at the predicted future position of the offset
        Vector3 finalVec = Arrive(WorldOffsetPos.transform.position + leader.rb.velocity * LookAheadTime, vehicle);
        Destroy(WorldOffsetPos);
        return finalVec;
    }
    public Vector3 Interpose(Vehicle AgentA, Vehicle AgentB, Vehicle vehicle)
    {
        //first we need to figure out where the two agents are going to be at 
        //time T in the future. This is approximated by determining the time
        //taken to reach the mid way point at the current time at at max speed.
        Vector3 MidPoint = (AgentA.transform.position + AgentB.transform.position) / 2.0f;

        float TimeToReachMidPoint = Vector3.Distance(vehicle.transform.position, MidPoint) /
                                     vehicle.MaxSpeed;

        //now we have T, we assume that agent A and agent B will continue on a
        //straight trajectory and extrapolate to get their future positions
        Vector3 APos = AgentA.transform.position + AgentA.rb.velocity * TimeToReachMidPoint;
        Vector3 BPos = AgentB.transform.position + AgentB.rb.velocity * TimeToReachMidPoint;

        //calculate the mid point of these predicted positions
        MidPoint = (APos + BPos) / 2.0f;

        //then steer to Arrive at it
        return Arrive(MidPoint, vehicle);
    }
    public Vector3 Hide(Vehicle hunter, GameObject[] obstacles,Vehicle vehicle)
    {
        float DistToClosest = 999999.99f;
        Vector3 BestHidingSpot = Vector3.zero;

        GameObject closest;

        foreach(GameObject gameObject in obstacles)
        {
            Vector3 HidingSpot = GetHidingPosition(gameObject.transform.position,
                                                     1.5f,
                                                      hunter.transform.position);
            float dist = Vector3.Distance(HidingSpot, vehicle.transform.position);
            if (dist < DistToClosest)
            {
                DistToClosest = dist;

                BestHidingSpot = HidingSpot;

                closest = gameObject;
            }
        }

        //if no suitable obstacles found then Evade the hunter
        if (DistToClosest == 999999.99f)
        {
            return Evade(hunter,vehicle);
        }

        //else use Arrive on the hiding spot
        return Arrive(BestHidingSpot, vehicle);
    }
    Vector3 GetHidingPosition(Vector3 posOb, float radiusOb, Vector3 posHunter)
    {
        //calculate how far away the agent is to be from the chosen obstacle's
        //bounding radius
        const float DistanceFromBoundary = 1.0f;
        float DistAway = radiusOb + DistanceFromBoundary;

        //calculate the heading toward the object from the hunter
        Vector3 ToOb = Vector3.Normalize(posOb - posHunter);

        //scale it to size and add to the obstacles position to get
        //the hiding spot.
        return (ToOb * DistAway) + posOb;
    }
}
