using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{

    bool isHurting;
    [SerializeField] int playerLives = 3, score = 0;

    [SerializeField] Text scoreText, livesText;

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

    private void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    IEnumerator waitForLoad()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<Player>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    //function for score calculation
    public void AddToScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    //function for health calculation
    public void AddToLife()
    {
        playerLives ++;
        livesText.text = playerLives.ToString();
    }

    //processing the players death

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
            livesText.text = playerLives.ToString();
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
