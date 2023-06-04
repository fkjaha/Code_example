using System;
using System.Collections.Generic;
using UnityEngine;

public class AToggleable : MonoBehaviour
{
    public bool IsEnabled => _enabled;

    private readonly List<MonoBehaviour> _disableClients = new();
    private int _disableClientsCount;
    private bool _enabled;

    private protected virtual void Start()
    {
        UpdateState();
    }

    public void Disable(MonoBehaviour client)
    {
        if(_disableClients.Contains(client)) return;
        
        _disableClients.Add(client);
        _disableClientsCount++;
        UpdateState();
    }
    
    public void Enable(MonoBehaviour client)
    {
        if(!_disableClients.Contains(client)) return;

        _disableClientsCount--;
        _disableClients.Remove(client);
        UpdateState();
    }

    private void UpdateState()
    {
        _enabled = _disableClientsCount < 1;
    }
}
