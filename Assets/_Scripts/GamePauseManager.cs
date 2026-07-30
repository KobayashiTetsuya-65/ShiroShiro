using System;
using UnityEngine;

public class GamePauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    public static event Action<bool> OnPauseChanged;

    public static void SetPaused(bool paused)
    {
        if (IsPaused == paused) return;
        IsPaused = paused;
        OnPauseChanged?.Invoke(paused);
    }

    public void PauseGame()
    {
        SetPaused(true);
    }

    public void ResumeGame()
    {
        SetPaused(false);
    }
}