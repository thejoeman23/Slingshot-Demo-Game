using UnityEngine;
using System.Collections.Generic;

public class SlingshotVisuals : MonoBehaviour
{
    [SerializeField] private List<Color> _colors = new List<Color>(); // Colors the objects can take
    [SerializeField] private List<GameObject> _objects = new List<GameObject>(); // All of the objects that can be slung
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _warningShakeForce;
    private bool _isShaking;

    // Updates the visual band position and position of the currentObject
    public void UpdateVisuals(Vector2 bandPosition, GameObject currentObject, bool stretched)
    {
        if (_isShaking) bandPosition += RandomOffset();

        currentObject.transform.position = bandPosition;
        _lineRenderer.SetPosition(1, bandPosition);

        if (!stretched) // Resets band position and current object when the player isnt using the slingshot
        {
            currentObject.transform.position = _lineRenderer.transform.position;
        }
    }

    // Sets the current object's visuals
    public void SetObjectVisuals(GameObject obj)
    {
        int random = Random.Range(0, _colors.Count);

        obj.GetComponent<SpriteRenderer>().color = _colors[random];
    }

    public GameObject SetRandomObject()
    {
        int randomIndex = Random.Range(0, _objects.Count); // Include all elements
        return _objects[randomIndex];
    }

    public Vector2 GetLineRendererPosition()
    {
        return _lineRenderer.transform.position;
    }

    // Enables warning shake
    public void EnableWarning(bool input)
    {
        _isShaking = input;
    }

    // Returns a random vector 2 offset for the warning shake
    private Vector2 RandomOffset()
    {
        float x = Random.Range(0f, 1f);
        float y = Random.Range(0f, 1f);

        return new Vector2(x, y) * _warningShakeForce;
    }
}
