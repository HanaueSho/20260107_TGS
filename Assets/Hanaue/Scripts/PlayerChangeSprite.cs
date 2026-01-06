using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChangeSprite : MonoBehaviour
{
    // SpriteRenderer
    private SpriteRenderer _spriteRenderer;

    // Sprite
    public Sprite _sprite0; // 待機状態
    public Sprite _sprite1; // こねる or つく状態

    // 位置とスケール調整
    [System.Serializable]
    public struct PositionScale
    {
        public Vector3 position;
        public Vector2 scale;
    }

    public PositionScale positionScale0;
    public PositionScale positionScale1;

    private float _timeAnimation = 0.0f;
    private float _timerAnimation = 0.0f;
    private bool _isAnimation = false;
    private int _numAnimation = 0;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        positionScale0.position = transform.position;
        positionScale0.scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAnimation) return;

        _timerAnimation += Time.deltaTime;

        if (_timerAnimation > _timeAnimation)
        {
            ChangeSprite();
            _isAnimation = false;
        }
    }

    public void ChangeSprite(int num, float time = 0.0f)
    {
        // 再生フラグ
        _isAnimation = true;
        // 時間設定
        _timeAnimation = time;
        _timerAnimation = 0.0f;
        // アニメ番号保存
        _numAnimation = num;
    }

    private void ChangeSprite()
    {
        if (_numAnimation == 0)
        {
            _spriteRenderer.sprite = _sprite0;
            transform.position = positionScale0.position;
            transform.localScale = positionScale0.scale;
        }
        else if (_numAnimation == 1)
        {
            _spriteRenderer.sprite = _sprite1;
            transform.position = positionScale1.position;
            transform.localScale = positionScale1.scale;
        }
    }

}
