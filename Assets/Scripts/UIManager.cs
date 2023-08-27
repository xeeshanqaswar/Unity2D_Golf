using BeastGames;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UIinstance;

    public TextMeshProUGUI scoreP1, scoreP2;
    public GameObject loadingScreen, winScreen, loseScreen,drawScreen;

    private NetworkRunner Runner;

    private void Awake()
    {
        UIinstance = this;
        Runner = NetworkRunner.Instances[0];
    }

    private void Start()
    {
        loadingScreen.SetActive(true);
    }

    public void UpdateScore(int score1, int score2)
    {
        scoreP1.text = score1.ToString();
        scoreP2.text = score2.ToString();
    }

    public void GameComplete()
    {
        print("Game Complete Called");
        int myScore = 0, opponentScore = 0;
        foreach (var p in LevelManager.Instance.PlayerToScore)
        {
            if (p.Key.InputAuthority == Runner.LocalPlayer)
            {
                myScore = p.Value;
            }
            else
            {
                opponentScore = p.Value;
            }
        }
        if (myScore == opponentScore)
        {
            Draw();
        }
        else if (myScore > opponentScore)
        {
            Win();
        }
        else
        {
            Lose();
        }

    }

    public void LoadingComplete()
    {
        loadingScreen.SetActive(false);
    }

    public void Win()
    {
        winScreen.SetActive(true);
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
    }

    public void Draw()
    {
        drawScreen.SetActive(true);
    }

}
