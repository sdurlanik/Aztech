using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private int _coinAmount = 0;
    [SerializeField] private RectTransform _coinImageTransform;
    [SerializeField] private GameObject _coinImagePrefab;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private TextMeshProUGUI _coinText;
    private Camera _cam;
    

    private void Awake()
    {
        instance = this;
    }
  

    void Start()
    {
        _cam = Camera.main;
        
    }

    void Update()
    {
        print(_coinAmount);
    }

    public void CollectCoin(Collider coinObjectCollider, int amount)
    {
        if (coinObjectCollider.CompareTag("Coin"))
        {
            Vector2 screenPos = _cam.WorldToScreenPoint(coinObjectCollider.transform.position);
            
            coinObjectCollider.gameObject.SetActive(false);
            var coinImage = Instantiate(_coinImagePrefab,screenPos,Quaternion.identity);
            coinImage.transform.SetParent(_canvas.transform);
            coinImage.transform.LeanMove(_coinImageTransform.position, 0.7f).setEaseInSine()
                .setOnComplete(() => UpdateCoin(amount));
            
        }
    }

    private void UpdateCoin(int amount)
    {
        _coinAmount += amount;
        _coinText.text = _coinAmount.ToString();
    }
}
