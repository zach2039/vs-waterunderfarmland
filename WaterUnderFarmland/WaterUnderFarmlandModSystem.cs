using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

using HarmonyLib;
using Vintagestory.GameContent;
using WaterUnderFarmland.ModPatches;

namespace WaterUnderFarmland
{
    public class WaterUnderFarmlandModSystem : ModSystem
    {
        private IServerNetworkChannel serverChannel;
        private ICoreAPI api;
        public Harmony harmonyInst;

        public override void Start(ICoreAPI api)
        {
            this.api = api;
            base.Start(api);

            api.Logger.Notification("Loaded WaterUnderFarmland!");
        }

        public override void StartServerSide(ICoreServerAPI sapi)
        {
            if (!Harmony.HasAnyPatches(Mod.Info.ModID))
            {
                harmonyInst = new Harmony(Mod.Info.ModID);

                PatchBlockBehaviorFiniteSpreadingLiquidTryLoweringLiquidLevel(sapi, harmonyInst);
            }

            base.StartServerSide(sapi);
        }

        internal void PatchBlockBehaviorFiniteSpreadingLiquidTryLoweringLiquidLevel(ICoreServerAPI sapi, Harmony harmony)
        {
            var original = typeof(BlockEntityFarmland).GetMethod("GetNearbyWaterDistance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var prefix = typeof(BlockEntityFarmlandPatches).GetMethod("GetNearbyWaterDistancePrefix", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

            harmony.Patch(original, new HarmonyMethod(prefix), null);

            sapi.Logger.Notification("Applied patch to VintageStory's BlockEntityFarmland.GetNearbyWaterDistance from WaterUnderFarmland!");
        }
    }
}
