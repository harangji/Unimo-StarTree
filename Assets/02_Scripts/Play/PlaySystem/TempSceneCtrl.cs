using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TempSceneCtrl : MonoBehaviour
{
    public Animator SceneMask;
    public void ResetScene()
    {
        if (SceneMask != null)
        {
            SceneMask.gameObject.SetActive(true);
            StartCoroutine(CoroutineExtensions.DelayedActionCall(() => { SceneMask.SetTrigger("maskout"); }, 0.02f));
        }
        StartCoroutine(CoroutineExtensions.DelayedActionCall(
            () => { Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name); }, 2f));
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        PlaySystemRefStorage.playProcessController.GamePaused();
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        PlaySystemRefStorage.playProcessController.GameResumed();
        Time.timeScale = 1f;
    }
    public void LoadLobby()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void LoadGame(int idx)
    {
        SceneManager.LoadScene(idx);
        Time.timeScale = 1f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
