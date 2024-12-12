using Unity.Entities;
using UnityEngine;

public class EntityReferencesAuthoring : MonoBehaviour {
    public GameObject particlePrefab;
   public class Baker : Baker<EntityReferencesAuthoring> {
        public override void Bake(EntityReferencesAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntityReferences {
                particlePrefabEntity = GetEntity(authoring.particlePrefab, TransformUsageFlags.Dynamic)
            });
        }
   }
}

public struct EntityReferences : IComponentData {
    public Entity particlePrefabEntity;
}