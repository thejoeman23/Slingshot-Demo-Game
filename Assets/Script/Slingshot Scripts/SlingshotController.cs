using System;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    private GameManager _manager;

    [SerializeField] private SlingshotVisuals _visuals;
    [SerializeField] private SlinghsotTrajectory _trajectory;
    [SerializeField] private float _forceMultiplier = 5; // The force of the sling
    [SerializeField] private float _stretchThreshold = 1; // The minimum threshold for slinging an object
    [SerializeField] private float _stretchWarningThreshold = 6; // The threshold for the warning which indicates that the band is about to snap
    [SerializeField] private float _stretchMaximum = 8; // The maximum amount the band can stretch

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
        _bandPosition = _visuals.GetLineRendererPosition();
        _firstClicked = Vector2.zero;
        NextObject();
    }

    private void Update()
    {
        // Updates the mouse position and the direction between it and the _firstClicked
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // The current mouse position
        _direction = (mousePos - _firstClicked).normalized;

        _visuals.UpdateVisuals(_bandPosition, _currentObject, _stretched); // Updates the visuals each frame

        // Updates the band position while the player is in the process of pulling back the slingshot
        if (Input.GetMouseButton(0) && _stretched)
        {
            float offset = Vector2.Distance(_firstClicked, mousePos);

            _bandPosition = _visuals.GetLineRendererPosition() + (_direction * offset);
            _trajectory.CalculateTrajectory(_visuals.GetLineRendererPosition(), _direction, _forceMultiplier, offset);

            if (offset >= _stretchMaximum)
            {
                EnableStretch(false, _firstClicked);
            } 
            else if (offset >= _stretchWarningThreshold)
            {
                Debug.Log("Enabled visual warning!");
                // Enable visual warning
            }
        }

        // Detects when the player begins pulling back the sling
        if (Input.GetMouseButtonDown(0) && !_stretched)
        {
            EnableStretch(true, mousePos);
        }

        // Slings the object when the player lets go
        if (Input.GetMouseButtonUp(0) && _stretched)
        {
            EnableStretch(false, _firstClicked);

            float distance = Vector2.Distance(mousePos, _firstClicked);

            if (distance >= _stretchThreshold)
            {
                Sling(distance);
            }

            _bandPosition = _visuals.GetLineRendererPosition();
        }
    }

    // Handles the launching of the object
    private void Sling(float extraForce)
    {
        EnableObject(_currentObject, true);

        Rigidbody2D rb = _currentObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = _direction * _forceMultiplier * extraForce * -1;

        NextObject();
    }

    // Updates what object is currently in the sling
    private void NextObject()
    {
        // If no next object exists, create one
        if (_nextObject == null)
        {
            _nextObject = _visuals.SetRandomObject();
        }

        // Assign the next object to the current object
        _currentObject = Instantiate(_nextObject, _bandPosition, Quaternion.identity);

        _visuals.SetObjectVisuals(_currentObject);
        EnableObject(_currentObject, false);

        // Prepare the next object
        _nextObject = _visuals.SetRandomObject();
    }

    // Enables or disables an objects rigidbody2D and Collider2D
    private void EnableObject(GameObject obj, bool input)
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (input)
            rb.bodyType = RigidbodyType2D.Dynamic;
        else
            rb.bodyType = RigidbodyType2D.Static;

        obj.GetComponent<Collider2D>().enabled = input;
    }

    // Sets up variables and some visuals
    private void EnableStretch(bool input, Vector2 mousePos)
    {
        _stretched = input;
        _trajectory.ShowTrajectory(input);

        _firstClicked = mousePos;
    }

}
