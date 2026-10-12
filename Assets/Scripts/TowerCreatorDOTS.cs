using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

// Authoring component. Lives inside a SubScene so the prefab is baked to an entity prefab at edit time
// (runtime GameObject conversion no longer exists in Entities 1.0+).
public class TowerCreatorDOTS : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width = 10;
    public int length = 10;

    class Baker : Baker<TowerCreatorDOTS>
    {
        public override void Bake(TowerCreatorDOTS authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new TowerSpawner
            {
                Prefab = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                Width = authoring.width,
                Length = authoring.length
            });
        }
    }
}

public struct TowerSpawner : IComponentData
{
    public Entity Prefab;
    public int Width;
    public int Length;
}

// Spawns the tower once per loaded spawner. Instances inherit the subscene's SceneTag,
// so they are destroyed automatically when the subscene unloads on scene change.
public partial struct TowerSpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<TowerSpawner>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var query = SystemAPI.QueryBuilder().WithAll<TowerSpawner>().Build();
        using var spawnerEntities = query.ToEntityArray(Allocator.Temp);
        using var spawners = query.ToComponentDataArray<TowerSpawner>(Allocator.Temp);

        for (int s = 0; s < spawners.Length; s++)
        {
            var spawner = spawners[s];
            var prefabTransform = state.EntityManager.GetComponentData<LocalTransform>(spawner.Prefab);

            int cubeMax = GameController.numObjects;
            var positions = new NativeList<float3>(cubeMax, Allocator.Temp);
            int height = cubeMax;
            for (int h = 0; h < height && positions.Length < cubeMax; h++)
            {
                for (int w = 0; w < spawner.Width && positions.Length < cubeMax; w++)
                {
                    for (int l = 0; l < spawner.Length && positions.Length < cubeMax; l++)
                    {
                        if (l != 0 && w != 0 && l != spawner.Length - 1 && w != spawner.Width - 1)
                        {
                            //do not instantiate a cube, skip to the next values
                            continue;
                        }

                        positions.Add(new float3(l, h + 0.5f, w));
                    }
                }
            }

            if (positions.Length > 0)
            {
                using var instances = state.EntityManager.Instantiate(spawner.Prefab, positions.Length, Allocator.Temp);
                for (int i = 0; i < instances.Length; i++)
                {
                    var transform = prefabTransform;
                    transform.Position = positions[i];
                    state.EntityManager.SetComponentData(instances[i], transform);
                }
            }
            positions.Dispose();

            state.EntityManager.RemoveComponent<TowerSpawner>(spawnerEntities[s]);
        }
    }
}
