using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectState : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    // Sprite
    public Sprite _sprite0;
    public Sprite _sprite1;
    bool _isSprite0 = true;

    public  float _time  = 0.5f;
    private float _timer = 0.0f;

    public  float _timeBlink  = 0.1f;
    private float _timerBlink = 0.0f;


    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // “_–Åˆ—
        _timerBlink += Time.deltaTime;
        if (_timerBlink > _timeBlink)
        {
            _timerBlink = 0.0f;
            ChangeSprite();
        }
        // ”jŠüˆ—
        _timer += Time.deltaTime;
        if (_timer > _time) 
            Destroy(gameObject);
    }

    private void ChangeSprite()
    {
        if (_isSprite0)
        {
            _spriteRenderer.sprite = _sprite1;
            _isSprite0 = false;
        }
        else
        {
            _spriteRenderer.sprite = _sprite0;
            _isSprite0 = true;
        }
    }
}
