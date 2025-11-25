using Robust.Shared.GameStates;

namespace Content.Shared._Erida.Traits.Masochist
{
    /// <summary>
    /// Increases arousal when owner taking damage.
    /// </summary>
    [RegisterComponent, NetworkedComponent]
    public sealed partial class MasochistComponent : Component
    {
        /// The degree of damage after which arousal doesnt increase.
        [DataField]
        public float BluntDamageLimit = 25f;
        [DataField]
        public float ThermalDamageLimit = 20f;
        [DataField]
        public float PiercingDamageLimit = 20f;
        [DataField]
        public float ShockDamageLimit = 25f;

        /// <summary>
        /// Multiplier for arousal increase per unit of damage taken.
        /// </summary>
        [DataField]
        public float ArousalPerDamageModifier = 1f;

        /// <summary>
        /// The total degree of damage after which arousal doesnt increase.
        /// </summary>
        [DataField]
        public float TotalDamageLimit = 40f;
    }
}
