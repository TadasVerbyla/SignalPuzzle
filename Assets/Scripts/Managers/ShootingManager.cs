using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown("space")) {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Spawner>().Build(entityManager);

            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Spawner> spawners = entityQuery.ToComponentDataArray<Spawner>(Allocator.Temp);
            for (int i = 0; i < spawners.Length; i++) {
                Spawner spawner = spawners[i];
                spawner.toSpawnPerTick = spawner.spawnCount;
                entityManager.SetComponentData(entities[i], spawner);
            }
        }
        if (Input.GetKeyUp("space")) {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Spawner>().Build(entityManager);

            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Spawner> spawners = entityQuery.ToComponentDataArray<Spawner>(Allocator.Temp);
            for (int i = 0; i < spawners.Length; i++) {
                Spawner spawner = spawners[i];
                spawner.toSpawnPerTick = 0;
                entityManager.SetComponentData(entities[i], spawner);
            }
        }
    }
}
