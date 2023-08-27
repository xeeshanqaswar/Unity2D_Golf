using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Fusion;

public class LobbyManager : MonoBehaviour
{
    public NetworkRunner runnerPrefab;
    private NetworkRunner _runner;

    public void PlayGame()
    {
        Application.targetFrameRate = 60;
        StartGame();
    }

    async void StartGame()
    {
        _runner = Instantiate(runnerPrefab);
        _runner.ProvideInput = true;

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "TestRoom",
            PlayerCount = 2,
            Scene = SceneManager.GetActiveScene().buildIndex + 1,
            SceneManager = _runner.gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }

}
