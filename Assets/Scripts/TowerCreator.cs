using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCreator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int width = 10;
    public int length = 10;

    private void Start()
    {
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

                    Instantiate(cubePrefab, new Vector3(l, h + 0.5f, w), Quaternion.identity);
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
}
