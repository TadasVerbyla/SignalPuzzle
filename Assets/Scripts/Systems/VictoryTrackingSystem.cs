using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;
using GenericEventSystem;

partial struct VictoryTrackingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        bool victory = true;
        foreach (RefRO<Objective> objective in SystemAPI.Query<RefRO<Objective>>())
        {
            if (objective.ValueRO.on != objective.ValueRO.desiredOn){
                victory = false;
            }
        }
        if (victory){
            Debug.Log("Victory");
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
