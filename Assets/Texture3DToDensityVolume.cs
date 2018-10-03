using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;
using UnityEngine.Experimental.Rendering;

[RequireComponent(typeof(DensityVolume))]
public class Texture3DToDensityVolume : MonoBehaviour
{

    Texture3D texture;

    void Start()
    {
        // The curent density volume texture size is hard coded to be 32
        texture = CreateTexture3D(32);

        DensityVolume densityVolume = GetComponent<DensityVolume>();
        densityVolume.parameters.volumeMask = texture;
    }

    Texture3D CreateTexture3D(int size)
    {
        Color[] colorArray = new Color[size * size * size];
        texture = new Texture3D(size, size, size, TextureFormat.Alpha8, true);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    // Calculate the radius
                    float f = Mathf.Sqrt(
                        Mathf.Pow(2f * (x - 0.5f * size) / size, 2) +
                        Mathf.Pow(2f * (y - 0.5f * size) / size, 2) +
                        Mathf.Pow(2f * (z - 0.5f * size) / size, 2)
                        );
                    // Fill pixels of radius <=1 with alpha = 1
                    f = (f <= 1f) ? 1f : 0f;
                    Color c = new Color(1.0f, 1.0f, 1.0f, f);
                    colorArray[x + (y * size) + (z * size * size)] = c;
                }
            }
        }
        texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }

}