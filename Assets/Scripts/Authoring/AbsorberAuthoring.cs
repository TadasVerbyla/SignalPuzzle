using Unity.Entities;
using UnityEngine;

public class AbsorberAuthoring : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float range;

    public class Baker : Baker<AbsorberAuthoring>{
        public override void Bake(AbsorberAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Absorber{
                range = authoring.range
            });
        }
    }
}


public struct Absorber : IComponentData {
    public float range;
}
