﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Action_Hitscan : Action {

    public float 
        m_damage, m_maxTravelDistance;
    // Use this for initialization
    public override void runLocal(PlayerController playerController)
    {
        //var pointOfImpact = fire(playerController.m_motor.m_face.position, playerController.m_motor.m_face.forward, 10, 10);
        //NetworkEffect.INSTANCE.CmdBulletTrail(playerController.m_bulletSpawn.position, pointOfImpact);

        //    );
    }
    public override void runServer(PlayerController playerController)
    {
        //var pointOfImpact = fire(playerController.m_motor.m_head.position, playerController.m_motor.m_head.forward, 10, 10);
        //NetworkEffect.INSTANCE.CmdBulletTrail(playerController.m_bulletSpawn.position, pointOfImpact);

        //    );
    }
    public override void use(PlayerMotor motor)
    {
        Debug.Log("Fired");
        base.use(motor);
        
        fire(motor.netId,  motor.getAvatar().m_head.transform.position, motor.getAvatar().m_head.transform.forward , m_damage, m_maxTravelDistance);

    }
    public void fire(
        NetworkInstanceId myMotor,
        Vector3 posBegin, Vector3 direction,
        float damage, float maxTravelDistance)
    {
        bool isHitSomething = false;
        float travelDistance = 0;
        var ray = new Ray(posBegin, direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.transform == null || hit.distance > maxTravelDistance)
        {
            travelDistance = maxTravelDistance;
            //hit the air
        }
        else
        {
            travelDistance = hit.distance;
            isHitSomething = true;
            // trail.transform.LookAt(hit.point);

        }
        if (isHitSomething)
        {
            var targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null && targetHealth.takeDamage(damage / 2) )
            {
                var targetMotor = hit.transform.GetComponent<NetworkIdentity>();
                ClientCommunication.ME.CmdDamage(myMotor, targetMotor.netId, damage / 2);
            }
        }
        //return this.transform.position + this.transform.forward * travelDistance;
        // trail.transform.position = transform.position;
    }


}
