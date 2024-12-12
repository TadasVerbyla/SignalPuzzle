using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        int left = -1;
        int right = 1; 
        int stop = 0;
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Rotator>().Build(entityManager);

        NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
        NativeArray<Rotator> rotators = entityQuery.ToComponentDataArray<Rotator>(Allocator.Temp);
        for (int i = 0; i < rotators.Length; i++) {
            Rotator rotator = rotators[i];
            if (Input.GetKey("q"))  { rotator.direction = left; }   else
            if (Input.GetKey("e"))  { rotator.direction = right; }  else
                                        { rotator.direction = stop; }

            entityManager.SetComponentData(entities[i], rotator);
        }
        
    }
}
