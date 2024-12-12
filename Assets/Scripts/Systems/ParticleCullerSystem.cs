using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ParticleCullerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = 
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach ((
            RefRO<LocalTransform> localTransform,
            RefRO<Particle> particleVariables,
            Entity entity)
            in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<Particle>>().WithEntityAccess())
        {
            float3 position = localTransform.ValueRO.Position;
            float distance = math.distance(position, float3.zero);

            if (distance > particleVariables.ValueRO.cullingDistance)
            {
                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
