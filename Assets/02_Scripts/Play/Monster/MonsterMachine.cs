using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMachine : MonoBehaviour
{
    private MonsterState currentState;

    private void FixedUpdate()
    {
        if (currentState != null) { currentState.FixedUpdateAction(); }
    }
    // Update is called once per frame
    private void Update()
    {
        if (currentState != null) { currentState.UpdateAction(); }
    }
    public void TransitState(MonsterState newstate, MonsterController controller)
    {
        currentState = newstate;
        newstate.TransitionAction(controller);
    }
}
