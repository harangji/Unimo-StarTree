using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Test_PlayerMover : MonoBehaviour
{
    private Transform playerTransform;
    private float moveSpeed = 7f;
    private Vector3 destination;
    private Coroutine moveCoroutine;
    private bool ismoving;

    private float maxInertiaSpeed = 7.5f;
    private Vector3 inertiaMoveVelocity;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (inertiaMoveVelocity.magnitude > 0.001f)
        {
            playerTransform.position += Time.deltaTime * inertiaMoveVelocity;
            inertiaMoveVelocity -= 0.7f * Time.deltaTime * inertiaMoveVelocity;
        }
    }
    public void MoveByDirection(Vector2 direction)
    {
        if (ismoving)
        {
            StopCoroutine(moveCoroutine);
            ismoving = false;
        }
        Vector3 dir = new Vector3(direction.x, 0, direction.y);
        playerTransform.position += moveSpeed * Time.deltaTime * dir;
    }
    public void MoveByPosition(Vector3 dest)
    {
        destination = dest;
        if (ismoving) 
        { 
            StopCoroutine(moveCoroutine);
            ismoving = false;
        }
        moveCoroutine = StartCoroutine(movedestCoroutine());
    }
    public void AddForceByPosition(Vector3 dest)
    {
        if (ismoving)
        {
            StopCoroutine(moveCoroutine);
            ismoving = false;
        }
        Vector3 dir = dest - playerTransform.position;
        Vector3 velDiffperTouch = 0.4f * maxInertiaSpeed * (1f-Mathf.Exp(-dir.magnitude/1f)) * dir.normalized;
        inertiaMoveVelocity += velDiffperTouch;
        if (inertiaMoveVelocity.magnitude > maxInertiaSpeed)
        {
            inertiaMoveVelocity *= maxInertiaSpeed / inertiaMoveVelocity.magnitude;
        }
    }
    private IEnumerator movedestCoroutine()
    {
        float dist = (playerTransform.position - destination).magnitude;
        ismoving = true;
        while (dist > 0.001f)
        {
            Vector3 dir = (destination - playerTransform.position).normalized;
            playerTransform.position += moveSpeed * Time.deltaTime * dir;
            yield return null;
        }
        ismoving = false;
        yield break;
    }
}
