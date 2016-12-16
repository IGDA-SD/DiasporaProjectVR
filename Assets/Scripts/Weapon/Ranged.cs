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
    public class Ranged : MonoBehaviour
    {
        public new Transform transform
        {
            get
            {
                return _transform ?? (_transform = GetComponent<Transform>());
            }
        }
        private Transform _transform;
        public Attack attack;
        public PlayerData player;
        public LineRenderer lRend;

    }

    public static class RangedExtensions
    {
        public static void Fire(this Ranged r)
        {
            Debug.Log("Static call to fire");
            RaycastHit hit;
            Debug.DrawRay(r.transform.position, r.transform.forward * r.attack.Range, Color.red, 15f);
            if(Physics.Raycast(r.transform.position, r.transform.forward, out hit, r.attack.Range))
            {
                Debug.Log("Raycast hit something");
                r.lRend.SetPositions(new Vector3[] { r.transform.position, hit.point != Vector3.zero ? hit.point : r.transform.forward.normalized * r.attack.Range });
                r.lRend.enabled = true;
                if (hit.collider.tag == "Character")
                {
                    hit.transform.root.GetComponent<PlayerData>().TakeDamage(r.attack);
                }
                else if (hit.collider.tag == "Weakness")
                {
                    hit.transform.root.GetComponent<PlayerData>().CriticalHit(r.attack);
                }
            }
        }
        
    }
}
