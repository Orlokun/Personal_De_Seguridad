using UnityEngine;
using Utils;

public class ShopPositionsManager : InitializeManager
{
    private int positionsInLevel;
    private Transform[] m_positionObjects;

    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        if (MIsInitialized)
        {
            return;
        }
        if (transform.childCount == 0)
        {
            return;
        }
        CompleteInitialization();
    }

    private void CompleteInitialization()
    {
        positionsInLevel = transform.childCount;
        m_positionObjects = new Transform[positionsInLevel];
        for (var i = 0; i < positionsInLevel; i++)
        {
            m_positionObjects[i] = transform.GetChild(i);
        }
        MIsInitialized = true;
    }
    public Vector3[] GetRandomPositions(int numberOfPositions)
    {
        if (!MIsInitialized)
        {
            return null;
        }
        var vectors = new Vector3[numberOfPositions];
        for (var i = 0; i < numberOfPositions; i++)
        {
            var positionIndex = Random.Range(0, positionsInLevel);
            vectors[i] = m_positionObjects[positionIndex].position;
        }
        
        if (vectors.Length == numberOfPositions && vectors.Length != 0)
        {
            return vectors;
        }
        
        Debug.LogError("Random Positions length must have the expected length");
        return null;
    }
}
