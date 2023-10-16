using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tony
{
    public abstract class FiniteStateMachine<TState, TTrigger>
    {
        private Dictionary<TState, Dictionary<TTrigger, TState>> transitions;

        public TState CurrentState { get; protected set; }

        protected FiniteStateMachine(TState initialState)
        {
            CurrentState = initialState;
            transitions = new Dictionary<TState, Dictionary<TTrigger, TState>>();
        }

        public void AddTransition(TState fromState, TTrigger trigger, TState toState)
        {
            if (!transitions.ContainsKey(fromState))
            {
                transitions[fromState] = new Dictionary<TTrigger, TState>();
            }

            transitions[fromState][trigger] = toState;
        }

        public void Fire(TTrigger trigger)
        {
            if (transitions.ContainsKey(CurrentState) && transitions[CurrentState].ContainsKey(trigger))
            {
                TState nextState = transitions[CurrentState][trigger];
                OnTransition(CurrentState, nextState, trigger);
                CurrentState = nextState;
            }
            else
            {
                OnInvalidTransition(CurrentState, trigger);
            }
        }

        protected abstract void OnTransition(TState fromState, TState toState, TTrigger trigger);
        protected abstract void OnInvalidTransition(TState currentState, TTrigger trigger);
    }

    public abstract class State
    {
        protected abstract void Enter();
        protected abstract void Exit();
        public abstract void Tick();
    }

    //https://stackoverflow.com/questions/5923767/simple-state-machine-example-in-c
    public enum StateTrigger
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }

    public abstract class DefaultFSM : FiniteStateMachine<State, StateTrigger>
    {
        protected DefaultFSM(State initialState) : base(initialState)
        {
        }
    }
}

