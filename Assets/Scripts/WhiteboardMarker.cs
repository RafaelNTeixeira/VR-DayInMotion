using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [Header("Pen Settings")]
    public Color penColor = Color.black;
    public int penSize = 10; 

    private Whiteboard _lastTouchedBoard;

    void OnCollisionStay(Collision collision)
    {
        // Check if we hit the whiteboard script
        Whiteboard board = collision.gameObject.GetComponent<Whiteboard>();
        if (board == null) return;

        _lastTouchedBoard = board;

        // Loop through touch points
        foreach (ContactPoint contact in collision.contacts)
        {
            // Create a Ray starting 5cm away from wall, pointing IN
            Ray ray = new Ray(contact.point + (contact.normal * 0.05f), -contact.normal);
            RaycastHit hit;

            // Raycast ONLY against the board collider we touched
            if (collision.collider.Raycast(ray, out hit, 0.2f))
            {
                // Call the new "Draw" function (we need to add this to the board script!)
                board.DrawAt(hit.textureCoord, penColor, penSize);
            }
        }
    }

    // Reset when lifting pen
    void OnCollisionExit(Collision collision)
    {
        if (_lastTouchedBoard != null && collision.gameObject == _lastTouchedBoard.gameObject)
        {
            _lastTouchedBoard = null;
        }
    }
}