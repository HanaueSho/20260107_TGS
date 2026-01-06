using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoundGaugeArea : MonoBehaviour
{
    public enum AreaType
    {
        None,
        Combo,
        Critical
    }
    public AreaType _type;

    // PoundGaugeState を参照して自身の長さを補正する
    PoundGaugeState _poundGauge;
    RectTransform _rectTransform;
    RectTransform _rectTransformPG;

    void Start()
    {
        _poundGauge = transform.parent.GetComponent<PoundGaugeState>();
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
            case AreaType.Combo:
                size.y = (_poundGauge._comboRange * 2.0f / 100.0f) * baseHeight; // 高さ更新
                position.y = (_poundGauge._comboValue / 100.0f) * baseHeight - baseHeight / 2.0f; // 位置更新
                break;
            case AreaType.Critical:
                size.y = (_poundGauge._criticalRange * 2.0f / 100.0f) * baseHeight; // 高さ更新
                position.y = (_poundGauge._criticalValue / 100.0f) * baseHeight - baseHeight / 2.0f; // 位置更新
                break;
        }
        // 位置サイズ更新
        _rectTransform.localPosition = position;
        _rectTransform.sizeDelta = size;
    }
}
