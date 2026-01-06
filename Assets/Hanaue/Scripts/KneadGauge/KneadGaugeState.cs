using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KneadGaugeState : MonoBehaviour
{
    // 状態管理
    public enum State
    {
        None,
        NoPlay,  // 操作不可
        Wait,    // 待機状態
        Knead,   // こねる状態
        Stanned, // スタン状態
    }
    public State _state;

    // slider
    private Slider _slider;

    [Header("----- 手動で設定 -----")]
    public PlayerChangeSprite _playerChangeSprite;

    // ----- バリューについて -----
    private float _maxValue = 100.0f;
    [Header("----- 現在の値（0~100） -----")]
    public float _nowValue = 50.0f;

    [Header("----- ノーマル判定区域 -----")]
    public float _normalValue = 50.0f; // ノーマル判定中央値
    public float _normalRange = 40.0f;

    [Header("----- グッド判定区域 -----")]
    public float _goodValue = 50.0f; // グッド判定中央値
    public float _goodRange = 10.0f;

    [Header("----- 増加量 -----")]
    public float _increaseValue = 15.0f;
    [Header("----- 減少量 -----")]
    public float _decreaseValue = 10.0f;

    [Header("----- もちゲージ増加量 -----")]
    public float _bonusGoodValue = 100.0f;
    public float _bonusBadValue = -50.0f;

    [Header("----- スタン時間 -----")]
    public float _timeStanned = 0.5f;
    private float _timerStanned = 0.0f;

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
                // ----- スペース打鍵処理 -----
                if (Input.GetKey(KeyCode.Space))
                {
                    // アニメーション再生処理
                    _playerChangeSprite.ChangeSprite(1);


                    _state = State.Knead;
                }
                break;
            case State.Knead:
                // ----- こねこねゲージの増加 -----
                IncreseValue();

                // ----- スペース離したとき処理 -----
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    // アニメーション再生処理
                    _playerChangeSprite.ChangeSprite(0);
                    _state = State.Wait;
                }
                break;
            case State.Stanned:
                _timerStanned += Time.deltaTime;
                if (_timerStanned > _timeStanned)
                {
                    _playerChangeSprite.ChangeSprite(0);
                    _state = State.Wait;
                    _timerStanned = 0.0f;
                }
                break;
        }

    }

    // --------------------------------------------------
    // プレイ中の処理
    // --------------------------------------------------
    // 増加処理
    private void IncreseValue()
    {
        _nowValue += _increaseValue * Time.deltaTime;
        if (_nowValue > _maxValue) 
            _maxValue = _nowValue;

        // ゲージ反映
        _slider.value = _nowValue / _maxValue;
    }

    // 減少処理
    public void DecreaseValue(float scale)
    {
        _nowValue -= _decreaseValue * scale;
        if (_nowValue < 0.0f) 
            _nowValue = 0.0f;

        // ゲージ反映
        _slider.value = _nowValue / _maxValue;
    }

    // グッド判定と結果を返す
    public  float GetBonusKnead()
    {
        // Good 判定
        if (_nowValue >= _goodValue - _goodRange && _nowValue <= _goodValue + _goodRange)
        {
            return _bonusGoodValue;
        }
        // Bad 判定
        else if (_nowValue <= _normalValue - _normalRange || _nowValue >= _normalValue + _normalRange)
        {
            return _bonusBadValue;
        }
        // normal 判定
        return 0.0f;
    }

}
