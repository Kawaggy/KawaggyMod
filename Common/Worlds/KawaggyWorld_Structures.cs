using KawaggyMod.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace KawaggyMod.Common.Worlds
{
    public class KawaggyWorld_Structures : ModWorld
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            List<IStructure> structures = KawaggyMod.Instance.Code.GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IStructure)))
            .Select(t => (IStructure)Activator.CreateInstance(t))
            .ToList();

            foreach (var structure in structures)
            {
                int index = tasks.FindIndex(genPass => genPass.Name.Equals(structure.TaskToGenerateInto));
                if (index != -1)
                {
                    tasks.Insert(index + 1, new PassLegacy(structure.TaskName, structure.Generate));
                }
                else
                {
                    KawaggyMod.Instance.Logger.Error($"Couldn't add structure {structure.StructureName} at task {structure.TaskToGenerateInto}");
                }
            }
        }
    }
}
