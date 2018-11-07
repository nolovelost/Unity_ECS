using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;


[Serializable]
public struct MoveSpeed : IComponentData
{
    public float value;
}

public class MoveSpeedComponent : ComponentDataWrapper<MoveSpeed> { }