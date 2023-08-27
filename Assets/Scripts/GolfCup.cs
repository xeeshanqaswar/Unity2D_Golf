using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BeastGames
{
    public class GolfCup : MonoBehaviour
    {
        private Sequence ballAnimation;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            ballAnimation = DOTween.Sequence();
            if (collision.TryGetComponent(out Ball ball))
            {
                ball.Stop();
                ballAnimation.Join(ball.transform.DOScale(0.5f, 0.5f));
                ballAnimation.Join(ball.transform.DOMove(transform.position, 0.3f));

                ballAnimation.Play().OnComplete(() =>
                {
                    ball.CountScore(1);
                });
            }
        }


    }
}
