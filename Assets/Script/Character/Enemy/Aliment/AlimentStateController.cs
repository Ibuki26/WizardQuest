using UnityEngine;
using System;

[Flags]
public enum AlimentState
{
    Posion = 1 << 0,
    DefenseDowm = 2
}

public class AlimentStateController
{
    private AlimentState currentState;

    public AlimentStateController()
    {
        currentState = default;
    }

    public void AddState(AlimentState state)
    {
        currentState |= state;
    }

    public void DeleteState(AlimentState state)
    {
        currentState &= ~state;
    }

    public bool HasState(AlimentState state)
    {
        return currentState.HasFlag(state);
    }
}
