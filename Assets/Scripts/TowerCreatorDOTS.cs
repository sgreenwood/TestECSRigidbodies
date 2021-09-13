using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class TowerCreatorDOTS : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width = 10;
    public int length = 10;


    private Entity cubeEntity;
    private World defaultWorld;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;
    private GameObjectConversionSettings settings;

    private void Start()
    {
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        entityManager = defaultWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        settings = GameObjectConversionSettings.FromWorld(defaultWorld, blobAssetStore);

        cubeEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cubePrefab, settings);

        int cubeMax = GameController.numObjects;
        int height = cubeMax;
        int cubeCount = 0;
        for (int h = 0; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                for (int l = 0; l < length; l++)
                {
                    if (l != 0 && w != 0 && l != length - 1 && w != width - 1)
                    {
                        //do not instantiate a cube, skip to the next values
                        continue;
                    }

                    Entity entity = entityManager.Instantiate(cubeEntity);
                    Translation translation = new Translation
                    {
                        Value = new Unity.Mathematics.float3(l, h + 0.5f, w)
                    };
                    entityManager.SetComponentData(entity, translation);

                    cubeCount++;
                    if (cubeCount >= cubeMax)
                    {
                        break;
                    }
                }
                if (cubeCount >= cubeMax)
                {
                    break;
                }
            }
            if (cubeCount >= cubeMax)
            {
                break;
            }
        }

    }

    private void OnDisable()
    {
        blobAssetStore.Dispose();

      /*  using (var allEntities = entityManager.GetAllEntities())
        {
            entityManager.DestroyEntity(allEntities);
        }*/

    }


}
