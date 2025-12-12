using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D sourceTexture; 
    public int eraserSize = 40;

    private Texture2D _clonedTexture;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        // Safety Check
        if (sourceTexture == null || !sourceTexture.isReadable) {
            Debug.LogError("TEXTURE ERROR: Check Read/Write settings on " + sourceTexture?.name);
            return;
        }

        // Clone the texture
        _clonedTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        _clonedTexture.SetPixels(sourceTexture.GetPixels());
        _clonedTexture.Apply();

        _renderer.material.mainTexture = _clonedTexture;
    }

    public void DrawAt(Vector2 uv, Color color, int size)
    {
        int centerX = (int)(uv.x * _clonedTexture.width);
        int centerY = (int)(uv.y * _clonedTexture.height);
        int radius = size / 2;

        // Loop through pixels (Square brush)
        for (int x = centerX - radius; x < centerX + radius; x++)
        {
            for (int y = centerY - radius; y < centerY + radius; y++)
            {
                // Safety Check
                if (x >= 0 && x < _clonedTexture.width && y >= 0 && y < _clonedTexture.height)
                {
                    _clonedTexture.SetPixel(x, y, color);
                }
            }
        }

        _clonedTexture.Apply();
    }

    public void EraseAt(Vector2 uv)
    {
        // Convert % coordinates to Pixel coordinates
        int centerX = (int)(uv.x * _clonedTexture.width);
        int centerY = (int)(uv.y * _clonedTexture.height);
        int radius = eraserSize / 2;

        // Loop through pixels around the touch point
        for (int x = centerX - radius; x < centerX + radius; x++)
        {
            for (int y = centerY - radius; y < centerY + radius; y++)
            {
                // Strict bounds checking to prevent crashes
                if (x >= 0 && x < _clonedTexture.width && y >= 0 && y < _clonedTexture.height)
                {
                    _clonedTexture.SetPixel(x, y, Color.white);
                }
            }
        }
        
        // Apply changes
        _clonedTexture.Apply();
    }

    public float GetCleanPercentage()
    {
        if (_clonedTexture == null) return 0f;

        Color[] pixels = _clonedTexture.GetPixels();
        int whitePixels = 0;
        int totalPixels = pixels.Length;

        for (int i = 0; i < totalPixels; i++)
        {
            // Check if pixel is white (Clean)
            if (pixels[i] == Color.white)
            {
                whitePixels++;
            }
        }

        // Returns a value between 0.0 and 1.0 (e.g., 0.45 for 45%)
        return (float)whitePixels / totalPixels;
    }
}