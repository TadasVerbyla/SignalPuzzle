using Unity.Entities;
using UnityEngine;

public class RotatorAuthoring : MonoBehaviour {
    public float rotationSpeed;
    public float direction;

    public class Baker : Baker<RotatorAuthoring>{
        public override void Bake(RotatorAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Rotator{
                rotationSpeed = authoring.rotationSpeed,
                direction = authoring.direction
            });
        }
    }
}


public struct Rotator : IComponentData {
    public float rotationSpeed;
    public float direction;
}
