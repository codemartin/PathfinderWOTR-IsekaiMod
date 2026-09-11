using IsekaiMod.Utilities;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace IsekaiMod.Content.Features.IsekaiProtagonist
{
	public class ContextActionConsumeSkillBook : ContextAction
	{
		public BlueprintFeatureReference m_Feature;

		public string BookName;

		public override string GetCaption()
		{
			return "Consumes skill book and grants " + BookName;
		}

		public override void RunAction()
		{
			Player player = Game.Instance?.Player;
			UnitEntityData unitEntityData = base.Target.Unit ?? player?.SafeGetMainCharacter();
			BlueprintFeature feature = m_Feature?.Get();
			if (unitEntityData == null || feature == null)
			{
				return;
			}
			if (unitEntityData.Descriptor.Progression.Features.HasFact(feature))
			{
				EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
				{
					h.HandleLogMessage("<color=#DC143C>[Skill Book]</color> You have already absorbed the knowledge of " + BookName + "!");
				});
				return;
			}
			unitEntityData.Descriptor.AddFact(feature);
			EventBus.RaiseEvent(delegate(ILogMessageUIHandler h)
			{
				h.HandleLogMessage("<color=#FFD700>[Skill Book Learned]</color> You read <b>" + BookName + "</b> and permanently acquired: <b>" + feature.Name + "</b>!");
			});
		}
	}
}
