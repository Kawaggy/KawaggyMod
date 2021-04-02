using KawaggyMod.Content.Items.Accessories.Ranger;
using KawaggyMod.Core.Helpers;
using KawaggyMod.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace KawaggyMod
{
    public class KawaggyMod : Mod
    {
        public static KawaggyMod Instance;

        private List<ILoadable> loadCache;
        private List<IPostLoadable> postLoadCache;
        private List<INoServerLoadable> serverLoadCache;
        private List<INoServerPostLoadable> serverPostLoadCache;

        public override void Load()
        {
            Instance = this;
            loadCache = Code.GetTypes()
                .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(ILoadable)) && !t.GetInterfaces().Contains(typeof(INoServerLoadable)))
                .Select(t => (ILoadable)Activator.CreateInstance(t))
                .ToList();

            foreach (var loadable in loadCache)
            {
                loadable.Load(this);
            }

            serverLoadCache = Code.GetTypes()
                .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(INoServerLoadable)))
                .Select(t => (INoServerLoadable)Activator.CreateInstance(t))
                .ToList();

            if (!Main.dedServ)
            {
                foreach (var serverLoadable in serverLoadCache)
                {
                    serverLoadable.Load(this);
                }
            }
        }

        public override void AddRecipeGroups()
        {
            KawaggyHelper.MakeNewGroup("Mods.KawaggyMod.Common.SniperBullet", "AnySniperBullet", new int[]
                {
                    ModContent.ItemType<SniperBullet_GoldCopper>(),
                    ModContent.ItemType<SniperBullet_GoldTin>(),
                    ModContent.ItemType<SniperBullet_PlatinumCopper>(),
                    ModContent.ItemType<SniperBullet_PlatinumTin>()
                });

            KawaggyHelper.MakeNewGroup("Mods.KawaggyMod.Common.Crown", "AnyCrown", new int[]
            {
                ItemID.GoldCrown,
                ItemID.PlatinumCrown
            });
        }

        public override void PostSetupContent()
        {
            postLoadCache = Code.GetTypes()
                .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(IPostLoadable)) && !t.GetInterfaces().Contains(typeof(INoServerPostLoadable)))
                .Select(t => (IPostLoadable)Activator.CreateInstance(t))
                .ToList();

            serverPostLoadCache = Code.GetTypes()
                .Where(t => !t.IsInterface && t.GetInterfaces().Contains(typeof(INoServerPostLoadable)))
                .Select(t => (INoServerPostLoadable)Activator.CreateInstance(t))
                .ToList();

            foreach (var loadable in postLoadCache)
            {
                loadable.PostLoad(this);
            }

            if (!Main.dedServ)
            {
                foreach (var serverLoadable in serverPostLoadCache)
                {
                    serverLoadable.PostLoad(this);
                }
            }
        }

        public override void Unload()
        {
            try
            {
                foreach (var loadable in loadCache)
                {
                    loadable.Unload(this);
                }

                foreach (var postLoadable in postLoadCache)
                {
                    postLoadable.Unload(this);
                }

                if (!Main.dedServ)
                {
                    foreach (var serverLoadable in serverLoadCache)
                    {
                        serverLoadable.Unload(this);
                    }
                    foreach (var serverPostLoadable in serverPostLoadCache)
                    {
                        serverPostLoadable.Unload(this);
                    }
                }
            }
            catch(Exception e)
            {
                Logger.Error(e);
                Logger.Info("Kawaggy's Mod did not fully unload because of the error above. Reload your mods to have it unload properly.");
                Logger.Info("This is likely because of a different mod. If it still happens with no other mods, contact Kawaggy on Discord.");
            }
            finally
            {
                loadCache = null;
                postLoadCache = null;
                serverLoadCache = null;
                serverPostLoadCache = null;
            }

            Instance = null;
        }
    }
}