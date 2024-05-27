using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class WheelsLobbyService : MonoBehaviour
{
    [SerializeField] private float heartbeatTime;
    [SerializeField] private float lobbyInfoUpdateTime;

    public Lobby currentLobby;

    private void Awake()
    {
        ConnectionSetup();
    }

    private void OnDestroy()
    {
        RemoveFromLobby();
    }
    private async void ConnectionSetup()
    {
        try
        {
            await UnityServices.InitializeAsync();
            print("SDK Initialized");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Anonymously Sign-In was successful!");
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }
    }

    public async void CreateLobby()
    {
        try
        {
            //Metodi per gettarsi le informazioni da UI
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync("Random Name", 3);
            currentLobby = lobby;
            print($"{currentLobby.Name} Created!");
            StartCoroutine(LobbyHeartbeat());
            StartCoroutine(UpdateLobbyInfoCoroutine());
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void JoinLobby(TMP_InputField inputField)
    {
        try
        {
            Lobby joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(inputField.text);
            currentLobby = joinedLobby;
            print($"Joined lobby {joinedLobby.Name}");
            StartCoroutine(UpdateLobbyInfoCoroutine());
        }
        catch (LobbyServiceException ex)
        {
            print(ex.Message);
        }
    }

    private async void RemoveFromLobby()
    {
        await LobbyService.Instance.RemovePlayerAsync(currentLobby.Id, AuthenticationService.Instance.PlayerId);
    }

    private IEnumerator LobbyHeartbeat()
    {
        while (currentLobby != null)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(currentLobby.Id);
            yield return new WaitForSeconds(heartbeatTime);
        }
    }
    private IEnumerator UpdateLobbyInfoCoroutine() 
    {
        while(currentLobby != null)
        {
            UpdateLobbyInfo();
            yield return new WaitForSeconds(lobbyInfoUpdateTime);
        }
    }

    private async void UpdateLobbyInfo()
    {
        Lobby updatedLobby = await LobbyService.Instance.GetLobbyAsync(currentLobby.Id);
        currentLobby = updatedLobby;
        LobbyEventsManager.UpdateLobbyInfo?.Invoke(updatedLobby);
    }


}
