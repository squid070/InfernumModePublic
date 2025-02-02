﻿using CalamityMod;
using InfernumMode.Content.Buffs;
using InfernumMode.Content.Projectiles.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static InfernumMode.Content.Items.Misc.BrimstoneCrescentStaff;

namespace InfernumMode.Core.GlobalInstances.Players
{
    public class BrimstoneCrescentForcefieldPlayer : ModPlayer
    {
        public int ForcefieldHits
        {
            get;
            set;
        }

        public float ForcefieldStrengthInterpolant
        {
            get;
            set;
        }

        public bool ForcefieldIsActive
        {
            get;
            set;
        }

        public override void PostUpdateMiscEffects()
        {
            ForcefieldStrengthInterpolant = Clamp(ForcefieldStrengthInterpolant + ForcefieldIsActive.ToDirectionInt() * 0.02f, 0f, 1f);
            if (ForcefieldIsActive)
                Player.AddBuff(ModContent.BuffType<BrimstoneBarrier>(), CalamityUtils.SecondsToFrames(DebuffTime));
        }

        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (ForcefieldIsActive)
            {
                // Apply DR and disable typical hit graphical/sound effects.
                modifiers.FinalDamage *= (1f - ForcefieldDRMultiplier);

                // Play a custom fire hit effect.
                SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, Player.Center);

                int explosionDamage = (int)Player.GetBestClassDamage().ApplyTo(ExplosionBaseDamage);
                if (Main.myPlayer == Player.whoAmI)
                    Projectile.NewProjectile(Player.GetSource_OnHurt(Player), Player.Center, Vector2.Zero, ModContent.ProjectileType<BrimstoneForcefieldExplosion>(), explosionDamage, 0f, Player.whoAmI, 0f, 100f);

                // Break the forcefield once it incurs enough hits.
                ForcefieldHits++;
                if (ForcefieldHits >= MaxForcefieldHits)
                {
                    Player.AddBuff(ModContent.BuffType<BrimstoneExhaustion>(), CalamityUtils.SecondsToFrames(ForcefieldCreationDelayAfterBreak));
                    ForcefieldHits = 0;
                    ForcefieldIsActive = false;
                }
            }
            else
                ForcefieldHits = 0;
        }

        // Disable the forcefield for when the player respawns.
        public override void UpdateDead() => ForcefieldIsActive = false;
    }
}
