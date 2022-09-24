using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnInitStartGame()
    {
        animator.SetTrigger("Start");
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
