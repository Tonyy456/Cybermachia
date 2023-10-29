using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tony
{
    [CreateAssetMenu(fileName = "FSM", menuName = "FSM/FiniteStateMachine")]
    public class FiniteStateMachineSO : ScriptableObject
    {
        public string Name;
        public StateSO CurrentState;
        public List<StateTransition> AnyStateTransitions;

        /* Author: Anthony D'Alesandro
         * Send message to machine. Transitions if able to.
         * 
         * return: true if state transitioned. false otherwise.
         */
        public void Fire(string message)
        {
            if (TryAnyStateTransition(message)) return;
            TryStateTransition(message);
        }

        /* Author: Anthony D'Alesandro
         * Try to transition using transitions from any state.
         * 
         * return: true if state transitioned. false otherwise.
         */
        private bool TryAnyStateTransition(string message)
        {
            StateTransition any_match = AnyStateTransitions.Find(x => x.trigger == message);
            if (any_match != null && CurrentState.name != any_match.state.name)
            {
                return TryTransition(any_match.state);
            }
            return false;
        }

        /* Author: Anthony D'Alesandro
         * Try to transition using transitions on current state.
         * 
         * return: true if state transitioned. false otherwise.
         */
        private bool TryStateTransition(string message)
        {
            if (CurrentState == null) return false;
            StateTransition match = CurrentState.Transitions.Find(x => x.trigger == message);
            if (match != null)
            {
                var result = TryTransition(match.state);
                return result;
            }
            return false;

        }

        /* Author: Anthony D'Alesandro
         * Try to transition to @code{state}
         * 
         * return: true if state transitioned. false otherwise.
         */
        private bool TryTransition(StateSO state)
        {
            if (CurrentState.name != state.name)
            {
                CurrentState.OnExit?.Invoke();
                CurrentState = state;
                CurrentState.OnEnter?.Invoke();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
