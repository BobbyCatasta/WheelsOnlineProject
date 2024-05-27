using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyUserInterface : MonoBehaviour
{
    [SerializeField] private Transform parentPlayerList;
    [SerializeField] private GameObject playerTextPrefab;

    [SerializeField] private TextMeshProUGUI lobbycodeText;

    private void OnEnable()
    {
        LobbyEventsManager.UpdateLobbyInfo += OnUpdateLobbyInfos;
    }

    private void OnDisable()
    {
        LobbyEventsManager.UpdateLobbyInfo -= OnUpdateLobbyInfos;
    }

    private void OnUpdatedPlayerNameList(List<Player> playerList)
    {
        foreach (Transform child in parentPlayerList.transform)
            Destroy(child.gameObject);
        foreach(Player player in playerList)
        {
            GameObject textPrefabInstantiated = Instantiate(playerTextPrefab, parentPlayerList);
            textPrefabInstantiated.GetComponent<TextMeshProUGUI>().text = player.Id;
        }
    }

    private void OnUpdateLobbyInfos(Lobby lobby)
    {
        lobbycodeText.text = $"Lobby Code : {lobby.LobbyCode}";
        OnUpdatedPlayerNameList(lobby.Players);
    }
}
