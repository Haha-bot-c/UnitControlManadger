using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(AudioSource))]
public class BaseWarehouse : MonoBehaviour
{
    [SerializeField] private UnitDispatcher _unitDispatcher;

    private AudioSource _audioSource;
    private Queue<Resource> _collectedResources = new Queue<Resource>();
    private List<int> _indexResours = new List<int>();

    public event Action<int> CollectedResourcesChanged;
    public event Action<Queue<Resource>> QueueChanged;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource) && _collectedResources.Contains(resource) == false && CheckResourceIndex(resource.ResourceIndex))
        {
            PlayAudioWhenResourceMoved();
            AddCollectedResource(resource);
        }
    }

    private void PlayAudioWhenResourceMoved()
    {
        _audioSource.Play();
    }

    private void AddCollectedResource(Resource resource)
    {
        _collectedResources.Enqueue(resource);
        CollectedResourcesChanged?.Invoke(_collectedResources.Count);
        QueueChanged?.Invoke(_collectedResources);
    }

    public bool CheckResourceIndex(int index)
    {
        foreach (var resource in _indexResours)
        {
            if (resource == index)
            {
                return true;
            }
        }

        return false;
    }

    public void SpendResources(int count)
    {
        for (int i = 0; i < count && _collectedResources.Count > 0; i++)
        {
            Resource resource = _collectedResources.Dequeue();

            if (resource != null)
            {
                Destroy(resource.gameObject);
            }
        }

        CollectedResourcesChanged?.Invoke(_collectedResources.Count);
    }


    public void AddIndexResirse(int index)
    {
        _indexResours.Add(index);
    }
}
