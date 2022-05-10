using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private int _length, _width;
    [SerializeField] private List<Transform> _parentBalls;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Collider _finishFixCollider;
    
    public int Length
    {
        get => _length;
        set => _length = value;
    }

    public int Width
    {
        get => _width;
        set => _width = value;
    }

    public Collider FinishFixCollider => _finishFixCollider;


    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

    }

    // Update is called once per frame
    void Update()
    {

        
        
        if (_parentBalls.Count < Width)
        {
            _parentBalls.Add(GetNewBall(true));
        }

        if (_parentBalls.Count > Width)
        {
            
            Transform toRemove = _parentBalls.ElementAt(_parentBalls.Count-1);
            _parentBalls.Remove(toRemove);
            RemoveNextBalls(toRemove.GetComponent<BallObject>());
            Destroy(toRemove.gameObject);
        }

        float offset = (_parentBalls.Count / 2f) * -0.5f;
        for (int i = 0; i < _parentBalls.Count; i++)
        {
            Transform parentBall = _parentBalls.ElementAt(i);
            parentBall.transform.position = new Vector3(transform.position.x + (i * 0.5f) + offset, transform.position.y, transform.position.z);
            MoveNextBall(parentBall);
        }
    }

    public void MoveNextBall(Transform parentBall, int ballIndex = 0)
    {
        BallObject parentBallObject = parentBall.GetComponent<BallObject>();
        if (ballIndex > _length - 2)
        {
            RemoveNextBalls(parentBallObject);
            return;
        }
        else if (parentBallObject.NextBall.Equals(null))
        {
            parentBallObject.NextBall = GetNewBall();
        }
        Transform childBallTransform = parentBallObject.NextBall;
        float progress = GetProgress(Length - ballIndex) + 0.3f;
        float xPosition = Mathf.Lerp(childBallTransform.localPosition.x, parentBall.localPosition.x,   progress);
        float yPosition = Mathf.Lerp(childBallTransform.localPosition.y, parentBall.localPosition.y,   progress);
        //float zPosition = Mathf.Lerp(childBallTransform.localPosition.z, parentBall.localPosition.z - 0.5f, progress);
        childBallTransform.localPosition = new Vector3(xPosition, yPosition, parentBall.localPosition.z - parentBall.localScale.z);
        MoveNextBall(childBallTransform, ++ballIndex);
    }

    public float GetProgress(int ballIndex)
    {
        float progress = (float)ballIndex / Length;
        return _curve.Evaluate(progress);
    }

    private Transform GetNewBall(bool isParent = false)
    {
        GameObject newObject = Instantiate(_ballPrefab);
        if (isParent) newObject.name = "Parent";
        return newObject.transform;
    }

    private void RemoveNextBalls(BallObject parentBall)
    {
        
        while (!parentBall.NextBall.Equals(null))
        {
            parentBall = parentBall.NextBall.GetComponent<BallObject>();
            Destroy(parentBall.gameObject);
        }
    }

    public IEnumerator SetParentFree()
    {
        while (_parentBalls.Count > 0)
        {
            List<Transform> newParents = new();
            List<Transform> removedParents = new();
            _parentBalls.ForEach(parentBall =>
            {
                newParents.Add(parentBall.GetComponent<BallObject>().NextBall);
                removedParents.Add(parentBall);
                Rigidbody parentBallRigidBody = parentBall.GetComponent<Rigidbody>();
                parentBall.GetComponent<Collider>().enabled = true;
                parentBallRigidBody.isKinematic = false;
                parentBallRigidBody.useGravity = true;

                parentBallRigidBody.AddForce(new Vector3(Random.Range(-3f,3f), Random.Range(3,8), Random.Range(10,20)), ForceMode.Impulse);

            
            });
        
            _length--;
            removedParents.ForEach(ball => _parentBalls.Remove(ball));
            newParents.ForEach(ball => _parentBalls.Add(ball));
            yield return new WaitForSeconds(0.05f);
        }
    }
    
}