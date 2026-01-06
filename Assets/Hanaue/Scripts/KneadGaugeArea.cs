using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KneadGaugeArea : MonoBehaviour
{
    public enum AreaType
    {
        None,
        Normal,
        Good
    }
    public AreaType _type;

    // KneadGaugeState を参照して自身の長さを補正する
    KneadGaugeState _kneadGauge;
    RectTransform _rectTransform;
    RectTransform _rectTransformPG;

    void Start()
    {
        _kneadGauge = transform.parent.GetComponent<KneadGaugeState>();
        _rectTransform = GetComponent<RectTransform>();
        _rectTransformPG = transform.parent.GetComponent<RectTransform>();

        //Vector2 width = _rectTransformPG.sizeDelta;
        //width.y = _rectTransform.sizeDelta.y;
        //_rectTransform.sizeDelta = width;
    }

    void Update()
    {
        // 位置補正
        float baseHeight = _rectTransformPG.sizeDelta.y;
        Vector3 position = Vector3.zero;
        Vector2 size = _rectTransform.sizeDelta;
        switch (_type)
        {
            case AreaType.Normal:
                size.y = (_kneadGauge._normalRange * 2.0f / 100.0f) * baseHeight; // 高さ更新
                position.y = (_kneadGauge._normalValue / 100.0f) * baseHeight - baseHeight / 2.0f; // 位置更新
                break;
            case AreaType.Good:
                size.y = (_kneadGauge._goodRange * 2.0f / 100.0f) * baseHeight; // 高さ更新
                position.y = (_kneadGauge._goodValue / 100.0f) * baseHeight - baseHeight / 2.0f; // 位置更新
                break;
        }
        // 位置サイズ更新
        _rectTransform.localPosition = position;
        _rectTransform.sizeDelta = size;
    }
}
