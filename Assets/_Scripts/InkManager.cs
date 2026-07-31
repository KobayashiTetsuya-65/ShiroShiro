using UnityEngine;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance { get; private set; }
    [Header("-----参照-----")]
    [SerializeField] private GameObject _prefab;
    [SerializeField]private Transform _parent;

    [Header("-----パラメータ調整-----")]
    [SerializeField] private Color[] _colors;
    [SerializeField] private Sprite[] _sprites;
    private void Awake()
    {
        Instance = this;
    }
    public void DisplayInk(Vector3 pos)
    {
        GameObject ink = Instantiate(_prefab, pos,
            Quaternion.Euler(0, 0, Random.Range(0f, 360f)),_parent);
        SpriteRenderer sr = ink.GetComponent<SpriteRenderer>();
        sr.sprite = _sprites[Random.Range(0, _sprites.Length)];
        sr.color = _colors[Random.Range(0, _colors.Length)];
    }
}
