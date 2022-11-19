using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringLoader : MonoBehaviour
{
    public BaseGameEntity Owner;
    public Vector3 SteeringPower = Vector3.zero;
    
    public SteeringMode Mode = SteeringMode.Invalid;

    public Vector3 Seek(Vector3 TargetPos)
    {
        Vector3 DesiredVelocity = Vector3.Normalize(TargetPos - Owner.transform.position)
                          * Owner.MaxSpeed;


        return (DesiredVelocity - Owner.rb.velocity);
    }
    public Vector3 Wander()
    {
        float JitterThisTimeSlice = Owner.WanderJitter * Time.deltaTime;

        Owner.WanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * JitterThisTimeSlice, Random.Range(-1.0f, 1.0f) * JitterThisTimeSlice,
                                    0);

        Owner.WanderTarget.Normalize();

        Owner.WanderTarget *= Owner.WanderRadius;

        Vector3 target = Owner.WanderTarget * 3.0f;

        return target;
    }
    public Vector3 WallAvoidance()
    {
        Ray ray = new Ray(Owner.transform.position, Owner.transform.position+Owner.LastHeaded);
        RaycastHit hit;
        int layerMask = (1 << LayerMask.NameToLayer("Wall"));
        float RayLength = 1.0f;
        if (Physics.Raycast(ray, out hit, RayLength, layerMask))
        {
            Debug.DrawLine(Owner.transform.position, hit.point,Color.cyan);
            Debug.DrawRay(Owner.transform.position, /*Owner.transform.position+ */Owner.LastHeaded, Color.green);
            float PenetrationLength = RayLength - Vector3.Distance(Owner.transform.position, hit.point);
            Vector3 RepulsiveForce = -1 * Vector3.Normalize(hit.transform.position - Vector3.zero) * PenetrationLength;
            Debug.DrawLine(Owner.transform.position, Owner.transform.position+ RepulsiveForce * 3,Color.red);
            return RepulsiveForce * 3;

        }
        return Vector3.zero;
    }
    public Vector3 Arrive(Vector3 TargetPos)
    {
        Vector3 ToTarget = TargetPos - Owner.transform.position;
        float dist = ToTarget.magnitude;

        if (dist > 0.0001f)
        {
            const float DecelerationTweaker = 0.6f;

            float speed = dist / DecelerationTweaker;

            speed = Mathf.Min(speed, Owner.MaxSpeed);

            Vector3 DesiredVelocity = ToTarget * speed / dist;

            return (DesiredVelocity - Owner.rb.velocity);
        }
        return Vector3.zero;
    }
    public Vector3 Pursuit(BaseGameEntity evader)
    {

        //return Vector3.zero;
        Vector3 ToEvader = evader.transform.position - Owner.transform.position;
        float RelativeHeading = Vector3.Dot(Owner.rb.velocity, evader.rb.velocity);


        if ((Vector3.Dot(ToEvader, Owner.rb.velocity) > 0) &&
       (RelativeHeading < -0.95))  //acos(0.95)=18 degs
        {
            return Seek(evader.transform.position);
        }

        float LookAheadTime = ToEvader.magnitude /
                        (Owner.MaxSpeed + evader.rb.velocity.magnitude);
        return Seek(evader.transform.position + evader.rb.velocity * LookAheadTime);
    }
    public Vector3 OffsetPursuit(BaseGameEntity leader, Vector3 offset)
    {


        GameObject WorldOffsetPos = new GameObject();
        WorldOffsetPos.transform.position = leader.transform.position;
        WorldOffsetPos.transform.rotation = leader.transform.rotation;
        WorldOffsetPos.transform.Translate(offset);

        Vector3 ToOffset = WorldOffsetPos.transform.position - Owner.transform.position;

        float LookAheadTime = ToOffset.magnitude /
                              (Owner.MaxSpeed + leader.rb.velocity.magnitude);
        //now Arrive at the predicted future position of the offset
        Vector3 finalVec = Seek(WorldOffsetPos.transform.position + leader.rb.velocity * LookAheadTime);
        Destroy(WorldOffsetPos);
        return finalVec;
    }
    public Vector3 Interpose(BaseGameEntity AgentA, BaseGameEntity AgentB)
    {
        //first we need to figure out where the two agents are going to be at 
        //time T in the future. This is approximated by determining the time
        //taken to reach the mid way point at the current time at at max speed.
        Vector3 MidPoint = (AgentA.transform.position + AgentB.transform.position) / 2.0f;

        float TimeToReachMidPoint = Vector3.Distance(Owner.transform.position, MidPoint) /
                                     Owner.MaxSpeed;

        //now we have T, we assume that agent A and agent B will continue on a
        //straight trajectory and extrapolate to get their future positions
        Vector3 APos = AgentA.transform.position + AgentA.rb.velocity * TimeToReachMidPoint;
        Vector3 BPos = AgentB.transform.position + AgentB.rb.velocity * TimeToReachMidPoint;

        //calculate the mid point of these predicted positions
        MidPoint = (APos + BPos) / 2.0f;

        //then steer to Arrive at it
        return Arrive(MidPoint);
    }
    public Vector3 Hide(BaseGameEntity hunter, GameObject[] obstacles, float distance)
    {
        float DistToClosest = 999999.99f;
        Vector3 BestHidingSpot = Vector3.zero;

        GameObject closest;

        foreach (GameObject gameObject in obstacles)
        {
            Vector3 HidingSpot = GetHidingPosition(gameObject.transform.position,
                                                     distance,
                                                      hunter.transform.position);
            float dist = Vector3.Distance(HidingSpot, Owner.transform.position);
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
            return Evade(hunter);
        }

        //else use Arrive on the hiding spot
        return Arrive(BestHidingSpot);
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
    public Vector3 Evade(BaseGameEntity pursuer)
    {
        //return Vector3.zero;
        Vector3 ToPursuer = pursuer.transform.position - Owner.transform.position;

        const double ThreatRange = 100.0;
        if (ToPursuer.magnitude > ThreatRange) return Vector3.zero;

        float LookAheadTime = ToPursuer.magnitude /
                        (Owner.MaxSpeed + pursuer.rb.velocity.magnitude);
        return Flee(pursuer.transform.position + pursuer.rb.velocity * LookAheadTime);
    }
    public Vector3 Flee(Vector3 TargetPos)
    {
        // only flee if the target is within 'panic distance'.Work in distance
        //squared space.
        const double PanicDistanceSq = 8.0f;
        if (Vector3.Magnitude(Owner.transform.position - TargetPos) > PanicDistanceSq)
        {
            return new Vector3(0, 0, 0);
        }


        Vector3 DesiredVelocity = Vector3.Normalize(Owner.transform.position - TargetPos)
                                  * Owner.MaxSpeed;

        return (DesiredVelocity - Owner.rb.velocity);
    }
    public Vector3 MoveAlongThePath(List<GameObject> Paths,ref int pathIndex,int maxPath)
    {
        if (Vector3.Distance(Paths[pathIndex].transform.position,Owner.transform.position) < 0.1f)
        {
            pathIndex = (pathIndex + 1) % maxPath;
            return Vector3.zero;
        }
        else
        {
            return Seek(Paths[pathIndex].transform.position);
        }
    }
    public Vector3 ObstacleAvoidance(List<GameObject> obstacles)
    {
        Vector3 SteeringPower = Vector3.zero;
        foreach(GameObject obstacle in obstacles)
        {
            float colliderRadius = obstacle.transform.GetComponent<SphereCollider>().radius;
            if(Vector3.Distance(obstacle.transform.position,Owner.transform.position) < 3f)
            {

                Vector3 OA = Vector3.Normalize(Owner.transform.position - obstacle.transform.position);
                float temp = OA.x;
                OA.x = - OA.y;
                OA.y = temp;
                float reverseDistance = Mathf.Clamp(1.0f / Vector3.Distance(obstacle.transform.position, Owner.transform.position),0,10.0f);
                Debug.Log("distance "+ Vector3.Distance(obstacle.transform.position, Owner.transform.position));
                SteeringPower += OA * reverseDistance * 3;

                Debug.DrawLine(Owner.transform.position, Owner.transform.position + OA * reverseDistance, Color.red);
            }
        }

        return SteeringPower;
    }
    public void LimitCurrentSpeed()
    {
        if (Owner.rb.velocity.magnitude > Owner.MaxSpeed)
        {
            Owner.rb.velocity = Owner.rb.velocity.normalized;
            Owner.rb.velocity *= Owner.MaxSpeed;
        }
        else if (Owner.rb.velocity.magnitude < 0.01f)
        {
            Owner.rb.velocity = Vector3.zero;
        }
    }
}

public enum SteeringMode
{
    Invalid = -1,
    Seek,
}