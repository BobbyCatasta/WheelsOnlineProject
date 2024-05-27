using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public static class LobbyEventsManager 
{
    public static Action<Lobby> UpdateLobbyInfo;
}
