using System.Numerics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ParticleMoverSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((
            RefRW<LocalTransform> localTransform,
            RefRO<Particle> particleVariables)
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<Particle>>())
        {
            float speed = particleVariables.ValueRO.speed;

            
            UnityEngine.Vector3 direction = GeometricHelpers.AngleToVector(particleVariables.ValueRO.direction);
            localTransform.ValueRW.Position = localTransform.ValueRO.Position + new float3(
                direction.x * speed * SystemAPI.Time.DeltaTime,
                direction.y * speed * SystemAPI.Time.DeltaTime,
                direction.z * speed * SystemAPI.Time.DeltaTime);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
