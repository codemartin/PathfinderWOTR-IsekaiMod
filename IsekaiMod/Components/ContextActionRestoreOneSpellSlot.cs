using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;

namespace IsekaiMod.Components {
    /// <summary>
    /// Restores one expended spell slot on the caster: the highest spell level, up to MaxSpellLevel, that has
    /// a spent slot in any of the caster's spellbooks. Spontaneous casters get one use of that level back;
    /// prepared casters get the first spent memorised slot of that level back. Used by Shadow Reaping:
    /// Devour Core, whose text promises exactly this. Does nothing if every slot is still available.
    /// </summary>
    [TypeId("9c4e7b2a1d3f4a6b8e5c0d7f2b1a9e4c")]
    public class ContextActionRestoreOneSpellSlot : ContextAction {
        public int MaxSpellLevel = 5;

        public override string GetCaption() => $"Restore one spent spell slot (highest level up to {MaxSpellLevel})";

        public override void RunAction() {
            UnitEntityData caster = Context?.MaybeCaster;
            if (caster == null) return;
            for (int level = MaxSpellLevel; level >= 1; level--) {
                foreach (Spellbook book in caster.Descriptor.Spellbooks) {
                    if (book == null || level > book.MaxSpellLevel) continue;
                    if (book.Blueprint.Spontaneous) {
                        if (book.GetSpontaneousSlots(level) < book.GetSpellsPerDay(level)) {
                            book.RestoreSpontaneousSlots(level, 1);
                            return;
                        }
                    } else {
                        SpellSlot spent = book.GetMemorizedSpells(level).FirstOrDefault(s => s != null && !s.Available && s.SpellShell != null);
                        if (spent != null) {
                            spent.Available = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
