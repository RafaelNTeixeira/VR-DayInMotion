using UnityEngine;

public class Eraser : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        // 1. Check if we hit the whiteboard script
        WhiteboardEraser board = collision.gameObject.GetComponent<WhiteboardEraser>();
        if (board == null) return;

        // 2. Loop through touch points
        foreach (ContactPoint contact in collision.contacts)
        {
            // Create a Ray that starts 10cm away from the wall, pointing IN.
            // We use the contact point + normal to find a safe starting spot.
            Ray ray = new Ray(contact.point + (contact.normal * 0.1f), -contact.normal);

            RaycastHit hit;

            // 3. THE MAGIC FIX: "collision.collider.Raycast"
            // This ignores the Eraser, the Player, and the World.
            // It ONLY checks the specific collider we just touched (The Board).
            if (collision.collider.Raycast(ray, out hit, 1.0f))
            {
                board.EraseAt(hit.textureCoord);
            }
        }
    }
}