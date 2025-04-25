using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AimingState : ISlingshotState
{
    private Slingshot _slinshot;

    public AimingState(Slingshot slinshot) => _slinshot = slinshot;

    public void Enter()
    {
        _slinshot.Trajectory.ShowTrajectory(true);
    }

    public void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 aimDirection = (Vector2)(mouseWorldPos - _slinshot.BandOrigin.position);
        float pullDistance = Mathf.Clamp(aimDirection.magnitude, 0, _slinshot.maxPull);
        Vector2 pullVector = aimDirection.normalized * pullDistance * _slinshot.forceMultiplier;

        _slinshot.Trajectory.CalculateTrajectory(_slinshot.BandOrigin.position, pullVector);
         
        // Fire on mouse release
        if (Input.GetMouseButtonUp(0))
        {
            _slinshot.LaunchProjectile(pullVector);
            _slinshot.ChangeState(new IdleState(_slinshot));
        }
    }

    public void Exit()
    {
        _slinshot.Trajectory.ShowTrajectory(false);
    }
}
