///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright(c) 2016, Sidney Fernandez                                                                                                                                                                                                              //
// All rights reserved.                                                                                                                                                                                                                      //
//                                                                                                                                                                                                                                           //
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:                                                                                            //
//                                                                                                                                                                                                                                           //
// 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.                                                                                                           //
//                                                                                                                                                                                                                                           //
// 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.                             //
//                                                                                                                                                                                                                                           //
// 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.                                            //
//                                                                                                                                                                                                                                           //
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A                             //
// PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,                     //
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)   //
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                                                                                                                    //
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Player;
using System.Collections;

namespace Assets.Scripts.Modules
{
    public enum AreaType
    {
        Self,
        Line,
        Cone,
        Circle,
        Impact
    }
    public class Drive : MonoBehaviour
    {
        public Attack AttackEffect;
        public Locomotion MoveEffect;
        public Attack ResourceTransfer;

        public float coolDownTime;
        public float ActivationDelay;

        internal float LastActivation;

        public AreaType areaType;
    }

    public static class DriveExtensions
    {
        public static void Activate(this Drive d, PlayerData p)
        {
            if (d.LastActivation + d.coolDownTime < Time.timeSinceLevelLoad)
            {
                d.LastActivation = Time.deltaTime;
                d.StartCoroutine(DelayedActivate(d.ActivationDelay, () =>
                 {
                     p.PayCost(d.ResourceTransfer);
                     p.PayCost(d.AttackEffect);
                     p.ImbueWeapon(d.AttackEffect);
                     p.ForceMove(d.MoveEffect.ImpulseDirection * d.MoveEffect.ImpulseMagnitude);
                 }));
            }
        }

        public static IEnumerator DelayedActivate(float delay, Action Effect)
        {
            yield return new WaitForSeconds(delay);
            Effect();
        }
    }
}
