using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Status_machine
{
    public enum Status
    {
        Idle,
        Move,
        defend,
        Roll,
        Fall,
        hurt,
        death,
        jump,
        attack1,
        attack2,
        attack3,
        slideWall,
        slideWalAno,
        block
    }
    public static class Status_Mach
    {
        static Dictionary<Status, HashSet<Status>> dic = new Dictionary<Status, HashSet<Status>>
        {
            {Status.Idle,new HashSet<Status>(){Status.Move,Status.defend,Status.Roll,Status.Fall,Status.hurt,Status.death,Status.jump,Status.attack1} },
            {Status.Move,new HashSet<Status>(){Status.defend,Status.Roll,Status.Fall,Status.hurt,Status.death,Status.jump,Status.attack1} },
            {Status.defend,new HashSet<Status>(){Status.block,Status.Roll,Status.Fall,Status.hurt,Status.death} },
            {Status.Roll,new HashSet<Status>(){Status.Fall,Status.death} },
            {Status.Fall,new HashSet<Status>(){Status.hurt,Status.death,Status.slideWall} },
            {Status.hurt,new HashSet<Status>(){Status.death} },
            {Status.attack1,new HashSet<Status>(){Status.Fall,Status.hurt,Status.death,Status.attack2} },
            {Status.attack2,new HashSet<Status>(){Status.attack3,Status.Fall,Status.hurt,Status.death} },
            {Status.attack3,new HashSet<Status>(){Status.Fall,Status.hurt,Status.death} },
            {Status.slideWall,new HashSet<Status>(){Status.hurt,Status.death,Status.jump,Status.slideWalAno} },
            {Status.slideWalAno,new HashSet<Status>(){Status.hurt,Status.death,Status.jump,Status.slideWall} },
            {Status.jump,new HashSet<Status>(){Status.hurt,Status.death,Status.Fall} },
            {Status.death,new HashSet<Status>(){ } },
            {Status.block,new HashSet<Status>(){Status.hurt,Status.death,Status.Fall} }
        };
        public static bool IfCanTransit(Status Now,Status target)
        {
            bool output = dic[Now].Contains(target);
            return output;
        }
    }
} 
