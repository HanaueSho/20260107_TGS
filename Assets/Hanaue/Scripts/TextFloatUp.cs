using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextFloatUp : MonoBehaviour
{
    [Header("表示テキスト")]
    public string _textDisplay; // 表示テキスト
    [Header("表示数字")]
    public float _numberDisplay; // 表示スコア
    [Header("数字の前後")]
    public bool _isNumberForward = true; // 数字が前か後ろか設定フラグ
    private float _timer = 0.0f; // タイマー
    [Header("上昇時間(second)")]
    public float _floatingTime   = 1.0f; // 時間
    [Header("上昇距離(pixel)")]
    public float _floatingLength = 1.0f; // 距離
    [Header("上昇カーブ")]
    public AnimationCurve _floating =  new AnimationCurve();
    [Header("フェードカーブ")]
    public AnimationCurve _fade     =  new AnimationCurve();

    private Vector3 _positionBase = Vector3.zero;

    private TextMeshProUGUI _textMeshPro;

    void Start()
    {
        // 取得
        _textMeshPro = GetComponent<TextMeshProUGUI>();

        if (_isNumberForward)
        {
            _textMeshPro.text = _numberDisplay.ToString();
            _textMeshPro.text += _textDisplay;
        }
        else
        {
            _textMeshPro.text = _textDisplay;
            _textMeshPro.text += _numberDisplay.ToString();
        }

        // 初期位置保存
        _positionBase = transform.position;
    }

    void Update()
    {
        _timer += Time.deltaTime / _floatingTime;

        // 位置反映
        Vector3 position = transform.position;
        position.y = _positionBase.y + _floating.Evaluate(_timer) * _floatingLength;
        transform.position = position;

        // フェード
        Color color = _textMeshPro.color;
        color.a = 1 - _fade.Evaluate(_timer);
        _textMeshPro.color = color;

        if (_timer > _floatingTime)
        {
            Destroy(this.gameObject);
        }
    }
}
