using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SplashScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayStart());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(3.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("InitScene");
    }
}
