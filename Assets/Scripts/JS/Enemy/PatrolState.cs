using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : Basic_State
{
    public int wayPointIndex;

    public float waitTimer;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        PatrolCycle();
  
    }

    //enemy walking along the wayPoints and change state when they see player
   public void PatrolCycle()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > enemy.patrolWaitingTime)
        {
            if (enemy.NavMeshAgent.remainingDistance < 0.2f)
            {
                if (wayPointIndex < enemy.path.wayPoints.Count - 1)
                {
                    wayPointIndex++;
                }
                else
                {
                    wayPointIndex = 0;
                }

                //set path way to yidong
                enemy.NavMeshAgent.SetDestination(enemy.path.wayPoints[wayPointIndex].position);

                waitTimer = 0;
            }
        }

        if (enemy.CanSeePlayer())
        {
            enemy_StateMachine.ChangeState(new GunState());

        }

    }
}
