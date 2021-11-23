using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    [SerializeField] int livesAmount = 3;
    [SerializeField] int scoreAmount = 0;
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = livesAmount.ToString();
        scoreText.text = scoreAmount.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (livesAmount > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        scoreAmount += pointsToAdd;
        scoreText.text = scoreAmount.ToString();
    }

    void TakeLife()
    {
        livesAmount--;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        livesText.text = livesAmount.ToString();
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
