using System;
using UnityEngine;

namespace ThirdPersonCover
{
    /// <summary>
    /// Description of a weapon held by a CharacterMotor. 
    /// </summary>
    [Serializable]
    public struct WeaponDescription
    {
        /// <summary>
        /// Link to the weapon object containg a Gun component.
        /// </summary>
        [Tooltip("Link to the weapon object containg a Gun component.")]
        public GameObject Item;

        /// <summary>
        /// Link to the holstered weapon object which is made visible when the weapon is not used.
        /// </summary>
        [Tooltip("Link to the holstered weapon object which is made visible when the weapon is not used.")]
        public GameObject Holster;

        /// <summary>
        /// Defines character animations used with this weapon.
        /// </summary>
        [Tooltip("Defines character animations used with this weapon.")]
        public WeaponType Type;

        /// <summary>
        /// Shortcut for getting the gun component of the Item.
        /// </summary>
        public Gun Gun
        {
            get
            {
                return Item == null ? null : Item.GetComponent<Gun>();
            }
        }
    }

    /// <summary>
    /// Defines character animations used with a weapon.
    /// </summary>
    public enum WeaponType
    {
        Pistol,
        Rifle
    }

    /// <summary>
    /// Defines a set of character animator states used with a weapon.
    /// </summary>
    public struct WeaponAnimationStates
    {
        /// <summary>
        /// Name of the reload animator state.
        /// </summary>
        public string Reload;

        /// <summary>
        /// Name of animator states when a weapon is in full use.
        /// </summary>
        public string[] Common;

        /// <summary>
        /// Name of transitional animator states like equipping.
        /// </summary>
        public string[] Transitional;

        /// <summary>
        /// Returns default states used on a default CharacterMotor.
        /// </summary>
        public static WeaponAnimationStates[] Default()
        {
            var states = new WeaponAnimationStates[2];
            states[0] = Pistol();
            states[1] = Rifle();

            return states;
        }

        /// <summary>
        /// Returns animator states for a pistol.
        /// </summary>
        public static WeaponAnimationStates Pistol()
        {
            var states = new WeaponAnimationStates();
            states.Reload = "Pistol Reload";
            states.Common = new string[] { "Pistol", "Pistol Cover", "Empty Pistol state", "Pistol Jump" };
            states.Transitional = new string[] { "Equip Pistol" };

            return states;
        }

        /// <summary>
        /// Returns animator states for a rifle.
        /// </summary>
        public static WeaponAnimationStates Rifle()
        {
            var states = new WeaponAnimationStates();
            states.Reload = "Rifle Reload";
            states.Common = new string[] { "Rifle", "Rifle Cover", "Empty Rifle state", "Rifle Jump" };
            states.Transitional = new string[] { "Equip Rifle" };

            return states;
        }
    }
}