using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test_Animation : MonoBehaviour
{
    Animator animator;
    public Button IDLEButton, MOVEButton, HITButton, StartAnimButton, LobbyButton;

    public Renderer renderer;
    public ParticleSystem[] particles;
    private void Start()
    {
        animator = GetComponent<Animator>();

        IDLEButton.onClick.AddListener(() => GetAnimation("IDLE"));
        MOVEButton.onClick.AddListener(() => GetAnimation("MOVE"));
        HITButton.onClick.AddListener(() => GetAnimation("HIT"));
        StartAnimButton.onClick.AddListener(() => GetAnimation("START"));
        LobbyButton.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene("GR_Lobby_20240201"));
    }
    private void GetAnimation(string temp)
    {

        switch (temp)
        {
            case "IDLE":
                for (int i = 0; i < particles.Length; i++) particles[i].Stop();
                renderer.material.SetFloat("_facial", 4.0f);
                Invoke("FaceReturn", 1.0f);
                animator.SetTrigger("IDLE");
                break;
            case "MOVE":
                renderer.material.SetFloat("_facial", 5.0f);
                Invoke("FaceReturn", 1.0f);
                Invoke("ParticleReturn", 0.7f);
                animator.SetTrigger("MOVE");
                break;
            case "HIT":
                renderer.material.SetFloat("_facial", 8.0f);
                Invoke("FaceReturn", 1.0f);
                animator.SetTrigger("HIT"); break;
            case "START":
                renderer.material.SetFloat("_facial", 8.0f);
                for (int i = 0; i < particles.Length; i++) particles[i].Stop();
                Invoke("FaceReturn", 1.0f);
                animator.SetTrigger("START"); break;
        }
    }

    private void FaceReturn() => renderer.material.SetFloat("_facial", 0.0f);
    private void ParticleReturn()
    {
        for (int i = 0; i < particles.Length; i++) particles[i].Play();
    }
}
