using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideState : MonoBehaviour
{
    public enum State
    {
        None,
        Wait,
        Guide_0,
        Guide_1,
    }
    public State _state;

    [Header("----- 手動でガイドを設定 -----")]
    public GameObject _guide_0;
    public GameObject _guide_1;

    private AudioSource _audioSource;
    [Header("----- 手動で効果音を設定 -----")]
    public AudioClip _clipClick;


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
            case State.Guide_0:
                if (Input.GetMouseButtonDown(0))
                {
                    _guide_0.SetActive(false);
                    _guide_1.SetActive(true);
                    _state = State.Guide_1;

                    _audioSource.PlayOneShot(_clipClick);
                }
                break;
            case State.Guide_1:
                if (Input.GetMouseButtonDown(0))
                {
                    _guide_0.SetActive(false);
                    _guide_1.SetActive(false);
                    _state = State.Wait;

                    _audioSource.PlayOneShot(_clipClick);
                }
                break;
        }
    }

    // こいつがクリックされたら呼ぶやつ
    public void OnClickThis()
    {
        if (_state == State.Wait)
        {
            _guide_0.SetActive(true);
            _state = State.Guide_0;

            _audioSource.PlayOneShot(_clipClick);
        }
    }


}
