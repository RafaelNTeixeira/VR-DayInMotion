using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [Header("Setup")]
    public Transform tipTransform; 
    
    [Header("Settings")]
    public Color penColor = Color.black;
    public int penSize = 10;

    void OnCollisionStay(Collision collision)
    {
        Whiteboard board = collision.gameObject.GetComponent<Whiteboard>();
        if (board == null) return;
        
        // Start 2cm "behind" the tip
        Vector3 startPos = tipTransform.position - (tipTransform.up * 0.02f);
        
        // Shoot the ray in the direction of the tip (marker y axis)
        Ray ray = new Ray(startPos, tipTransform.up);

        RaycastHit hit;

        if (collision.collider.Raycast(ray, out hit, 0.2f))
        {
            board.DrawAt(hit.textureCoord, penColor, penSize);
        }
    }
}