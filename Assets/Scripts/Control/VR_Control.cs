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
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.Weapon;
using Assets.Scripts.Modules;

namespace Assets.Scripts.Control
{
    public static class GatheredInputs
    {
        public static Vector2 JumpVector { get; set; }
        public static Vector2 MovementVector { get; set; }
        public static Vector2 DashVector { get; set; }

        public static int UseDrive { get; set; }
        public static bool FireWeapon { get; set; }
    }

    public class VR_Control : MonoBehaviour
    {
        public Transform HeadTrans;
        public PlayerData player;

        public bool isLocomotionController;
        private SteamVR_TrackedObject trackedObj;

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        private void Update()
        {
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            Vector2 tPad = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (isLocomotionController)
            {            
                GatheredInputs.JumpVector = device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) ? tPad : Vector2.zero;
                GatheredInputs.DashVector = device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) ? tPad : Vector2.zero;
                GatheredInputs.MovementVector = GatheredInputs.JumpVector == Vector2.zero && GatheredInputs.DashVector == Vector2.zero ? tPad : Vector2.zero;
            }
            else
            {
                if (tPad != Vector2.zero)
                {
                    var quadrant = Mathf.Atan2(((Vector2)tPad).y, ((Vector2)tPad).x);
                    if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                    {
                        if (quadrant > -Mathf.PI * 0.25f && quadrant < Mathf.PI * 0.25f)
                        {
                            GatheredInputs.UseDrive = 2;
                        }
                        else if (quadrant > Mathf.PI * 0.25f && quadrant < Mathf.PI * 0.75f)
                        {
                            //GatheredInputs.UseDrive = 1;
                            if (player.Equipment.Drives[0] != null) player.Equipment.Drives[0].GetComponent<Drive>().Activate(player);
                        }
                        else if (quadrant > -Mathf.PI * 0.75f && quadrant < -Mathf.PI * 0.75f)
                        {
                            GatheredInputs.UseDrive = 3;
                        }
                        else
                        {
                            GatheredInputs.UseDrive = 4;
                        }
                    }
                    else
                    {
                        GatheredInputs.UseDrive = 0;
                    }
                }

                if (device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    print("Tried to fire");
                    SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse((ushort)3555);
                    Ranged gun;
                    if((gun = player.Equipment.RightHandObject.GetComponent<Ranged>()))
                    {
                        gun.Fire();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (GatheredInputs.JumpVector != Vector2.zero)
            {
                //print("jumping" + " " + GatheredInputs.JumpVector);
                player.JumpAction(HeadTrans.PlanarToLocal(GatheredInputs.JumpVector));
            }
            else if(GatheredInputs.DashVector != Vector2.zero)
            {
                //print("Dashing" + " " + GatheredInputs.DashVector);
                player.DashAction(HeadTrans.PlanarToLocalForward(GatheredInputs.DashVector));
            }
            else if (GatheredInputs.MovementVector != Vector2.zero)
            {
                //print("Walking" + " " + GatheredInputs.MovementVector);
                player.WalkAction(HeadTrans.PlanarToLocal(GatheredInputs.MovementVector));
                //print(HeadTrans.PlanarToLocal(GatheredInputs.MovementVector));
            }
        }

    }

}
