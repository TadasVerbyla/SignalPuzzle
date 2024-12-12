using Unity.Entities;
using UnityEngine;

public class ObjectiveAuthoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float range;
    public int signalCount;
    public bool on;
    public bool desiredOn;

    public class Baker : Baker<ObjectiveAuthoring>{
        public override void Bake(ObjectiveAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Objective{
                range = authoring.range,
                signalCount = authoring.signalCount,
                on = authoring.on,
                desiredOn = authoring.desiredOn
            });
        }
    }
}


public struct Objective : IComponentData {
    public float range;
    public int signalCount;
    public bool on;
    public bool desiredOn;
}
