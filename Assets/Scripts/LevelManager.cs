using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using Fusion;

namespace BeastGames
{

    public class LevelManager : MonoBehaviour
    {
        public LevelData levelData;
        public List<Level> levels;
        public Dictionary<NetworkObject, int> PlayerToScore = new Dictionary<NetworkObject, int>();

        public int currentLevel;
        public int currentPlayer = 0;

        public static LevelManager Instance;
        private NetworkRunner Runner;

        private void Awake()
        {
            Instance = this; 
            
            foreach (var level in levels)
                level.Init(this);
        }

        private void Start()
        {
            Runner = NetworkRunner.Instances[0];
        }

        [ContextMenu("Print Playes")]
        public void DebugPlayers()
        {
            foreach (var plr in PlayerToScore)
                print($"Player:{plr.Key.gameObject.name} Score:{plr.Value}");
        }

        public void RegisterPlayer(NetworkObject obj)
        {
            PlayerToScore.Add(obj, 0);
            obj.gameObject.gameObject.name = $"Player {(int)obj.InputAuthority}";
            obj.GetComponent<Ball>().Hide();
        }

        public void InititateLevel()
        {
            for (int i = 0; i < levels.Count; i++)
                levels[i].gameObject.SetActive(i == currentLevel);

            foreach (var p in PlayerToScore)
            {
                if (p.Key.InputAuthority == currentPlayer)
                {
                    if (p.Key.TryGetComponent<Ball>(out Ball ball))
                    {
                        ball.Hide();
                        ball.Show(levels[currentLevel].GetStandingPoint.position);
                    }
                }
            }

            levels[currentLevel].Setup(currentPlayer);
        }

        public void UpdateScores(NetworkObject obj, int score)
        {
            if (!PlayerToScore.ContainsKey(obj) )
                return;

            // Update Score for Registered Player
            int currentScore = PlayerToScore[obj];
            currentScore += score;
            PlayerToScore[obj] = currentScore;

            LevelChangeCheckUp();

            currentPlayer = (currentPlayer + 1) % PlayerToScore.Count;

            InititateLevel();
        }

        private void LevelChangeCheckUp()
        {
            if (currentPlayer == PlayerToScore.Count-1)
            {
                if (currentLevel >= levels.Count - 1)
                {
                    UIManager.UIinstance.GameComplete();
                    return;
                }

                currentLevel++;
            }
        }

        

    }

}
