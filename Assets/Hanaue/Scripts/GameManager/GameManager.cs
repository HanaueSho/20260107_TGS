using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        None,
        FadeIn,    // フェードイン
        Countdown, // カウントダウン
        Playing,   // プレイ中
        Clear,     // クリア
        FadeOut,   // フェードアウト
    }
    public State _state;

    [Header("----- 手動でゲージを設定 -----")]
    public MochiGaugeState _mochiGauge;
    public KneadGaugeState _kneadGauge;
    public PoundGaugeState _poundGauge;
    public GameObject _canvas;

    [Header("----- 手動でフェード画像を設定 -----")]
    public GameObject _imageFade;

    [Header("----- 手動で経過時間テキストを設定 -----")]
    public TextMeshProUGUI _textTimer;

    [Header("----- 手動でカウントダウンテキストを設定 -----")]
    public GameObject _prefabTextCountdown;

    private AudioSource _audioSource;
    [Header("----- 手動で効果音を設定 -----")]
    public AudioClip _clipCountdown;
    public AudioClip _clipStart;
    public AudioClip _bgmResult;

    [Header("----- 手動でリザルト関係を設定 -----")]
    public Image _resultEnd;
    public ResultScoreState _resultScore;
    public TextMeshProUGUI  _resultText;
    public ButtonChangeScene _buttonToTitle;
    public ButtonChangeScene _buttonRetry;
    private float _timerResult = 0.0f;
    private bool _isFallScore = false;

    [Header("カウントダウンスケール")]
    public float _scaleCountdown = 1.0f;
    private float _timerCountDown = 1.0f;
    private int _countdownCount = 0;

    [Header("フェード時間")]
    public float  _timeFade = 0.5f;
    private float _timerFade = 0.0f;

    [Header("経過時間")]
    public float _timerPlaying = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // フェード画像の初期化処理
        Color color = Color.black;
        color.a = 1.0f;
        _imageFade.GetComponent<Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.None:
                break;
            case State.FadeIn:
                FadeIn();
                break;
            case State.Countdown:
                Countdown();
                break;
            case State.Playing:
                CountTimer();

                // クリア判定
                if (_mochiGauge.CheckGameClear())
                {
                    // ステート遷移
                    _state = State.Clear;

                    // 各ゲージのステート設定
                    _kneadGauge._state = KneadGaugeState.State.NoPlay;
                    _poundGauge._state = PoundGaugeState.State.NoPlay;

                    // リザルト表示
                    _audioSource.Stop();
                    _audioSource.PlayOneShot(_bgmResult); // リザルト
                }
                break;
            case State.Clear:
                Result();
                break;
            case State.FadeOut:
                FadeOut();
                break;
        }
    }

    // フェード処理
    private void FadeIn()
    {
        _timerFade += Time.deltaTime;
        Color color = Color.black;
        color.a = 1.0f - _timerFade / _timeFade;
        _imageFade.GetComponent<Image>().color = color;
        if (_timerFade > _timeFade)
        {
            _timerFade = 0.0f;
            _state = State.Countdown;
        }
    }
    private void FadeOut()
    {
        _timerFade += Time.deltaTime;
        Color color = Color.black;
        color.a = _timerFade / _timeFade;
        _imageFade.GetComponent<Image>().color = color;
        if (_timerFade > _timeFade)
        {
            _timerFade = 0.0f;
            _state = State.Countdown;
        }
    }

    // カウントダウン処理
    private void Countdown()
    {
        _timerCountDown += Time.deltaTime * _scaleCountdown;

        // 1, 2, 3 秒のタイミングで音を鳴らす
        if ( _timerCountDown >= 1.0f )
        {
            // 初期化
            _timerCountDown = 0.0f;

            // テキスト表示
            CreateTextCountdown(3 - _countdownCount);

            // 効果音再生
            if (_countdownCount == 3)
                _audioSource.PlayOneShot(_clipStart);
            else
                _audioSource.PlayOneShot(_clipCountdown);


            // カウント
            _countdownCount++;
        }

        // カウントダウン終了処理
        if (_countdownCount == 4)
        {
            _state = State.Playing;

            // 各ゲージのステート設定
            _kneadGauge._state = KneadGaugeState.State.Wait;
            _poundGauge._state = PoundGaugeState.State.Wait;
        }
    }

    // カウントダウン生成
    private void CreateTextCountdown(int num)
    {
        GameObject clone = Instantiate(_prefabTextCountdown);
        clone.transform.SetParent(_canvas.transform, false);
        clone.GetComponent<RectTransform>().position = _canvas.transform.position;
        clone.GetComponent<TextFloatUp>()._numberDisplay = num;
        clone.GetComponent<TextFloatUp>()._floatingTime /= _scaleCountdown;
        if (num == 0)
        {
            clone.GetComponent<TextFloatUp>()._isNumberEnable = false;
            clone.GetComponent<TextFloatUp>()._textDisplay = "START";
        }
    }

    // 経過時間
    private void CountTimer()
    {
        _timerPlaying += Time.deltaTime;

        // 反映
        _textTimer.text = _timerPlaying.ToString("0.00");
    }

    // リザルト処理
    private void Result()
    {
        // リザルトエンドの表示
        if (_timerResult == 0.0f)
        {
            _resultEnd.gameObject.SetActive(true); 
        }
        
        // 時間経過
        _timerResult += Time.deltaTime;

        // リザルトスコアの落下処理
        if (_timerResult > 1.0f && !_isFallScore)
        {
            _resultScore.StartFall();
            _isFallScore = true;
        }

        // リザルトスコア表示
        if (_timerResult > 2.5f)
        {
            _resultText.gameObject.SetActive(true);
            _resultText.text = _timerPlaying.ToString("0.00");
        }

        // ボタン有効化
        if (_timerResult > 3.0f)
        {
            _buttonRetry.gameObject.SetActive(true);
            _buttonToTitle.gameObject.SetActive(true);
        }
    }

}
