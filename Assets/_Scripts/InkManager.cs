using System.Collections.Generic;
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
    [SerializeField] private float _minScaleMag = 0.8f;
    [SerializeField] private float _maxScaleMag = 1.5f;
    [SerializeField] private float _scatter = 1.5f;
    private void Awake()
    {
        Instance = this;
    }
    public void DisplayInk(Vector3 pos)
    {
        int amount = Random.Range(3,5);
        List<Color> colors = new List<Color>(_colors);

        for(int i  = 0; i < amount; i++)
        {
            //座標計算
            float x = Random.Range(-_scatter, _scatter);
            float y = Random.Range(-_scatter, _scatter);
            Vector3 spawnPos = pos + new Vector3(x, y, 1f);

            GameObject ink = Instantiate(_prefab, spawnPos,
                Quaternion.Euler(0, 0, Random.Range(0f, 360f)), _parent);
            ink.transform.localScale *= Random.Range(_minScaleMag, _maxScaleMag);

            SpriteRenderer sr = ink.GetComponent<SpriteRenderer>();
            sr.sprite = _sprites[Random.Range(0, _sprites.Length)];

            //色計算
            int index = Random.Range(0, colors.Count);
            sr.color = colors[index];
            colors.RemoveAt(index);
        }
    }
}
