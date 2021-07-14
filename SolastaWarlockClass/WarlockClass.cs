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

        static public FeatureDefinitionFeatureSet invocations;
        static public FeatureDefinition agonizing_blast;
        static public FeatureDefinitionBonusCantrips repelling_blast;
        static public FeatureDefinition miring_blast;
        static public FeatureDefinitionBonusCantrips alien_ectoplasm;
        static public FeatureDefinitionBonusCantrips armor_of_shadow;
        static public FeatureDefinitionBonusCantrips eldritch_sight;
        static public FeatureDefinitionBonusCantrips fiendish_vigor;
        static public FeatureDefinitionBonusCantrips otherworldy_leap;
        static public FeatureDefinitionBonusCantrips ascendant_step;

        static public FeatureDefinitionFeatureSet pact_boon;
        static public NewFeatureDefinitions.FeatureDefinitionExtraSpellSelection pact_of_tome;
        static public FeatureDefinitionFeatureSet pact_of_blade;


        static NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects eldritch_blast;
        static NewFeatureDefinitions.SpellWithCasterFeatureDependentEffects repelling_eldritch_blast;


        //pacts: Arrow - ranged weapons + infinite ammo
        //invocations: 
        //acidic damage (acid damage equal to charisma to attacker)
        //beguiling influence (prof in deception and persuasion)
        //blade and bolt ?
        //devil's sight (darkvision)
        //thirsting blade (+1 attack)
        //




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

            var warlock_spellcasting = Helpers.CustomSpellcastingBuilder<NewFeatureDefinitions.WarlockCastSpell>
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
            warlock_spellcasting.replacedSpells = new List<int> {0, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                                                 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
            warlock_spellcasting.SetSlotsRecharge(RuleDefinitions.RechargeRate.ShortRest);


            createInvocations();
            createPactBoon();
            Definition.FeatureUnlocks.Clear();
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(saving_throws, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(armor_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(weapon_proficiency, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(skills, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(warlock_spellcasting, 1));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 2));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 2));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(pact_boon, 3));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 4));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 5));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 7));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 8));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 9));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 12));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 12));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(DatabaseHelper.FeatureDefinitionFeatureSets.FeatureSetAbilityScoreChoice, 16));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 15));
            Definition.FeatureUnlocks.Add(new FeatureUnlockByLevel(invocations, 18));
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

            pact_boon = Helpers.FeatureSetBuilder.createFeatureSet("WarlockClassPactBoon",
                                                                    "",
                                                                    title_string,
                                                                    description_string,
                                                                    false,
                                                                    FeatureDefinitionFeatureSet.FeatureSetMode.Exclusion,
                                                                    true,
                                                                    pact_of_blade,
                                                                    pact_of_tome
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

            invocations = Helpers.FeatureSetBuilder.createFeatureSet("WarlockInvocations",
                                                            "",
                                                            title_string,
                                                            description_string,
                                                            false,
                                                            FeatureDefinitionFeatureSet.FeatureSetMode.Exclusion,
                                                            true
                                                            );

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




            invocations.featureSet = new List<FeatureDefinition>()
            {
                agonizing_blast,
                miring_blast,
                repelling_blast,
                alien_ectoplasm,
                armor_of_shadow,
                eldritch_sight,
                fiendish_vigor,
                otherworldy_leap, //at lvl 9
                ascendant_step, //at lvl 9
            };
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
            condition_form.ConditionForm.ConditionDefinition = DatabaseHelper.ConditionDefinitions.ConditionHindered;

            var mire_effect = Helpers.Misc.addEffectFormsToEffectDescription(effect, condition_form);
            var agonizing_mire_effect = Helpers.Misc.addEffectFormsToEffectDescription(agonizing_effect, condition_form);
            mire_effect.DurationParameter = 1;
            agonizing_mire_effect.DurationParameter = 1;

            eldritch_blast.featuresEffectList = new List<(List<FeatureDefinition>, EffectDescription)>()
            {
                (new List<FeatureDefinition>{agonizing_blast, miring_blast}, agonizing_mire_effect),
                (new List<FeatureDefinition>{agonizing_blast}, agonizing_effect),
                (new List<FeatureDefinition>{miring_blast}, mire_effect),
            };

        }


        static CharacterSubclassDefinition createNatureCollege()
        {
            var gui_presentation = new GuiPresentationBuilder(
                    "Subclass/&WarlockSubclassPatronOfNatureDescription",
                    "Subclass/&WarlockSubclassPatronOfNatureTitle")
                    .SetSpriteReference(DatabaseHelper.CharacterSubclassDefinitions.TraditionGreenmage.GuiPresentation.SpriteReference)
                    .Build();

            CharacterSubclassDefinition definition = new CharacterSubclassDefinitionBuilder("WarlockSubclassPatronOfNature", "911ed94e-3664-4916-b92b-909f0382b3a2")
                                                                                            .SetGuiPresentation(gui_presentation)
                                                                                            .AddToDB();

            return definition;
        }


        public static void BuildAndAddClassToDB()
        {
            var WarlockClass = new WarlockClassBuilder(WarlockClassName, WarlockClassNameGuid).AddToDB();
            WarlockClass.FeatureUnlocks.Sort(delegate (FeatureUnlockByLevel a, FeatureUnlockByLevel b)
                                          {
                                              return a.Level - b.Level;
                                          }
                                         );

            WarlockFeatureDefinitionSubclassChoice.Subclasses.Add(createNatureCollege().Name);
        }

        private static FeatureDefinitionSubclassChoice WarlockFeatureDefinitionSubclassChoice;
    }
}
