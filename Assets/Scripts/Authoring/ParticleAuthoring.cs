using Unity.Entities;
using UnityEngine;

public class ParticleAuthoring : MonoBehaviour {
    public float speed;
    public float direction;
    public float cullingDistance;

    public class Baker : Baker<ParticleAuthoring>{
        public override void Bake(ParticleAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Particle{
                direction = authoring.direction,
                speed = authoring.speed,
                cullingDistance = authoring.cullingDistance
            });
        }
    }
}


public struct Particle : IComponentData {
    public float speed;
    public float direction;
    public float cullingDistance;
}
