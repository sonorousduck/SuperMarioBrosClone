using System;
using System.Collections.Generic;

namespace SequoiaEngine
{
    public class Cooldown
    {

        /// <summary>
        /// Current time that has elapsed
        /// </summary>
        public float CurrentTime;

        /// <summary>
        /// Max Cooldown time
        /// </summary>
        public float CooldownTime;

        /// <summary>
        /// What should happen when the cooldown starts? May not use this function.
        /// </summary>
        public Action OnCooldownStart;


        /// <summary>
        /// Called everytime the thing is updated. May not use this function. Parameter for how much time has elapsed
        /// </summary>
        public Action<float> OnCooldownUpdate;




        /// <summary>
        /// Used for when the cooldown is done.
        /// </summary>
        public Action OnCooldownEnd;


        /// <summary>
        /// Used to designate the cooldown as actually running and needs updating
        /// </summary>
        public bool IsRunning = false;


        /// <summary>
        /// This bool is what is changed to actually start the cooldown
        /// </summary>
        public bool ShouldStart = false;

        public Cooldown(float cooldownTime, Action onCooldownStart = null, Action<float> onCooldownUpdate = null, Action onCooldownEnd = null)
        {
            CooldownTime = cooldownTime;
            CurrentTime = 0f;
            OnCooldownStart = onCooldownStart;
            OnCooldownUpdate = onCooldownUpdate;
            OnCooldownEnd = onCooldownEnd;
        }

        public void StartCooldown()
        {
            if (!ShouldStart && !IsRunning)
            {
                ShouldStart = true;
            }
        }

    }

    /// <summary>
    /// An entity may have several cooldowns. (Weapons, abilities, etc.)
    /// This allows it all to be managed on a single entity
    /// </summary>
    public class CooldownCollection : Component
    {
        public Dictionary<string, Cooldown> Cooldowns;

        public CooldownCollection()
        {
            Cooldowns = new Dictionary<string, Cooldown>();
        }

        public void Add(string cooldownName, Cooldown cooldown)
        {
            Cooldowns.Add(cooldownName, cooldown);
        }

        public void Remove(string cooldownName)
        {
            Cooldowns.Remove(cooldownName);
        }
    }
}
