using UnityEngine;

/// <summary>スコアの種類</summary>
public enum ScoreMode
{
    Normal,
    Endless,
}
public class HighScore
{
    // enum 名から文字列を生成すると、リネーム時に保存データが消える。
    // 明示的に定数で持っておく
    private const string NormalKey = "HighScore_Normal";
    private const string EndlessKey = "HighScore_Endless";

    private static string KeyOf(ScoreMode mode) => mode switch
    {
        ScoreMode.Normal => NormalKey,
        ScoreMode.Endless => EndlessKey,
        _ => throw new System.ArgumentOutOfRangeException(nameof(mode)),
    };

    /// <summary>指定モードのハイスコア（未保存なら 0）</summary>
    public static int Get(ScoreMode mode) => PlayerPrefs.GetInt(KeyOf(mode), 0);

    /// <summary>一度でも保存されたことがあるか</summary>
    public static bool Exists(ScoreMode mode) => PlayerPrefs.HasKey(KeyOf(mode));

    /// <summary>スコアが更新されていれば保存する。</summary>
    /// <returns>更新した場合 true（NEW RECORD 演出の出し分けに使える）</returns>
    public static bool TrySubmit(ScoreMode mode, int score)
    {
        if (score <= Get(mode)) return false;

        PlayerPrefs.SetInt(KeyOf(mode), score);
        PlayerPrefs.Save();
        return true;
    }

    /// <summary>条件を無視して上書きする（デバッグ用）</summary>
    public static void Overwrite(ScoreMode mode, int score)
    {
        PlayerPrefs.SetInt(KeyOf(mode), score);
        PlayerPrefs.Save();
    }

    /// <summary>指定モードのハイスコアを削除する</summary>
    public static void Reset(ScoreMode mode)
    {
        PlayerPrefs.DeleteKey(KeyOf(mode));
        PlayerPrefs.Save();
    }

    /// <summary>全モードのハイスコアを削除する</summary>
    public static void ResetAll()
    {
        // DeleteAll ではなく個別削除。音量設定などを巻き込まない
        foreach (ScoreMode mode in System.Enum.GetValues(typeof(ScoreMode)))
        {
            PlayerPrefs.DeleteKey(KeyOf(mode));
        }
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    private const string AutoResetPrefKey = "HighScore.AutoResetOnExitPlayMode";
    private const string MenuPath = "Tools/HighScore/Play Mode 終了時にリセット";

    private static bool AutoReset
    {
        get => UnityEditor.EditorPrefs.GetBool(AutoResetPrefKey, true);
        set => UnityEditor.EditorPrefs.SetBool(AutoResetPrefKey, value);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void SubscribeEditorHook()
    {
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state != UnityEditor.PlayModeStateChange.ExitingPlayMode) return;

        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

        if (AutoReset)
        {
            ResetAll();
        }
    }

    [UnityEditor.MenuItem(MenuPath)]
    private static void ToggleAutoReset() => AutoReset = !AutoReset;

    [UnityEditor.MenuItem(MenuPath, true)]
    private static bool ToggleAutoResetValidate()
    {
        UnityEditor.Menu.SetChecked(MenuPath, AutoReset);
        return true;
    }

    [UnityEditor.MenuItem("Tools/HighScore/今すぐ全てリセット")]
    private static void ResetAllFromMenu()
    {
        ResetAll();
    }
#endif
}
