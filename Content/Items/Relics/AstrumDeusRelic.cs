﻿using InfernumMode.Content.Tiles.Relics;
using Terraria.ModLoader;

namespace InfernumMode.Content.Items.Relics
{
    public class AstrumDeusRelic : BaseRelicItem
    {
        public override string DisplayNameToUse => "Infernal Astrum Deus Relic";

        public override int TileID => ModContent.TileType<AstrumDeusRelicTile>();
    }
}
