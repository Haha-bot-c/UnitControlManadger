using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

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
        if (other.TryGetComponent(out Resource resource)
            && _collectedResources.Contains(resource) == false 
            && CheckResourceIndex(resource.ResourceIndex))
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
        return _indexResours.Contains(index);
    }

    public void SpendResources(int count)
    {
        for (int i = 0; i < count; i++)
        {
           Destroy(_collectedResources.Dequeue());
        }

        CollectedResourcesChanged?.Invoke(_collectedResources.Count);
    }


    public void AddIndexResirse(int index)
    {
        _indexResours.Add(index);
    }
}
