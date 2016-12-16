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
using System.Text;
using UnityEngine;
using Assets.Scripts.Modules;
using Assets.Scripts.Player;
namespace Assets.Scripts.Weapon
{
    public class Melee : MonoBehaviour
    {
        public Attack baseAttack;
        public PlayerData player;
        public SteamVR_TrackedObject tObject;
        public float MinCutDistance;
        public float MinHitVelocity;
        public float ArmorCrushVelocity;

        public float HitVelocity;

        private Vector3 CutStart;

        void OnTriggerEnter(Collider other)
        {
            if (MinHitVelocity != 0f)
            {
                if (this.CanHit(other))
                {
                    PlayerData Other;
                    if ((Other = other.transform.root.GetComponent<PlayerData>()) != null)
                    {
                        this.HitPlayer(player, Other);
                    }
                }
            }
            else
            {
                if (this.CanHit(other))
                {
                    CutStart = transform.position;
                }
                else
                {
                    CutStart = Vector3.zero;
                    //play the blocked sound
                }
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (CutStart != Vector3.zero)
            {
                var cDist = Vector3.Distance(transform.position, CutStart);
                if (cDist > MinCutDistance)//|| other is CapsuleCollider ? cDist > ((CapsuleCollider)other).radius : cDist > ((BoxCollider)other).size.x * 0.5f
                {
                    PlayerData Other;
                    if ((Other = other.transform.root.GetComponent<PlayerData>()) != null)
                    {
                        this.HitPlayer(player, Other);
                        //play hit sound
                    }
                    CutStart = Vector3.zero;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            CutStart = Vector3.zero;
        }

        void OnCollisionEnter(Collision col)
        {
            
        }

        void FixedUpdate()
        {
            HitVelocity = SteamVR_Controller.Input((int)tObject.index).velocity.magnitude;
        }

    }

    public static class MeleeExtensions
    {
        public static void HitPlayer(this Melee m, PlayerData owner, PlayerData otherPlayer)
        {
            otherPlayer.TakeDamage(m.baseAttack);
        }

        public static bool CanHit(this Melee m, Collider other)
        {
            if (other.tag != "Armor" && m.HitVelocity >= m.MinHitVelocity)
            {
                return true;
            }
            else if(m.HitVelocity >= m.ArmorCrushVelocity)
            {
                return true;
            }
            return false;
        }
    }
}
