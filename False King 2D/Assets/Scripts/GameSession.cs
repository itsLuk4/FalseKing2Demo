using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    bool isHurting;
    [SerializeField] int playerLives = 3;
    //method called awake that start before the start method
    private void Awake()
    {
        
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    IEnumerator waitForLoad()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<Player>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    //processing the players death

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGame();
        }
    }

    private void TakeLife()
    {
        playerLives--;
    }


    private void ResetGame()
    {
        FindObjectOfType<Player>().PlayDeath();
        StartCoroutine(waitForLoad());
    }

    
    
}
