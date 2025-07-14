// using System;
// using UnityEngine;
//
// public class Gimmick_BlackHole : Gimmick
// {
//     [Header("중력 세기")]
//     public float gravityStrength = 10f;
//     
//     [Header("검사 대상 레이어")]
//     public LayerMask playerLayer;
//     
//     [Header("OBB 범위 설정")]
//     public Vector3 boxSize = new Vector3(5f, 0f, 5f);  // 블랙홀 가로세로 깊이
//     
//     private Rigidbody target; // 감지한 플레이어 물리
//     private float timeElapsed; // 시작 시간
//     
//     private void Start()
//     {
//         timeElapsed = 0f;
//     }
//
//     private void Update()
//     {
//         timeElapsed += Time.deltaTime;
//
//         // OBB 충돌 검사 (박스 중심, 크기/2, 회전, 레이어)
//         Collider[] hits = Physics.OverlapBox(
//             transform.position,                  // 중심
//             boxSize * 0.5f,                      // 반 크기 (halfExtents)
//             transform.rotation,                 // 회전 적용
//             playerLayer                         // 대상 레이어
//         );
//
//         target = null;
//         foreach (var hit in hits)
//         {
//             if (hit.attachedRigidbody != null)
//             {
//                 target = hit.attachedRigidbody;
//                 break;
//             }
//         }
//
//         if (target != null)
//         {
//             Vector3 direction = (transform.position - target.position).normalized;
//             target.AddForce(direction * gravityStrength, ForceMode.Acceleration);
//         }
//
//         if (timeElapsed >= GimmickDuration)
//         {
//             Destroy(gameObject);
//         }
//     }
//
//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.green;
//         Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
//         Gizmos.DrawWireCube(Vector3.zero, boxSize);
//     }
// }
