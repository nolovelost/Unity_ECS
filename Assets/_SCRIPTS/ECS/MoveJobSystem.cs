using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Burst;

/// JobComponentSystem is a part of Unity.Entities not Unity.Jobs
public class MoveJobSystem : JobComponentSystem
{
    /// This is where execution job is defined. Every IComponentData (or equivalent) needs an
    /// IJobProcessComponentData (or equivalent) that processes it and mutates it into new data in the
    /// form of output.
    // [MoveSpeedComponent is the input data along with the Unity.Entities defaults i.e. Position and Rotation.]
    [BurstCompile]
    struct MovementJob : IJobProcessComponentData<Position, Rotation, MoveSpeed>
    {
        /// These extra data can be set by a monobehaviour (or anything that calls it really) class.
        public float upperBound;
        public float lowerBound;
        public float deltaTime;

        /// Every child of IJobProcessComponentData<...> needs to implement the Execute(...) method
        public void Execute(ref Position position, [ReadOnly]ref Rotation rotation, [ReadOnly]ref MoveSpeed movementSpeed)
        {
            float3 tempPosValue = position.Value;

            tempPosValue += math.forward(rotation.Value) * movementSpeed.value * deltaTime;

            if (tempPosValue.y < lowerBound)
                tempPosValue.y = upperBound;

            position.Value = tempPosValue;
        }
    }

    /// Every JobComponentSystem has an OnUpdate but NOT an OnStart function. One needs to rather use
    /// a filter component (?) to do stuff once.
    /// inputDeps are the dependencies that the compoinent requires to complete calculations before
    /// commencing with this one.
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        /// The execution job, defined above, needs to have proper data initialised. That is done here
        /// through its constructor and the GameManager.
        MovementJob movementJob = new MovementJob
        {
            upperBound = GameObject.FindObjectOfType<ECSCarManager>().upperBound,
            lowerBound = GameObject.FindObjectOfType<ECSCarManager>().lowerBound,
            deltaTime = Time.deltaTime
        };

        JobHandle movementJobHandle = movementJob.Schedule(this, inputDeps);
        return movementJobHandle;
    }
}
