using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pattern1;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("패턴 1 실행");
            Instantiate(pattern1,  transform.position, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("패턴 2 실행");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("패턴 3 실행");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("패턴 4 실행");
        }
    }
}
