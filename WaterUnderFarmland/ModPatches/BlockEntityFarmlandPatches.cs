using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace WaterUnderFarmland.ModPatches
{
    public class BlockEntityFarmlandPatches
    {
        public static bool GetNearbyWaterDistancePrefix(BlockEntityFarmland __instance, ref float __result, ref object result, float hoursPassed, ref bool ___farmlandIsAtChunkEdge, ref float[] ___damageAccum, ref double ___lastWaterSearchedTotalHours)
        {
            Type enumWaterSearchResultType = typeof(BlockEntityFarmland).GetNestedType("EnumWaterSearchResult", BindingFlags.NonPublic | BindingFlags.Instance);
            float waterDistance = 99f;
            ___farmlandIsAtChunkEdge = false;
            bool isAtChunkEdge = false;
            bool saltWater = false;
            __instance.Api.World.BlockAccessor.SearchFluidBlocks(new BlockPos(__instance.Pos.X - 4, __instance.Pos.Y, __instance.Pos.Z - 4), new BlockPos(__instance.Pos.X + 4, __instance.Pos.Y, __instance.Pos.Z + 4), delegate (Block block, BlockPos pos)
            {
                if (block.LiquidCode == "water")
                {
                    waterDistance = Math.Min(waterDistance, (float)Math.Max(Math.Abs(pos.X - __instance.Pos.X), Math.Abs(pos.Z - __instance.Pos.Z)));
                }
                if (block.LiquidCode == "saltwater")
                {
                    saltWater = true;
                }
                return true;
            }, delegate (int cx, int cy, int cz)
            {
                isAtChunkEdge = true;
            });
            if (waterDistance > 1f) // Search for block below only if found water distance is greater than 1f
            {
                if (__instance.Api.World.BlockAccessor.GetBlock(new BlockPos(__instance.Pos.X, __instance.Pos.Y - 1, __instance.Pos.Z), BlockLayersAccess.Fluid) is Block liquidBlockBelow)
                {
                    if (liquidBlockBelow.LiquidCode == "water")
                    {
                        waterDistance = 1f;
                    }
                    if (liquidBlockBelow.LiquidCode == "saltwater")
                    {
                        saltWater = true;
                    }
                }
            }
            if (saltWater)
            {
                ___damageAccum[4] += hoursPassed;
            }
            result = Enum.ToObject(enumWaterSearchResultType, 2); // Deferred
            if (isAtChunkEdge)
            {
                ___farmlandIsAtChunkEdge = true;
                __result = 99f;
                return false; // skip original method
            }
            ___lastWaterSearchedTotalHours = __instance.Api.World.Calendar.TotalHours;
            if (waterDistance < 4f)
            {
                result = Enum.ToObject(enumWaterSearchResultType, 0); // Found
                __result = waterDistance;
                return false; // skip original method
            }
            
            result = Enum.ToObject(enumWaterSearchResultType, 1); // Not Found
            __result = 99f;
            return false; // skip original method
        }
    }
}
