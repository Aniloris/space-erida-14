using Content.Server._Erida.Arousal;
using Content.Shared._Erida.Traits.Masochist;
using Content.Shared.Damage;
using Content.Shared.Mobs.Components;

namespace Content.Server._Erida.Traits.Masochist;

public sealed class MasochistSystem : EntitySystem
{
    [Dependency] private readonly ArousalSystem _arousalSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MasochistComponent, DamageChangedEvent>(OnDamageExamine);
    }

    public void OnDamageExamine(Entity<MasochistComponent> ent, ref DamageChangedEvent args)
    {
        if (!args.DamageIncreased || args.DamageDelta == null)
            return;

        if (!HasComp<MobStateComponent>(ent.Owner))
            return;

        var damageable = CompOrNull<DamageableComponent>(ent.Owner);
        if (damageable == null || damageable.TotalDamage >= ent.Comp.TotalDamageLimit)
            return;

        // TODO add debaff after total damage limit is reached
        foreach (var damage in args.DamageDelta.DamageDict)
        {
            switch (damage.Key)
            {
                case "Blunt" when damageable.Damage["Blunt"] >= ent.Comp.BluntDamageLimit:
                    continue;
                case "Thermal" when damageable.Damage["Thermal"] >= ent.Comp.ThermalDamageLimit:
                    continue;
                case "Piercing" when damageable.Damage["Piercing"] >= ent.Comp.PiercingDamageLimit:
                    continue;
                case "Shock" when damageable.Damage["Shock"] >= ent.Comp.ShockDamageLimit:
                    continue;
                default:
                    _arousalSystem.IncreaseArousal(ent.Owner, damage.Value.Float() * ent.Comp.ArousalPerDamageModifier);
                    break;
            }
        }
    }
}
