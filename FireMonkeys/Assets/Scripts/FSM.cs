using System;
using System.Collections;
using UnityEngine;

class FSM
{
    public delegate IEnumerator StateMethod();
    private MonoBehaviour script;
    private StateMethod[] states;
    private int currentState;

    public FSM(MonoBehaviour script, StateMethod[] states)
    {
        this.states = states;
        this.script = script;
    }


    public void Start(int state = 0)
    {
        script.StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        while (true)
            yield return script.StartCoroutine(states[currentState]());
    }
    
    public int CurrentState()
    {
        return currentState;
    }

    public void ChangeState(int nextState)
    {
        //Debug.Log(states[currentState].Method.Name + " -> " + states[nextState].Method.Name);
        currentState = nextState;
    }
    
    public void ChangeState(Enum nextState)
    {
        ChangeState(Convert.ToInt32(nextState));
    }

    public bool isState(int state)
    {
        return state == currentState;
    }

    public bool isState(Enum state)
    {
        return isState(Convert.ToInt32(state));
    }

}

