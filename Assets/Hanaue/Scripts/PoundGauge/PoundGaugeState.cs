using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoundGaugeState : MonoBehaviour
{
    // 状態管理
    public enum State
    {
        None,
        NoPlay,  // 操作不可
        Wait,    // 待機状態
        Pound,   // つき状態
        Stanned, // スタン状態
    }
    public State _state;

    // 増加処理
    // 初期化処理
    // クリックされた処理

    private Slider _slider;

    [Header("----- 手動で設定 -----")]
    public PlayerChangeSprite _playerChangeSprite;

    [Header("----- 手動でスライダーを設定 -----")]
    public KneadGaugeState _kneadGauge;
    public MochiGaugeState _mochiGauge;

    [Header("----- 手動でプレファブを設定 -----")]
    public GameObject _prefabTextCombo;
    public GameObject _prefabEffecStanned;

    [Header("----- コンボ数 -----")]
    public int _comboCount = 0;

    // ----- バリューについて -----
    private float _maxValue = 100.0f;
    [Header("----- 現在の値（0~100） -----")]
    public float _nowValue = 0.0f;

    [Header("----- コンボ判定区域 -----")]
    public float _comboValue = 70.0f; // コンボ判定中央値
    public float _comboRange = 20.0f;

    [Header("----- クリティカル判定区域 -----")]
    public float _criticalValue = 70.0f; // クリティカル判定中央値
    public float _criticalRange = 10.0f;

    [Header("----- スタン時間 -----")]
    public float  _timeStanned = 0.5f;
    private float _timerStanned = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();

        // ゲージ反映
        _slider.value = _nowValue / _maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.None:
                break;
            case State.NoPlay:
                break;
            case State.Wait:
                // ----- ゲージ増加処理 -----
                IncreaseGaugeValue();

                // ----- 餅つき処理 -----
                Pounding();
                break;
            case State.Pound:
                // アニメーション処理
                _playerChangeSprite.ChangeSprite(0, 0.2f);
                _state = State.Wait;
                break;
            case State.Stanned:
                _timerStanned += Time.deltaTime;
                if (_timerStanned > _timeStanned)
                {
                    _playerChangeSprite.ChangeSprite(0, 0.0f);
                    _state = State.Wait;
                    _timerStanned = 0.0f;
                }
                break;
        }
    }

    // --------------------------------------------------
    // プレイ中の処理
    // --------------------------------------------------
    private void IncreaseGaugeValue()
    {
        // 増加処理
        _nowValue += 30.0f * Time.deltaTime;
        // ゲージ反映処理
        _slider.value = _nowValue / _maxValue;
        if (_nowValue >= _maxValue)
        {
            _nowValue = 0.0f;
        }
    }

    private void Pounding()
    {
        // 左クリック処理
        if (Input.GetMouseButtonDown(0))
        {
            // ----- アニメーション再生処理 -----
            _playerChangeSprite.ChangeSprite(1);

            // ----- スタン判定 -----
            if (_kneadGauge._state == KneadGaugeState.State.Knead)
            {
                // state 変更
                _state = State.Stanned;
                _kneadGauge._state = KneadGaugeState.State.Stanned;

                // ゲージ初期化
                _nowValue = 0.0f;

                // コンボ減少
                _comboCount -= 5;
                if (_comboCount <= 0)
                    _comboCount = 0;

                // スタンエフェクト表示
                CreateEffectStanned();
                return;
            }

            // ----- コンボ判定 -----
            if (_nowValue >= _comboValue - _comboRange && _nowValue <= _comboValue + _comboRange)
            {
                _comboCount++;
            }
            else // コンボ減少処理
            {
                _comboCount -= 3;
                if (_comboCount <= 0)
                    _comboCount = 0;
            }
            // ----- クリティカル判定 -----
            float scaleCritical = 1.0f;
            if (_nowValue >= _criticalValue - _criticalRange && _nowValue <= _criticalValue + _criticalRange)
            {
                scaleCritical = 2.0f;
            }

            // ----- コンボ数表示 -----
            CreateTextCombo();

            // ----- こねこねゲージグッド判定 -----
            float kneadBonus = _kneadGauge.GetBonusKnead();

            // ----- もちゲージ増加処理 -----
            _mochiGauge.IncreaseValue(scaleCritical, kneadBonus);

            // ----- ゲージ初期化処理 -----
            _nowValue = 0.0f;

            // ----- こねこねゲージ減少 -----
            _kneadGauge.DecreaseValue(1.0f);

            // ----- つき状態へ -----
            _state = State.Pound;
        }
    }

    private void CreateTextCombo()
    {
        GameObject clone = Instantiate(_prefabTextCombo);
        clone.transform.SetParent(transform, false);
        Vector3 position = transform.position;
        position.x += 300.0f;
        clone.transform.position = position;
        clone.GetComponent<TextFloatUp>()._numberDisplay = _comboCount;
    }

    private void CreateEffectStanned()
    {
        GameObject clone = Instantiate(_prefabEffecStanned);
        Vector3 position = Vector3.zero;
        clone.transform.position = position;
    }
}
