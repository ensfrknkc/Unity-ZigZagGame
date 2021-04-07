using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("RestartGame",0.5f);
        StartGame();
        Invoke("StartGame",1f);
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public void RestartGame()
    {
        Invoke("Restart",1f);
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
