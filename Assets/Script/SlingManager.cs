using System.Collections.Generic;
using UnityEngine;

public class SlingManager : MonoBehaviour
{
    private GameManager _manager;

    [SerializeField] private List<GameObject> _objects = new List<GameObject>();
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _forceMultiplier = 5;
    [SerializeField] private float _stretchThreshold = 1;

    private bool _stretched = false;
    // private bool _canSling = false;

    private Vector2 _direction;
    private Vector2 _firstClicked;
    private Vector2 _bandPosition;

    private GameObject _currentObject;
    private GameObject _nextObject;

    private void Awake()
    {
        _manager = GameManager.Instance;
    }

    private void Start()
    {
        _firstClicked = Vector2.zero;
        NextObject();
    }

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // The current mouse position
        _direction = (mousePos - _firstClicked).normalized;

        _lineRenderer.SetPosition(1, _bandPosition);
        _currentObject.transform.position = _bandPosition;

        if (Input.GetMouseButton(0) && _stretched)
        {
            float offset = Vector2.Distance(_firstClicked, mousePos);

            _bandPosition = (Vector2)transform.position + (_direction * offset);
        }

        if (Input.GetMouseButtonDown(0) && !_stretched)
        {
            _stretched = true;

            _firstClicked = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && _stretched)
        {
            _stretched = false;
            float distance = Vector2.Distance(mousePos, _firstClicked);

            if (distance >= _stretchThreshold)
            {
                Sling(distance);
            }

            _bandPosition = transform.position;
        }
    }

    private void Sling(float extraForce)
    {
        Rigidbody2D objectRb = _currentObject.GetComponent<Rigidbody2D>();
        objectRb.bodyType = RigidbodyType2D.Dynamic;

        objectRb.linearVelocity = _direction * _forceMultiplier * extraForce * -1;

        NextObject();
    }

    private void NextObject()
    {
        // If no next object exists, create one
        if (_nextObject == null)
        {
            _nextObject = SetRandomObject();
        }

        // Assign the next object to the current object
        _currentObject = Instantiate(_nextObject);

        // Prepare the next object
        _nextObject = SetRandomObject();
    }

    private GameObject SetRandomObject()
    {
        int randomIndex = Random.Range(0, _objects.Count); // Include all elements
        return _objects[randomIndex];
    }
}
