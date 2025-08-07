using System.Collections;
using UnityEngine;

public class Boss1State_Pattern_3 : BossState_Pattern
{
    private static readonly int PATTERN3 = Animator.StringToHash("Pattern3");
    [SerializeField] private GameObject indicator;

    private const float DASH_SPEED = 24f;
    private const float ARENA_RADIUS = 16f;
    private const float INDICATOR_ALPHA = 20f / 255f;
    private const float INDICATOR_DURATION = 1.5f;
    private const float DASH_DELAY = 3f;

    private bool canDash;
    private bool hasDashed; 

    public override void TransitionAction(MonsterController controller)
    {
        base.TransitionAction(controller);
        Debug.Log("[Boss Pattern] 패턴 3");

        canDash = false;
        hasDashed = false;
        
        StartCoroutine(AnimateIndicator());
        StartCoroutine(EnableDashAfterDelay());
    }

    public override void UpdateAction()
    {
        base.UpdateAction();

        if (!canDash)
        {
            RotateMonster();
            return;
        }

        if (!hasDashed)
        {
            controller.enemyAnimator.SetBool(PATTERN3, true);
            ((BossController)controller).isPattern3 = true;
            hasDashed = true; // 딱 한 번만 실행됨
        }

        controller.transform.position += controller.transform.forward * (DASH_SPEED * Time.deltaTime);

        if (IsOutsideArena(controller.transform.position))
        {
            canDash = false;
            Debug.Log("[Boss Pattern] 원 밖으로 나가 돌진 멈춤");
            StartCoroutine(AfterDashRoutine());
        }
    }

    private IEnumerator AnimateIndicator()
    {
        float elapsed = 0f;

        indicator.SetActive(true);

        var spriteRenderer = indicator.GetComponent<SpriteRenderer>();
        Vector3 originalScale = indicator.transform.localScale;
        float startX = 0f;
        float endX = 6f;

        Color startColor = new Color(1f, 1f, 1f, INDICATOR_ALPHA);
        Color endColor = new Color(1f, 0f, 0f, INDICATOR_ALPHA);

        // 초기 상태 설정
        indicator.transform.localScale = new Vector3(startX, originalScale.y, originalScale.z);
        spriteRenderer.color = startColor;

        while (elapsed < INDICATOR_DURATION)
        {
            float t = elapsed / INDICATOR_DURATION;

            // X만 보간, YZ는 고정
            float newX = Mathf.Lerp(startX, endX, t);
            indicator.transform.localScale = new Vector3(newX, originalScale.y, originalScale.z);

            spriteRenderer.color = Color.Lerp(startColor, endColor, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 보정
        indicator.transform.localScale = new Vector3(endX, originalScale.y, originalScale.z);
        spriteRenderer.color = endColor;
    }


    private IEnumerator EnableDashAfterDelay()
    {
        yield return new WaitForSeconds(DASH_DELAY);
        canDash = true;
    }

    private IEnumerator AfterDashRoutine()
    {
        indicator.SetActive(false);
        controller.enemyAnimator.SetBool(PATTERN3, false);
        ((BossController)controller).isPattern3 = false;
        yield return new WaitForSeconds(1f);
        controller.EnemyPreaction();
    }

    private bool IsOutsideArena(Vector3 position)
    {
        Vector2 flatPos = new Vector2(position.x, position.z);
        return flatPos.magnitude > ARENA_RADIUS;
    }
}
