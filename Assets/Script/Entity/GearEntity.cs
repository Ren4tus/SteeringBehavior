using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearEntity : EnemyEntity
{
    private void Start()
    {
        //WanderRadius  = 1.0f;
        //WanderDistance= 1.0f;
        //WanderJitter  = 40.0f;
        mStateMachine.CurrentState = PurpleFlyIdle.Instance;
    }
    protected override void Update()
    {
        base.Update();
        Vector2 starting = new Vector2(transform.position.x, transform.position.y);
        Vector2 targeting = new Vector2(transform.position.x + rb.velocity.x, transform.position.y+rb.velocity.y);

        transform.rotation = LookAt2D(starting, targeting, transform.rotation.z);
    }
    public enum FacingDirection
    {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
    }
    public static Quaternion LookAt2D(Vector2 startingPosition, Vector2 targetPosition, float facing)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
