using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Basic_State 
{
    public Enemy enemy;
    public EnemyStateMachine enemy_StateMachine;
    //instant of enemy class
    //instant of stateMechine class
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Perform();

}
