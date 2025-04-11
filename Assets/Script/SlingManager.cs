using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SlingManager : MonoBehaviour
{
    [SerializeField] PointSystem pointSystem;
    [SerializeField] CircleEffect circleEffect;

    [SerializeField] List<GameObject> objects = new List<GameObject>();
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float forceMultiplier = 5;

    public bool isPlayerOne;
    public bool stretched = false;
    public bool launched = false;
    public float gravity = 1f; // Fixed gravity
    public int resolution = 10; // Fixed number of points for the trajectory
    [SerializeField] float resolutionDivision = 1;
    public float objectMass = 100f; // Mass of each object

    private Vector2 direction;
    private Vector2 firstClicked;
    private Vector2 bandPosition;
    public int stillnessThreshold = 200; // Number of frames the tower has to be still before ending the turn
    private float band;
    private int stillness = 0; // The number of frames since the tower moved last
    public bool singlePlayer = false;

    private GameObject currentObject;
    private GameObject nextObject;

    SpriteRenderer sr;

    List<GameObject> trajectoryPoints = new List<GameObject>();
    [SerializeField] GameObject trajectoryPointPrefab; // The prefab for trajectory points

    [SerializeField] Animator animator;
}