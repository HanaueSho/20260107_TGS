using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public enum State
    {
        None,
        FadeIn,  // フェードイン
        Wait,    // 待機
        FadeOut, // フェードアウト
    }
    public State _state;

    [Header("----- 手動でフェード画像を設定 -----")]
    public GameObject _imageFade;

    [Header("フェード時間")]
    public float _timeFade = 0.5f;
    private float _timerFade = 0.0f;

    private AudioSource _audioSource;
    [Header("----- 手動で効果音を設定 -----")]
    public AudioClip _clipClick;

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
            case State.Wait:
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
            _state = State.Wait;
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

            // ----- シーンチェンジ -----
            SceneManager.LoadScene("GameScene");
        }
    }

    public void StartGame()
    {
        if (_state == State.Wait)
        {
            // SE 再生
            _audioSource.PlayOneShot(_clipClick);

            // state 移行
            _state = State.FadeOut;
        }
    }
}
