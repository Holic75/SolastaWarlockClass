using SolastaModApi;
using SolastaModApi.Extensions;
using SolastaModHelpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using static FeatureDefinitionSavingThrowAffinity;

using Helpers = SolastaModHelpers.Helpers;
using NewFeatureDefinitions = SolastaModHelpers.NewFeatureDefinitions;
using ExtendedEnums = SolastaModHelpers.ExtendedEnums;

namespace SolastaWarlockClass
{
    internal class WarlockClassBuilder : CharacterClassDefinitionBuilder
    {
        const string WarlockClassName = "WarlockClass";
        const string WarlockClassNameGuid = "bc097115-5bea-41cd-a5b5-f8f4ceeec00b";
        const string WarlockClassSubclassesGuid = "4697f47d-2ad7-4aec-a6b0-682ebf0d3fd5";

       
        static public CharacterClassDefinition warlock_class;
        static public SpellListDefinition warlock_spelllist;

        static public NewFeatureDefinitions.WarlockCastSpell warlock_spellcasting;
        static public Dictionary<int, FeatureDefinitionFeatureSet> invocations = new Dictionary<int, FeatureDefinitionFeatureSet>();
        static public FeatureDefinition agonizing_blast;
        static public FeatureDefinitionBonusCantrips repelling_blast;
        static public FeatureDefinition miring_blast;
        static public FeatureDefinitionBonusCantrips alien_ectoplasm;
        static public FeatureDefinitionBonusCantrips armor_of_shadow;
        static public FeatureDefinitionBonusCantrips eldritch_sight;
        static public FeatureDefinitionBonusCantrips fiendish_vigor;
        static public FeatureDefinitionBonusCantrips otherworldy_leap;
        static public FeatureDefinitionBonusCantrips ascendant_step;
        static public Dictionary<int, NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection> book_of_secrets = new Dictionary<int, NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection>();
        static public FeatureDefinitionProficiency beguiling_influence;
        static public FeatureDefinitionFeatureSet devils_sight;
       

        static public FeatureDefinitionFeatureSet pact_boon;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection pact_of_tome;
        static public FeatureDefinitionFeatureSet pact_of_blade;
        static public FeatureDefinitionFeatureSet pact_of_arrow;
        //static public FeatureDefinitionFeatureSet pact_of_aegis;


        static NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects eldritch_blast;
        static NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects repelling_eldritch_blast;

        static public NewFeatureDefinitions.AttackModeExtraMainAttackWithSpecificWeaponType thirsting_blade;
        static public NewFeatureDefinitions.AttackModeExtraMainAttackWithSpecificWeaponType eldritch_archery;
        static public NewFeatureDefinitions.PowerWithRestrictions eldritch_smite;
        //invocations: 
        //blade and bolt  - bonus action attack after cantrip
        static List<string> blade_pact_weapon_types = new List<string>()
                                                        {
                                                            Helpers.WeaponProficiencies.QuarterStaff,
                                                            Helpers.WeaponProficiencies.Spear,
                                                            Helpers.WeaponProficiencies.Mace,
                                                            Helpers.WeaponProficiencies.Dagger,
                                                            Helpers.WeaponProficiencies.Handaxe,
                                                            Helpers.WeaponProficiencies.Club,
                                                            Helpers.WeaponProficiencies.LongSword,
                                                            Helpers.WeaponProficiencies.Rapier,
                                                            Helpers.WeaponProficiencies.ShortSword,
                                                            Helpers.WeaponProficiencies.GreatSword,
                                                            Helpers.WeaponProficiencies.Scimitar,
                                                            Helpers.WeaponProficiencies.MorningStar,
                                                            Helpers.WeaponProficiencies.BattleAxe,
                                                            Helpers.WeaponProficiencies.Warhammer
                                                        };
        static List<string> arrow_pact_weapon_types = new List<string>()
                                                        {
                                                            Helpers.WeaponProficiencies.Longbow,
                                                            Helpers.WeaponProficiencies.Shortbow
                                                        };



        //patrons
        //FIend
        static public FeatureDefinitionMagicAffinity fiend_spells;
        static public NewFeatureDefinitions.InitiatorApplyPowerToSelfOnTargetSlain dark_ones_blessing;
        static public NewFeatureDefinitions.RerollFailedSavePower dark_ones_own_luck;
        static public FeatureDefinitionFeatureSet fiendish_resilence;


        protected WarlockClassBuilder(string name, string guid) : base(name, guid)
        {
            var warlock_class_image = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("WarlockClassImage",
                                                                                           $@"{UnityModManagerNet.UnityModManager.modsPath}/SolastaWarlockClass/Sprites/WarlockClass.png",
                                                                                           1024, 576);
            var sorcerer = DatabaseHelper.CharacterClassDefinitions.Sorcerer;
            warlock_class = Definition;
            Definition.GuiPresentation.Title = "Class/&WarlockClassTitle";
            Definition.GuiPresentation.Description = "Class/&WarlockClassDescription";
            Definition.GuiPresentation.SetSpriteReference(warlock_class_image);

            Definition.SetClassAnimationId(AnimationDefinitions.ClassAnimationId.Sorcerer);
            Definition.SetClassPictogramReference(sorcerer.ClassPictogramReference);
            Definition.SetDefaultBattleDecisions(sorcerer.DefaultBattleDecisions);
            Definition.SetHitDice(RuleDefinitions.DieType.D8);
            Definition.SetIngredientGatheringOdds(sorcerer.IngredientGatheringOdds);
            Definition.SetRequiresDeity(false);

            Definition.AbilityScoresPriority.Clear();
            Definition.AbilityScoresPriority.AddRange(new List<string> {Helpers.Stats.Charisma,
                                                                        Helpers.Stats.Dexterity,
                                                                        Helpers.Stats.Constitution,
                                                                        Helpers.Stats.Intelligence,
                                                                        Helpers.Stats.Strength,
                                                                        Helpers.Stats.Wisdom});

            Definition.FeatAutolearnPreference.AddRange(sorcerer.FeatAutolearnPreference);
            Definition.PersonalityFlagOccurences.AddRange(sorcerer.PersonalityFlagOccurences);

            Definition.SkillAutolearnPreference.Clear();
            Definition.SkillAutolearnPreference.AddRange(new List<string> { Helpers.Skills.Deception,
                                                                            Helpers.Skills.Intimidation,
                                                                            Helpers.Skills.Arcana,
                                                                            Helpers.Skills.History,
                                                                            Helpers.Skills.Investigation,
                                                                            Helpers.Skills.Religion,
                                                                            Helpers.Skills.Nature,
                                                                            Helpers.Skills.Persuasion});

            Definition.ToolAutolearnPreference.Clear();
            Definition.ToolAutolearnPreference.AddRange(new List<string> { Helpers.Tools.EnchantingTool, Helpers.Tools.HerbalismKit });


            Definition.EquipmentRows.AddRange(sorcerer.EquipmentRows);
            Definition.EquipmentRows.Clear();

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.LightCrossbow, EquipmentDefinitions.OptionWeapon, 1),
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Bolt, EquipmentDefinitions.OptionAmmoPack, 20),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.LightCrossbow, EquipmentDefinitions.OptionWeaponSimpleChoice, 1),
                                    }
            );
            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ScholarPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    },
                                new List<CharacterClassDefinition.HeroEquipmentOption>
                                    {
                                        EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.DungeoneerPack, EquipmentDefinitions.OptionStarterPack, 1),
                                    }

            );

            this.AddEquipmentRow(new List<CharacterClassDefinition.HeroEquipmentOption>
            {
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Leather, EquipmentDefinitions.OptionArmor, 1),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.ComponentPouch, EquipmentDefinitions.OptionFocus, 1),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Dagger, EquipmentDefinitions.OptionWeapon, 2),
                EquipmentOptionsBuilder.Option(DatabaseHelper.ItemDefinitions.Dagger, EquipmentDefinitions.OptionWeaponSimpleChoice, 1),
            });

            var saving_throws = Helpers.ProficiencyBuilder.CreateSavingthrowProficiency("WarlockSavingthrowProficiency",
                                                                                        "",
                                                                                        Helpers.Stats.Charisma, Helpers.Stats.Wisdom);

            var armor_proficiency = Helpers.ProficiencyBuilder.createCopy("WarlockArmorProficiency",
                                                                          "",
                                                                          "Feature/&WarlockArmorProficiencyTitle",
                                                                          "",
                                                                          DatabaseHelper.FeatureDefinitionProficiencys.ProficiencyRogueArmor
                                                                          );

            var weapon_proficiency = Helpers.ProficiencyBuilder.createCopy("WarlockWeaponProficiency",
                                                                          "",
                                                                          "Feature/&WarlockWeaponProficiencyTitle",
                                                                          "",
                                                                          DatabaseHelper.FeatureDefinitionProficiencys.ProficiencySorcererWeapon
                                                                          );

            var skills = Helpers.PoolBuilder.createSkillProficiency("WarlockSkillProficiency",
                                                                    "",
                                                                    "Feature/&WarlockClassSkillPointPoolTitle",
                                                                    "Feature/&SkillGainChoicesPluralDescription",
                                                                    2,
                                                                    Helpers.Skills.Arcana, Helpers.Skills.Deception, Helpers.Skills.History,
                                                                    Helpers.Skills.Intimidation, Helpers.Skills.Investigation, Helpers.Skills.Nature,
                                                                    Helpers.Skills.Religion
                                                                    );

            createEldritchBlast();

            warlock_spelllist = Helpers.SpelllistBuilder.create9LevelSpelllist("WarlockClassSpelllist", "", "",
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.ChillTouch,
                                                                                    eldritch_blast,
                                                                                    DatabaseHelper.SpellDefinitions.DancingLights,
                                                                                    DatabaseHelper.SpellDefinitions.Dazzle,
                                                                                    DatabaseHelper.SpellDefinitions.PoisonSpray,
                                                                                    SolastaExtraContent.Cantrips.sunlight_blade,
                                                                                    DatabaseHelper.SpellDefinitions.TrueStrike,
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.CharmPerson,
                                                                                    DatabaseHelper.SpellDefinitions.ComprehendLanguages,
                                                                                    DatabaseHelper.SpellDefinitions.ExpeditiousRetreat,
                                                                                    //hellish rebuke
                                                                                    DatabaseHelper.SpellDefinitions.ProtectionFromEvilGood,
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Darkness,
                                                                                    DatabaseHelper.SpellDefinitions.HoldPerson,
                                                                                    DatabaseHelper.SpellDefinitions.Invisibility,
                                                                                    //DatabaseHelper.SpellDefinitions.MirrorImage,
                                                                                    DatabaseHelper.SpellDefinitions.MistyStep,
                                                                                    DatabaseHelper.SpellDefinitions.RayOfEnfeeblement,
                                                                                    DatabaseHelper.SpellDefinitions.Shatter,
                                                                                    DatabaseHelper.SpellDefinitions.SpiderClimb
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Counterspell,
                                                                                    DatabaseHelper.SpellDefinitions.DispelMagic,
                                                                                    DatabaseHelper.SpellDefinitions.Fear,
                                                                                    DatabaseHelper.SpellDefinitions.Fly,
                                                                                    DatabaseHelper.SpellDefinitions.HypnoticPattern,
                                                                                    DatabaseHelper.SpellDefinitions.RemoveCurse,
                                                                                    DatabaseHelper.SpellDefinitions.Tongues,
                                                                                    DatabaseHelper.SpellDefinitions.VampiricTouchIntelligence
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.Banishment,
                                                                                    DatabaseHelper.SpellDefinitions.Blight,
                                                                                    DatabaseHelper.SpellDefinitions.DimensionDoor
                                                                                },
                                                                                new List<SpellDefinition>
                                                                                {
                                                                                    DatabaseHelper.SpellDefinitions.HoldMonster
                                                                                }
                                                                                );

            warlock_spellcasting = Helpers.CustomSpellcastingBuilder<NewFeatureDefinitions.WarlockCastSpell>
                                                               .createSpontaneousSpellcasting("WarlockClassSpellcasting",
                                                                                              "",
                                                                                              "Feature/&WarlockClassSpellcastingTitle",
                                                                                              "Feature/&WarlockClassSpellcastingDescription",
                                                                                              warlock_spelllist,
                                                                                              Helpers.Stats.Charisma,
                                                                                              new List<int> { 2,  2,  2,  3,  3,  3, 3, 3, 3, 4,
                                                                                                              4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
                                                                                              new List<int> { 2,  3,  4,  5,  6,  7,  8, 9,  10, 10,
                                                                                                             11, 11, 12, 12, 13, 13, 14, 14, 15, 15},
                                                                                              Helpers.Misc.createSpellSlotsByLevel(new List<int> { 1, 0, 0, 0, 0 },
                                                                                                                                   new List<int> { 2, 0, 0, 0, 0 },
                                                                                                                                   new List<int> { 2, 2, 0, 0, 0 },//3
                                                                                                                                   new List<int> { 2, 2, 0, 0, 0 },//4
                                                                                                                                   new List<int> { 2, 2, 2, 0, 0 },//5
                                                                                                                                   new List<int> { 2, 2, 2, 0, 0 },//6
                                                                                                                                   new List<int> { 2, 2, 2, 2, 0 },//7
                                                                                                                                   new List<int> { 2, 2, 2, 2, 0 },//8
                                                                                                                                   new List<int> { 2, 2, 2, 2, 2 },//9
                                                                                                                                   new List<int> { 2, 2, 2, 2, 2 },//10
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//11
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//12
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//13
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//14
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//15
                                                                                                                                   new List<int> { 3, 3, 3, 3, 3 },//16
                                                                                                                                   new List<int> { 4, 4, 4, 4, 4 },//17
                                                                                                                                   new List<int> { 4, 4, 4, 4, 4 },//18
                                                                                                                                   new List<int> { 4, 4, 4, 4, 4 },//19
                                                                                                                                   new List<int> { 4, 4, 4, 4, 4 }//20
                                                                                                                                   )
                                                                                              );
            warlock_spellcasting.replacedSpells = new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 1, 0,
                                                                 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
            warlock_spellcasting.SetSlotsRecharge(RuleDefinitions.RechargeRate.ShortRest);

            createPactBoon();
            createInvocations();
            Definition.FeatureUnlocks.Clear();
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(saving_throws, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(armor_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(weapon_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(skills, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(warlock_spellcasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[1], 2));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[2], 2));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(pact_boon, 3));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 4));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[5], 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[7], 7));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 8));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[9], 9));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 12));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[12], 12));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[15], 15));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations[18], 18));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 19));

            var subclassChoicesGuiPresentation = new GuiPresentation();
            subclassChoicesGuiPresentation.Title = "Subclass/&WarlockSubclassPatronTitle";
            subclassChoicesGuiPresentation.Description = "Subclass/&WarlockSubclassPatronDescription";
            WarlockFeatureDefinitionSubclassChoice = this.BuildSubclassChoice(1, "Patron", false, "SubclassChoiceWarlockSpecialistArchetypes", subclassChoicesGuiPresentation, WarlockClassSubclassesGuid);
        }


        static void createPactBoon()
        {
            var title_string = "Feature/&WarlockClassPactBoonTitle";
            var description_string = "Feature/&WarlockClassPactBoonDescription";
            createPactOfTome();
            createPactOfBlade();
            createPactOfArrow();

            pact_boon = Helpers.FeatureSetBuilder.createFeatureSet("WarlockClassPactBoon",
                                                                    "",
                                                                    title_string,
                                                                    description_string,
                                                                    false,
                                                                    FeatureDefinitionFeatureSet.FeatureSetMode.Exclusion,
                                                                    true,
                                                                    pact_of_blade,
                                                                    pact_of_tome,
                                                                    pact_of_arrow
                                                                    );
        }


        static void createPactOfArrow()
        {
            var title_string = "Feature/&WarlockClassPactOfArrowTitle";
            var description_string = "Feature/&WarlockClassPactOfArrowDescription";

            var proficiency = Helpers.ProficiencyBuilder.CreateWeaponProficiency("WarlockClassPactOfArrowProficiency",
                                                                                 "",
                                                                                 Common.common_no_title,
                                                                                 Common.common_no_title,
                                                                                 Helpers.WeaponProficiencies.Longbow,
                                                                                 Helpers.WeaponProficiencies.Shortbow
                                                                                 );
            var magic_tag = Helpers.FeatureBuilder<NewFeatureDefinitions.AddAttackTagForSpecificWeaponType>.createFeature("WarlockClassPactOfArrowMagicTag",
                                                                                                                         "",
                                                                                                                         Common.common_no_title,
                                                                                                                         Common.common_no_title,
                                                                                                                         Common.common_no_icon,
                                                                                                                         a =>
                                                                                                                         {
                                                                                                                             a.weaponTypes = arrow_pact_weapon_types;
                                                                                                                             a.tag = "Magical";
                                                                                                                         }
                                                                                                                         );

            var power = Helpers.CopyFeatureBuilder<FeatureDefinitionPower>.createFeatureCopy("WarlockClassPactOfArrowPower",
                                                                                             "",
                                                                                             title_string,
                                                                                             description_string,
                                                                                             null,
                                                                                             DatabaseHelper.FeatureDefinitionPowers.PowerFunctionEndlessQuiver,
                                                                                             a =>
                                                                                             {
                                                                                                 a.rechargeRate = RuleDefinitions.RechargeRate.AtWill;
                                                                                                 a.fixedUsesPerRecharge = 1;
                                                                                             }
                                                                                             );


            pact_of_arrow = Helpers.FeatureSetBuilder.createFeatureSet("WarlockClassPactOfArrow",
                                                        "",
                                                        title_string,
                                                        description_string,
                                                        false,
                                                        FeatureDefinitionFeatureSet.FeatureSetMode.Union,
                                                        false,
                                                        proficiency,
                                                        magic_tag,
                                                        power
                                                        );

        }


        static void createPactOfBlade()
        {
            var title_string = "Feature/&WarlockClassPactOfBladeTitle";
            var description_string = "Feature/&WarlockClassPactOfBladeDescription";

            var proficiency = Helpers.ProficiencyBuilder.CreateWeaponProficiency("WarlockClassPactOfBladeProficiency",
                                                                                 "",
                                                                                 Common.common_no_title,
                                                                                 Common.common_no_title,
                                                                                 Helpers.WeaponProficiencies.LongSword,
                                                                                 Helpers.WeaponProficiencies.Rapier,
                                                                                 Helpers.WeaponProficiencies.ShortSword,
                                                                                 Helpers.WeaponProficiencies.GreatSword,
                                                                                 Helpers.WeaponProficiencies.Scimitar,
                                                                                 Helpers.WeaponProficiencies.MorningStar,
                                                                                 Helpers.WeaponProficiencies.BattleAxe,
                                                                                 Helpers.WeaponProficiencies.Warhammer
                                                                                 );
            var magic_tag = Helpers.FeatureBuilder<NewFeatureDefinitions.AddAttackTagForSpecificWeaponType>.createFeature("WarlockClassPactOfBladeMagicTag",
                                                                                                                         "",
                                                                                                                         Common.common_no_title,
                                                                                                                         Common.common_no_title,
                                                                                                                         Common.common_no_icon,
                                                                                                                         a =>
                                                                                                                         {
                                                                                                                             a.weaponTypes = blade_pact_weapon_types;
                                                                                                                             a.tag = "Magical";
                                                                                                                         }
                                                                                                                         );

            pact_of_blade = Helpers.FeatureSetBuilder.createFeatureSet("WarlockClassPactOfBlade",
                                                        "",
                                                        title_string,
                                                        description_string,
                                                        false,
                                                        FeatureDefinitionFeatureSet.FeatureSetMode.Union,
                                                        false,
                                                        proficiency,
                                                        magic_tag
                                                        );

        }

        static void createPactOfTome()
        {
            var spelllist = Helpers.SpelllistBuilder.createCombinedSpellListWithLevelRestriction("WarlockClassPactOfTomeSpelllist", "", "",
                                                                                                 (warlock_spelllist, 10),
                                                                                                 (DatabaseHelper.SpellListDefinitions.SpellListWizard, 0),
                                                                                                 (DatabaseHelper.SpellListDefinitions.SpellListCleric, 0),
                                                                                                 (DatabaseHelper.SpellListDefinitions.SpellListPaladin, 0),
                                                                                                 (DatabaseHelper.SpellListDefinitions.SpellListRanger, 0),
                                                                                                 (DatabaseHelper.SpellListDefinitions.SpellListWizardGreenmage, 0)
                                                                                                 );

             pact_of_tome = Helpers.ExtraSpellSelectionBuilder.createExtraCantripSelection("WarlockClassPactOfTome",
                                                                                            "",
                                                                                            "Feature/&WarlockClassPactOfTomeTitle",
                                                                                            "Feature/&WarlockClassPactOfTomeDescription",
                                                                                            warlock_class,
                                                                                            3,
                                                                                            3,
                                                                                            spelllist
                                                                                            );
        }


        static FeatureDefinitionBonusCantrips createSpellLikeInvocation(SpellDefinition spell, string name, string title, string description, bool self_only = false)
        {
            var cantrip = Helpers.Misc.convertSpellToCantrip(spell, name + "Spell", title, self_only);

            var feature = Helpers.BonusCantripsBuilder.createLearnBonusCantrip(name,
                                                                       "",
                                                                       title,
                                                                       description,
                                                                       cantrip);
            return feature;
        }

        static void createInvocations()
        {
            var title_string = "Feature/&WarlockInvocationsTitle";
            var description_string = "Feature/&WarlockInvocationsDescription";



            createRepellingBlast();

            alien_ectoplasm = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.Grease, "WarlockAlienEctoplasmInvocation",
                                                        "Feature/&WarlockAlienEctoplasmInvocationsTitle",
                                                        "Feature/&WarlockAlienEctoplasmInvocationsDescription");

            armor_of_shadow = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.MageArmor, "WarlockShadowArmorInvocation",
                                                        "Feature/&WarlockShadowArmorInvocationsTitle",
                                                        "Feature/&WarlockShadowArmorInvocationsDescription",
                                                        true);

            eldritch_sight = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.DetectMagic, "WarlockEldritchSightInvocation",
                                                        "Feature/&WarlockEldritchSightInvocationsTitle",
                                                        "Feature/&WarlockEldritchSightInvocationsDescription");

            fiendish_vigor = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.FalseLife, "WarlockFiendishVigorInvocation",
                                                        "Feature/&WarlockFiendishVigorInvocationsTitle",
                                                        "Feature/&WarlockFiendishVigorInvocationsDescription");

            otherworldy_leap = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.Jump, "WarlockOtherWordlyLeapInvocation",
                                                        "Feature/&WarlockOtherWordlyLeapInvocationsTitle",
                                                        "Feature/&WarlockOtherWordlyLeapInvocationsDescription",
                                                        true);

            ascendant_step = createSpellLikeInvocation(DatabaseHelper.SpellDefinitions.Levitate, "WarlockAscendantStepInvocation",
                                                        "Feature/&WarlockAscendantStepInvocationsTitle",
                                                        "Feature/&WarlockAscendantStepInvocationsDescription",
                                                        true);

            createBookOfEldritchSecrets();
            createBeguilingInfluence();
            createDevilsSight();
            createThirstingBlade();
            createEldritchArchery();
            createEldritchStrike();
            var invocations_features = new List<(FeatureDefinition, int)>()
            {
                (agonizing_blast, 0),
                (beguiling_influence, 0),
                (miring_blast, 0),
                (devils_sight, 0),
                (repelling_blast, 0),
                (alien_ectoplasm, 0),
                (armor_of_shadow, 0),
                (eldritch_sight, 0),
                (fiendish_vigor, 0),
                (eldritch_archery, 5),
                (thirsting_blade, 5),
                (eldritch_smite, 5),
                (otherworldy_leap, 9),
                (ascendant_step, 9)
            };


            var invocations_levels = new int[] { 1, 2, 5, 7, 9, 12, 15, 18 };

            foreach (var lvl in invocations_levels)
            {
                var feature = Helpers.FeatureSetBuilder.createFeatureSet("WarlockInvocations" + lvl.ToString(),
                                                                            "",
                                                                            title_string,
                                                                            description_string,
                                                                            false,
                                                                            FeatureDefinitionFeatureSet.FeatureSetMode.Exclusion,
                                                                            true
                                                                            );
                feature.featureSet = invocations_features.Where(f => f.Item2 <= lvl).Select(f => f.Item1).ToList();
                if (book_of_secrets.ContainsKey(lvl))
                {
                    feature.featureSet.Add(book_of_secrets[lvl]);
                }
                invocations[lvl] = feature;
            }
        }


        static void createEldritchStrike()
        {
            string title_string = "Feature/&WarlockEldritchStrikeInvocationTitle";
            string description_string = "Feature/&WarlockEldritchStrikeInvocationDescription";
            string use_react_description = "Reaction/&SpendWarlockEldritchStrikeInvocationPowerReactDescription";
            string use_react_title = "Reaction/&CommonUsePowerReactTitle";

            var sprite = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("EldritchStrikePowerImage",
                                                    $@"{UnityModManagerNet.UnityModManager.modsPath}/SolastaWarlockClass/Sprites/EldritchSmite.png",
                                                    128, 64);

            var used_condition = Helpers.ConditionBuilder.createConditionWithInterruptions("WarlockEldritchStrikeInvocationUsedCondition",
                                                                                        "",
                                                                                        "",
                                                                                        "",
                                                                                        null,
                                                                                        DatabaseHelper.ConditionDefinitions.ConditionDummy,
                                                                                        new RuleDefinitions.ConditionInterruption[] { RuleDefinitions.ConditionInterruption.AnyBattleTurnEnd }
                                                                                        );
            NewFeatureDefinitions.ConditionsData.no_refresh_conditions.Add(used_condition);

            Dictionary<int, EffectDescription> lvl_effects = new Dictionary<int, EffectDescription>();

            for (int i = 1; i <= 9; i += 2)
            {
                var effect = new EffectDescription();
                effect.Copy(DatabaseHelper.FeatureDefinitionPowers.PowerDomainBattleDecisiveStrike.EffectDescription);
                effect.DurationParameter = 1;
                effect.DurationType = RuleDefinitions.DurationType.Round;
                effect.SetSavingThrowDifficultyAbility(Helpers.Stats.Charisma);
                effect.SavingThrowAbility = Helpers.Stats.Charisma;
                effect.hasSavingThrow = false;
                effect.SetDifficultyClassComputation(RuleDefinitions.EffectDifficultyClassComputation.AbilityScoreAndProficiency);
                effect.EffectForms.Clear();

                var effect_form = new EffectForm();
                effect_form.DamageForm = new DamageForm();
                effect_form.FormType = EffectForm.EffectFormType.Damage;
                effect_form.DamageForm.dieType = RuleDefinitions.DieType.D10;
                effect_form.DamageForm.diceNumber = 1 + (i - 1) / 2;
                effect_form.DamageForm.damageType = Helpers.DamageTypes.Force;
                effect.EffectForms.Add(effect_form);

                effect_form = new EffectForm();
                effect_form.motionForm = new MotionForm();
                effect_form.FormType = EffectForm.EffectFormType.Motion;
                effect_form.motionForm.type = MotionForm.MotionType.FallProne;
                effect_form.motionForm.distance = 0;
                effect_form.hasSavingThrow = false;
                effect.EffectForms.Add(effect_form);

                effect_form = new EffectForm();
                effect_form.ConditionForm = new ConditionForm();
                effect_form.FormType = EffectForm.EffectFormType.Condition;
                effect_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
                effect_form.ConditionForm.ConditionDefinition = used_condition;
                effect_form.conditionForm.applyToSelf = true;
                effect.EffectForms.Add(effect_form);

                lvl_effects[i] = effect;

            }

            var power = Helpers.GenericPowerBuilder<NewFeatureDefinitions.PowerWithRestrictionsAndCasterLevelDependentEffect>
                                                        .createPower("WarlockEldritchStrikeInvocation",
                                                                     "",
                                                                     title_string,
                                                                     description_string,
                                                                     sprite,
                                                                     lvl_effects[1],
                                                                     RuleDefinitions.ActivationTime.OnAttackHit,
                                                                     4,
                                                                     RuleDefinitions.UsesDetermination.Fixed,
                                                                     RuleDefinitions.RechargeRate.SpellSlot
                                                                     );
            power.spellcastingFeature = warlock_spellcasting;
            power.minCustomEffectLevel = 3;
            power.levelEffectList = new List<(int, EffectDescription)>
            {
                (4, lvl_effects[3]),
                (6, lvl_effects[5]),
                (8, lvl_effects[7]),
                (20, lvl_effects[9])
            };
            power.checkReaction = true;
            power.restrictions = new List<NewFeatureDefinitions.IRestriction>()
                                            {
                                                new NewFeatureDefinitions.AndRestriction(
                                                    new NewFeatureDefinitions.OrRestriction(new NewFeatureDefinitions.SpecificWeaponInMainHandRestriction(blade_pact_weapon_types), new NewFeatureDefinitions.HasFeatureRestriction(pact_of_blade)),
                                                    new NewFeatureDefinitions.OrRestriction(new NewFeatureDefinitions.SpecificWeaponInMainHandRestriction(arrow_pact_weapon_types), new NewFeatureDefinitions.HasFeatureRestriction(pact_of_arrow))
                                                ),
                                                new NewFeatureDefinitions.NoConditionRestriction(used_condition)
                                            };
         

            Helpers.StringProcessing.addPowerReactStrings(power, title_string, use_react_description,
                                            use_react_title, use_react_description, "SpendPower");
            eldritch_smite = power;
            NewFeatureDefinitions.FeatureData.addFeatureRestrictions(eldritch_smite, new NewFeatureDefinitions.HasAnyFeatureFromListRestriction(pact_of_blade, pact_of_arrow));
        }

        static void createThirstingBlade()
        {
            thirsting_blade = Helpers.FeatureBuilder<NewFeatureDefinitions.AttackModeExtraMainAttackWithSpecificWeaponType>
                                                                        .createFeature("WarlockThirstingBladeInvocation",
                                                                                        "",
                                                                                        "Feature/&WarlockThirstingBladeInvocationTitle",
                                                                                        "Feature/&WarlockThirstingBladeInvocationDescription",
                                                                                        Common.common_no_icon,
                                                                                        a =>
                                                                                        {
                                                                                            a.weaponTypes = new List<string>()
                                                                                            {
                                                                                                Helpers.WeaponProficiencies.QuarterStaff,
                                                                                                Helpers.WeaponProficiencies.Spear,
                                                                                                Helpers.WeaponProficiencies.Mace,
                                                                                                Helpers.WeaponProficiencies.Dagger,
                                                                                                Helpers.WeaponProficiencies.Handaxe,
                                                                                                Helpers.WeaponProficiencies.Club,
                                                                                                Helpers.WeaponProficiencies.LongSword,
                                                                                                Helpers.WeaponProficiencies.Rapier,
                                                                                                Helpers.WeaponProficiencies.ShortSword,
                                                                                                Helpers.WeaponProficiencies.GreatSword,
                                                                                                Helpers.WeaponProficiencies.Scimitar,
                                                                                                Helpers.WeaponProficiencies.MorningStar,
                                                                                                Helpers.WeaponProficiencies.BattleAxe,
                                                                                                Helpers.WeaponProficiencies.Warhammer
                                                                                            };
                                                                                        }
                                                                                        );
           NewFeatureDefinitions.FeatureData.addFeatureRestrictions(thirsting_blade, new NewFeatureDefinitions.HasFeatureRestriction(pact_of_blade));
        }


        static void createEldritchArchery()
        {
            eldritch_archery = Helpers.FeatureBuilder<NewFeatureDefinitions.AttackModeExtraMainAttackWithSpecificWeaponType>
                                                                        .createFeature("WarlockEldritchArcheryInvocation",
                                                                                        "",
                                                                                        "Feature/&WarlockEldritchArcheryInvocationTitle",
                                                                                        "Feature/&WarlockEldritchArcheryInvocationDescription",
                                                                                        Common.common_no_icon,
                                                                                        a =>
                                                                                        {
                                                                                            a.weaponTypes = new List<string>()
                                                                                            {
                                                                                                Helpers.WeaponProficiencies.Shortbow,
                                                                                                Helpers.WeaponProficiencies.Longbow,
                                                                                            };
                                                                                        }
                                                                                        );
            NewFeatureDefinitions.FeatureData.addFeatureRestrictions(eldritch_archery, new NewFeatureDefinitions.HasFeatureRestriction(pact_of_arrow));
        }



        static void createDevilsSight()
        {
            var immunity = Helpers.CopyFeatureBuilder<FeatureDefinitionConditionAffinity>.createFeatureCopy("WarlockDevilsSightImmunityDarknessInvocation",
                                                                                                  "",
                                                                                                  Common.common_no_title,
                                                                                                  Common.common_no_title,
                                                                                                  Common.common_no_icon,
                                                                                                  DatabaseHelper.FeatureDefinitionConditionAffinitys.ConditionAffinityVeilImmunity,
                                                                                                  a =>
                                                                                                  {
                                                                                                      a.conditionType = DatabaseHelper.ConditionDefinitions.ConditionDarkness.name;
                                                                                                  }
                                                                                                  );

            devils_sight = Helpers.FeatureSetBuilder.createFeatureSet("WarlockDevilsSightInvocation",
                                                            "",
                                                            "Feature/&WarlockDevilsSightInvocationTitle",
                                                            "Feature/&WarlockDevilsSightInvocationDescription",
                                                            false,
                                                            FeatureDefinitionFeatureSet.FeatureSetMode.Union,
                                                            false,
                                                            immunity,
                                                            DatabaseHelper.FeatureDefinitionSenses.SenseDarkvision24
                                                            );

        }


        static void createBeguilingInfluence()
        {
           beguiling_influence = Helpers.ProficiencyBuilder.CreateSkillsProficiency("WarlockBeguilingInfluenceInvocation",
                                                                                    "",
                                                                                    "Feature/&WarlockBeguilingInfluenceInvocationTitle",
                                                                                    "Feature/&WarlockBeguilingInfluenceInvocationDescription",
                                                                                    Helpers.Skills.Deception, Helpers.Skills.Persuasion
                                                                                    );
        }


        static void createBookOfEldritchSecrets()
        {
            var invocations_levels = new int[] {5, 7, 9, 12, 15, 18 };

            foreach (var lvl in invocations_levels)
            {
                var feature = Helpers.ExtraSpellSelectionBuilder.createExtraSpellSelection("WarlockClassBookOfEldritchSecretsInvocation" + lvl.ToString(),
                                                                                               "",
                                                                                               "Feature/&WarlockClassBookOfEldritchSecretsInvocationTitle",
                                                                                               "Feature/&WarlockClassBookOfEldritchSecretsInvocationDescription",
                                                                                               warlock_class,
                                                                                               lvl,
                                                                                               1,
                                                                                               DatabaseHelper.SpellListDefinitions.SpellListWizard
                                                                                               );
                book_of_secrets[lvl] = feature;
                NewFeatureDefinitions.FeatureData.addFeatureRestrictions(feature, new NewFeatureDefinitions.HasFeatureRestriction(pact_of_tome));
            }
        }


        static void createRepellingBlast()
        {
            var repelling_blast_title_string = "Feature/&WarlockRepellingBlastInvocationTitle";
            var repelling_blast_description_string = "Feature/&WarlockRepellingBlastInvocationDescription";
            var sprite = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("RepellingEldritchBlastCantripImage",
                                                                $@"{UnityModManagerNet.UnityModManager.modsPath}/SolastaWarlockClass/Sprites/RepellingEldritchBlast.png",
                                                                128, 128);

            var push_form = new EffectForm();
            push_form.formType = EffectForm.EffectFormType.Motion;
            push_form.motionForm = new MotionForm();
            push_form.motionForm.distance = 2;
            push_form.motionForm.type = MotionForm.MotionType.PushFromOrigin;

            repelling_eldritch_blast = Helpers.GenericSpellBuilder<NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects>
                                                                .createSpell("RepellingEldritchBlastCantrip",
                                                                             "",
                                                                             repelling_blast_title_string,
                                                                             Helpers.StringProcessing.concatenateStrings(eldritch_blast.guiPresentation.description,
                                                                                                                         repelling_blast_description_string,
                                                                                                                         "Spell/&RepellingEldritchBlastDescription",
                                                                                                                         "\n"
                                                                                                                         ),
                                                                             sprite,
                                                                             Helpers.Misc.addEffectFormsToEffectDescription(eldritch_blast.effectDescription, push_form),
                                                                             RuleDefinitions.ActivationTime.Action,
                                                                             0,
                                                                             false,
                                                                             true,
                                                                             true,
                                                                             Helpers.SpellSchools.Evocation
                                                                             );
            repelling_eldritch_blast.featuresEffectList = new List<(List<FeatureDefinition>, EffectDescription)>();

            foreach (var fl in eldritch_blast.featuresEffectList)
            {
                repelling_eldritch_blast.featuresEffectList.Add((fl.Item1, Helpers.Misc.addEffectFormsToEffectDescription(fl.Item2, push_form)));
            }

            repelling_blast = Helpers.BonusCantripsBuilder.createLearnBonusCantrip("WarlockRepellingBlastCantrip",
                                                                                   "",
                                                                                   repelling_blast_title_string,
                                                                                   repelling_blast_description_string,
                                                                                   repelling_eldritch_blast);

            NewFeatureDefinitions.FeatureData.addFeatureRestrictions(repelling_blast, new NewFeatureDefinitions.CanCastSpellRestriction(eldritch_blast, false));


        }

        static void createEldritchBlast()
        {
            var agonizing_blast_title_string = "Feature/&WarlockAgonizingBlastInvocationTitle";
            var agonizing_blast_description_string = "Feature/&WarlockAgonizingBlastInvocationDescription";

            agonizing_blast = Helpers.OnlyDescriptionFeatureBuilder.createOnlyDescriptionFeature("WarlockClassAgonizingBlastInvocation",
                                                                                                 "",
                                                                                                 agonizing_blast_title_string,
                                                                                                 agonizing_blast_description_string
                                                                                                 );

            var miring_blast_title_string = "Feature/&WarlockMiringBlastInvocationTitle";
            var miring_blast_description_string = "Feature/&WarlockMiringBlastInvocationDescription";

            miring_blast = Helpers.OnlyDescriptionFeatureBuilder.createOnlyDescriptionFeature("WarlockClassMireBlastInvocation",
                                                                                                 "",
                                                                                                 miring_blast_title_string,
                                                                                                 miring_blast_description_string
                                                                                                 );


            var title_string = "Spell/&EldritchBlastTitle";
            var description_string = "Spell/&EldritchBlastDescription";

            var sprite = SolastaModHelpers.CustomIcons.Tools.storeCustomIcon("EldritchBlastCantripImage",
                                                                            $@"{UnityModManagerNet.UnityModManager.modsPath}/SolastaWarlockClass/Sprites/EldritchBlast.png",
                                                                            128, 128);

            var effect = new EffectDescription();
            effect.Copy(DatabaseHelper.SpellDefinitions.MagicMissile.EffectDescription);
            effect.SetRangeType(RuleDefinitions.RangeType.RangeHit);
            effect.SetRangeParameter(24);
            effect.EffectForms.Clear();
            effect.effectAdvancement.Clear();
            effect.targetParameter = 1;
            //effect.durationParameter = 1;
            //effect.durationType = RuleDefinitions.DurationType.Round;

            var effect_advancement = new EffectAdvancement();
            effect_advancement.Clear();
            effect_advancement.effectIncrementMethod = RuleDefinitions.EffectIncrementMethod.CasterLevelTable;
            effect_advancement.incrementMultiplier = 1;
            effect_advancement.additionalTargetsPerIncrement = 1;
            effect.SetEffectAdvancement(effect_advancement);

            var damage = new EffectForm();
            damage.formType = EffectForm.EffectFormType.Damage;
            damage.createdByCharacter = true;
            damage.damageForm = new DamageForm();
            damage.damageForm.damageType = Helpers.DamageTypes.Force;
            damage.damageForm.diceNumber = 1;
            damage.damageForm.dieType = RuleDefinitions.DieType.D10;
            damage.applyAbilityBonus = false;
            effect.effectForms.Add(damage);

            eldritch_blast = Helpers.GenericSpellBuilder<NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects>
                                            .createSpell("EldritchBlastCantrip",
                                                         "",
                                                         title_string,
                                                         description_string,
                                                         sprite,
                                                         effect,
                                                         RuleDefinitions.ActivationTime.Action,
                                                         0,
                                                         false,
                                                         true,
                                                         true,
                                                         Helpers.SpellSchools.Evocation
                                                         );

            var agonizing_effect = new EffectDescription();
            agonizing_effect.Copy(effect);
            agonizing_effect.effectForms.Clear();
            var damage2 =  new EffectForm();
            damage2.Copy(damage);
            damage2.applyAbilityBonus = true;
            agonizing_effect.effectForms.Add(damage2);


            var condition_form = new EffectForm();
            condition_form.ConditionForm = new ConditionForm();
            condition_form.FormType = EffectForm.EffectFormType.Condition;
            condition_form.ConditionForm.Operation = ConditionForm.ConditionOperation.Add;
            condition_form.ConditionForm.ConditionDefinition = DatabaseHelper.ConditionDefinitions.ConditionHindered_By_Frost;
            condition_form.createdByCharacter = true;

            var mire_effect = Helpers.Misc.addEffectFormsToEffectDescription(effect, condition_form);
            var agonizing_mire_effect = Helpers.Misc.addEffectFormsToEffectDescription(agonizing_effect, condition_form);
            mire_effect.durationParameter = 1;
            mire_effect.durationType = RuleDefinitions.DurationType.Round;
            agonizing_mire_effect.durationParameter = 1;
            agonizing_mire_effect.durationType = RuleDefinitions.DurationType.Round;

            eldritch_blast.featuresEffectList = new List<(List<FeatureDefinition>, EffectDescription)>()
            {
                (new List<FeatureDefinition>{agonizing_blast, miring_blast}, agonizing_mire_effect),
                (new List<FeatureDefinition>{agonizing_blast}, agonizing_effect),
                (new List<FeatureDefinition>{miring_blast}, mire_effect),
            };

            NewFeatureDefinitions.FeatureData.addFeatureRestrictions(agonizing_blast, new NewFeatureDefinitions.CanCastSpellRestriction(eldritch_blast, false));
            NewFeatureDefinitions.FeatureData.addFeatureRestrictions(miring_blast, new NewFeatureDefinitions.CanCastSpellRestriction(eldritch_blast, false));
        }


        static CharacterSubclassDefinition createFiendPatron()
        {
            var gui_presentation = new GuiPresentationBuilder(
                    "Subclass/&WarlockSubclassPatronFiendDescription",
                    "Subclass/&WarlockSubclassPatronFiendTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.TraditionShockArcanist.GuiPresentation.SpriteReference)
                    .Build();

            createFiendSpells();
            createDarkOnesBlessing();
            createDarkOnesOwnLuck();
            createFiendishResilence();
            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder("WarlockSubclassPatronFiend", "911ed94e-3664-4916-b92b-909f0382b3a2")
                                                                                            .SetGuiPresentation(gui_presentation)
                                                                                            .AddFeatureAtLevel(fiend_spells, 1)
                                                                                            .AddFeatureAtLevel(dark_ones_blessing, 1)
                                                                                            .AddFeatureAtLevel(dark_ones_own_luck, 6)
                                                                                            .AddFeatureAtLevel(fiendish_resilence, 10)
                                                                                            .AddToDB();

            return definition;
        }

        static void createFiendishResilence()
        {
            var title_string = "Feature/&WarlockFiendFiendishResilenceTitle";
            var description_string = "Feature/&WarlockFiendFiendishResilenceDescription";
           
            fiendish_resilence = Helpers.FeatureSetBuilder.createFeatureSet("WarlockFiendFiendishResilence",
                                                                            "",
                                                                            title_string,
                                                                            description_string,
                                                                            false,
                                                                            FeatureDefinitionFeatureSet.FeatureSetMode.Exclusion,
                                                                            true
                                                                            );

            Dictionary<string, FeatureDefinitionDamageAffinity> resistances_map = new Dictionary<string, FeatureDefinitionDamageAffinity>()
            {
                {"Bludgeoning", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityBludgeoningResistance},
                {"Piercing", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPiercingResistance},
                {"Slashing", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinitySlashingResistance},
                {"Acid", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityAcidResistance},
                {"Cold", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityColdResistance},
                {"Fire", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityFireResistance},
                {"Lightning", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityLightningResistance},
                {"Poison", DatabaseHelper.FeatureDefinitionDamageAffinitys.DamageAffinityPoisonResistance},
            };

            foreach (var r in resistances_map)
            {
                var feature = Helpers.CopyFeatureBuilder<FeatureDefinitionDamageAffinity>.createFeatureCopy("WarlockFiendFiendishResilence" + r.Key,
                                                                                                            "",
                                                                                                            DatabaseRepository.GetDatabase<DamageDefinition>().GetElement(r.Value.damageType).GuiPresentation.Title,
                                                                                                            "",
                                                                                                            null,
                                                                                                            r.Value
                                                                                                            );
                fiendish_resilence.featureSet.Add(feature);
            }
        }


        static void createDarkOnesOwnLuck()
        {
            string title_string = "Feature/&WarlockFiendSubclassDarkOnesOwnLuckTitle";
            string description_string = "Feature/&WarlockFiendSubclassDarkOnesOwnLuckDescription";
            dark_ones_own_luck = Helpers.GenericPowerBuilder<NewFeatureDefinitions.RerollFailedSavePower>
                                                                                .createPower("WarlockFiendSubclassDarkOnesOwnLuckPower",
                                                                                             "",
                                                                                             title_string,
                                                                                             description_string,
                                                                                             DatabaseHelper.FeatureDefinitionPowers.PowerWinterWolfBreath.guiPresentation.SpriteReference,
                                                                                             new EffectDescription(),
                                                                                             RuleDefinitions.ActivationTime.NoCost,
                                                                                             1,
                                                                                             RuleDefinitions.UsesDetermination.Fixed,
                                                                                             RuleDefinitions.RechargeRate.ShortRest
                                                                                             );
            Helpers.StringProcessing.addStringCopy(title_string,
                                                   "Reaction/&ConsumePowerUseWarlockFiendSubclassDarkOnesOwnLuckPowerTitle");
            Helpers.StringProcessing.addStringCopy("Reaction/&IndomitableResistanceReactTitle",
                                                   "Reaction/&ConsumePowerUseWarlockFiendSubclassDarkOnesOwnLuckPowerReactTitle");
            Helpers.StringProcessing.addStringCopy("Reaction/&IndomitableResistanceReactDescription",
                                                   "Reaction/&ConsumePowerUseWarlockFiendSubclassDarkOnesOwnLuckPowerReactDescription");
        }


        static void createDarkOnesBlessing()
        {
            string title_string = "Feature/&WarlockFiendSubclassDarkOnesBlessingTitle";
            string description_string = "Feature/&WarlockFiendSubclassDarkOnesBlessingDescription";

            var effect = new EffectDescription();
            effect.Copy(DatabaseHelper.FeatureDefinitionPowers.PowerDomainBattleDivineWrath.EffectDescription);
            effect.EffectForms.Clear();
            var effect_form = new EffectForm();
            effect_form.temporaryHitPointsForm = new TemporaryHitPointsForm();
            effect_form.FormType = EffectForm.EffectFormType.TemporaryHitPoints;
            effect_form.temporaryHitPointsForm.dieType = RuleDefinitions.DieType.D1;
            effect_form.temporaryHitPointsForm.diceNumber = 1;
            effect_form.applyAbilityBonus = true;
            effect_form.applyLevel = EffectForm.LevelApplianceType.Multiply;
            effect_form.levelMultiplier = 1;
            effect.EffectForms.Add(effect_form);


            var power = Helpers.PowerBuilder.createPower("WarlockFiendSubclassDarkOnesBlessingPower",
                                                     "",
                                                     title_string,
                                                     description_string,
                                                     null,
                                                     DatabaseHelper.FeatureDefinitionPowers.PowerDomainBattleDivineWrath,
                                                     effect,
                                                     RuleDefinitions.ActivationTime.NoCost,
                                                     1,
                                                     RuleDefinitions.UsesDetermination.Fixed,
                                                     RuleDefinitions.RechargeRate.AtWill,
                                                     show_casting: false,
                                                     ability: Helpers.Stats.Charisma);

            dark_ones_blessing = Helpers.FeatureBuilder<NewFeatureDefinitions.InitiatorApplyPowerToSelfOnTargetSlain>
                                                                                    .createFeature("WarlockFiendSubclassDarkOnesBlessing",
                                                                                                    "",
                                                                                                    title_string,
                                                                                                    description_string,
                                                                                                    Common.common_no_icon,
                                                                                                    f =>
                                                                                                    {
                                                                                                        f.power = power;
                                                                                                        f.scaleClass = warlock_class;
                                                                                                    }
                                                                                                    );
        }


        static void createFiendSpells()
        {
            var spelllist = Helpers.SpelllistBuilder.create9LevelSpelllist("WarlockFiendSubclassFiendSpellsSpelllist", "", "",
                                                                            new List<SpellDefinition>
                                                                            {

                                                                            },
                                                                            new List<SpellDefinition>
                                                                            {
                                                                                                        DatabaseHelper.SpellDefinitions.BurningHands,
                                                                                                        DatabaseHelper.SpellDefinitions.Bane,
                                                                            },
                                                                            new List<SpellDefinition>
                                                                            {
                                                                                                        DatabaseHelper.SpellDefinitions.Blindness,
                                                                                                        DatabaseHelper.SpellDefinitions.ScorchingRay,
                                                                            },
                                                                            new List<SpellDefinition>
                                                                            {
                                                                                                        DatabaseHelper.SpellDefinitions.Fireball,
                                                                                                        DatabaseHelper.SpellDefinitions.StinkingCloud,
                                                                            },
                                                                            new List<SpellDefinition>
                                                                            {
                                                                                                        DatabaseHelper.SpellDefinitions.FireShield,
                                                                                                        DatabaseHelper.SpellDefinitions.WallOfFire,
                                                                            },
                                                                            new List<SpellDefinition>
                                                                            {
                                                                                                        DatabaseHelper.SpellDefinitions.FlameStrike,
                                                                                                        DatabaseHelper.SpellDefinitions.CloudKill,
                                                                            }
                                                                            );

            string title = "Feature/&WarlockFiendSubclassFiendSpellsTitle";
            string description = "Feature/&WarlockFiendSubclassFiendSpellsDescription";
            fiend_spells = Helpers.CopyFeatureBuilder<FeatureDefinitionMagicAffinity>.createFeatureCopy("WarlockFiendSubclassFiendSpells",
                                                                                                                         "",
                                                                                                                         title,
                                                                                                                         description,
                                                                                                                         null,
                                                                                                                         DatabaseHelper.FeatureDefinitionMagicAffinitys.MagicAffinityGreenmageGreenMagicList,
                                                                                                                         c =>
                                                                                                                         {
                                                                                                                             c.SetExtendedSpellList(spelllist);
                                                                                                                         }
                                                                                                                         );
        }


        public static void BuildAndAddClassToDB()
        {
            var WarlockClass = new WarlockClassBuilder(WarlockClassName, WarlockClassNameGuid).AddToDB();
            WarlockClass.FeatureUnlocks.Sort(delegate (FeatureUnlockByLevel a, FeatureUnlockByLevel b)
                                          {
                                              return a.Level - b.Level;
                                          }
                                         );

            WarlockFeatureDefinitionSubclassChoice.Subclasses.Add(createFiendPatron().Name);
        }

        private static FeatureDefinitionSubclassChoice WarlockFeatureDefinitionSubclassChoice;
    }
}
