using UnityEngine;
using System.Collections;

public class ElevatorDoorController : MonoBehaviour
{
    [Header("Door Objects")]
    public Transform leftInsideDoor;
    public Transform leftOutsideDoor;
    public Transform rightInsideDoor;
    public Transform rightOutsideDoor;

    [Header("Settings")]
    public float moveDistance = 1f;
    public float duration = 2f;

    [Header("References")]
    public Elevator elevator;         

    private bool isAnimating = false;

    public void OpenDoors()
    {
        if (!isAnimating)
            StartCoroutine(MoveDoors(opening: true));
    }

    public void CloseDoors()
    {
        if (!isAnimating)
            StartCoroutine(MoveDoors(opening: false));
    }

    private IEnumerator MoveDoors(bool opening)
    {
        isAnimating = true;
        float time = 0f;

        Vector3 liStart = leftInsideDoor.localPosition;
        Vector3 loStart = leftOutsideDoor.localPosition;
        Vector3 riStart = rightInsideDoor.localPosition;
        Vector3 roStart = rightOutsideDoor.localPosition;

        float leftDir = opening ? -moveDistance : moveDistance;
        float rightDir = opening ? moveDistance : -moveDistance;

        Vector3 liEnd = liStart + new Vector3(0, 0, leftDir);
        Vector3 loEnd = loStart + new Vector3(0, 0, leftDir);
        Vector3 riEnd = riStart + new Vector3(0, 0, rightDir);
        Vector3 roEnd = roStart + new Vector3(0, 0, rightDir);

        while (time < duration)
        {
            float t = time / duration;

            leftInsideDoor.localPosition = Vector3.Lerp(liStart, liEnd, t);
            leftOutsideDoor.localPosition = Vector3.Lerp(loStart, loEnd, t);
            rightInsideDoor.localPosition = Vector3.Lerp(riStart, riEnd, t);
            rightOutsideDoor.localPosition = Vector3.Lerp(roStart, roEnd, t);

            time += Time.deltaTime;
            yield return null;
        }

        leftInsideDoor.localPosition = liEnd;
        leftOutsideDoor.localPosition = loEnd;
        rightInsideDoor.localPosition = riEnd;
        rightOutsideDoor.localPosition = roEnd;

        isAnimating = false;

        if (!opening) // Only trigger if doors were closing
        {
            yield return new WaitForSeconds(1f);

            // Call the elevator movement coroutine
            StartCoroutine(elevator.MoveElevator(elevator.playerTransform));
        }
    }
}
