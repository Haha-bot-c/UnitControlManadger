using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(NavMeshAgent), typeof(ItemPicker))]
public abstract class Unit : MonoBehaviour
{
    public Resource CurrentResource { get; private set; }

    protected enum UnitState
    {
        Idle,
        MovingToResource,
        ReturningToBase,
        Colonizing
    }

    [SerializeField] protected UnitDispatcher UnitDispather;
    
    protected UnitState CurrentState = UnitState.Idle;

    protected NavMeshAgent NavMeshAgent;
    protected Vector3 PositionBase;
    protected Vector3 _positionFlag;

    private Vector3 _positionResourse;
    private Vector3 _startingPosition;

    

    protected virtual void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PositionBase = UnitDispather.transform.position;
        _startingPosition = transform.position;
    }

    protected virtual void Update()
    {

        switch (CurrentState)
        {
            case UnitState.Idle:
                MoveTo(_startingPosition);
                break;
            case UnitState.MovingToResource:
                MoveTo(_positionResourse);
                break;
            case UnitState.ReturningToBase:
                MoveTo(PositionBase);
                break;
            case UnitState.Colonizing:
                MoveTo(_positionFlag);
                break;
        }
    }

    protected abstract void MoveTo(Vector3 position);

    public void ResetCurrentResource()
    {
        CurrentResource = null;
    }

    public void InitiateMoveToResource(Resource resource)
    {
        CurrentResource = resource;
        _positionResourse = resource.transform.position; 
        CurrentState = UnitState.MovingToResource;
    }


    public void StartColonizing(Vector3 position)
    {
        _positionFlag = position;
        CurrentState = UnitState.Colonizing;
    }

    public void AssignBase(UnitDispatcher dispatcher)
    {
        UnitDispather = dispatcher;
        PositionBase = UnitDispather.transform.position;
    }
}
