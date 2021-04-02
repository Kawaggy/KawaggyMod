using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace KawaggyMod.Core.Helpers
{
    public static partial class KawaggyHelper
    {
        public static void NewText(object obj, Color color = default, bool force = false, int excludedPlayer = -1)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(obj, color, force);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                if (obj is string key)
                {
                    NetMessage.BroadcastChatMessage(NetworkText.FromKey(key), color, excludedPlayer);
                }
            }
        }

        public static bool AnyBossAlive() => Main.npc.Any(n => n.active && n.boss && n.whoAmI != Main.maxNPCs);

        public static bool DungeonIsOnLeftSide() => WorldGen.dungeonX < Main.maxTilesX / 2;

        public static bool ActiveEvent() => Main.pumpkinMoon || Main.snowMoon || Main.bloodMoon || Main.eclipse;

        public static bool ActiveInvasion() => Main.invasionType > 0;

        public static bool ActiveInvasionOrEvent() => ActiveEvent() || ActiveInvasion();

        public static bool ActiveInvasionOrEventOrBoss() => AnyBossAlive() || ActiveInvasionOrEvent();

        public static string WorldSize()
        {
            switch (WorldSizeFloat())
            {
                case 1f:
                    return "small";
                case 1.5f:
                    return "medium";
                case 2f:
                    return "big";
                default:
                    return "dontknow";
            }
        }

        public static float WorldSizeFloat() => Main.maxTilesX / 4200f;

        public static string UsedKeys(this ModHotKey hotkey)
        {
            if (Main.dedServ || hotkey == null)
                return "";

            List<string> assinedKeys = hotkey.GetAssignedKeys();
            if (assinedKeys.Count == 0) //there are none
                return "[NONE]";

            string keys = assinedKeys[0];
            for (int i = 1; i < assinedKeys.Count; i++)
                keys += " | " + assinedKeys[i];

            return keys;
        }

        public static void MakeNewGroup(string description, string name, int[] itemList) 
        {
            RecipeGroup.RegisterGroup(
                "KawaggyMod:" + name,
                new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " " + Language.GetTextValue(description),
                itemList));
        }

        public static void EdgyTalk(this Entity entity, string text, Color color = default, bool combatText = false, bool dramatic = false, bool dot = false, bool force = false, int excludedPlayer = -1)
        {
            if (combatText)
            {
                if (entity is Player player)
                {
                    if (player.whoAmI == Main.myPlayer)
                    {
                        CombatText.NewText(player.Hitbox, color, text, dramatic, dot);
                    }
                }
                else
                {
                    CombatText.NewText(entity.Hitbox, color, text, dramatic, dot);
                }
            }
            else
            {
                NewText(text, color, force, excludedPlayer);
            }
        }

        public static bool IsInsideTile(this Entity entity)
        {
            int i = (int)(entity.position.X + (entity.width / 2)) / 16;
            int j = (int)(entity.position.Y + (entity.height / 2)) / 16;
            return (WorldGen.SolidTile(i, j) || Main.tile[i, j].halfBrick() || Main.tile[i, j].slope() > 0) && Main.tile[i, j].type != TileID.Platforms;
        }

        public static void CapSpeed(this Entity entity, float minSpeedX, float minSpeedY, float maxSpeedX, float maxSpeedY)
        {
            entity.velocity.X = MathHelper.Clamp(entity.velocity.X, minSpeedX, maxSpeedX);
            entity.velocity.Y = MathHelper.Clamp(entity.velocity.Y, minSpeedY, maxSpeedY);
        }
        public static void CapSpeed(this Entity entity, float speedX, float speedY) => entity.CapSpeed(-speedX, -speedY, speedX, speedY);
        public static void CapSpeed(this Entity entity, float speed) => entity.CapSpeed(-speed, -speed, speed, speed);


        /// <returns>Seconds</returns>
        public static int ToSeconds(this int ticks) => ticks * 60;
        /// <returns>Ticks</returns>
        public static int ToTicks(this int seconds) => seconds / 60;

        public static int RandomDamage(this int damage, float loss)
        {
            return (int)((damage * loss) * (1 + (Main.rand.NextBool() ? MathHelper.Clamp(Main.rand.NextFloat(), 0.1f, 0.5f) : -MathHelper.Clamp(Main.rand.NextFloat(), 0.1f, 0.5f))));
        }

        public static int RandomDamage(this int damage) => RandomDamage(damage, 1f);
    }
}
