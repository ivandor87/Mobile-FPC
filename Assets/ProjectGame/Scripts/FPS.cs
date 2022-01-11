using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    float _updateInterval = 1f; 
    float _accum = .0f;
    int _frames = 0;
    float _timeLeft;
    string fpsFormat;

    void Start()
    {
        _timeLeft = _updateInterval;
    }

    void OnGUI()
    {
        GUILayout.Label("FPS: " + fpsFormat);
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;// Номер кадра  

        if (_timeLeft <= 0)
        {
            float fps = _accum / _frames;
            //Debug.Log(_accum + "__" + _frames);  
            fpsFormat = System.String.Format("{0:F2}FPS", fps); 
          //  Debug.LogError(fpsFormat);

            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }
    }
}
