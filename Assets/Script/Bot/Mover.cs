using UnityEngine;
using UnityEngine.AI;
using System;

public class Mover : Unit
{
    [SerializeField] private UnitDispatcher _basePrefab;

    public event Action<Vector3> ColonizingPositionReached;
    protected override void MoveTo(Vector3 destination)
    {
        NavMeshAgent.SetDestination(destination);

        if (NavMeshAgent.pathPending == false && NavMeshAgent.remainingDistance <= NavMeshAgent.stoppingDistance)
        {
            if (CurrentState == UnitState.MovingToResource)
            {
                CurrentState = UnitState.ReturningToBase;
            }
            else if (CurrentState == UnitState.ReturningToBase)
            {
                CurrentState = UnitState.Idle;
            }

            if( CurrentState == UnitState.Colonizing)
            {
                ColonizingPositionReached?.Invoke(destination);
                CurrentState = UnitState.Idle;
            }
        }
    } 
}
