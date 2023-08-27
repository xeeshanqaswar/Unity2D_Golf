using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace BeastGames
{
    public class Level : MonoBehaviour
    {
        public SpriteRenderer GolfCupSprite;

        [SerializeField] private Transform standingPoint;

        public static Level instance;

        private LevelData m_levelData;
        private LevelManager m_levelManager;

        public Transform GetStandingPoint { get { return standingPoint; } }

        public void Init(LevelManager levelManager)
        {
            m_levelManager = levelManager;
            m_levelData = m_levelManager.levelData;
        }

        public void Setup(int player)
        {
            GolfCupSprite.sprite = m_levelData.golfCupSprites[player];
        }

    }

}
