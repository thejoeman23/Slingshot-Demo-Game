using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IdleState : ISlingshotState
{
    private Slingshot _slingshot;

    public IdleState(Slingshot slingshot) => _slingshot = slingshot;

    public void Enter()
    {

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _slingshot.ChangeState(new AimingState(_slingshot));
        }
    }

    public void Exit()
    {

    }
}
