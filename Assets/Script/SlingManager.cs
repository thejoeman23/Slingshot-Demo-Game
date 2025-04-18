using System.Collections.Generic;
using UnityEngine;

public class SlingManager : MonoBehaviour
{
    private GameManager _manager;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private List<Color> _colors = new List<Color>(); // All of the objects that can be slung
    [SerializeField] private List<GameObject> _objects = new List<GameObject>(); // Colors the objects can take
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _forceMultiplier = 5; // The force of the sling
    [SerializeField] private float _stretchThreshold = 1; // The minimum threshold for slinging an object

    private bool _stretched = false;

    private Vector2 _direction; // The direction between _firstclicked and the current mouse position used to calculate trajectory
    private Vector2 _firstClicked; // Where the player first clicked to pull the sling back
    private Vector2 _bandPosition; // The position of the slingshot band (also the position of the object which will be launched)

    private GameObject _currentObject; // The current displayed object which will be launched
    private GameObject _nextObject; // The object next in line

    private void Awake()
    {
        _manager = GameManager.Instance;
    }

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _firstClicked = Vector2.zero;
        NextObject();
    }

    private void Update()
    {
        // Updates the mouse position and the direction between it and the _firstClicked
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // The current mouse position
        _direction = (mousePos - _firstClicked).normalized;

        UpdateVisuals(); // Updates the visuals each frame

        // Updates the band position while the player is in the process of pulling back the slingshot
        if (Input.GetMouseButton(0) && _stretched)
        {
            float offset = Vector2.Distance(_firstClicked, mousePos);

            _bandPosition = (Vector2)_lineRenderer.transform.position + (_direction * offset);
        }

        // Detects when the player begins pulling back the sling
        if (Input.GetMouseButtonDown(0) && !_stretched)
        {
            _stretched = true;

            _firstClicked = mousePos;
        }

        // Slings the object when the player lets go
        if (Input.GetMouseButtonUp(0) && _stretched)
        {
            _stretched = false;
            float distance = Vector2.Distance(mousePos, _firstClicked);

            if (distance >= _stretchThreshold)
            {
                Sling(distance);
            }
        }
    }

    // Handles the launching of the object
    private void Sling(float extraForce)
    {
        GameObject obj = Instantiate(_currentObject, transform.position, Quaternion.identity);
        _currentObject.GetComponent<SpriteRenderer>().color = _spriteRenderer.color;
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        rb.linearVelocity = _direction * _forceMultiplier * extraForce * -1;

        NextObject();
    }

    // Updates the visual band position and position of the currentObject
    private void UpdateVisuals()
    {
        transform.position = _bandPosition;
        _lineRenderer.SetPosition(1, _bandPosition);

        if (!_stretched) // Resets band position and current object when the player isnt using the slingshot
        {
            transform.position = _lineRenderer.transform.position;
            _bandPosition = _lineRenderer.transform.position;
        }
    }

    // Updates what object is currently in the sling
    private void NextObject()
    {
        // If no next object exists, create one
        if (_nextObject == null)
        {
            _nextObject = SetRandomObject();
        }

        // Assign the next object to the current object
        _currentObject = _nextObject;
        SetObjectVisuals();

        // Prepare the next object
        _nextObject = SetRandomObject();
    }

    // Sets the current object's visuals
    private void SetObjectVisuals()
    {
        int random = Random.Range(0, _colors.Count);

        _spriteRenderer.sprite = _currentObject.GetComponent<SpriteRenderer>().sprite;
        transform.localScale = _currentObject.transform.localScale;
        _spriteRenderer.color = _colors[random];
    }

    // Creates a random object
    private GameObject SetRandomObject()
    {
        int randomIndex = Random.Range(0, _objects.Count); // Include all elements
        return _objects[randomIndex];
    }
}
