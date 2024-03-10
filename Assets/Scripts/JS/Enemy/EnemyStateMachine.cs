using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public Basic_State activeState;
    public PatrolState patrolState;
    public GunState gunState;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    //StateChanging
    public void Initialise()
    {

        ChangeState(new PatrolState());
    }
    public void ChangeState(Basic_State newState)
    {

        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.enemy_StateMachine = this;

            activeState.enemy = GetComponent<Enemy>();

            activeState.Enter();
        }
    }
}
