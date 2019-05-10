using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour, PostProcessor<float>, PostProcessor<double>, PostProcessor<int>,PostProcessor<Vector2>,PostProcessor<Vector3>,PostProcessor<Vector4>, PostProcessor<Vector2Int>, PostProcessor<Vector3Int>
{
    public float Factor = 1;

    public float Proccess(float unproccessedValue)
    {
        return unproccessedValue * Factor;
    }

    public double Proccess(double unproccessedValue)
    {
        return unproccessedValue * Factor;
    }

    public int Proccess(int unproccessedValue)
    {
        return (int)(unproccessedValue * Factor);
    }

    public Vector2 Proccess(Vector2 unproccessedValue)
    {
        return unproccessedValue * Factor;
    }

    public Vector3 Proccess(Vector3 unproccessedValue)
    {
        return unproccessedValue * Factor;
    }

    public Vector4 Proccess(Vector4 unproccessedValue)
    {
        return unproccessedValue * Factor;
    }
    
    Vector2Int PostProcessor<Vector2Int>.Proccess(Vector2Int unproccessedValue)
    {
        return new Vector2Int((int)(unproccessedValue.x * Factor),(int)(unproccessedValue.y * Factor));
    }
    
    Vector3Int PostProcessor<Vector3Int>.Proccess(Vector3Int unproccessedValue)
    {
        return new Vector3Int((int)(unproccessedValue.x * Factor),(int)(unproccessedValue.y * Factor),(int)(unproccessedValue.z * Factor));
    }
}
