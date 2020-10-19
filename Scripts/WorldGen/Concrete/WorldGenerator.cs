using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    Renderer quadRenderer;

    public int width;
    public int height;

    public int noiseOffsetX;
    public int noiseOffsetY;

    public int noiseScale;

    [Range(0.0f, 1.0f)]
    public float magmaThreshold;
    [Range(0.0f, 1.0f)]
    public float stoneThreshold;
    [Range(0.0f, 1.0f)]
    public float mineralThreshold;
    [Range(0.0f, 1.0f)]
    public float dirtThreshold;
    
    void Awake()
    {
        quadRenderer = GetComponent<Renderer>();
        MakeSomeNoise();
    }

    void OnValidate()
    {
        if (Application.isPlaying)
        {
            quadRenderer.material.mainTexture = GenerateTexture();
        }
    }

    public void MakeSomeNoise()
    {
        quadRenderer.material.mainTexture = GenerateTexture();
    }
    
    public Texture GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int currentPosX = 0; currentPosX < width; currentPosX++)
        {
            for (int currentPosY = 0; currentPosY < height; currentPosY++)
            {
                Color color = GenerateNoiseColor(currentPosX, currentPosY);
                texture.SetPixel(currentPosX, currentPosY, color);
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }

    private Color GenerateNoiseColor(int x, int y)
    {
        float sample = Mathf.PerlinNoise((float)x/width * noiseScale + noiseOffsetX, (float)y /height * noiseScale + noiseOffsetY);
        if (sample >= magmaThreshold)
        {
            return new Color(255,0,0);
        }
        else if (sample >= stoneThreshold)
        {
            return Color.gray;
        }
        else if (sample >= mineralThreshold)
        {
            return new Color(0,255,0);
        }
        else if(sample >= dirtThreshold)
        {
            return new Color(0,0,0);
        }
        else
        {
            return new Color(255, 255, 255);
        }

        
    }
}
