
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared.Nutrition.Components;
using Content.Shared.Smoking;

namespace Content.Server._Erida.SundayLobotomy.BetterSmoke;

public sealed partial class BetterSmokeSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphere = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<SmokableComponent>();
        while (query.MoveNext(out var uid, out var smokable))
        {
            if (smokable.State == SmokableState.Lit)
            {
                var environment = _atmosphere.GetContainingMixture((uid, Transform(uid)), true, true);
                var merger = new GasMixture(0.1f) {Temperature = environment!.Temperature};
                merger.SetMoles(5, 0.03f); // 5 - water vapor
                _atmosphere.Merge(environment!, merger);
            }
        }
    }
}
