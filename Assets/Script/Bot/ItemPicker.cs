using UnityEngine;
[RequireComponent(typeof(Movement))]

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private Transform _backpackPoint;

    private Resource _heldObject;
    private Movement _movement;

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_heldObject == null && other.TryGetComponent(out Resource resource) && resource.ResourceIndex == _movement.CurrentResource?.ResourceIndex)
        {
            PickupObject(resource);
        }
        else if (_heldObject != null && other.TryGetComponent(out BaseWarehouse baseWarehouse) && baseWarehouse.CheckResourceIndex(_movement.CurrentResource.ResourceIndex))
        {
            DropObject(other.transform);
        }
    }

    private void PickupObject(Resource objectToPickup)
    {
        _heldObject = objectToPickup;

        if (_heldObject.TryGetComponent(out Rigidbody objectRigidbody))
        {
            objectRigidbody.isKinematic = true;
        }

        _heldObject.transform.position = _backpackPoint.position;
        _heldObject.transform.SetParent(transform);
    }

    private void DropObject(Transform transform)
    {
        if (_heldObject.TryGetComponent(out Rigidbody objectRigidbody))
        {
            objectRigidbody.isKinematic = false;
        }

        _heldObject.transform.position = transform.position;

        _heldObject.transform.SetParent(null);
        _movement.ResetCurrentResource();
        _heldObject = null;
    }
}
