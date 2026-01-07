using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour
{
    // SpriteRenderer
    private Image _image;

    // Sprite
    public Sprite _sprite0;
    public Sprite _sprite1;

    public float _timeAnimation = 0.0f;
    private float _timerAnimation = 0.0f;
    public bool _isAnimation = false;
    private int _numAnimation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAnimation) return;

        _timerAnimation += Time.deltaTime;

        if (_timerAnimation > _timeAnimation)
        {
            ChangeSprite();
            _timerAnimation = 0.0f;
        }
    }

    private void ChangeSprite()
    {
        if (_numAnimation == 0)
        {
            _image.sprite = _sprite0;
            _numAnimation = 1;
        }
        else if (_numAnimation == 1)
        {
            _image.sprite = _sprite1;
            _numAnimation = 0;
        }
    }
}
