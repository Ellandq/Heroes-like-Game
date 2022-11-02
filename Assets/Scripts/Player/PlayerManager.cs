using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    private string defaultPlayerName = "Player";
    private string [] playerColours;
    public string [] playablePlayerColours;
    private short playersReady = 0;

    [Header("Player information")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] public GameObject neutralPlayer;
    [SerializeField] Player currentPlayer;
    public GameObject[] players;
    private Color currentPlayerColor;

    [Header("Player colors")]
    [SerializeField] private Color blue;
    [SerializeField] private Color lightBlue;
    [SerializeField] private Color purple;
    [SerializeField] private Color red;
    [SerializeField] private Color orange;
    [SerializeField] private Color yellow;
    [SerializeField] private Color lightGreen;
    [SerializeField] private Color green;

    [Header("Events")]
    public UnityEvent OnPlayerManagerReady;
    public UnityPlayerEvent OnNextPlayerTurn;
    public UnityEvent OnNewDayPlayerUpdate;
    public static event Action OnCurrentPlayerResourcesGained;

    public void Awake ()
    {
        playerColours = GameManager.Instance.allPlayerColours;
        playablePlayerColours = GameManager.Instance.playerColours;
        CreatePlayers(GameManager.Instance.numberOfPlayers);
        TurnManager.OnNewPlayerTurn += NextPlayerTurn;
        TurnManager.OnNewPlayerTurn += UpdateCurrentPlayer;
        
    }

    private void CreatePlayers (int howManyPlayers)
    {
        players = new GameObject[howManyPlayers];

        if (playerPrefab == null)
        {
            Debug.LogError("Error: Player Prefab on the Player Manager is not assigned");
        }

        for (int i = 0; i < howManyPlayers; i++)
        {
            players[i] = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            players[i].GetComponent<Player>();
            players[i].transform.parent = transform;
            players[i].gameObject.name = playerColours[i] + " " + defaultPlayerName;
            players[i].GetComponent<Player>().playerColor = AssignPlayerColour(playerColours[i]);
        }
    }

    public void NewDayUpdate ()
    {
        OnNewDayPlayerUpdate?.Invoke();
    }

    public void NextPlayerTurn (Player _player)
    {
        OnNextPlayerTurn?.Invoke(_player);
    }

    public void PlayerManagerReady ()
    {
        playersReady++;
        if (playersReady == players.Length){
            OnPlayerManagerReady.Invoke();
        }
    }

    private void UpdateCurrentPlayer (Player _player)
    {
        currentPlayer = _player;
    }

    public void UpdatePlayerUI (Player _player)
    {
        if (_player == currentPlayer) OnCurrentPlayerResourcesGained?.Invoke();
    }

    private Color AssignPlayerColour (string playerColour)
    {
        switch (playerColour){
            case "Blue":
                currentPlayerColor = blue;
            break;

            case "LightBlue":
                currentPlayerColor = lightBlue;
            break;

            case "Purple":
                currentPlayerColor = purple;
            break;

            case "Red":
                currentPlayerColor = red;
            break;

            case "Orange":
                currentPlayerColor = orange;
            break;

            case "Yellow":
                currentPlayerColor = yellow;
            break;

            case "LightGreen":
                currentPlayerColor = lightGreen;
            break;

            case "Green":
                currentPlayerColor = green;
            break;
        }
        return currentPlayerColor;
    }
}

[System.Serializable]
public class UnityPlayerEvent : UnityEvent<Player> { }