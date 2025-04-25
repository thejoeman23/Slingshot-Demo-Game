using UnityEngine;
using System.Collections.Generic;

public class SlingshotVisuals : MonoBehaviour
{
    [SerializeField] private List<Color> _colors = new List<Color>(); // Colors the objects can take
    [SerializeField] private List<GameObject> _objects = new List<GameObject>(); // All of the objects that can be slung
    [SerializeField] private LineRenderer _lineRenderer;

    // Updates the visual band position and position of the currentObject
    public void UpdateVisuals(Slingshot slingshot)
    {
        slingshot.currentObject.transform.position = slingshot.BandOrigin.position;
        _lineRenderer.SetPosition(1, slingshot.BandOrigin.position);

        // Fix: Compare the type of the current state to IdleState using `is` instead of `==`
        if (slingshot.currentState is IdleState) // Resets band position and current object when the player isn't using the slingshot
        {
            slingshot.currentObject.transform.position = _lineRenderer.transform.position;
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
}
