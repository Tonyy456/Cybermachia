using System;
using System.Collections;
using System.Collections.Generic;
using Tony;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FSMBehaviour : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineSO fsm;
    [SerializeField] private List<StateSOUnityEventWrapper> events;

    [Header("Dont Change. Debug Purposes Only")]
    [SerializeField] private string currentState;
    private SimpleFSM result;

    private void Awake()
    {
        if (fsm == null) Destroy(this.gameObject);

        //Wire up state events
        foreach(var item in events)
        {
            item.State.OnEnter += () => { item.OnEnter?.Invoke(); };
        }

        // Wire up fsm representation
        result = convertStateMachine(fsm);
        currentState = result.CurrentState.Name;
        result.OnStateTransition += () =>
        {
            currentState = result.CurrentState.Name;
            result.CurrentState.behaviour = this;
        };  
    }

    public void Fire(string message)
    {
        result.Fire(message);
    }

    private SimpleFSM convertStateMachine(FiniteStateMachineSO fsm_so)
    {

        StateSO startState = this.fsm.StartState;
        SimpleFSM fsm = new SimpleFSM(startState);

        List<StateSO> visited = new List<StateSO>();
        Queue<StateSO> toVisit = new Queue<StateSO>();
        toVisit.Enqueue(startState);

        foreach (var anyT in this.fsm.AnyStateTransitions)
        {
            fsm.AddAnyTransition(anyT.trigger, anyT.state);
        }

        while (toVisit.Count > 0)
        {
            StateSO currentItem = toVisit.Dequeue();
            visited.Add(currentItem);

            //peep transitions
            foreach (var transition in currentItem.Transitions)
            {
                string trigger = transition.trigger;
                StateSO nextState = transition.state;
                fsm.AddTransition(currentItem, trigger, nextState);

                //add next state to recursive loop
                if (!visited.Contains(nextState)) toVisit.Enqueue(nextState);
            }

        }

        return fsm;
    }
}

public class SimpleFSM : FiniteStateMachine<StateSO, string>
{
    
    public SimpleFSM(StateSO initialState) : base(initialState) {
        initialState.OnEnter?.Invoke();
    }

    protected override void OnInvalidTransition(StateSO currentState, string trigger)
    {
        Debug.Log($"Attempted Invalid Transition. From:{currentState.Name} with {trigger}");
    }

    protected override void OnTransition(StateSO fromState, StateSO toState, string trigger)
    {
        fromState.OnExit?.Invoke();
        fromState.behaviour = null;
        toState.OnEnter?.Invoke();
    }
}

[Serializable]
public class StateSOUnityEventWrapper
{
    public StateSO State;
    public UnityEngine.Events.UnityEvent OnEnter; 
}
