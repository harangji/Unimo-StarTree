using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFloGetMover : MonoBehaviour
{
    private static readonly float approachRatioSTATIC = 0.8f;
    private static readonly float arriveDistSTATIC = 3f;
    private static readonly float arriveMinSpeedSTATIC = 3f;
    private static readonly float stopDistSTATIC = 1.5f;
    private static readonly float accelerationSTATIC = 9f;

    private static readonly float rotateThresholdSTATIC = 0.05f;
    private static readonly float rotateTimeSTATIC = 0.6f;
    private static readonly float flowerFindCycleSTATIC = 5f;

    private float maxSpeed = 6f;
    private float currentSpeed = 0f;
    private float harvestTime = 2f;
    private Vector3 prevTargetPos = Vector3.zero;

    private AStarStartNode astarStart;
    private FlowerController_Lobby targetFlower;

    private bool isFinding = true;
    private Coroutine autoMoveCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        astarStart = GetComponent<AStarStartNode>();
        isFinding = true;
        StartCoroutine(findFlowerCoroutine());
    }
    public void CancelAutoMove()
    {
        isFinding = false;
        if (targetFlower.TryGetComponent<AStarGoalNode>(out AStarGoalNode goal))
        {
            goal.CancelGoal();
        }

        StopCoroutine(autoMoveCoroutine);
    }
    public void RetrieveAutoMove()
    {
        isFinding = true;
    }
    private void startFindingFlower()
    {
        targetFlower = astarStart.FindGoal();
        List<Vector3> pathPoints = new List<Vector3>();
        if (targetFlower != null) { pathPoints = astarStart.FindPath(); }
        if (pathPoints.Count > 0 ) { autoMoveCoroutine = StartCoroutine(moveCoroutine(pathPoints)); }
        else { isFinding = true; }
    }
    private Vector3 calculateNextTargetPos(List<Vector3> paths, int idx, Vector3 position)
    {
        if(paths.Count == 2) 
        { 
            idx = paths.Count - 1;

            return paths[idx];
        }
        else if ( idx >= paths.Count - 1)
        {
            idx = paths.Count - 1;
            float ratio = Mathf.Clamp01(4f * Time.deltaTime);
            prevTargetPos = Vector3.Lerp(prevTargetPos, paths[idx], ratio);
            return prevTargetPos;
        }
        else
        {
            Vector3 target;
            float ratio = (position - paths[idx]).magnitude/(paths[idx-1] - paths[idx]).magnitude;
            if (ratio < approachRatioSTATIC)
            {
                float aratio = Mathf.Pow(ratio / approachRatioSTATIC, 1f);
                target = aratio * paths[idx] + (1f- aratio) * paths[idx + 1];
            }
            else
            {
                target = paths[idx];
            }
            prevTargetPos = target;
            return target;
        }
    }
    private void changerotation(Vector3 dir)
    {
        dir.y = 0f;
        if (Time.deltaTime < float.Epsilon) { return; }
        if (rotateTimeSTATIC < float.Epsilon) { transform.forward = dir; return; }
        float pow = Time.deltaTime / rotateTimeSTATIC;
        float followratio = 1 - Mathf.Clamp01(Mathf.Pow(rotateThresholdSTATIC, pow));
        Vector3 newforward = Vector3.Slerp(transform.forward, dir, followratio);
        transform.forward = newforward;
    }
    private float calculateMinSpeed(float distToGoal)
    {
        float multi = Mathf.Clamp(distToGoal / stopDistSTATIC, 1f, 1.5f);
        return multi * arriveMinSpeedSTATIC;
    }
    private IEnumerator findFlowerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.7f, 1.2f) * flowerFindCycleSTATIC);
            if (isFinding) { startFindingFlower(); }
        }
    }
    private IEnumerator moveCoroutine(List<Vector3> paths)
    {
        isFinding = false;
        int idx = 1;
        while (idx < paths.Count-1 || (transform.position - targetFlower.transform.position).magnitude > arriveDistSTATIC)
        {
            if (paths.Count == 2) { break; }
            float dt = Time.deltaTime;
            Vector3 dir = (calculateNextTargetPos(paths, idx, transform.position) - transform.position).normalized;
            currentSpeed += dt * accelerationSTATIC;
            if (currentSpeed > maxSpeed) { currentSpeed = maxSpeed; }
            transform.position += currentSpeed * dt * dir;
            if (idx < paths.Count - 1 && (transform.position - paths[idx + 1]).magnitude < (paths[idx] - paths[idx + 1]).magnitude)
            {
                idx++;
                if (idx >= paths.Count - 1) { idx = paths.Count - 1; }
            }
            changerotation(dir);
            //if (masterMover != null && masterMover.IKActions != null) { masterMover.IKActions.Invoke(); }
            yield return null;
        }
        idx = paths.Count - 1;
        float decelTime = 0f;
        while ((transform.position - paths[idx]).magnitude >= stopDistSTATIC)
        {
            float dt = Time.deltaTime;
            decelTime += dt;
            currentSpeed -= dt * accelerationSTATIC;
            currentSpeed = Mathf.Max(calculateMinSpeed((transform.position - paths[idx]).magnitude), currentSpeed);
            Vector3 dir = (calculateNextTargetPos(paths, idx, transform.position) - transform.position).normalized;
            transform.position += currentSpeed * dt * dir;
            changerotation(dir);
            //if (masterMover != null && masterMover.IKActions != null) { masterMover.IKActions.Invoke(); }
            yield return null;
        }
        StartCoroutine(harvestCoroutine());
        yield break;
    }
    private IEnumerator harvestCoroutine()
    {
        float lapse = 0f;
        targetFlower.StartHarvest();
        while (lapse <= harvestTime)
        {
            float dt = Time.deltaTime;
            currentSpeed -= 3f * dt * accelerationSTATIC;
            if (currentSpeed < 0f) { currentSpeed = 0f; }
            transform.position += currentSpeed * dt * transform.forward;
            lapse += Time.deltaTime;
            float ratio = Mathf.Clamp01(lapse/harvestTime);
            targetFlower.GrowFlower(ratio);
            Vector3 dir = (targetFlower.transform.position - transform.position).normalized;
            changerotation(dir);
            //if (masterMover != null && masterMover.IKActions != null) { masterMover.IKActions.Invoke(); }
            yield return null;
        }
        targetFlower.HarvestFlower(transform);
        lapse = 0f;
        while (lapse <= 1f)
        {
            lapse += Time.deltaTime;
            //if (masterMover != null && masterMover.IKActions != null) { masterMover.IKActions.Invoke(); }
            yield return null;
        }
        isFinding = true;
        yield break;
    }
}
