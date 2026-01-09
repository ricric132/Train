using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal.Commands;

public class LivingEngine : Passenger
{
    int skipAmount = 1;
    public override void Warm()
    {
        base.Warm();
        UpdateStationsRemaining(-skipAmount);
    }
}
