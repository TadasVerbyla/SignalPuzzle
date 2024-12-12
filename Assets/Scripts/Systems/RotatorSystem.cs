using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial struct RotatorSystem : ISystem
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
            RefRO<Rotator> rotationSpeed)
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<Rotator>>())
        {
            quaternion xRotation = quaternion.EulerXYZ(
                new float3(0, 0, math.radians(rotationSpeed.ValueRO.rotationSpeed * rotationSpeed.ValueRO.direction) * SystemAPI.Time.DeltaTime)
            );
            localTransform.ValueRW.Rotation = math.mul(localTransform.ValueRW.Rotation, xRotation);

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
