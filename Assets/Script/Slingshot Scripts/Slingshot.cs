using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public SlingshotVisuals Visuals;
    public SlinghsotTrajectory Trajectory;
    public Transform BandOrigin;
    public Vector2 BandPosition;
    public float maxPull;
    public float forceMultiplier;

    public GameObject currentObject { get; private set; }

    public ISlingshotState currentState { get; private set; }

    public static event Action<GameObject> OnObjectLaunched;

    void Start()
    {
        ChangeState(new IdleState(this));
        SetNewObject();

        BandPosition = BandOrigin.position;
    }

    void Update()
    {
        currentState?.Update();
        Visuals.UpdateVisuals(this);
    }

    public void ChangeState(ISlingshotState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void LaunchProjectile(Vector2 force)
    {
        EnableObjectRB(currentObject, true);

        Rigidbody2D rb = currentObject.GetComponent<Rigidbody2D>();
        rb.linearVelocity = force * -1;
        OnObjectLaunched?.Invoke(currentObject);

        SetNewObject();
    }

    private void SetNewObject()
    {
        currentObject = Instantiate(Visuals.SetRandomObject(), BandOrigin.position, Quaternion.identity);
        Visuals.SetObjectVisuals(currentObject);
        EnableObjectRB(currentObject, false);
    }

    private void EnableObjectRB(GameObject obj, bool input) 
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();

        if (input)
            rb.bodyType = RigidbodyType2D.Dynamic;
        else
            rb.bodyType = RigidbodyType2D.Static;

        obj.GetComponent<Collider2D>().enabled = input;
    }

    public void UpdateCurrentObjectPosition(Vector2 difference)
    {
        if (Mathf.Clamp(difference.magnitude, 0, maxPull) >= maxPull) return; // Difference is greater than the max pull
        
        BandPosition = (Vector2)BandOrigin.position + difference;
    }
}


public interface ISlingshotState
{
    void Enter();
    void Update();
    void Exit();
}

