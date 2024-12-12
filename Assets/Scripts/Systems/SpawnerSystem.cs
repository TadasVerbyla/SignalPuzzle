using System;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.SocialPlatforms;

partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityReferences entityReferences = SystemAPI.GetSingleton<EntityReferences>();
        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRO<Spawner> spawner)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Spawner>>())
        {
            int toSpawn = spawner.ValueRO.toSpawnPerTick;
            float directionLimit = spawner.ValueRO.directionLimit;
            while (toSpawn > 0){
                toSpawn--;
                Entity particle = state.EntityManager.Instantiate(entityReferences.particlePrefabEntity);
                float3 startingPosition = localTransform.ValueRO.Position;
                startingPosition.y -= 0.1f;
                SystemAPI.SetComponent(particle, LocalTransform.FromPositionRotation(
                    startingPosition, quaternion.EulerXYZ(math.radians(90), 0, 0)));
                
                float3 euler = math.degrees(math.Euler(localTransform.ValueRO.Rotation));
                float currentFront = euler.z;

                RefRW<Particle> particleParticle = SystemAPI.GetComponentRW<Particle>(particle);
                particleParticle.ValueRW.direction = -1 * UnityEngine.Random.Range(currentFront - directionLimit, currentFront + directionLimit);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
