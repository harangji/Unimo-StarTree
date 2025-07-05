using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : MonoBehaviour
{
    protected MonsterController controller;
    virtual public void TransitionAction(MonsterController controller) 
    { this.controller = controller; }

    virtual public void FixedUpdateAction() { }

    virtual public void UpdateAction() { }
}
