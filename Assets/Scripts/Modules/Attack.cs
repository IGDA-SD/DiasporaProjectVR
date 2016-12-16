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
using Assets.Scripts.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    [Serializable]
    public class Attack
    {
        public int HealthDamage;
        public int FocusDamage;
        public int StaminaDamage;

        public int HealthCost;
        public int FocusCost;
        public int StaminaCost;

        public float Range;

        public BaseStatusEffect[] Effects;
        [HideInInspector]
        public delegate void OnHitEffect(Player.PlayerData player);
        [HideInInspector]
        public OnHitEffect ExtraEffect;
        /// <summary>
        /// USE ONLY FOR GLYPHS
        /// </summary>
        /// <param name="a1">non-secondary effect</param>
        /// <param name="a2">secondary glyph effect</param>
        /// <returns></returns>
        public static Attack operator +(Attack a1, Attack a2)
        {
            var t = new Attack();
            t.HealthDamage = Mathf.Abs(a1.HealthDamage) + Mathf.Abs(a2.HealthDamage) * (a2.HealthDamage < 0 ? -1 : 1);
            t.FocusDamage = Mathf.Abs(a1.FocusDamage) + Mathf.Abs(a2.FocusDamage) * (a2.FocusDamage < 0 ? -1 : 1);
            t.StaminaDamage = Mathf.Abs(a1.StaminaDamage) + Mathf.Abs(a2.StaminaDamage) * (a2.StaminaDamage < 0 ? -1 : 1);
            t.HealthCost = a1.HealthCost + a2.HealthCost;
            t.FocusCost = a1.FocusCost + a2.FocusCost;
            t.StaminaCost = a1.StaminaCost + a2.StaminaCost;
            t.Range = a1.Range > a2.Range ? a1.Range : a2.Range;
            if (a1.Effects != null)
                if (a2.Effects != null)
                    t.Effects = a1.Effects.Concat(a2.Effects).ToArray();
                else
                    t.Effects = a1.Effects;
            else
                if (a2.Effects != null)
                t.Effects = a2.Effects;
            return t;
        }
    }

}
