using System;
using System.Collections.Generic;
using UnityEngine;

public class SlinghsotTrajectory : MonoBehaviour
{
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private int _resolution;
    [Range(0.00f,0.5f)][SerializeField] private float _scalingFactor;

    private GameObject[] _points;

    private void Start()
    {
        _points = new GameObject[_resolution];
        CreatePoints();
    }

    public void ShowTrajectory(bool input)
    {
        Debug.Log("Trajectory shown: " + input);

        for (int i = 0; i < _resolution; i++)
        {
            _points[i].SetActive(input);
        }
    }

    public void CalculateTrajectory(Vector2 startPosition, Vector2 direction, float force, float forceMultiplier)
    {
        Vector2 initialVelocity = direction * force * forceMultiplier * _scalingFactor;

        // Total time until the projectile hits the ground
        float totalTime = CalculateFlightTime(initialVelocity.y);

        // Calculate trajectory points over time (fixed resolution of 10)
        float timeStep = totalTime / _resolution;

        for (int i = 0; i < _resolution; i++)
        {
            float time = i * timeStep;
            Vector2 point = CalculatePositionAtTime(startPosition, initialVelocity, time);
            _points[i].transform.position = point;
        }
    }

    // Function to calculate flight time based on initial vertical velocity
    private float CalculateFlightTime(float initialVerticalVelocity)
    {
        // The formula to calculate the time until the object hits the ground
        return (2 * initialVerticalVelocity);
    }

    // Function to calculate the position of the object at a given time
    private Vector2 CalculatePositionAtTime(Vector2 startPosition, Vector2 initialVelocity, float time)
    {
        float x = startPosition.x + initialVelocity.x * time;
        float y = startPosition.y + initialVelocity.y * time - 0.5f * time * time;

        return new Vector2(x, y);
    }

    private void CreatePoints()
    {
        for (int i = 0; i < _resolution; i++)
        {
            _points[i] = Instantiate(_dotPrefab);
            _points[i].SetActive(false);
        }
    }
}
