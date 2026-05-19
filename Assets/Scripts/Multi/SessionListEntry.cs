using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListEntry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI roomName, playerCount;

    public Button joinButton;

    public void JoinRoom()
    {
        NetworkManager.runnerInstance.StartGame(new StartGameArgs()
        {
            SessionName = roomName.text,
            GameMode = GameMode.Shared,
        });
    }
}
