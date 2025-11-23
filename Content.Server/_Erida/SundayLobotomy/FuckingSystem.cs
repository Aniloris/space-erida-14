

using Content.Server._Erida.Arousal;
using Content.Server.Popups;
using Content.Shared._Erida.Arousal.Components;
using Content.Shared.Examine;
using Content.Shared.Humanoid;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

public sealed partial class FuckingSystem : EntitySystem
{
    [Dependency] private readonly PopupSystem _popupSystem = default!;
    [Dependency] private readonly ArousalSystem _arousalSystem = default!;
    [Dependency] private readonly ExamineSystemShared _examineSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ArousalComponent, GetVerbsEvent<Verb>>(OnGetVerbs);
    }

    private void OnGetVerbs(EntityUid uid, ArousalComponent component, GetVerbsEvent<Verb> args)
    {
        if (!_examineSystem.InRangeUnOccluded(args.User, args.Target, 3))
            return;

        args.Verbs.Add(new Verb
        {
            Act = () => FuckPlayer(args.User, args.Target),
            Priority = 15,
            Text = Loc.GetString("Трахнуть"),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/_Erida/SundayLobotomy/jokerge.png")),
            ClientExclusive = false
        });
    }

    private void FuckPlayer(EntityUid user, EntityUid target)
    {
        if (!_examineSystem.InRangeUnOccluded(user, target, 3))
            return;

        _arousalSystem.IncreaseArousal(target, 40f);
        _popupSystem.PopupEntity($"{MetaData(user).EntityName} трахнул {MetaData(target).EntityName}!", user, PopupType.MediumCaution);
    }
}

