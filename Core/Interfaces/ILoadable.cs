using Terraria.ModLoader;

namespace KawaggyMod.Core.Interfaces
{
    /// <summary>
    /// Makes something able to load on mod load
    /// </summary>
    interface ILoadable
    {
        void Load(Mod mod);
        void Unload(Mod mod);
    }

    /// <summary>
    /// Makes something able to load not on the server
    /// </summary>
    interface INoServerLoadable : ILoadable { }

    /// <summary>
    /// Makes something able to load after all content has been loaded
    /// </summary>
    interface IPostLoadable
    {
        void PostLoad(Mod mod);
        void Unload(Mod mod);
    }

    /// <summary>
    /// Makes something able to load not on the server after all content has been loaded
    /// </summary>
    interface INoServerPostLoadable : IPostLoadable { }
}
