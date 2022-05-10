using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _leftLine;
    [SerializeField] private float _rightLine;

    void Update()
    {
        if (transform.position.x <= _leftLine)
        {
            transform.position = new Vector3(_leftLine, transform.position.y,transform.position.z);
        }
        else if (transform.position.x >= _rightLine)
        {
            transform.position = new Vector3(_rightLine, transform.position.y,transform.position.z);

        }
        transform.Translate(Vector3.forward * _forwardSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        UIManager.instance.CollectCoin(other, 5);
        GateTrigger(other);
        FinishTrigger(other);
    }

    public void FinishTrigger(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(BallManager.instance.SetParentFree());
            other.enabled = false;
            BallManager.instance.FinishFixCollider.isTrigger = false;
        }
      
    }

    public void GateTrigger(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            var gateScript = other.GetComponent<Gate>();
            
            if (gateScript.IsWidth)
                BallManager.instance.Width += gateScript.WidthToAdd / BallManager.instance.Length;
            else
                BallManager.instance.Length += gateScript.HeightToAdd / BallManager.instance.Width ;

            transform.localScale = new Vector3(BallManager.instance.Width * 0.5f, 0.5f, 0.5f);

            //other.GetComponent<Collider>().enabled = false;
            gateScript.gameObject.SetActive(false);
            gateScript.ConnectedGate.SetActive(false);
            Gate.Initialize?.Invoke();
        }
    }
}
