# Alpha March 2026 build audit

Compared archive: `Alpha-March-2026-734-Alpha-Testing-1774207281-1.zip`

- Archive SHA-256: `B34C2EC7D87CDB86094DEC2D6E529B1396CD0045AED46DCD52E01D9C99F01724`
- `IsekaiMod.dll` version: `5.4.2`
- DLL SHA-256: `612B9CF0F73CAF516FBC507AD242E48CA9DBD5936896A8488B7B52F67F8CECE1`
- The packaged 59 assets match the maintained repository.
- The added-content settings match the maintained repository.
- The package has 691 configured blueprint IDs. The maintained repository has 700, with the additional IDs belonging to later fixes.

## Feature parity

`IsekaiKitsuneHeritage` was renamed to `IsekaiFurryHeritage`. Both use blueprint ID `f7131b02-7a40-4eac-914f-05bbeaac9582`, so the rename remains save compatible.

The March binary contains bloodline, oracle, shaman, and witch legacy helpers that are already represented in the maintained implementation. Its versions do not contain additional active features that should replace the maintained versions.

## Dormant prototypes

### Demon Lord heritage

`DemonLordHeritage.Add()` exists in the March binary but is never called by `ContentAdder.AddIsekaiHeritages()`. Its required `IsekaiDemonLordHeritage` blueprint ID is absent from the package configuration. Its description promises a daily charm ability, but the implementation only grants three wing abilities. Its statistics and resistances also substantially overlap the active Lust Demon heritage.

Status: acknowledged but not enabled. Enabling it without finishing its identity, charm ability, and blueprint configuration would expose incomplete content.

### Hidden Power dialogue duplicate

`HiddenPower.Add()` is empty, and its three private dialogue-building methods are never called. However, the same implementation is active under `IsekaiAneviaIrabethHarem.Add()` and is registered by `ContentAdder.AddIsekaiDialogue()`. The active sequence adds an Anevia answer, unlocks an Irabeth answer after selecting it, performs a Bluff DC 36 check with large experience, and adds a final Anevia answer after the successful Irabeth cue. The final cue increments the vanilla `IrabethConfidence` story flag by one.

Status: feature accounted for and enabled by the `Isekai Dialogue` setting. The unused `HiddenPower` class is a duplicate prototype, not missing content. The story-flag mutation remains a compatibility risk and should be tested through the Defender's Heart dialogue sequence before release.

## Package defects not imported

The March binary references nine blueprint keys missing from its own configuration: `Ascension`, `IsekaiDemonLordHeritage`, `MerchantsGambleResource`, `MindControlResource`, `PowerLevelingAura`, `PowerLevelingTempBuff`, `SummonExaltedChoirAbility`, `SuperStrength`, and `UnlimitedPowerResource`. Every active literal blueprint reference in the maintained source has a configured ID.

Several March implementations are older than maintained fixes and were not adopted:

- Perfect Roll uses broad statistic mutation that can contribute to level-up and combat lag.
- Crossbreed changes shared feat prerequisites globally instead of limiting the compatibility check to Harmony.
- Mind Control, Instakill, Power Leveling, Training Montage, Supreme Being, Extreme Speed, Ascension, and Deathsnatcher contain defects or incomplete behavior already addressed by later commits.
- Serious Strike, Invincibility, Super Speed, and Ascension contain description-to-effect mismatches.

## Adopted difference

The maintained Exalted Choir description listed a nymph, but its action spawned only the three azatas. Commit `963d5ea` adds the missing CR 7 nymph and updates the feature description.

The full decompilation is retained outside the repository under `outputs/comparisons/Alpha-March-2026-734` for future forensic comparison. Decompiled output is not treated as trusted source code.
