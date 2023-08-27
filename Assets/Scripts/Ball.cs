using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Fusion;

namespace BeastGames
{
    public class Ball : NetworkBehaviour
    {
        [SerializeField] private SpriteRenderer ballGraphics;
        [SerializeField] private LineRenderer lr;

        [Header("Attributes")]
        [SerializeField] private float maxPower = 10f;
        [SerializeField] private float power = 2f;

        private bool dragging;
        private Vector3 m_InitalPos;
        private Vector3 offset;

        public bool m_IsBallReleased = false;

        private Rigidbody2D rb;
        private Collider2D m_Collider2D;

        public override void Spawned()
        {
            m_Collider2D = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody2D>();

            if (Runner.IsClient)
            {
                LevelManager.Instance.RegisterPlayer(Object);
            }

        }

        private void OnMouseDown()
        {
            if (!Object.HasStateAuthority)
                return;
            
            if (m_IsBallReleased)
                return;

            m_InitalPos = transform.position;
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragging = true;

            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, transform.position);
        }

        private void OnMouseUp()
        {
            if (!Object.HasStateAuthority)
                return;

            if (m_IsBallReleased)
                return;

            ReleaseBall();
            m_InitalPos = Vector2.zero;
            dragging = false;

            lr.positionCount = 0;
        }

        private void OnMouseDrag()
        {
            if (!Object.HasStateAuthority)
                return;

            if (m_IsBallReleased)
                return;

            if (dragging)
            {
                Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, newPos);

                Debug.DrawLine(transform.position, newPos);
            }
        }

        private void ReleaseBall()
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            float distance = Vector3.Distance(m_InitalPos, newPos);

            if (distance < 1f)
            {
                print("Not Enough Force");
                return;
            }

            rb.velocity = Vector2.ClampMagnitude((transform.position - newPos) * power, maxPower);
            m_IsBallReleased = true;
        }

        public override void FixedUpdateNetwork()
        {
            if (!Object.HasStateAuthority)
                return;

            if (rb.velocity.magnitude < 5)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.fixedDeltaTime * 3.5f);
            }

            if (m_IsBallReleased)
            {
                if (rb.velocity == Vector2.zero)
                {
                    RPC_UpdateScores(0);
                    m_IsBallReleased = false;
                }
            }
        }

        public void CountScore(int score)
        {
            if(Object.HasStateAuthority)
            {
                RPC_UpdateScores(score);
            }
        }

        [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
        public void RPC_UpdateScores(int score)
        {
            LevelManager.Instance.UpdateScores(Object, score);
            Hide();
        }

        public void Stop()
        {
            m_IsBallReleased = false;
            rb.velocity = Vector2.zero;
        }

        public void Hide()
        {
            ToggleGraphics(false);
            transform.position = LevelManager.Instance.levels[LevelManager.Instance.currentLevel].GetStandingPoint.position;
        }

        public void Show(Vector3 pos)
        {
            transform.localScale = Vector3.one;
            if (Object.StateAuthority == Runner.LocalPlayer)
            {
                transform.position = pos;
            }

            ToggleGraphics(true);
        }
        
        private void ToggleGraphics(bool status)
        {
            m_Collider2D.enabled = status;
            ballGraphics.enabled = status;
        }

    }

}

