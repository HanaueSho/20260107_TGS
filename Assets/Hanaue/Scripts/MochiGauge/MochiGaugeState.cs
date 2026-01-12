using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MochiGaugeState : MonoBehaviour
{
    private Slider _slider;

    [Header("----- 手動でスライダーを設定 -----")]
    public PoundGaugeState _poundGauge;
    public KneadGaugeState _kneadGauge;

    [Header("----- 基礎値 -----")]
    public float _baseIncreaseValue = 100.0f;

    // マックススコア
    private float _maxValue = 10000.0f;
    // 現在のスコア
    private float _nowValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 増加処理
    public void IncreaseValue(float scaleCritical, float kneadBonus)
    {
        // 増加値 = ( 基礎値 + コンボ値 + こねこね値 ) * クリティカル倍率
        // コンボ値 = 100 * (1 + コンボ数 / 10)
        float comboValue = 50.0f * (1.0f + _poundGauge._comboCount / 10.0f);
        float addValue = (_baseIncreaseValue + comboValue + kneadBonus) * scaleCritical;

        _nowValue += addValue;

        // ----- ゲージへ反映 -----
        _slider.value = _nowValue / _maxValue;
    }

    // クリア判定
    public bool CheckGameClear()
    {
        return _nowValue >= _maxValue;
    }

    // ベーススコア上昇
    public void IncreaseBaseScore()
    {
        _baseIncreaseValue *= 2.0f;
    }

}
