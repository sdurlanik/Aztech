using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gate : MonoBehaviour
{
    public static Gate instance;
    public delegate void GateInit();
    public static GateInit Initialize;
    
    
    [SerializeField] private bool _isWidth;
    [SerializeField] private TextMeshProUGUI _gateText;
    [SerializeField] private GameObject _connectedGate;

    private int _addWidth;
    private int _addHeight;

    public int WidthToAdd => _addWidth;
    public int HeightToAdd => _addHeight;

    public bool IsWidth => _isWidth;

    public TextMeshProUGUI GateText
    {
        get => _gateText;
        set => _gateText = value;
    }

    public GameObject ConnectedGate
    {
        get => _connectedGate;
        set => _connectedGate = value;
    }

    private void OnEnable()
    {
        instance = this;
        Initialize += Init;
    }

    private void OnDisable()
    {
        Initialize -= Init;
    }
    
    private void Start()
    {
        Initialize?.Invoke();
    }

    public void Init()
    {
        _addWidth = Random.Range(1, 20) * BallManager.instance.Length;
        _addHeight = Random.Range(1, 20) * BallManager.instance.Width;

        GateText.text = IsWidth ? "Width " + WidthToAdd : "Height " + HeightToAdd;
    }
}
