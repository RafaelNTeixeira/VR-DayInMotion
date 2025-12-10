using UnityEngine;

public class WhiteboardEraser : MonoBehaviour
{
    public Texture2D sourceTexture; 
    public int eraserSize = 20;

    private Texture2D _clonedTexture;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        // Create the clone
        _clonedTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
        _clonedTexture.SetPixels(sourceTexture.GetPixels());
        _clonedTexture.Apply();

        _renderer.material.mainTexture = _clonedTexture;
    }

    public void EraseAt(Vector2 uv)
    {
        int x = (int)(uv.x * _clonedTexture.width);
        int y = (int)(uv.y * _clonedTexture.height);

        // Safety check to prevent errors if we go off the edge
        if (x < 0 || x >= _clonedTexture.width || y < 0 || y >= _clonedTexture.height)
            return;

        Color[] cleanColors = new Color[eraserSize * eraserSize];
        for (int i = 0; i < cleanColors.Length; i++)
            cleanColors[i] = Color.white; 

        // Apply pixels
        _clonedTexture.SetPixels(x - eraserSize/2, y - eraserSize/2, eraserSize, eraserSize, cleanColors);
        _clonedTexture.Apply();
    }
}