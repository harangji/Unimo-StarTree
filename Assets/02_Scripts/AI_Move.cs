using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI_Move : MonoBehaviour
{
    NavMeshAgent agent;
    public string Name;
    private static readonly float accelerationSTATIC = 9f;

    private static readonly float flowerFindCycleSTATIC = 5f;

    private float currentSpeed = 0f;
    private float harvestTime = 2f;

    private FlowerController_Lobby targetFlower;

    public bool isFinding = true;
    private Coroutine autoMoveCoroutine;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        isFinding = true;
        StartCoroutine(findFlowerCoroutine());
        agent.speed = 2.5f;
    }
    private void startFindingFlower()
    {
        if(Pinous_Flower_Holder.FlowerHolder.Count <= 0)
        {
            isFinding = false;
            autoMoveCoroutine = StartCoroutine(moveCoroutine(transform.position + Random.insideUnitSphere * 2.0f));

            return;
        }
        else
        {
            isFinding = false;
            targetFlower = Pinous_Flower_Holder.FlowerHolder[Random.Range(0, Pinous_Flower_Holder.FlowerHolder.Count)].GetComponent<FlowerController_Lobby>();
            Pinous_Flower_Holder.FlowerHolder.Remove(targetFlower.transform);
            if (targetFlower == null)
            {
                isFinding = true;
                return;
            }
            autoMoveCoroutine = StartCoroutine(moveCoroutine(targetFlower.transform.position));
        }
    }
    IEnumerator moveCoroutine(Vector3 pos)
    {
        
        if (agent == null || !agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            Debug.LogWarning($"[{Name}] NavMeshAgent 비활성 상태이거나 NavMesh에 없음. 이동 불가.");
            yield break;
        }
        
        agent.isStopped = false;
        agent.SetDestination(pos);
        Vector3 velocity = Vector3.zero;
        yield return new WaitForSeconds(0.05f);
        
        while (agent.isActiveAndEnabled && agent.isOnNavMesh && agent.remainingDistance > 1.5f)
        {
            transform.position = Vector3.SmoothDamp(transform.position, agent.nextPosition, ref velocity, 0.1f);
            yield return null;
        }
        
        agent.isStopped = true;
        StartCoroutine(harvestCoroutine());
    }
    private IEnumerator findFlowerCoroutine()
    {
        while (isFinding)
        {
            yield return new WaitForSeconds(Random.Range(0.7f, 1.2f));
            if (isFinding)
            {
                startFindingFlower();
            }
            else break;
        }
    }

    private IEnumerator harvestCoroutine()
    {
        if(targetFlower == null)
        {
            isFinding = true;
            StartCoroutine(findFlowerCoroutine());
            yield break;
        }
        float lapse = 0f;
        targetFlower.StartHarvest();
        while (lapse <= harvestTime)
        {
            if(targetFlower == null)
            {
                isFinding = true;
                StartCoroutine(findFlowerCoroutine());
                yield break;
            }
            lapse += Time.deltaTime;
            float ratio = Mathf.Clamp01(lapse / harvestTime);
            targetFlower.GrowFlower(ratio);
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

        StartCoroutine(findFlowerCoroutine());
    }


}
