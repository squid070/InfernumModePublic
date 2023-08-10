﻿using CalamityMod.Systems;
using CalamityMod.World;
using InfernumMode.Assets.Sounds;
using InfernumMode.Core.GlobalInstances.Systems;
using InfernumMode.Core.Netcode;
using InfernumMode.Core.Netcode.Packets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static CalamityMod.Systems.DifficultyModeSystem;

namespace InfernumMode.Content.UI
{
    public class InfernumDifficulty : DifficultyMode
    {
        public override bool Enabled
        {
            get => WorldSaveSystem.InfernumMode;
            set
            {
                WorldSaveSystem.InfernumMode = value;
                if (value)
                    CalamityWorld.revenge = true;
                if (Main.netMode != NetmodeID.SinglePlayer)
                    PacketManager.SendPacket<InfernumModeActivityPacket>();
            }
        }

        private Asset<Texture2D> _texture;
        public override Asset<Texture2D> Texture
        {
            get
            {
                _texture ??= ModContent.Request<Texture2D>("InfernumMode/Assets/ExtraTextures/UI/InfernumIcon");

                return _texture;
            }
        }

        public override LocalizedText ExpandedDescription => Language.GetText("Mods.InfernumMode.DifficultyUI.ExpandedDescription");

        public InfernumDifficulty()
        {
            DifficultyScale = 99999999f;
            Name = Language.GetText("Mods.InfernumMode.DifficultyUI.Name");
            ShortDescription = Language.GetText("Mods.InfernumMode.DifficultyUI.ShortDescription");

            ActivationTextKey = "Mods.InfernumMode.DifficultyUI.InfernumText";
            DeactivationTextKey = "Mods.InfernumMode.DifficultyUI.InfernumText2";

            ActivationSound = InfernumSoundRegistry.ModeToggleLaugh;
            ChatTextColor = Color.DarkRed;

            MostAlternateDifficulties = 1;
            Difficulties = new DifficultyMode[] { new NoDifficulty(), new RevengeanceDifficulty(), new DeathDifficulty(), this };
            Difficulties = Difficulties.OrderBy(d => d.DifficultyScale).ToArray();

            DifficultyTiers = new List<DifficultyMode[]>();
            float currentTier = -1;
            int tierIndex = -1;

            for (int i = 0; i < Difficulties.Length; i++)
            {
                // If at a new tier, create a new list of difficulties at that tier.
                if (currentTier != Difficulties[i].DifficultyScale)
                {
                    DifficultyTiers.Add(new DifficultyMode[] { Difficulties[i] });
                    currentTier = Difficulties[i].DifficultyScale;
                    tierIndex++;
                }

                // If the tier already exists, just add it to the list of other difficulties at that tier.
                else
                {
                    DifficultyTiers[tierIndex] = DifficultyTiers[tierIndex].Append(Difficulties[i]).ToArray();
                    MostAlternateDifficulties = Math.Max(DifficultyTiers[tierIndex].Length, MostAlternateDifficulties);
                }
            }
        }

        public override int FavoredDifficultyAtTier(int tier)
        {
            DifficultyMode[] tierList = DifficultyTiers[tier];

            for (int i = 0; i < tierList.Length; i++)
            {
                if (tierList[i].Name.Value == "Death")
                    return i;
            }

            return 0;
        }
    }
}
