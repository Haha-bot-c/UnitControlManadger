using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(ItemPicker))]
public class Movement : Unit
{
    [SerializeField] private UnitDispatcher _basePrefab;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); 
    }

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
                UnitDispatcher dispatcher =  Instantiate(_basePrefab, _positionFlag, Quaternion.identity);
                dispatcher.RegisterUnit(this);
                PositionBase = _positionFlag;
                CurrentState = UnitState.Idle;

            }
        }
    } 
}
