using Terraria.World.Generation;

namespace KawaggyMod.Core.Interfaces
{
    public interface IStructure
    {
        void Generate(GenerationProgress progress);
        string TaskToGenerateInto { get; }
        string TaskName { get; }
        string StructureName { get; }
    }
}
