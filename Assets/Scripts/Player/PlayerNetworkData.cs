using UnityEngine;
using Fusion;
using Lobby;

public class PlayerNetworkData : NetworkBehaviour
{
    [Networked] public string PlayerName { get; set; }
    
    [Networked] public NetworkBool IsReady { get; set; }
    
    [Networked] public int SelectedCharacterIndex { get; set; }

    [Networked] public int KillAmount { get; set; }
    [Networked] public int DeathAmount { get; set; }
    [Networked] public NetworkBool HasCoin { get; set; }

    private GameApp _gameApp = null;
    
    public override void Spawned()
    {
        _gameApp = GameApp.Instance;

        if (_gameApp != null)
        {
            _gameApp.AddPlayerNetworkData(this);

            if (Object.HasInputAuthority)
            {
                SetPlayerName_RPC(_gameApp.playerName);
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void SetPlayerName_RPC(string value)
    {
        PlayerName = value;
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void SetIsReady_RPC(bool value)
    {
        IsReady = value;
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void SetSelectCharacterIndex_RPC(int value)
    {
        SelectedCharacterIndex = value;
    }
}