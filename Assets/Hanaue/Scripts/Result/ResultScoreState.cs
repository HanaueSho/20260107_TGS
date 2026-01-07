using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultScoreState : MonoBehaviour
{
    public enum State
    {
        None,
        Wait,
        Fall,
    }
    public State _state;

    [Header("落下時間")]
    public float _timeFall = 0.5f;
    private float _timerFall = 0.0f;

    [Header("落下距離")]
    public float _lengthFall = 1000.0f;
    public AnimationCurve _curveFall = new AnimationCurve();

    // ベース位置
    private Vector3 _basePosition;

    // Start is called before the first frame update
    void Start()
    {
        _basePosition = transform.GetComponent<RectTransform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.None:
                break;
            case State.Wait:
                break;
            case State.Fall:
                Fall();
                break;
        }
    }

    // 落下スタート
    public void StartFall()
    {
        _state = State.Fall;
    }

    // 落下処理
    private void Fall()
    {
        // 時間経過
        _timerFall += Time.deltaTime / _timeFall;

        // 位置反映
        Vector3 position = _basePosition;
        position.y -= _lengthFall * _curveFall.Evaluate(_timerFall);
        transform.GetComponent<RectTransform>().position = position;

        // 終了処理
        if ( _timerFall >= _timeFall)
        {
            // 位置補正
            position = _basePosition;
            position.y -= _lengthFall;
            transform.GetComponent<RectTransform>().position = position;

            _state = State.Wait;
        }
    }



}
