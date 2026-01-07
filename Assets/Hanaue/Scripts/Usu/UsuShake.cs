using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsuShake : MonoBehaviour
{
    public enum State
    {
        None,
        Wait,
        Shaking
    }
    public State _state;

    // Šî€ˆÊ’u
    private Vector3 _basePosition;

    // \‘¢‘Ì
    [System.Serializable]
    public struct ShakeType
    {
        public bool isVertical;   // c—h‚ê
        public bool isHorizontal; // ‰¡—h‚ê
        public bool isAttenuation; // Œ¸Š
        public float time; // ŠÔ
        public float interval; // ŠÔŠu
        public float valueVertical; // c—h‚ê•
        public float valueHorizontal; // ‰¡—h‚ê•
    }
    private ShakeType _currentShake;
    public ShakeType _shakeSmall;
    public ShakeType _shakeMidium;
    public ShakeType _shakeLarge;

    private float _timer = 0.0f;
    private float _timerTotal = 0.0f;
    private int _sign = 1;

    // Start is called before the first frame update
    void Start()
    {
        _basePosition = transform.position;
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
            case State.Shaking:
                _timer += Time.deltaTime;
                _timerTotal += Time.deltaTime;

                if ( _timer > _currentShake.interval )
                {
                    _timer = 0.0f;

                    float scale = 1.0f;
                    if (_currentShake.isAttenuation )
                    {
                        scale = 1.0f - _timerTotal / _currentShake.time;
                    }

                    Vector3 position = _basePosition;
                    if (_currentShake.isVertical) // c—h‚ê
                    {
                        position.y += _currentShake.valueVertical * _sign * scale;
                    }
                    if ( _currentShake.isHorizontal) // ‰¡—h‚ê
                    {
                        position.x += _currentShake.valueHorizontal * _sign * scale;
                    }

                    // ˆÊ’u”½‰f
                    transform.position = position;

                    // •„†”½“]
                    _sign = -_sign;
                }

                if (_timerTotal > _currentShake.time )
                {
                    _state = State.Wait;
                    _timer = 0.0f;
                    _timerTotal = 0.0f;
                    transform.position = _basePosition;
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Shake(0);
        }
    }

    public void Shake(int num)
    {
        _state = State.Shaking;

        // ‰Šú‰»ˆ—
        _timer = 0.0f;
        _timerTotal = 0.0f;
        transform.position = _basePosition;

        if (num == 0)
        {
            _currentShake = _shakeSmall;
        }
        else if (num == 1)
        {
            _currentShake = _shakeMidium;
        }
        else if (num == 2)
        {
            _currentShake = _shakeLarge;
        }
    }

}
