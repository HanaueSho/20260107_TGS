using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFluffy : MonoBehaviour
{
    private RectTransform _rectTransform;

    [Header("往復時間")]
    public float _time = 1.0f;
    [Header("距離（半径）")]
    public float _length = 100.0f;

    // タイマー
    private float _timer = 0.0f;

    private Vector3 _basePosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _basePosition = _rectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime * Mathf.PI * 2 / _time;

        if ( _timer > Mathf.PI * 2 )
        {
            _timer -= Mathf.PI * 2;
        }

        Vector3 position = _basePosition;
        position.y += _length * Mathf.Sin(_timer);
        _rectTransform.position = position;
    }
}
