using IsekaiMod.Content.Constellations;
using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace IsekaiMod.Content.Features.IsekaiProtagonist;

[TypeId("06d7722d79ac4725a7e216d63596d8f1")]
public class ContextActionConsumeSkillBook : IsekaiContextAction
{
	public BlueprintFeatureReference m_Feature;

	public string BookName;

	public override string GetCaption()
	{
		return "Consumes skill book and grants " + BookName;
	}

	protected override void RunSafely()
	{
		Player player = Game.Instance?.Player;
		UnitEntityData unitEntityData = base.Target.Unit ?? player?.MainCharacter.Value;
		BlueprintFeature feature = m_Feature?.Get();
		if (unitEntityData == null || feature == null)
		{
			return;
		}
		if (unitEntityData.Descriptor.Progression.Features.HasFact(feature))
		{
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.SkillBook.AlreadyRead", "<color=#7A1010>[Skill Book]</color> You have already absorbed the knowledge of {0}!", ConstellationText.LocalizeIfKnown(BookName)));
			});
			return;
		}
		unitEntityData.Descriptor.AddFact(feature);
		EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
		{
			h.HandleLogMessage(ConstellationText.LocalizedFormat("Constellation.SkillBook.Learned", "<color=#5B2A86>[Skill Book Learned]</color> You read <b>{0}</b> and permanently acquired: <b>{1}</b>!", ConstellationText.LocalizeIfKnown(BookName), feature.Name));
		});
	}
}
