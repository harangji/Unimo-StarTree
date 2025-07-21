using UnityEngine;

public class PatternSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pattern1;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("���� 1 ����");
            Instantiate(pattern1,  transform.position, Quaternion.identity);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("���� 2 ����");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("���� 3 ����");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("���� 4 ����");
        }
    }
}
