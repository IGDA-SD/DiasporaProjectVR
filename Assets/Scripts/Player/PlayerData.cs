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
using Assets.Scripts.Modules;
using UnityEngine;
using System.Collections;
using Assets.Scripts.StatusEffects;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Player
{
    public class PlayerData : MonoBehaviour
    {
        public EquipTable Equipment;

        public new Rigidbody rigidbody
        {
            get
            {
                return _rigidbody ?? (_rigidbody = GetComponent<Rigidbody>());
            }
        }
        private Rigidbody _rigidbody;
        public new Transform transform
        {
            get
            {
                return _transform ?? (_transform = GetComponent<Transform>());
            }
        }
        private Transform _transform;

        public Animator animator
        {
            get
            {
                return _animator ?? (_animator = GetComponent<Animator>());
            }
        }
        private Animator _animator;

        #region Mutable Data
        [Header("Mutable Values")]
        public int HP;
        public int FP;
        public int SP;
        public bool IsAlive
        {
        get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
                if (value == false)
                {                   
                    try
                    {
                        
                    }
                    catch (System.Exception e)
                    {
                        print("Has no death trigger");                        
                    }
                }
            }
        }
        private bool _isAlive;
        public TickEvent OnTick;
        internal Hashtable Durations;
        internal Hashtable Inventory;
        #endregion

        #region Derived Stats
        [Header("Derived Values")]
        [SerializeField]
        internal int Health;
        [SerializeField]
        internal int Focus;
        [SerializeField]
        internal int Stamina;
        #endregion

        #region Base Stats
        [Header("Base Stats")]
        [SerializeField]
        internal int Body;
        [SerializeField]
        internal int Mind;
        [SerializeField]
        internal int Spirit;
        #endregion

        #region Modules
        [Header("Movements")]
        public Locomotion Walk;
        public Locomotion Jump;
        public Locomotion Dash;        
        #endregion

        private void Awake()
        {
            this.BuildStats();
            this.Init();
            StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            var tick = new WaitForSeconds(2f);
            Durations = new Hashtable();
            while (IsAlive)
            {
                
                if (OnTick != null)
                {
                    var results = OnTick.GetInvocationList().Select(x => 
                    {
                        Durations[x] = x.DynamicInvoke(this, (int)Durations[x]);
                        return x;
                    }).Where(x => (int)Durations[x] == 0);
                    foreach (var result in results)
                    {
                        Durations.Remove(result);
                        OnTick -= (TickEvent)result;
                    }
                }
                yield return tick;
            }
        }

    }

    public static class PlayerDataExtensions
    {
        public static void WalkAction(this PlayerData player, Vector3 InputVector)
        {
            //Debug.Log(player.Walk.Move(InputVector).ToString());
            if (Mathf.Abs(player.rigidbody.velocity.y) < 0.01f)
                player.rigidbody.MovePosition(player.transform.position + player.Walk.Move(InputVector));
        }

        public static void JumpAction(this PlayerData player, Vector3 InputVector)
        {
            if (Mathf.Abs(player.rigidbody.velocity.y) < 0.01f)
                player.rigidbody.AddForce(player.Jump.Impulse(InputVector), ForceMode.Acceleration);
        }

        public static void DashAction(this PlayerData player, Vector3 InputVector)
        {
            player.rigidbody.AddForce(player.Dash.Impulse(InputVector), ForceMode.Acceleration);
        }

        public static void TakeDamage(this PlayerData player, Attack attack)
        {
            player.HP -= attack.HealthDamage;
            player.FP -= attack.FocusDamage;
            player.SP -= attack.StaminaDamage;
            if(attack.ExtraEffect != null) attack.ExtraEffect(player);
            attack.ExtraEffect = null;
            if(attack.Effects != null)
            {
                for (int i = 0; i < attack.Effects.Length; i++)
                {
                    player.ApplyStatusEfect(attack.Effects[i]);
                }
            }
            if (player.HP <= 0 || player.SP <= 0 || player.FP <= 0)
            {
                player.IsAlive = false;
                player.Animate(true);
            }
            else
            {
                player.Animate(false);
            }
        }

        public static void CriticalHit(this PlayerData player, Attack attack)
        {
            player.HP -= attack.HealthDamage * 2;
            player.FP -= attack.FocusDamage * 2;
            player.SP -= attack.StaminaDamage * 2;
            if (player.HP <= 0 || player.SP <= 0 || player.FP <= 0)
            {
                player.IsAlive = false;
                player.Animate(true);
            }
            else
            {
                player.Animate(false);
            }
        }

        public static void PayCost(this PlayerData player, Attack attack)
        {
            player.HP -= attack.HealthCost;
            player.FP -= attack.FocusCost;
            player.SP -= attack.StaminaCost;
            if (player.HP <= 0 || player.SP <= 0 || player.FP <= 0)
            {
                player.IsAlive = false;
                player.Animate(true);
            }
        }

        public static void Animate(this PlayerData player, bool isDeath)
        {
            if (player.animator != null)
            {
                if (isDeath)
                {
                    player.animator.SetTrigger("DeathTrigger");
                }
                else
                {
                    player.animator.SetTrigger("LightHitTrigger");
                }
            }
        }

        public static void BuildStats(this PlayerData player)
        {
            player.Health = player.Body * player.Spirit + player.Mind;
            player.Stamina = player.Mind * player.Body + player.Spirit;
            player.Focus = player.Mind * player.Spirit + player.Body;
        }

        public static void Init(this PlayerData player)
        {
            player.HP = player.Health;
            player.SP = player.Stamina;
            player.FP = player.Focus;
            player.IsAlive = true;
        }

        public static void ForceMove(this PlayerData player, Vector3 ForceVector)
        {
            player.rigidbody.AddRelativeForce(ForceVector,ForceMode.Acceleration);
        }

        public static void ImbueWeapon(this PlayerData player, Attack attack)
        {
            var a = attack;
            if (player.Equipment.RightHandObject != null)
            {
                object rHandWeapon = player.Equipment.RightHandObject.GetComponent<Weapon.Ranged>();
                if (rHandWeapon == null)
                {
                    rHandWeapon = player.Equipment.RightHandObject.GetComponent<Weapon.Melee>();
                    if (rHandWeapon != null)
                        ((Weapon.Melee)rHandWeapon).baseAttack.ExtraEffect += (p) => p.TakeDamage(a);
                }
                else
                {
                    ((Weapon.Ranged)rHandWeapon).attack.ExtraEffect += (p) => p.TakeDamage(a);
                }
            }
            if (player.Equipment.LeftHandObject != null)
            {
                object LHandWeapon = player.Equipment.LeftHandObject.GetComponent<Weapon.Ranged>();
                if (LHandWeapon == null)
                {
                    LHandWeapon = player.Equipment.LeftHandObject.GetComponent<Weapon.Melee>();
                    if (LHandWeapon != null)
                        ((Weapon.Melee)LHandWeapon).baseAttack.ExtraEffect += (p) => p.TakeDamage(a);
                }
                else
                {
                    ((Weapon.Ranged)LHandWeapon).attack.ExtraEffect += (p) => p.TakeDamage(a);
                }
            }
        }

        public static void ApplyStatusEfect(this PlayerData player, BaseStatusEffect effect)
        {            
            var eot = new TickEvent(effect.OnTick);
            if (player.Durations.Contains(eot))
            {
                player.Durations[eot] = (int)player.Durations[eot] + effect.Duration;
            }
            else
            {
                player.OnTick += new TickEvent(effect.OnTick);
                player.Durations.Add(new TickEvent(effect.OnTick), effect.Duration);
            }
        }

        public static void EquipItem(this PlayerData player, GameObject Item, EquipSlot Slot )
        {
            var reqs = Item.GetComponent<ItemModifiers.ItemRequirement>();
            if (reqs != null)
            {
                if (reqs.Body <= player.Body && reqs.Mind <= player.Mind && reqs.Spirit <= player.Spirit)
                {
                    Object.Destroy(Item.GetComponent<Rigidbody>());
                    switch (Slot)
                    {
                        case EquipSlot.None:
                            player.Inventory.Add(Item.name, Item);
                            break;
                        case EquipSlot.RightHand:
                            if (player.Equipment.RightHandObject != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.RightHandObject = Item;
                            break;
                        case EquipSlot.LeftHand:
                            if (player.Equipment.LeftHandObject != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.LeftHandObject = Item;
                            break;
                        case EquipSlot.DriveUp:
                            if (player.Equipment.Drives[0] != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.Drives[0] = Item;
                            break;
                        case EquipSlot.DriveRight:
                            if (player.Equipment.Drives[1] != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.Drives[1] = Item;
                            break;
                        case EquipSlot.DriveDown:
                            if (player.Equipment.Drives[2] != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.Drives[2] = Item;
                            break;
                        case EquipSlot.DriveLeft:
                            if (player.Equipment.Drives[3] != null)
                            {
                                player.Equipment.DropItem(Slot);
                            }
                            //To DO: attach the weapon to the skeleton
                            player.Equipment.Drives[3] = Item;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static void DropItem(this EquipTable table, EquipSlot slot)
        {
            GameObject item = null;
            switch (slot)
            {
                case EquipSlot.None:
                    Debug.Log("Invalid Selection");
                    //maybe drop item on ground instead of inventory for now all drop on ground
                    break;
                case EquipSlot.RightHand:
                    item = table.RightHandObject;
                    break;
                case EquipSlot.LeftHand:
                    item = table.LeftHandObject;
                    break;
                case EquipSlot.DriveUp:
                    item = table.Drives[0];
                    break;
                case EquipSlot.DriveRight:
                    item = table.Drives[1];
                    break;
                case EquipSlot.DriveDown:
                    item = table.Drives[2];
                    break;
                case EquipSlot.DriveLeft:
                    item = table.Drives[3];
                    break;
                default:
                    break;
            }
            if(item != null)
            {
                item.transform.parent = null;
                item.AddComponent<Rigidbody>();
            }
        }
    }
}
