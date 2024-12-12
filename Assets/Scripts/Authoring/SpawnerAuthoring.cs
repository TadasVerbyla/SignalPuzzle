using Unity.Entities;
using UnityEngine;

public class SpawnerAuthoring : MonoBehaviour {
    public float directionLimit;
    public int toSpawnPerTick;
    public int spawnCount;

    public class Baker : Baker<SpawnerAuthoring>{
        public override void Bake(SpawnerAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Spawner{
                directionLimit = authoring.directionLimit,
                toSpawnPerTick = authoring.toSpawnPerTick,
                spawnCount = authoring.spawnCount
            });
        }
    }
}


public struct Spawner : IComponentData {
    public float directionLimit;
    public int toSpawnPerTick;
    public int spawnCount;
    
}
