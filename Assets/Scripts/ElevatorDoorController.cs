using UnityEngine;
using System.Collections;

public class ElevatorDoorController : MonoBehaviour
{
    [Header("Door Objects")]
    public Transform leftInsideDoor;
    public Transform leftOutsideDoor;
    public Transform rightInsideDoor;
    public Transform rightOutsideDoor;

    [Header("Door Settings")]
    public float moveDistance = 1f;
    public float doorDuration = 2f;

    [Header("Elevator")]
    public Elevator elevator;

    [Header("Timing")]
    public float waitBeforeElevator = 1f;

    private bool isBusy = false;
    private bool isOpen = true;   // doors start OPEN

    // 🔘 CALL FROM OUTSIDE BUTTON
    public void CloseDoors()
    {
        if (isBusy || !isOpen) return;
        StartCoroutine(MoveDoorsRoutine(open: false));
    }

    // 🔘 CALL FROM INSIDE BUTTON
    public void OpenDoors()
    {
        if (isBusy || isOpen) return;
        StartCoroutine(MoveDoorsRoutine(open: true));
    }

    private IEnumerator MoveDoorsRoutine(bool open)
    {
        isBusy = true;

        float time = 0f;

        Vector3 liStart = leftInsideDoor.localPosition;
        Vector3 loStart = leftOutsideDoor.localPosition;
        Vector3 riStart = rightInsideDoor.localPosition;
        Vector3 roStart = rightOutsideDoor.localPosition;

        float leftDir = open ? -moveDistance : moveDistance;
        float rightDir = open ? moveDistance : -moveDistance;

        Vector3 liEnd = liStart + new Vector3(0, 0, leftDir);
        Vector3 loEnd = loStart + new Vector3(0, 0, leftDir);
        Vector3 riEnd = riStart + new Vector3(0, 0, rightDir);
        Vector3 roEnd = roStart + new Vector3(0, 0, rightDir);

        while (time < doorDuration)
        {
            float t = time / doorDuration;

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

        isOpen = open;
        isBusy = false;

        // If doors just closed → move elevator
        if (!open && elevator != null)
        {
            yield return new WaitForSeconds(waitBeforeElevator);
            StartCoroutine(elevator.MoveElevator(elevator.playerTransform));
        }
    }
}
