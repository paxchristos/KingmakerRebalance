﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Recommendations;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;


namespace CallOfTheWild
{
    class Spiritualist
    {
        static LibraryScriptableObject library => Main.library;
        internal static bool test_mode = false;
        static public BlueprintCharacterClass spiritualist_class;
        static public BlueprintProgression spiritualist_progression;
        static public BlueprintFeature spiritualist_proficiencies;
        static public BlueprintFeature spiritualist_knacks;
        static public BlueprintFeatureSelection emotional_focus_selection;
        static public BlueprintFeature spiritualist_spellcasting;
        static public BlueprintFeature bonded_manifestation;

        static public BlueprintFeature link;
        static public BlueprintFeature etheric_tether;
        static public BlueprintFeature spiritual_bond;
        static public BlueprintFeature spiritual_inference;
        static public BlueprintFeature greater_spiritual_inference;

        static public BlueprintAbilityResource phantom_recall_resource;
        static public BlueprintFeature phantom_recall;
        static public BlueprintAbility phantom_recall_ability;
        static public BlueprintFeature fused_consciousness;
        static public BlueprintFeature dual_bond;
        static public BlueprintFeatureSelection potent_phantom;

        static public BlueprintFeature shared_consciousness;
        static public BlueprintBuff unsummon_buff;
        static public BlueprintAbility summon_companion_ability;
        static public BlueprintAbility summon_call_ability;

        //phantom blade or extoplasmotist
        //onymoji
        //necrologist
        //priest of the fallen
        //exciter

        internal static void createSpiritualistClass()
        {
            Main.logger.Log("Spiritualist class test mode: " + test_mode.ToString());
            var inquisitor_class = library.TryGet<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");

            spiritualist_class = Helpers.Create<BlueprintCharacterClass>();
            spiritualist_class.name = "SpiritualistClass";
            library.AddAsset(spiritualist_class, "");

            spiritualist_class.LocalizedName = Helpers.CreateString("Spiritualist.Name", "Spiritualist");
            spiritualist_class.LocalizedDescription = Helpers.CreateString("Spiritualsit.Description",
                                                                         "Becoming a spiritualist is not a calling — it’s a phenomenon.\n"
                                                                         + "When a creature dies, its spirit flees its body and begins the next stage of its existence. Debilitating emotional attachments during life and other psychic corruptions cause some spirits to drift into the Ethereal Plane and descend toward the Negative Energy Plane. Some of these spirits are able to escape the pull of undeath and make their way back to the Material Plane, seeking refuge in a psychically attuned mind. Such a fusing of consciousnesses creates a spiritualist—the master of a single powerful phantom whom the spiritualist can manifest to do her bidding.\n"
                                                                         + "Role: The spiritualist seeks the occult and esoteric truth about life, death, and the passage beyond, using her phantom as a guide and tool. The connection with her phantom allows her to harness the powers of life and death, thought and nightmare, shadow and revelation."
                                                                         );
            spiritualist_class.m_Icon = inquisitor_class.Icon;
            spiritualist_class.SkillPoints = inquisitor_class.SkillPoints - 1;
            spiritualist_class.HitDie = DiceType.D8;
            spiritualist_class.BaseAttackBonus = inquisitor_class.BaseAttackBonus;
            spiritualist_class.FortitudeSave = inquisitor_class.FortitudeSave;
            spiritualist_class.ReflexSave = inquisitor_class.ReflexSave;
            spiritualist_class.WillSave = inquisitor_class.WillSave;
            spiritualist_class.Spellbook = createSpiritualistSpellbook();
            spiritualist_class.ClassSkills = new StatType[] { StatType.SkillPersuasion, StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillPerception,
                                                         StatType.SkillUseMagicDevice};
            spiritualist_class.IsDivineCaster = false;
            spiritualist_class.IsArcaneCaster = false;
            spiritualist_class.StartingGold = inquisitor_class.StartingGold;
            spiritualist_class.PrimaryColor = inquisitor_class.PrimaryColor;
            spiritualist_class.SecondaryColor = inquisitor_class.SecondaryColor;
            spiritualist_class.RecommendedAttributes = new StatType[] { StatType.Wisdom };
            spiritualist_class.NotRecommendedAttributes = new StatType[0];
            spiritualist_class.EquipmentEntities = inquisitor_class.EquipmentEntities;
            spiritualist_class.MaleEquipmentEntities = inquisitor_class.MaleEquipmentEntities;
            spiritualist_class.FemaleEquipmentEntities = inquisitor_class.FemaleEquipmentEntities;
            spiritualist_class.ComponentsArray = inquisitor_class.ComponentsArray;
            spiritualist_class.StartingItems = new BlueprintItem[]
            {
                library.Get<BlueprintItemArmor>("afbe88d27a0eb544583e00fa78ffb2c7"), //studded leather
                library.Get<BlueprintItemWeapon>("1052a1f7128861942aa0c2ee6078531e"), //scythe
                library.Get<BlueprintItemWeapon>("511c97c1ea111444aa186b1a58496664"), //light crossbow
                library.Get<BlueprintItemEquipmentUsable>("807763fd874989e4d96eb2d8e234139e"), //shield scroll
                library.Get<BlueprintItemEquipmentUsable>("fe244c39bdd5cb64eae65af23c6759de"), //cause fear
                library.Get<BlueprintItemEquipmentUsable>("cd635d5720937b044a354dba17abad8d") //cure light wounds
            };

            createSpiritualistProgression();
            spiritualist_class.Progression = spiritualist_progression;
            spiritualist_class.Archetypes = new BlueprintArchetype[] { };
            Helpers.RegisterClass(spiritualist_class);
        }

        public static BlueprintCharacterClass[] getSpiritualistArray()
        {
            return new BlueprintCharacterClass[] { spiritualist_class };
        }


        static void createSpiritualistProgression()
        {
            createSpiritualistProficiencies();
            createSpiritualistKnacks();
            createPhantom(); //add fractured mind emotional power instead of spell like abilities
            createSummonUnsummonPhantom();
            //createLink(); //not sure if this one is needed since phantom are already weeker than 
            createEthericTether();
            createSpiritualBond();
            createSharedAndFusedConsciousness();
            createBondedManifestationAndDualBond();
            createSpiritualInferenceAndGreaterSpiritualInference();
            createPhantomRecall();
            createPotentPhantom();

            var detect_magic = library.Get<BlueprintFeature>("ee0b69e90bac14446a4cf9a050f87f2e");

            spiritualist_progression = Helpers.CreateProgression("SpiritualistProgression",
                                                              spiritualist_class.Name,
                                                              spiritualist_class.Description,
                                                              "",
                                                              spiritualist_class.Icon,
                                                              FeatureGroup.None);
            spiritualist_progression.Classes = getSpiritualistArray();

            spiritualist_progression.LevelEntries = new LevelEntry[] {Helpers.LevelEntry(1, spiritualist_proficiencies, spiritualist_spellcasting, detect_magic,
                                                                                            spiritualist_knacks, emotional_focus_selection, etheric_tether, shared_consciousness, /*link,*/
                                                                                        library.Get<BlueprintFeature>("d3e6275cfa6e7a04b9213b7b292a011c"), // ray calculate feature
                                                                                        library.Get<BlueprintFeature>("62ef1cdb90f1d654d996556669caf7fa")), // touch calculate feature                                                                                      
                                                                    Helpers.LevelEntry(2),
                                                                    Helpers.LevelEntry(3, bonded_manifestation),
                                                                    Helpers.LevelEntry(4, spiritual_inference),
                                                                    Helpers.LevelEntry(5),
                                                                    Helpers.LevelEntry(6, phantom_recall),
                                                                    Helpers.LevelEntry(7),
                                                                    Helpers.LevelEntry(8),
                                                                    Helpers.LevelEntry(9),
                                                                    Helpers.LevelEntry(10, fused_consciousness),
                                                                    Helpers.LevelEntry(11),
                                                                    Helpers.LevelEntry(12, greater_spiritual_inference),
                                                                    Helpers.LevelEntry(13),
                                                                    Helpers.LevelEntry(14, spiritual_bond),
                                                                    Helpers.LevelEntry(15 ),
                                                                    Helpers.LevelEntry(16),
                                                                    Helpers.LevelEntry(17, dual_bond),
                                                                    Helpers.LevelEntry(20, potent_phantom)
                                                                    };

            spiritualist_progression.UIDeterminatorsGroup = new BlueprintFeatureBase[] { spiritualist_proficiencies, spiritualist_spellcasting, detect_magic,
                                                                                            spiritualist_knacks, emotional_focus_selection };
            spiritualist_progression.UIGroups = new UIGroup[]  {Helpers.CreateUIGroup(etheric_tether, spiritual_inference, phantom_recall, spiritual_bond, greater_spiritual_inference, potent_phantom),
                                                            Helpers.CreateUIGroup(shared_consciousness, bonded_manifestation, fused_consciousness, dual_bond)
                                                           };
        }


        static void createBondedManifestationAndDualBond()
        {
            var bonded_manifestation_buff = Helpers.CreateBuff("BondedManifestationBuff",
                                                               "Bonded Manifestation",
                                                               "At 3rd level, as a swift action, a spiritualist can pull on the consciousness of her phantom and the substance of the Ethereal Plane to partially manifest aspects of both in her own body. When she does, she uses this bonded manifestation to enhance her own abilities while the phantom is still bound to her consciousness.\n"
                                                               + "For the spiritualist to use this ability, the phantom must be confined in the spiritualist’s consciousness; it can’t be manifested in any other way.\n"
                                                               + "During a bonded manifestation, the phantom can’t be damaged, dismissed, or banished. A spiritualist can use bonded manifestation a number of rounds per day equal to 3 + her spiritualist level. The rounds need not be consecutive. She can dismiss the effects of a bonded manifestation as a free action, but even if she dismisses a bonded manifestation on the same round that she used it, it counts as 1 round of use.\n"
                                                               + "Spiritualist gains an ectoplasmic shield that protects her without restricting her movement or actions. She gains a +4 shield bonus to Armor Class; this bonus applies to incorporeal touch attacks.\n"
                                                               + "The ectoplasmic shield has no armor check penalty or arcane spell failure chance. At 8th level, the spiritualist also sprouts a pair of ectoplasmic tendrils from her body. The spiritualist can use these tendrils to attack creatures within her melee reach (using the damage dice of her manifested phantom).\n"
                                                               + "At 13th level, the bonus from ectoplasmic shield increases to +6. At 18th level, the spiritualist can take a full-round action to attack all creatures within her melee reach with her tendrils (using the damage dice of her manifested phantom). When she does, she rolls the attack roll twice, takes the better of the two results, and uses that as her attack roll result against all creatures within her melee reach.",
                                                               "",
                                                               NewSpells.barrow_haze.Icon,
                                                               Common.createPrefabLink("e9a8af06810719e4d9885c10c827b131"), //from ghost form
                                                               Helpers.CreateAddContextStatBonus(StatType.AC, ModifierDescriptor.Shield),
                                                               Helpers.CreateContextRankConfig(ContextRankBaseValueType.ClassLevel, classes: getSpiritualistArray(),
                                                                                               progression: ContextRankProgression.Custom,
                                                                                               customProgression: new (int, int)[] { (12, 4), (20, 6) }
                                                                                               )
                                                               );

            DiceFormula[] diceFormulas = new DiceFormula[] {new DiceFormula(1, DiceType.D6),
                                                            new DiceFormula(1, DiceType.D8),
                                                            new DiceFormula(1, DiceType.D10),
                                                            new DiceFormula(2, DiceType.D6),
                                                            new DiceFormula(2, DiceType.D8)};

            var bonded_manifestation_buff8_1 = Helpers.CreateBuff("BondedManifestation81Buff",
                                                    "",
                                                    "",
                                                    "",
                                                    null,
                                                    null,
                                                    Common.createAddAdditionalLimb(library.Get<BlueprintItemWeapon>("767e6932882a99c4b8ca95c88d823137")),
                                                    Common.createAddAdditionalLimb(library.Get<BlueprintItemWeapon>("767e6932882a99c4b8ca95c88d823137")),
                                                    Helpers.Create<NewMechanics.ContextWeaponDamageDiceReplacementForSpecificCategory>(c =>
                                                                                                   {
                                                                                                       c.category = WeaponCategory.OtherNaturalWeapons;
                                                                                                       c.value = Helpers.CreateContextValue(AbilityRankType.Default);
                                                                                                       c.dice_formulas = diceFormulas;
                                                                                                   }),
                                                    Helpers.CreateContextRankConfig(baseValueType: ContextRankBaseValueType.ClassLevel,
                                                                                type: AbilityRankType.Default,
                                                                                progression: ContextRankProgression.DelayedStartPlusDivStep,
                                                                                startLevel: 5,
                                                                                stepLevel: 4,
                                                                                classes: getSpiritualistArray())
                                                   );
            bonded_manifestation_buff8_1.SetBuffFlags(BuffFlags.HiddenInUi);

            DiceFormula[] diceFormulas2 = new DiceFormula[] {new DiceFormula(1, DiceType.D8),
                                                            new DiceFormula(2, DiceType.D6),
                                                            new DiceFormula(2, DiceType.D8),
                                                            new DiceFormula(3, DiceType.D6),
                                                            new DiceFormula(3, DiceType.D8)};
            var slam = library.Get<BlueprintItemWeapon>("767e6932882a99c4b8ca95c88d823137");
            var bonded_manifestation_buff8_2 = Helpers.CreateBuff("BondedManifestation82Buff",
                                        "",
                                        "",
                                        "",
                                        null,
                                        null,
                                        Common.createAddAdditionalLimb(slam),
                                        Common.createAddAdditionalLimb(slam),
                                        Helpers.Create<NewMechanics.ContextWeaponDamageDiceReplacementForSpecificCategory>(c =>
                                        {
                                            c.category = WeaponCategory.OtherNaturalWeapons;
                                            c.value = Helpers.CreateContextValue(AbilityRankType.Default);
                                            c.dice_formulas = diceFormulas2;
                                        }),
                                        Helpers.CreateContextRankConfig(baseValueType: ContextRankBaseValueType.ClassLevel,
                                                                    type: AbilityRankType.Default,
                                                                    progression: ContextRankProgression.DelayedStartPlusDivStep,
                                                                    startLevel: 5,
                                                                    stepLevel: 4,
                                                                    classes: getSpiritualistArray())
                                       );
            bonded_manifestation_buff8_2.SetBuffFlags(BuffFlags.HiddenInUi);

            var bonded_manifestaion8_feature = Helpers.CreateFeature("BondedManifestation8Feature",
                                                                     "",
                                                                     "",
                                                                     "",
                                                                     null,
                                                                     FeatureGroup.None);
            bonded_manifestaion8_feature.HideInCharacterSheetAndLevelUp = true;
            bonded_manifestaion8_feature.HideInUI = true;
            var apply_slams = Helpers.CreateConditional(Common.createContextConditionCasterHasFact(Phantom.phantom_progressions["Anger"]),
                                                                                                    Common.createContextActionApplyBuff(bonded_manifestation_buff8_2, Helpers.CreateContextDuration(), is_child: true),
                                                                                                    Common.createContextActionApplyBuff(bonded_manifestation_buff8_1, Helpers.CreateContextDuration(), is_child: true)
                                                                                                    );
            Common.addContextActionApplyBuffOnConditionToActivatedAbilityBuffNoRemove(bonded_manifestation_buff,
                                                                                      Helpers.CreateConditional(Common.createContextConditionCasterHasFact(bonded_manifestaion8_feature),
                                                                                                                apply_slams
                                                                                                                )
                                                                                     );
            var buff_reroll_attacks = Helpers.CreateBuff("BondedManifestation13RerollAttacksBuff",
                                                         "",
                                                         "",
                                                         "",
                                                         null,
                                                         null,
                                                         Helpers.Create<ModifyD20>(m =>
                                                         {
                                                             m.Rule = RuleType.AttackRoll;
                                                             m.RollsAmount = 1;
                                                         })
                                                         );
            buff_reroll_attacks.SetBuffFlags(BuffFlags.HiddenInUi);
            var attack = Helpers.CreateAbility("BondedManifestationCleaveAbility",
                                               "Bonded Manifestation Attack",
                                                "At 18th level, while using bonded manifestation, the spiritualist can take a full-round action to attack all creatures within her melee reach with her tendrils (using the damage dice of her manifested phantom). When she does, she rolls the attack roll twice, takes the better of the two results, and uses that as her attack roll result against all creatures within her melee reach.",
                                                "",
                                                LoadIcons.Image2Sprite.Create(@"AbilityIcons/StormOfSouls.png"),
                                                AbilityType.Supernatural,
                                                CommandType.Standard,
                                                AbilityRange.Personal,
                                                "",
                                                "",
                                                Helpers.CreateRunActions(Common.createContextActionApplyBuff(buff_reroll_attacks, Helpers.CreateContextDuration(1)),
                                                                         Helpers.Create<TeamworkMechanics.ContextActionOnUnitsWithinRadius>(c =>
                                                                         {
                                                                             c.Radius = 7.Feet();
                                                                             var attack_action = Common.createContextActionAttack();
                                                                             attack_action.specific_weapon = slam;
                                                                             c.actions = Helpers.CreateActionList(Helpers.CreateConditional(Helpers.Create<ContextConditionIsEnemy>(),
                                                                                                                  attack_action)
                                                                                                                 );
                                                                         }
                                                                         ),
                                               Common.createContextActionRemoveBuff(buff_reroll_attacks)),
                                               Helpers.Create<NewMechanics.AttackAnimation>(),
                                               Common.createAbilityCasterHasFacts(bonded_manifestation_buff),
                                               Common.createAbilityAoERadius(20.Feet(), TargetType.Enemy)
                                               );
            attack.setMiscAbilityParametersSelfOnly();
            attack.NeedEquipWeapons = true;
            Common.setAsFullRoundAction(attack);
            var bonded_manifestaion18_feature = Helpers.CreateFeature("BondedManifestation18Feature",
                                                         "",
                                                         "",
                                                         "",
                                                         null,
                                                         FeatureGroup.None,
                                                         Helpers.CreateAddFact(attack));
            bonded_manifestaion18_feature.HideInCharacterSheetAndLevelUp = true;
            bonded_manifestaion18_feature.HideInUI = true;

            var bonded_manifestation_resource = Helpers.CreateAbilityResource("BondedManigfestationResource", "", "", "", null);
            bonded_manifestation_resource.SetIncreasedByLevel(3, 1, getSpiritualistArray());
            var toggle = Common.buffToToggle(bonded_manifestation_buff, CommandType.Swift, false,
                                             bonded_manifestation_resource.CreateActivatableResourceLogic(ResourceSpendType.NewRound),
                                             Helpers.Create<CompanionMechanics.ActivatableAbilityCompanionUnsummoned>()
                                             );

            bonded_manifestation = Common.ActivatableAbilityToFeature(toggle, false);
            bonded_manifestation.AddComponents(bonded_manifestation_resource.CreateAddAbilityResource(),
                                               Helpers.CreateAddFeatureOnClassLevel(bonded_manifestaion8_feature, 8, getSpiritualistArray()),
                                               Helpers.CreateAddFeatureOnClassLevel(bonded_manifestaion18_feature, 18, getSpiritualistArray())
                                              );

            dual_bond = Helpers.CreateFeature("DualBondSpiritualistFeature",
                                              "Dual Bond",
                                              "At 17th level, the spiritualist can use her bonded manifestation ability a number of rounds per day equal to 3 + twice her spiritualist level.",
                                              "",
                                              bonded_manifestation.Icon,
                                              FeatureGroup.None,
                                              Helpers.Create<IncreaseResourceAmountBySharedValue>(i => { i.Resource = bonded_manifestation_resource; i.Value = Helpers.CreateContextValue(AbilityRankType.Default);}),
                                              Helpers.CreateContextRankConfig(ContextRankBaseValueType.ClassLevel, classes: getSpiritualistArray())
                                              );
            dual_bond.ReapplyOnLevelUp = true;

        }


        static void createPhantom()
        {
            var phantom_rank_progression = library.CopyAndAdd<BlueprintProgression>("125af359f8bc9a145968b5d8fd8159b8", "SpiritualistPhantomProgression", "");
            phantom_rank_progression.Classes = getSpiritualistArray();
            emotional_focus_selection = Helpers.CreateFeatureSelection("PhantomFeatureSelection",
                                                                  "Phantom",
                                                                  "A spiritualist begins play with the aid of a powerful and versatile spirit entity called a phantom. The phantom forms a link with the spiritualist, who forever after can either harbor the creature within her consciousness or manifest it as an ectoplasmic or incorporeal entity. A phantom has the same alignment as the spiritualist, and it can speak all the languages its master can. A spiritualist can harbor her phantom in her consciousness (see the shared consciousness class feature), manifest it partially (see the bonded manifestation class feature), or fully manifest it. A fully manifested phantom is treated as a summoned creature from the Ethereal Plane, except it is not sent back to the Ethereal Plane until it is reduced to a negative amount of hit points equal to or greater than its Constitution score.\n"
                                                                  + "The phantom does not heal naturally, and can be healed only with magic or by being tended to with the Heal skill while fully manifested in ectoplasmic form. The phantom stays fully manifested until it is either returned to the spiritualist’s consciousness (a standard action) or banished to the Ethereal Plane. If the phantom is banished to the Ethereal Plane, it can’t return to the spiritualist’s consciousness or manifest again for 24 hours.\n"
                                                                  + "Fully manifested phantoms can use magic items (though not wield weapons) appropriate to their forms.\n"
                                                                  + "A fully manifested phantom’s abilities, feats, Hit Dice, saving throws, and skills are tied to the spiritualist’s class level and increase as the spiritualist gains levels.\n"
                                                                  + "Each phantom has an emotional focus—a powerful emotion based on some experience in life that keeps it tethered to the Material and Ethereal planes. This emotional focus also grants the phantom abilities that it can use while manifested. The type of each ability and its power are determined by the spiritualist’s level.\n"
                                                                  + "The emotional focus determines which bonus skill ranks the phantom gains, as well as the skills in which its spiritualist master gains Skill Focus. It also determines the good saving throws of the manifested phantom and the special abilities the phantom gains as it increases in level.\n"
                                                                  + "While phantoms tend to appear much as they did in life—at least as they did at the time of death— each emotional focus twists a phantom’s visage, mannerisms, and even personality in its own way. Unlike with most creatures, a phantom’s emotion aura often manifests for all to see, even those without the benefit of spells or abilities.\n"
                                                                  + "Often phantoms manifest these emotion auras in unique ways, some of which are described in individual emotional focus descriptions.",
                                                                  "",
                                                                  null,
                                                                  FeatureGroup.AnimalCompanion,
                                                                  Helpers.Create<AddFeatureOnApply>(a => a.Feature = phantom_rank_progression),
                                                                  Helpers.Create<AddFeatureOnApply>(a => a.Feature = library.Get<BlueprintFeature>("1670990255e4fe948a863bafd5dbda5d"))
                                                                  );

            Phantom.create();
            emotional_focus_selection.AllFeatures = new BlueprintFeature[] { library.Get<BlueprintFeature>("472091361cf118049a2b4339c4ea836a") }; //empty companion
            foreach (var kv in Phantom.phantom_progressions)
            {
                emotional_focus_selection.AllFeatures = emotional_focus_selection.AllFeatures.AddToArray(kv.Value);
            }
        }

        static void createPhantomRecall()
        {
            phantom_recall_resource = Helpers.CreateAbilityResource("PhantomRecallResource", "", "", "", null);
            phantom_recall_resource.SetIncreasedByLevelStartPlusDivStep(0, 6, 1, 4, 1, 0, 0.0f, getSpiritualistArray());


            var ability = library.CopyAndAdd<BlueprintAbility>("5bdc37e4acfa209408334326076a43bc", "PhantomRecallAbility", "");

            ability.Type = AbilityType.Supernatural;
            ability.Parent = null;
            ability.Range = AbilityRange.Personal;
            ability.RemoveComponents<SpellComponent>();
            ability.RemoveComponents<SpellListComponent>();
            ability.RemoveComponents<RecommendationNoFeatFromGroup>();
            ability.SetNameDescriptionIcon("Phantom Recall",
                                           "At 6th level, as a swift action, a spiritualist can call her manifested phantom to her side. This functions as dimension door, using the spiritualist’s caster level. When this ability is used, the phantom appears adjacent to the spiritualist (or as close as possible if all adjacent spaces are occupied). The spiritualist can use this ability once per day at 6th level, plus one additional time per day for every four levels beyond 6th.",
                                           Helpers.GetIcon("a5ec7892fb1c2f74598b3a82f3fd679f")); //stunning barrier

            var dimension_door_component = ability.GetComponent<AbilityCustomDimensionDoor>();

            var dimension_door_call = Helpers.Create<NewMechanics.CustomAbilities.AbilityCustomMoveCompanionToTarget>(a =>
            {
                a.CasterAppearFx = dimension_door_component.CasterAppearFx;
                a.CasterAppearProjectile = dimension_door_component.CasterAppearProjectile;
                a.CasterDisappearFx = dimension_door_component.CasterDisappearFx;
                a.CasterDisappearProjectile = dimension_door_component.CasterDisappearProjectile;
                a.PortalBone = dimension_door_component.PortalBone;
                a.PortalFromPrefab = dimension_door_component.PortalFromPrefab;
                a.Radius = 10.Feet();
                a.SideAppearFx = dimension_door_component.SideAppearFx;
                a.SideAppearProjectile = dimension_door_component.SideAppearProjectile;
                a.SideDisappearFx = dimension_door_component.SideDisappearFx;
                a.SideDisappearProjectile = dimension_door_component.SideDisappearProjectile;
            }
            );
            ability.ReplaceComponent(dimension_door_component, dimension_door_call);
            ability.AddComponent(phantom_recall_resource.CreateResourceLogic());
            ability.setMiscAbilityParametersSelfOnly();
            ability.AddComponent(Helpers.Create<NewMechanics.AbilityCasterCompanionDead>(a => a.not = true));

            phantom_recall = Common.AbilityToFeature(ability, false);
            phantom_recall.AddComponent(Helpers.CreateAddAbilityResource(phantom_recall_resource));
            phantom_recall_ability = ability;

            summon_call_ability = library.CopyAndAdd(ability, "PhantomSummonCallAbility", "");
            summon_call_ability.SetNameDescriptionIcon("Call Phantom", "", null);
            summon_call_ability.RemoveComponents<AbilityResourceLogic>();
            summon_call_ability.Parent = null;
            summon_companion_ability.ReplaceComponent<AbilityEffectRunAction>(a => a.Actions = Helpers.CreateActionList(a.Actions.Actions.AddToArray(Helpers.Create<ContextActionCastSpell>(c => c.Spell = summon_call_ability))));
        }


        static void createPotentPhantom()
        {
            potent_phantom = Helpers.CreateFeatureSelection("PotentPhantomFeatureSelection",
                                                            "Potent Phantom",
                                                            "At 20th level, the spiritualist’s phantom grows ever more complex and sophisticated in its manifestation. The phantom gains a second emotional focus. This does not change the phantom’s saving throws, but the phantom otherwise grants all the skills and powers of the focus.",
                                                            "",
                                                            Helpers.GetIcon("c60969e7f264e6d4b84a1499fdcf9039"),
                                                            FeatureGroup.None);

            foreach (var kv in Phantom.potent_phantom)
            {
                potent_phantom.AllFeatures = potent_phantom.AllFeatures.AddToArray(kv.Value);
            }
        }


        static void createSharedAndFusedConsciousness()
        {

            shared_consciousness = Helpers.CreateFeature("SharedConsciousnessFeature",
                                                         "Shared Consciousness",
                                                         "At 1st level, while a phantom is confined in a spiritualist’s consciousness (but not while it’s fully manifested or banished to the Ethereal Plane), it grants the spiritualist the Skill Focus feat in two skills determined by the phantom’s emotional focus, unless the spiritualist already has Skill Focus in those skills. It also grants a +4 bonus on saving throws against all mind - affecting effects; at 12th level, this bonus increases to + 8.",
                                                         "",
                                                         Helpers.GetIcon("b48674cef2bff5e478a007cf57d8345b"),
                                                         FeatureGroup.None);

            fused_consciousness = Helpers.CreateFeature("FusedConsciousnessFeature",
                                             "Fused Consciousness",
                                             "At 10th level, a spiritualist always gains the benefits of Shared Consciousness, even when her phantom is manifested.",
                                             "",
                                             Helpers.GetIcon("b48674cef2bff5e478a007cf57d8345b"),
                                             FeatureGroup.None,
                                             Common.createContextSavingThrowBonusAgainstDescriptor(Helpers.CreateContextValue(AbilityRankType.Default), ModifierDescriptor.UntypedStackable, SpellDescriptor.MindAffecting),
                                                      Helpers.CreateContextRankConfig(ContextRankBaseValueType.ClassLevel, classes: getSpiritualistArray(),
                                                                                      progression: ContextRankProgression.Custom,
                                                                                      customProgression: new (int, int)[] { (11, 4), (20, 8) })

                                            );

            foreach (var kv in Phantom.phantom_skill_foci)
            {
                foreach (var sf in kv.Value)
                {
                    fused_consciousness.AddComponents(Common.createAddFeatureIfHasFactAndNotHasFact(Phantom.phantom_progressions[kv.Key], sf, sf));  
                }
            }

            var shared_consciousness_buff = Helpers.CreateBuff("SharedConsciousnessBuff",
                                                   shared_consciousness.Name,
                                                   shared_consciousness.Description,
                                                   "",
                                                   shared_consciousness.Icon,
                                                   null,
                                                   fused_consciousness.ComponentsArray
                                                   );

            Common.addContextActionApplyBuffOnConditionToActivatedAbilityBuffNoRemove(unsummon_buff,
                                                                                      Helpers.CreateConditional(Common.createContextConditionHasFact(fused_consciousness, has: false),
                                                                                                                Common.createContextActionApplyBuff(shared_consciousness_buff, Helpers.CreateContextDuration(), is_child: true, is_permanent: true, dispellable: false)
                                                                                                                )
                                                                                                                );


        }

        static void createSummonUnsummonPhantom()
        {
            unsummon_buff = Helpers.CreateBuff("PhantomUnsummonedBuff",
                                       "Phantom Confined",
                                       "Your phantom is confined in your conciousness.",
                                       "",
                                       Helpers.GetIcon("4aa7942c3e62a164387a73184bca3fc1"), //disintegrate
                                       null,
                                       Helpers.CreateAddFactContextActions(activated: new GameAction[] { Helpers.Create<CompanionMechanics.ContextActionUnsummonCompanion>() },
                                                                           deactivated: new GameAction[] { Helpers.Create<CompanionMechanics.ContextActionSummonCompanion>()
                                                                           }
                                                                           )
                                      );
            unsummon_buff.SetBuffFlags(BuffFlags.RemoveOnRest);

            var unsummon_companion = Helpers.CreateAbility("SpiritUnsummonAbility",
                                                            "Confine Phantom",
                                                            "Confine your phantom in your consciousness.",
                                                            "",
                                                            unsummon_buff.Icon,
                                                            AbilityType.Supernatural,
                                                            CommandType.Standard,
                                                            AbilityRange.Personal,
                                                            "",
                                                            "",
                                                            Helpers.CreateRunActions(Common.createContextActionApplyBuff(unsummon_buff, Helpers.CreateContextDuration(), is_permanent: true, dispellable: false)),
                                                            Helpers.Create<NewMechanics.AbilityCasterCompanionDead>(a => a.not = true)
                                                            );

            unsummon_companion.setMiscAbilityParametersSelfOnly();
            var summon_companion = Helpers.CreateAbility("ManifestSpiritAbility",
                                                           "Manifest Phantom",
                                                           "Fully manifest your phantom.",
                                                           "",
                                                           unsummon_companion.Icon,
                                                           AbilityType.Supernatural,
                                                           CommandType.Standard,
                                                           AbilityRange.Personal,
                                                           "",
                                                           "",
                                                           Helpers.CreateRunActions(Helpers.Create<ContextActionRemoveBuff>(c => c.Buff = unsummon_buff)),
                                                           Helpers.Create<CompanionMechanics.AbilityCasterCompanionUnsummoned>());
            Common.setAsFullRoundAction(summon_companion);
            summon_companion.setMiscAbilityParametersSelfOnly();
            emotional_focus_selection.AddComponent(Helpers.CreateAddFacts(summon_companion, unsummon_companion));
            summon_companion_ability = summon_companion;
        }


        static void createSpiritualInferenceAndGreaterSpiritualInference()
        {
            var buff1 = Helpers.CreateBuff("SpiritualInferenceBuff",
                                              "",
                                              "",
                                              "",
                                              null,
                                              null,
                                              Helpers.CreateAddStatBonus(StatType.AC, 2, ModifierDescriptor.Shield),
                                              Helpers.CreateAddStatBonus(StatType.SaveFortitude, 2, ModifierDescriptor.Circumstance),
                                              Helpers.CreateAddStatBonus(StatType.SaveReflex, 2, ModifierDescriptor.Circumstance),
                                              Helpers.CreateAddStatBonus(StatType.SaveWill, 2, ModifierDescriptor.Circumstance)
                                              );
            var buff11 = library.CopyAndAdd<BlueprintBuff>(buff1, "GreaterSpiritualInferenceAllyBuff", "");
            var buff2 = Helpers.CreateBuff("GreaterSpiritualInferenceBuff",
                                  "",
                                  "",
                                  "",
                                  null,
                                  null, //shield spell
                                  Helpers.CreateAddStatBonus(StatType.AC, 4, ModifierDescriptor.Shield),
                                  Helpers.CreateAddStatBonus(StatType.SaveFortitude, 4, ModifierDescriptor.Circumstance),
                                  Helpers.CreateAddStatBonus(StatType.SaveReflex, 4, ModifierDescriptor.Circumstance),
                                  Helpers.CreateAddStatBonus(StatType.SaveWill, 4, ModifierDescriptor.Circumstance)
                                  );

            var shield_ally_eidolon = Common.createAuraEffectFeature("Spiritual Inference",
                                                                     "At 4th level, whenever a spiritualist is within the reach of her ectoplasmic manifested phantom, she gains a +2 shield bonus to her Armor Class and a +2 circumstance bonus on her saving throws. She doesn’t gain these bonuses when the ectoplasmic manifested phantom is grappled, helpless, or unconscious.",
                                                                     Helpers.GetIcon("ef768022b0785eb43a18969903c537c4"),
                                                                     buff1,
                                                                     10.Feet(),
                                                                     Helpers.CreateConditionsCheckerOr(Helpers.Create<NewMechanics.ContextConditionIsMaster>())
                                                                     );

            var add_shield_ally = Common.createAddFeatToAnimalCompanion(shield_ally_eidolon, "");
            add_shield_ally.HideInCharacterSheetAndLevelUp = true;

            spiritual_inference = Helpers.CreateFeature("SpiirtualInferenceFeature",
                                                shield_ally_eidolon.Name,
                                                shield_ally_eidolon.Description,
                                                "",
                                                shield_ally_eidolon.Icon,
                                                FeatureGroup.None,
                                                Helpers.CreateAddFeatureOnClassLevel(add_shield_ally, 12, getSpiritualistArray(), before: true)
                                                );

            var greater_shield_ally_eidolon = Common.createAuraEffectFeature("Greater Spiritual Inference",
                                                         "At 12th level, Whenever allies are within the phantom’s reach, as long as the manifested phantom is in ectoplasmic form, each ally gains a +2 shield bonus to its Armor Class and a +2 circumstance bonus on its saving throws. For the spiritualist, these bonuses increase to +4. The spiritualist and allies within range don’t gain this bonus if the manifested phantom is grappled, helpless, or unconscious. For the spiritualist, this bonus increases to +4. This bonus doesn’t apply if the phantom is unconscious.",
                                                         Helpers.GetIcon("ef768022b0785eb43a18969903c537c4"),
                                                         buff11,
                                                         10.Feet(),
                                                         Helpers.CreateConditionsCheckerAnd(Helpers.Create<ContextConditionIsAlly>(), Helpers.Create<NewMechanics.ContextConditionIsMaster>(c => c.Not = true), Helpers.Create<ContextConditionIsCaster>(c => c.Not = true))
                                                         );
            greater_shield_ally_eidolon.AddComponent(Common.createAuraEffectFeatureComponentCustom(buff2,
                                                                                                   10.Feet(),
                                                                                                   Helpers.CreateConditionsCheckerOr(Helpers.Create<NewMechanics.ContextConditionIsMaster>())));

            greater_spiritual_inference = Helpers.CreateFeature("GreaterSpiritualInferenceFeature",
                                                        greater_shield_ally_eidolon.Name,
                                                        greater_shield_ally_eidolon.Description,
                                                        "",
                                                        greater_shield_ally_eidolon.Icon,
                                                        FeatureGroup.None,
                                                        Helpers.Create<AddFeatureToCompanion>(a => a.Feature = greater_shield_ally_eidolon)
                                                        );
        }


        static void createEthericTether()
        {
            var etheric_tether_feature = Helpers.CreateFeature("SpiritualistEthericTetherFeature",
                                                                  "Etheric Tether",
                                                                  "Whenever her manifested phantom takes enough damage to send it back to the Ethereal Plane, as a reaction to the damage, the spiritualist can sacrifice any number of her hit points without using an action. Each hit point sacrificed in this way prevents 1 point of damage dealt to the phantom. This can prevent the phantom from being sent back to the Ethereal Plane.",
                                                                  "",
                                                                  Helpers.GetIcon("d5847cad0b0e54c4d82d6c59a3cda6b0"), //breath of life
                                                                  FeatureGroup.None,
                                                                  Helpers.Create<CompanionMechanics.TransferDamageToMaster>()
                                                                  );


            var buff = Helpers.CreateBuff("EthericTetherBuff",
                                          etheric_tether_feature.Name,
                                          etheric_tether_feature.Description,
                                          "",
                                          etheric_tether_feature.Icon,
                                          null,
                                          Helpers.Create<AddFeatureToCompanion>(a => a.Feature = etheric_tether_feature)
                                          );

            var toggle = Helpers.CreateActivatableAbility("SpiritualistEthericTetherAbility",
                                                          etheric_tether_feature.Name,
                                                          etheric_tether_feature.Description,
                                                          "",
                                                          etheric_tether_feature.Icon,
                                                          buff,
                                                          AbilityActivationType.Immediately,
                                                          CommandType.Free,
                                                          null);
            toggle.DeactivateImmediately = true;
            toggle.Group = ActivatableAbilityGroupExtension.EidolonLifeLink.ToActivatableAbilityGroup();
            etheric_tether = Common.ActivatableAbilityToFeature(toggle, false);
        }


        static void createSpiritualBond()
        {
            var buff = Helpers.CreateBuff("SpiritualBondBuff",
                                          "spiritual Bond",
                                          "At 14th level, a spiritualist’s life force becomes intrinsically linked with the phantom’s spiritual essence. As long as the phantom has 1 or more hit points, when the spiritualist takes damage that would reduce her to fewer than 0 hit points, those points of damage are transferred to the phantom instead. This transfer stops after the phantom takes all the points of damage or the phantom is reduced to a negative amount of hit points equal to its Constitution score. In the latter case, points of damage dealt in excess of this limit are dealt to the spiritualist. This ability affects only effects that deal hit point damage.",
                                          "",
                                          Helpers.GetIcon("7792da00c85b9e042a0fdfc2b66ec9a8"), //break enchantment
                                          null,
                                          Helpers.Create<CompanionMechanics.TransferDamageAfterThresholdToPet>(a => a.threshold = 1)
                                          );

            var toggle = Helpers.CreateActivatableAbility("SpiritualBondToggleAbility",
                                                          buff.Name,
                                                          buff.Description,
                                                          "",
                                                          buff.Icon,
                                                          buff,
                                                          AbilityActivationType.Immediately,
                                                          CommandType.Free,
                                                          null);
            toggle.DeactivateImmediately = true;
            toggle.Group = ActivatableAbilityGroupExtension.EidolonLifeLink.ToActivatableAbilityGroup();
            spiritual_bond = Common.ActivatableAbilityToFeature(toggle, false);
        }


        static void createSpiritualistProficiencies()
        {
            spiritualist_proficiencies = library.CopyAndAdd<BlueprintFeature>("25c97697236ccf2479d0c6a4185eae7f", //sorcerer proficiencies
                                                                "SpiritualistProficiencies",
                                                                "");
            spiritualist_proficiencies.SetName("Spiritualist Proficiencies");
            spiritualist_proficiencies.SetDescription("A spiritualist is proficient with all simple weapons, kukris and scythes, as well as with light armor.");
            spiritualist_proficiencies.ReplaceComponent<AddFacts>(a => a.Facts = new BlueprintUnitFact[] { a.Facts[0], library.Get<BlueprintFeature>("6d3728d4e9c9898458fe5e9532951132") });
        }


        static void createSpiritualistKnacks()
        {
            var daze = library.Get<BlueprintAbility>("55f14bc84d7c85446b07a1b5dd6b2b4c");
            spiritualist_knacks = Common.createCantrips("SpiritualistKnacksFeature",
                                                   "Knacks",
                                                   "A spiritualist learns a number of knacks, or 0-level psychic spells. These spells are cast like any other spell, but they are not expended when cast and may be used again.",
                                                   daze.Icon,
                                                   "",
                                                   spiritualist_class,
                                                   StatType.Wisdom,
                                                   spiritualist_class.Spellbook.SpellList.SpellsByLevel[0].Spells.ToArray());
        }


        static BlueprintSpellbook createSpiritualistSpellbook()
        {
            var inquisitor_class = ResourcesLibrary.TryGetBlueprint<BlueprintCharacterClass>("f1a70d9e1b0b41e49874e1fa9052a1ce");
            var spiritualist_spellbook = Helpers.Create<BlueprintSpellbook>();
            spiritualist_spellbook.name = "SpiritualistSpellbook";
            library.AddAsset(spiritualist_spellbook, "");
            spiritualist_spellbook.Name = spiritualist_class.LocalizedName;
            spiritualist_spellbook.SpellsPerDay = inquisitor_class.Spellbook.SpellsPerDay;
            spiritualist_spellbook.SpellsKnown = inquisitor_class.Spellbook.SpellsKnown;
            spiritualist_spellbook.Spontaneous = true;
            spiritualist_spellbook.IsArcane = false;
            spiritualist_spellbook.AllSpellsKnown = false;
            spiritualist_spellbook.CanCopyScrolls = false;
            spiritualist_spellbook.CastingAttribute = StatType.Wisdom;
            spiritualist_spellbook.CharacterClass = spiritualist_class;
            spiritualist_spellbook.CasterLevelModifier = 0;
            spiritualist_spellbook.CantripsType = CantripsType.Cantrips;
            spiritualist_spellbook.SpellsPerLevel = inquisitor_class.Spellbook.SpellsPerLevel;

            spiritualist_spellbook.SpellList = Helpers.Create<BlueprintSpellList>();
            spiritualist_spellbook.SpellList.name = "SpiritualistSpellList";
            library.AddAsset(spiritualist_spellbook.SpellList, "");
            spiritualist_spellbook.SpellList.SpellsByLevel = new SpellLevelList[10];
            for (int i = 0; i < spiritualist_spellbook.SpellList.SpellsByLevel.Length; i++)
            {
                spiritualist_spellbook.SpellList.SpellsByLevel[i] = new SpellLevelList(i);

            }

            Common.SpellId[] spells = new Common.SpellId[]
            {
                new Common.SpellId( "55f14bc84d7c85446b07a1b5dd6b2b4c", 0), //daze
                new Common.SpellId( "c3a8f31778c3980498d8f00c980be5f5", 0), //guidance
                new Common.SpellId( "95f206566c5261c42aa5b3e7e0d1e36c", 0), //mage light
                new Common.SpellId( "7bc8e27cba24f0e43ae64ed201ad5785", 0), //resistance
                new Common.SpellId( "5bf3315ce1ed4d94e8805706820ef64d", 0), //touch of fatigue
                new Common.SpellId( "d3a852385ba4cd740992d1970170301a", 0), //virtue

                new Common.SpellId( NewSpells.burst_of_adrenaline.AssetGuid, 1),
                new Common.SpellId( NewSpells.burst_of_insight.AssetGuid, 1),
                new Common.SpellId( "bd81a3931aa285a4f9844585b5d97e51", 1), //cause fear
                new Common.SpellId( NewSpells.chill_touch.AssetGuid, 1), 
                new Common.SpellId( "5590652e1c2225c4ca30c4a699ab3649", 1), //cure light wounds
                new Common.SpellId( "fbdd8c455ac4cde4a9a3e18c84af9485", 1), //doom
                new Common.SpellId( "4f8181e7a7f1d904fbaea64220e83379", 1), //expeditious retreat
                new Common.SpellId( "e5af3674bb241f14b9a9f6b0c7dc3d27", 1), //inflict light wounds
                new Common.SpellId( "9e1ad5d6f87d19e4d8883d63a6e35568", 1), //mage armor
                new Common.SpellId( "403cf599412299a4f9d5d925c7b9fb33", 1), //magic fang
                new Common.SpellId( NewSpells.obscuring_mist.AssetGuid, 1), 
                new Common.SpellId( "433b1faf4d02cc34abb0ade5ceda47c4", 1), //protection from alignment
                new Common.SpellId( "55a037e514c0ee14a8e3ed14b47061de", 1), //remove fear
                new Common.SpellId( NewSpells.sanctuary.AssetGuid, 1),
                new Common.SpellId( "ef768022b0785eb43a18969903c537c4", 1), //shield
                new Common.SpellId( "8fd74eddd9b6c224693d9ab241f25e84", 1), //summon monster 1
                new Common.SpellId( "ad10bfec6d7ae8b47870e3a545cc8900", 1), //touch of gracelessness

                new Common.SpellId( "03a9630394d10164a9410882d31572f0", 2), //aid
                new Common.SpellId( NewSpells.animate_dead_lesser.AssetGuid, 2),
                new Common.SpellId( "14ec7a4e52e90fa47a4c8d63c69fd5c1", 2), //blur
                new Common.SpellId( "b7731c2b4fa1c9844a092329177be4c3", 2),//boneshaker
                new Common.SpellId( "6b90c773a6543dc49b2505858ce33db5", 2), //cure moderate wounds
                new Common.SpellId( "7a5b5bf845779a941a67251539545762", 2), //false life
                new Common.SpellId( "446f7bf201dc1934f96ac0a26e324803", 2), //eagle's splendor
                new Common.SpellId( "e1291272c8f48c14ab212a599ad17aac", 2), //effortless armor
                new Common.SpellId( "7a5b5bf845779a941a67251539545762", 2), //false life
                new Common.SpellId( NewSpells.force_sword.AssetGuid, 2),
                new Common.SpellId( NewSpells.ghoul_touch.AssetGuid, 2),
                new Common.SpellId( "65f0b63c45ea82a4f8b8325768a3832d", 2), //inflict moderate wounds
                new Common.SpellId( NewSpells.inflict_pain.AssetGuid, 2),
                new Common.SpellId( "89940cde01689fb46946b2f8cd7b66b7", 2), //invisibility
                new Common.SpellId( "c28de1f98a3f432448e52e5d47c73208", 2), //protection from arrows
                new Common.SpellId( "e84fc922ccf952943b5240293669b171", 2), //restoration, lesser
                new Common.SpellId( "21ffef7791ce73f468b6fca4d9371e8b", 2), //resist energy
                new Common.SpellId( "08cb5f4c3b2695e44971bf5c45205df0", 2), //scare
                new Common.SpellId( "30e5dc243f937fc4b95d2f8f4e1b7ff3", 2), //see invisibility
                new Common.SpellId( NewSpells.stricken_heart.AssetGuid, 2),
                new Common.SpellId( "1724061e89c667045a6891179ee2e8e7", 2), //summon monster 2
                
                new Common.SpellId( "4b76d32feb089ad4499c3a1ce8e1ac27", 3), //animate dead
                new Common.SpellId( "989ab5c44240907489aba0a8568d0603", 3), //bestow curse
                new Common.SpellId( "46fd02ad56c35224c9c91c88cd457791", 3), //blindness
                new Common.SpellId( "3361c5df793b4c8448756146a88026ad", 3), //cure serious wounds
                new Common.SpellId( "92681f181b507b34ea87018e8f7a528a", 3), //dispel magic
                new Common.SpellId( "903092f6488f9ce45a80943923576ab3", 3), //displacement
                new Common.SpellId( NewSpells.fly.AssetGuid, 3),
                new Common.SpellId( "486eaff58293f6441a5c2759c4872f98", 3), //haste
                new Common.SpellId( "5ab0d42fb68c9e34abae4921822b9d63", 3), //heroism
                new Common.SpellId( NewSpells.howling_agony.AssetGuid, 3),
                new Common.SpellId( "bd5da98859cf2b3418f6d68ea66cabbe", 3), //inflict serious wounds
                new Common.SpellId( NewSpells.invisibility_purge.AssetGuid, 3),
                new Common.SpellId( "f1100650705a69c4384d3edd88ba0f52", 3), //magic fang, greater
                new Common.SpellId( NewSpells.pain_strike.AssetGuid, 3),
                new Common.SpellId( "d2f116cfe05fcdd4a94e80143b67046f", 3), //protection from energy
                new Common.SpellId( "c927a8b0cd3f5174f8c0b67cdbfde539", 3), //remove blindness
                new Common.SpellId( "4093d5a0eb5cae94e909eb1e0e1a6b36", 3), //remove disiese
                new Common.SpellId( NewSpells.ray_of_exhaustion.AssetGuid, 3),
                new Common.SpellId( NewSpells.rigor_mortis.AssetGuid, 3),
                new Common.SpellId( NewSpells.sands_of_time.AssetGuid, 3),
                new Common.SpellId( "f492622e473d34747806bdb39356eb89", 3), //slow
                new Common.SpellId( "8a28a811ca5d20d49a863e832c31cce1", 3), //vampyric touch
                new Common.SpellId( "5d61dde0020bbf54ba1521f7ca0229dc", 3), //summon monster 3

                new Common.SpellId( NewSpells.aura_of_doom.AssetGuid, 4),
                new Common.SpellId( "cf6c901fb7acc904e85c63b342e9c949", 4), //confusion
                new Common.SpellId( "4baf4109145de4345861fe0f2209d903", 4), //crushing despair
                new Common.SpellId( "41c9016596fe1de4faf67425ed691203", 4), //cure critical wounds
                new Common.SpellId( "e9cc9378fd6841f48ad59384e79e9953", 4), //death ward
                new Common.SpellId( "4a648b57935a59547b7a2ee86fb4f26a", 4), //dimensions door
                new Common.SpellId( "f34fb78eaaec141469079af124bcfa0f", 4), //enervation
                new Common.SpellId( "dc6af3b4fd149f841912d8a3ce0983de", 4), //false life, greater
                new Common.SpellId( "d2aeac47450c76347aebbc02e4f463e0", 4), //fear
                new Common.SpellId( "4c349361d720e844e846ad8c19959b1e", 4), //freedom of movement
                new Common.SpellId( "651110ed4f117a948b41c05c5c7624c0", 4), //inflict critical wounds
                new Common.SpellId( "ecaa0def35b38f949bd1976a6c9539e0", 4), //invisibility greater
                new Common.SpellId( "e7240516af4241b42b2cd819929ea9da", 4), //neutralize poison
                new Common.SpellId( "6717dbaef00c0eb4897a1c908a75dfe5", 4), //phantasmal killer
                new Common.SpellId( "b48674cef2bff5e478a007cf57d8345b", 4), //remove curse
                new Common.SpellId( "f2115ac1148256b4ba20788f7e966830", 4), //restoration
                new Common.SpellId( NewSpells.shadow_conjuration.AssetGuid, 4),
                new Common.SpellId( NewSpells.solid_fog.AssetGuid, 4),
                new Common.SpellId( "7ed74a3ec8c458d4fb50b192fd7be6ef", 4), //summon monster 4

                new Common.SpellId( "7792da00c85b9e042a0fdfc2b66ec9a8", 5), //break enchantment
                new Common.SpellId( "d5847cad0b0e54c4d82d6c59a3cda6b0", 5), //breath of life
                new Common.SpellId( "548d339ba87ee56459c98e80167bdf10", 5), //cloudkill
                new Common.SpellId( NewSpells.curse_major.AssetGuid, 5),
                new Common.SpellId( "95f7cdcec94e293489a85afdf5af1fd7", 5), //dismissal
                new Common.SpellId( "d7cbd2004ce66a042aeab2e95a3c5c61", 5), //dominate person
                new Common.SpellId( "444eed6e26f773a40ab6e4d160c67faa", 5), //feeblemind
                new Common.SpellId( NewSpells.fickle_winds.AssetGuid, 5),
                new Common.SpellId( NewSpells.inflict_pain_mass.AssetGuid, 5),
                new Common.SpellId( "eabf94e4edc6e714cabd96aa69f8b207", 5), //mind fog
                new Common.SpellId( NewSpells.overland_flight.AssetGuid, 5),
                new Common.SpellId( NewSpells.pain_strike_mass.AssetGuid, 5),
                new Common.SpellId( "12fb4a4c22549c74d949e2916a2f0b6a", 5), //phantasmal web
                new Common.SpellId( "a0fc99f0933d01643b2b8fe570caa4c5", 5), //raise dead
                new Common.SpellId( "237427308e48c3341b3d532b9d3a001f", 5), //shadow evocation
                new Common.SpellId( "4fbd47525382517419c66fb548fe9a67", 5), //slay living
                new Common.SpellId( "0a5ddfbcfb3989543ac7c936fc256889", 5), //spell resistance
                new Common.SpellId( NewSpells.suffocation.AssetGuid, 5),
                new Common.SpellId( "630c8b85d9f07a64f917d79cb5905741", 5), //summon monster 5
                new Common.SpellId( "a34921035f2a6714e9be5ca76c5e34b5", 5), //vampiric shadow shield
                new Common.SpellId( "8878d0c46dfbd564e9d5756349d5e439", 5), //waves of fatigue
                
                new Common.SpellId( "d361391f645db984bbf58907711a146a", 6), //banishment
                new Common.SpellId( "d42c6d3f29e07b6409d670792d72bc82", 6), //banshee blast
                new Common.SpellId( "a89dcbbab8f40e44e920cc60636097cf", 6), //circle of death
                new Common.SpellId( "76a11b460be25e44ca85904d6806e5a3", 6), //create undead
                new Common.SpellId( "f0f761b808dc4b149b08eaf44b99f633", 6), //dispel magic, greater
                new Common.SpellId( "4aa7942c3e62a164387a73184bca3fc1", 6), //disintegrate
                new Common.SpellId( "3167d30dd3c622c46b0c0cb242061642", 6), //eyebite
                new Common.SpellId( "cc09224ecc9af79449816c45bc5be218", 6), //harm
                new Common.SpellId( "5da172c4c89f9eb4cbb614f3a67357d3", 6), //heal
                new Common.SpellId( "e15e5e7045fda2244b98c8f010adfe31", 6), //heroism greater
                new Common.SpellId( "e740afbab0147944dab35d83faa0ae1c", 6), //summon monster 6
                new Common.SpellId( "27203d62eb3d4184c9aced94f22e1806", 6), //transformation     
                new Common.SpellId( "4cf3d0fae3239ec478f51e86f49161cb", 6), //true seeing
                new Common.SpellId( "474ed0aa656cc38499cc9a073d113716", 6), //umbral strike
            };

            foreach (var spell_id in spells)
            {
                var spell = library.Get<BlueprintAbility>(spell_id.guid);
                spell.AddToSpellList(spiritualist_spellbook.SpellList, spell_id.level);
            }

            spiritualist_spellbook.AddComponent(Helpers.Create<SpellbookMechanics.PsychicSpellbook>());

            spiritualist_spellcasting = Helpers.CreateFeature("SpiritualistSpellCasting",
                                             "Psychic Magic",
                                             "A spiritualist casts psychic spells drawn from the spiritualist spell list. She can cast any spell she knows without preparing it ahead of time, assuming she has not yet used up her allotment of spells per day for the spell’s level. To learn or cast a spell, a spiritualist must have a Wisdom score equal to at least 10 + the spell level. The Difficulty Class for a saving throw against a spiritualist’s spell equals 10 + the spell level + the spiritualist’s Wisdom modifier.\n"
                                             + "A spiritualist can cast only a certain number of spells of each spell level per day.",
                                             "",
                                             null,
                                             FeatureGroup.None);

            spiritualist_spellcasting.AddComponents(Helpers.Create<SpellFailureMechanics.PsychicSpellbook>(p => p.spellbook = spiritualist_spellbook),
                                               Helpers.CreateAddMechanics(AddMechanicsFeature.MechanicsFeatureType.NaturalSpell));
            spiritualist_spellcasting.AddComponent(Helpers.Create<SpellbookMechanics.AddUndercastSpells>(p => p.spellbook = spiritualist_spellbook));
            spiritualist_spellcasting.AddComponent(Helpers.CreateAddFact(Investigator.center_self));
            spiritualist_spellcasting.AddComponents(Common.createCantrips(spiritualist_class, StatType.Intelligence, spiritualist_spellbook.SpellList.SpellsByLevel[0].Spells.ToArray()));
            spiritualist_spellcasting.AddComponents(Helpers.CreateAddFacts(spiritualist_spellbook.SpellList.SpellsByLevel[0].Spells.ToArray()));

            return spiritualist_spellbook;
        }

    }
}