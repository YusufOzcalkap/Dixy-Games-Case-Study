using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public enum WallState
    {
        impact,
        divide,
        collection,
        extraction
    }
    public WallState wallstate;

    private TextMeshProUGUI _wallText;
    [HideInInspector]public int _number;

    void Start()
    {
        _wallText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        if (wallstate == WallState.impact || wallstate == WallState.divide)
            this._number = Random.Range(1, 4);
        if (wallstate == WallState.collection || wallstate == WallState.extraction)
            this._number = Random.Range(1, 5);

    }
    void Update()
    {
        if (wallstate == WallState.impact)
            _wallText.text = "×" + _number;
        
        if (wallstate == WallState.divide)
            _wallText.text = "÷" + _number;
        
        if (wallstate == WallState.collection)
            _wallText.text = "+" + _number;

        if (wallstate == WallState.extraction)
            _wallText.text = "-" + _number;
    }

    public void WallControl(int Count)
    {
        if (wallstate == WallState.impact)
            Count *= _number;

        if (wallstate == WallState.divide)
            Count /= _number;

    }
}
