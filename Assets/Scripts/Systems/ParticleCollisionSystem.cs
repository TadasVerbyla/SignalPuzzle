using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ParticleAbsorbSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        

        // Gather absorber data
        List<float3> absorberPositions = new List<float3>();
        float absorberRange = 0;

        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRO<Absorber> absorber)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Absorber>>())
        {
            absorberPositions.Add(localTransform.ValueRO.Position);
            absorberRange = absorber.ValueRO.range;
        }

        // Gather bouncer data
        List<float3> bouncerPositions = new List<float3>();
        float bouncerRange = 0;
        float bounceSpeedModifier = 1;

        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRO<Bouncer> bouncer)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Bouncer>>())
        {
            bouncerPositions.Add(localTransform.ValueRO.Position);
            bouncerRange = bouncer.ValueRO.range;
            bounceSpeedModifier = bouncer.ValueRO.speedModifier;
        }

        // Gather objective data
        List<float3> objectivePositions = new List<float3>();
        NativeList<Objective> objectives = new NativeList<Objective>(Allocator.TempJob);
        NativeList<Entity> objectiveEntities = new NativeList<Entity>(Allocator.TempJob);

        
        float objectiveRange = 0;
        // TODO BULLET TUTORIAL TO CHECK HOW TO TRANSFER "DAMAGE" TO OBJECTIVE

        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRW<Objective> objective,
            Entity entity)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRW<Objective>>().WithEntityAccess())
        {
            objectivePositions.Add(localTransform.ValueRO.Position);
            objectives.Add(objective.ValueRW);
            objectiveEntities.Add(entity);
            objectiveRange = objective.ValueRW.range;
        }

        // Check collisions
        foreach ((
            RefRW<LocalTransform> localTransform,
            RefRW<Particle> particleVariables,
            Entity entity)
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRW<Particle>>().WithEntityAccess())
        {
            float3 particlePosition = localTransform.ValueRO.Position;

            // Check absorber collisions
            bool absorbed = false;
            foreach (float3 absorberPosition in absorberPositions){
                absorbed = GeometricHelpers.IsInRange(particlePosition, absorberPosition, absorberRange);
                if (absorbed){
                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }
            }

            // Check bouncer collisions
            bool bounced = false;
            foreach (float3 bouncerPosition in bouncerPositions){
                bounced = GeometricHelpers.IsInRange(particlePosition, bouncerPosition, bouncerRange);
                if (bounced){
                    
                    float directionAwayFromCentre = GeometricHelpers.NormalizeDegrees(
                        GeometricHelpers.CalculateAngleToPositionXZ(particlePosition, bouncerPosition) + 180f
                    );
                    float reverseCurrentDirection = GeometricHelpers.NormalizeDegrees(
                        particleVariables.ValueRO.direction + 180f
                    );
                    float directionDelta = GeometricHelpers.FindAngleDelta(reverseCurrentDirection, directionAwayFromCentre);
                    float newDirection = directionAwayFromCentre + directionDelta;
                    particleVariables.ValueRW.direction = newDirection;

                    UnityEngine.Vector3 vectorAwayFromBouncer = GeometricHelpers.AngleToVector(directionAwayFromCentre);
                    localTransform.ValueRW.Position = bouncerPosition + math.normalize(vectorAwayFromBouncer) * bouncerRange;
                    continue;
                }
            }

            // Check objective collisions
            bool consumed = false;
            for (int i = 0; i < objectivePositions.Count; i++)
            {
                consumed = GeometricHelpers.IsInRange(particlePosition, objectivePositions[i], objectiveRange);
                if (consumed)
                {
                    if (objectives[i].signalCount > 1)
                    {
                        entityCommandBuffer.SetComponent(objectiveEntities[i], new Objective{ 
                            on = objectives[i].on,
                            range = objectives[i].range,
                            signalCount = objectives[i].signalCount - 1,
                            desiredOn = objectives[i].desiredOn
                        });
                        UnityEngine.Debug.Log($"New Signal count: {objectives[i].signalCount - 1}");
                    } else
                    {
                        entityCommandBuffer.SetComponent(objectiveEntities[i], new Objective{ 
                            on = !objectives[i].on,
                            range = objectives[i].range,
                            signalCount = 500,
                            desiredOn = objectives[i].desiredOn
                        });
                        UnityEngine.Debug.Log("Node swithced! New Signal count: 500");
                    }
                    entityCommandBuffer.DestroyEntity(entity);
                    continue;
                }
            }
        }
        objectives.Dispose();
        objectiveEntities.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
