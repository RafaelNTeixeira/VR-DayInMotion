using UnityEngine;

public class EraserTip : MonoBehaviour
{
    public Transform eraserTip;
    
    void OnCollisionStay(Collision collision)
    {
        // Check if we are touching the whiteboard
        WhiteboardEraser board = collision.gameObject.GetComponent<WhiteboardEraser>();

        if (board != null)
        {
            Debug.Log("Eraser tip is touching the whiteboard.");
            // Raycast from the tip towards the board to find WHERE we touched
            RaycastHit hit;
            if (Physics.Raycast(eraserTip.position, eraserTip.forward, out hit, 0.1f))
            {
                // If we hit, send the UV coordinates to the board to be erased
                Debug.Log("Erasing at UV: " + hit.textureCoord);
                board.EraseAt(hit.textureCoord);
            }
        }
    }
}