using UnityEngine;

public class Resource : MonoBehaviour
{
    public int ResourceIndex { get; private set; }

    public void AssignResourceIndex(int resourceIndex)
    {
        ResourceIndex = resourceIndex;
    }
}
