﻿using UnityEngine;
using UnityEngine.AI;

public abstract class FlyingEnemy : Enemy
{
    protected float movingStateTimer = 0;
    public abstract void Animation_Run(bool isRun);
    public abstract void Animation_Attack();
    protected void Update()
    {
        switch (walkingState)
        {
            case MovingState.MOVING:
                if (Time.time - movingStateTimer >= movingTime)
                {
                    walkingState = MovingState.STAYING;
                    movingStateTimer = Time.time;
                    Animation_Run(false);
                }
                else
                {
                    Animation_Run(true);
                    Vector3 dest = player.componentCache.position;
                    dest = new Vector3(dest.x, transform.position.y, dest.z);
                    transform.LookAt(dest);
                    transform.position = Vector3.MoveTowards(transform.position, dest, Speed * Time.deltaTime);
                }
                break;
            case MovingState.STAYING:
                if (Time.time - movingStateTimer >= waitingTime)
                {
                    if (touchingPlayer != null)  // 캐릭터랑 근접한 상태 일 때
                    {
                        Animation_Attack();
                        if (touchingPlayer.TakeDamage(new DamageReport(damage * touchDamageMultiplier, this)))
                            touchingPlayer = null;
                        movingStateTimer = Time.time;
                    }
                    else
                    {
                        walkingState = MovingState.MOVING;
                        movingStateTimer = Time.time + Random.Range(0, randomTime);
                    }   
                }
                break;
            default:
                break;
        }
        if (isUseSkillState == true)
        {
            Animation_Attack();
        }
    }

    protected new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.tag == Tags.playerTag)
        {
            walkingState = MovingState.STAYING;
            movingStateTimer = Time.time;
        }
    }
}
