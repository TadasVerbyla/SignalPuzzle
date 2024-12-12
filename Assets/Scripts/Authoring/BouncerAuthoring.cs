using Unity.Entities;
using UnityEngine;

public class BouncerAuthoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float range;
    public float speedModifier;

    public class Baker : Baker<BouncerAuthoring>{
        public override void Bake(BouncerAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bouncer{
                range = authoring.range,
                speedModifier = authoring.speedModifier
            });
        }
    }
}


public struct Bouncer : IComponentData {
    public float range;
    public float speedModifier;
}
