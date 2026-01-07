using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    // Slider
    private Slider _slider;

    [Header("----- 手動で設定 -----")]
    public PlayerChangeSprite _playerChangeSprite;

    [Header("----- 手動でスライダーを設定 -----")]
    public KneadGaugeState _kneadGauge;
    public MochiGaugeState _mochiGauge;

    [Header("----- 手動でうすを設定 -----")]
    public UsuShake _usu;

    [Header("----- 手動でプレファブを設定 -----")]
    public GameObject _prefabTextCombo;
    public GameObject _prefabTextCritical;
    public GameObject _prefabEffecStanned;

    private AudioSource _audioSource;
    [Header("----- 手動で効果音を設定 -----")]
    public AudioClip[] _clipPoundVoice;
    public AudioClip[] _clipStannedVoice;
    public AudioClip _clipPoundNormal;
    public AudioClip[] _clipCombo;
    public AudioClip _clipCritical;
    public AudioClip _clipStanned;

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

    [Header("----- クリティカル倍率 -----")]
    public float _criticalScale = 1.5f;

    [Header("----- スタン時間 -----")]
    public float  _timeStanned = 0.5f;
    private float _timerStanned = 0.0f;

    // ----- レベルについて -----
    [System.Serializable]
    public struct Level
    {
        public float speed;
        public float comboRange;
        public float criticalRange;
        public bool isRandom;
    }

    [Header("----- レベルについて -----")]
    [Header("speed     ... バーの速度\n"
          + "comboRange... コンボ区域の幅\n"
          + "criticalRange... クリティカル区域の幅\n"
          + "isRandom  ... バーの増加がランダムになる"
        )]
    public Level[] _levels = new Level[20];
    private float _speed = 1.0f;

    private float _timerCurve = 0.0f; // カーブ用
    private AnimationCurve _currentCurve;
    public AnimationCurve _curveNormal = new AnimationCurve();
    public AnimationCurve[] _curves = new AnimationCurve[3];

    void Start()
    {
        _slider = GetComponent<Slider>();
        _currentCurve = _curveNormal;

        _audioSource = GetComponent<AudioSource>();

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
    // レベル反映処理
    private void SetLevel()
    {
        // コンボ数からスピード、レンジを設定
        int index = _comboCount;
        if (index < 0) index = 0;
        if (index >= _levels.Length) index = _levels.Length - 1;

        _speed = _levels[index].speed;
        _comboRange = _levels[index].comboRange;
        _criticalRange = _levels[index].criticalRange;
        if (_levels[index].isRandom)
        {
            int random = Random.Range(0, _curves.Length);
            _currentCurve = _curves[random];
        }
        else
        {
            _currentCurve = _curveNormal;
        }
    }

    // 増加処理
    private void IncreaseGaugeValue()
    {
        // 増加処理
        _timerCurve += _speed * Time.deltaTime;
        //_nowValue += _speed * Time.deltaTime;
        _nowValue = _currentCurve.Evaluate(_timerCurve) * _maxValue;

        // ゲージ反映処理
        _slider.value = _nowValue / _maxValue;
        if (_nowValue >= _maxValue)
        {
            _timerCurve = 0.0f;
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
                _timerCurve = 0.0f;
                _nowValue = 0.0f;

                // コンボ減少
                AddComboCount(-8);

                // スタンエフェクト表示
                CreateEffectStanned();

                // 効果音再生
                int rand = Random.Range(0, _clipStannedVoice.Length);
                _audioSource.PlayOneShot(_clipStannedVoice[rand]);
                _audioSource.PlayOneShot(_clipStanned);

                return;
            }

            // ----- コンボ判定 -----
            int shakeIndex = 0;
            if (_nowValue >= _comboValue - _comboRange && _nowValue <= _comboValue + _comboRange)
            {
                AddComboCount(1);
                // 効果音再生
                int index = 0;
                if      (_comboCount > 5) index = 1;
                else if (_comboCount > 10) index = 2;
                _audioSource.PlayOneShot(_clipCombo[index]);

                shakeIndex = 1;
            }
            else // コンボ減少処理
            {
                AddComboCount(-5);
            }
            // ----- クリティカル判定 -----
            float scaleCritical = 1.0f;
            float scaleDecrease = 1.0f;
            if (_nowValue >= _criticalValue - _criticalRange && _nowValue <= _criticalValue + _criticalRange)
            {
                scaleCritical = _criticalScale;
                scaleDecrease = 1.5f;
                CreateTextCritical();
                // 効果音再生
                _audioSource.PlayOneShot(_clipCritical);

                shakeIndex = 2;
            }

            // ----- コンボ数表示 -----
            CreateTextCombo();

            // ----- コンボ数表示 -----
            _usu.Shake(shakeIndex);

            // ----- 効果音再生 -----
            int random = Random.Range(0, _clipPoundVoice.Length);
            _audioSource.PlayOneShot(_clipPoundVoice[random]);
            _audioSource.PlayOneShot(_clipPoundNormal);

            // ----- こねこねゲージグッド判定 -----
            float kneadBonus = _kneadGauge.GetBonusKnead();

            // ----- もちゲージ増加処理 -----
            _mochiGauge.IncreaseValue(scaleCritical, kneadBonus);

            // ----- ゲージ初期化処理 -----
            _timerCurve = 0.0f;
            _nowValue = 0.0f;

            // ----- こねこねゲージ減少 -----
            _kneadGauge.DecreaseValue(scaleDecrease);

            // ----- つき状態へ -----
            _state = State.Pound;
        }
    }

    // スコア増加
    private void AddComboCount(int num)
    {
        _comboCount += num;
        if (_comboCount <= 0)
            _comboCount = 0;

        // レベルセット
        SetLevel();
    }

    // コンボテキスト生成
    private void CreateTextCombo()
    {
        GameObject clone = Instantiate(_prefabTextCombo);
        clone.transform.SetParent(transform, false);
        Vector3 position = transform.position;
        position.x += 300.0f;
        clone.transform.position = position;
        clone.GetComponent<TextFloatUp>()._numberDisplay = _comboCount;
    }
    // クリティカルテキスト生成
    private void CreateTextCritical()
    {
        GameObject clone = Instantiate(_prefabTextCritical);
        clone.transform.SetParent(transform, false);
        Vector3 position = transform.position;
        position.y += 200.0f;
        clone.transform.position = position;
    }

    // スタンエフェクト生成
    private void CreateEffectStanned()
    {
        GameObject clone = Instantiate(_prefabEffecStanned);
        Vector3 position = Vector3.zero;
        position.y -= 0.5f;
        clone.transform.position = position;
    }
}
