using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Burst;

[BurstCompileAttribute]
public struct JobCarTransform : IJobParallelForTransform
{
    public float movementSpeed;
    public float upperBound;
    public float lowerBound;
    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        transform.position += (transform.rotation * new Vector3(0.0f, -1.0f, 0.0f)) * movementSpeed * deltaTime;

        if (transform.position.y < lowerBound)
        {
            Vector3 pos = transform.position;
            pos.y = upperBound;
            transform.position = pos;
        }
    }

}
