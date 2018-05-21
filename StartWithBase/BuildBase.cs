using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Terraria.ModLoader;
using Terraria.World.Generation;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.IO;
using System.Reflection;
using System.Threading;
using Terraria.GameContent.UI.States;


using ReLogic.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.UI;

using Terraria.Map;

namespace StartWithBase
{
    class BuildBase : ModWorld
    {
        ushort curTileType = TileID.HayBlock;
        byte curWallType = WallID.Hay;
        int curStyle = 17;

        int curChestStyle = 5;
        int curPlatformStyle = 17;
        int curLanternStyle = 13;
        int curChairStyle = 29;
        int curWorkBenchStyle = 22;
        int curLampStyle = 0;

        int cx, cy;

        public class DrawTask {
            public ushort type;
            public int count;
            public int spriteStyle;
            public bool placeWall;
            
            public DrawTask(ushort type, int count, int spriteStyle = -1, bool placeWall = true)
            {
                this.type = type;
                this.count = count;
                this.spriteStyle = spriteStyle;
                this.placeWall = placeWall;                
            }
        }
        public class DrawLineTask
        {
            public List<DrawTask> drawTaskList;

            public DrawLineTask()
            {
                drawTaskList = new List<DrawTask>();
            }
        }
        public class DrawObjectTask
        {
            public List<DrawLineTask> drawLineList;
            public DrawObjectTask()
            {
                drawLineList = new List<DrawLineTask>();
            }
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {

            tasks.Add(new PassLegacy("Stop Sead search", delegate (GenerationProgress progress)
            {
                int yoff = 42;
                cx = Main.spawnTileX - 20;
                cy = Main.spawnTileY - yoff;

                //WorldGen.Place1x2Top(num, i, 42, style2); lamp

                if (Main.worldName.Contains("all") && Main.worldName.Contains("item"))
                {
                    PlaceAllItems();
                    return;
                }
                
                int townNPCcount = Main.MaxShopIDs; //vanilla town npc
                for (int i=0; i < NPCLoader.NPCCount; i++)
                {
                    //adds town npc's from mods
                    ModNPC cur = NPCLoader.GetNPC(i);
                    if (cur != null)
                        if (cur.npc.CanTalk && cur.npc.friendly) townNPCcount++;
                }
                               
                int allItemCount = ItemLoader.ItemCount;
                int chestsNeeded = (allItemCount + 39) / 40;

                int chestsPerFloor = 9;
                int npcsPerFloor = 3;
                int baseFloor = 3;
                int groundChests = 2*10-1+6+2*4;
                int storageFloorChest = 2 * 18;
                int goundNPC = 2;
                int baseChests = chestsPerFloor * baseFloor + groundChests + storageFloorChest;
                int basNPCFlats = npcsPerFloor * baseFloor + goundNPC;
                                
                writeDebugFile("townNPCs: " + townNPCcount);
                writeDebugFile("basNPCFlats: " + basNPCFlats);
                writeDebugFile("allItemCount: " + allItemCount);
                writeDebugFile("chestsNeeded: " + chestsNeeded);
                writeDebugFile("baseChests: " + baseChests);

                //extra floors needed
                int floorsNeeded = (townNPCcount - basNPCFlats + npcsPerFloor - 1) / npcsPerFloor;
                int newChests = floorsNeeded * chestsPerFloor + baseChests;

                int extraStorageFloor = 0;
                while (newChests < chestsNeeded)
                {
                    newChests += storageFloorChest;
                    extraStorageFloor++;
                }
                if (extraStorageFloor == 0 && floorsNeeded > 4)
                    extraStorageFloor = (floorsNeeded) / 8;

                writeDebugFile("extraFloor: " + floorsNeeded);
                writeDebugFile("extraStorageFloor: " + extraStorageFloor);
                writeDebugFile("totalChest: " + newChests);

                cy -= 4 * floorsNeeded;
                cy -= 6 * extraStorageFloor;

                for (int xi = cx; xi < cx + 40; xi++)
                {
                    for (int yi = cy; yi < cy + yoff-5 + 4 * floorsNeeded + 6* extraStorageFloor; yi++)
                    {
                        Main.tile[xi, yi] = new Tile();
                    }
                }
                DrawBase(floorsNeeded, extraStorageFloor);


            }));
        }

        bool extend = false;
        //const int startingFloors = 4;
        public void DrawBase(int extraFloors, int extraStorageFloor)
        {
            DrawObjectTask floorType1 = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 17, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 17, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, - 1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoom = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
            }
                       

            int floorSpace = (extraFloors ) / (extraStorageFloor + 1);
            int florCountToStorage = 0;

            if (floorSpace < 4)
                extend = true;

            while (extraFloors > 0)
            {               
                if (extraFloors % 2 == 0)
                {
                    DrawAObject(floorType1); cy--;
                }
                else
                {
                    DrawAObject(floorType1, false); cy--;
                }
                extraFloors--;
                if(florCountToStorage+1 == floorSpace && extraFloors > 0)
                {
                    DrawAObject(storageRoom); cy--;
                    florCountToStorage = 0;
                }
                else
                {
                    florCountToStorage++;
                }
            }
           
            DrawAObject(storageRoom); cy--;
            DrawAObject(floorType1); cy--;
            DrawAObject(floorType1, false); cy--;

            DrawObjectTask groundFloor = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 16, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, 7 ));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 25));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 32));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 27, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }                
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 34, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Containers, 1, curChestStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 17));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 37));                                        
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
            }
            DrawAObject(groundFloor); cy--;

            int liquidY = cy;
            DrawObjectTask basement = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 9));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 9));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, - 1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 3, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));                    
                    basement.drawLineList.Add(drawLineTask);
                }                
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5, - 1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5, -1, false));                    
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();                    
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 10, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 5, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(0, 10, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }               
            }
            DrawAObject(basement); cy--;
                        
            Main.tile[cx + 7, liquidY].liquid = 255;                        
            Main.tile[cx + 31, liquidY].honey(true);
            Main.tile[cx + 31, liquidY].liquid = 255;

            int cid = Chest.FindChest(cx+17, liquidY-2);         
            Main.chest[cid].item[0].SetDefaults(ItemID.PalmWood);
            Main.chest[cid].item[0].stack = 100;
            Main.chest[cid].item[1].SetDefaults(ItemID.Hay);
            Main.chest[cid].item[1].stack = 200;

            int rope = 0;
            while ( (!Main.tile[cx + 19, liquidY + 1+ rope].active() || rope < 10)  )
            {
                WorldGen.PlaceTile(cx+19, liquidY+1+rope++, TileID.Rope, true, true, -1, 0);
            }

        }

        public void writeDebugFile(string content)
        {
            using (System.IO.StreamWriter file =
                  new System.IO.StreamWriter(Main.SavePath + @".\debug.txt", true))
            {
                file.WriteLine(content);                
            }
        }

        public void DrawATask(DrawTask task, bool goRightDirec = true )
        {
            int dir = goRightDirec ? 1 : -1;

            int curTileType = task.type;
            int curStyle = task.spriteStyle;
            int count = task.count;
            bool placeWall = task.placeWall;

            while (count > 0)
            {
                count--;
                if(placeWall) WorldGen.PlaceWall(cx, cy, curWallType, true);
                

                if (curTileType == 0)
                {
                    cx += dir;
                    continue;
                }

                if (task.spriteStyle < 0)
                {
                    WorldGen.PlaceTile(cx, cy, curTileType, true, true, -1, 0);
                }
                else 
                {
                    if (curTileType == TileID.Platforms)
                    {
                        WorldGen.PlaceTile(cx, cy, TileID.Platforms, true, true, -1, curStyle);
                    }
                    else if (curTileType == TileID.Containers)
                    {
                        WorldGen.PlaceTile(cx, cy + 2, TileID.Dirt, true, true, -1, 0);
                        WorldGen.PlaceTile(cx + dir, cy + 2, TileID.Dirt, true, true, -1, 0);
                        cx += dir < 0 ? dir : 0;
                        int chestNum = WorldGen.PlaceChest(cx, cy+1, TileID.Containers, false, curStyle);
                        cx += dir > 0 ? dir : 0;
                        if (placeWall) WorldGen.PlaceWall(cx, cy, curWallType, true);
                    }
                    else if (curTileType == TileID.HangingLanterns)
                    {
                        WorldGen.PlaceTile(cx, cy, TileID.HangingLanterns, true, true, -1, curStyle);
                    }
                    else if (curTileType == TileID.WorkBenches)
                    {
                        cx += dir < 0 ? dir : 0;                        
                        WorldGen.PlaceTile(cx, cy+1, TileID.Dirt, true, true, -1, 0); //dummy tiles to stand on
                        WorldGen.PlaceTile(cx+1, cy+1, TileID.Dirt, true, true, -1, 0);
                        WorldGen.PlaceTile(cx, cy, TileID.WorkBenches, true, true, -1, curStyle);
                        cx += dir > 0 ? dir : 0;
                        if (placeWall) WorldGen.PlaceWall(cx, cy, curWallType, true);
                    }
                    else if (curTileType == TileID.Chairs)
                    {
                        //they are drawn at bottom instead of top left
                        WorldGen.PlaceTile(cx, cy + 2, TileID.Dirt, true, true, -1, 0);
                        WorldGen.PlaceTile(cx, cy +1 , 15, true, false, -1, curStyle);
                        if (!((Main.tile[cx-dir, cy].type == 0 && dir > 0) || (Main.tile[cx - dir, cy].type != 0 && dir < 0)))
                        {
                            Tile expr_691 = Main.tile[cx, cy];
                            expr_691.frameX += 18;
                            Tile expr_6B2 = Main.tile[cx, cy - 1];
                            expr_6B2.frameX += 18;
                        }

                    }
                    else if (curTileType == TileID.TallGateOpen || curTileType == TileID.TallGateClosed)
                    {                        
                        WorldGen.PlaceTile(cx, cy + 5, TileID.Dirt, true, true, -1, 0); //dummy tiles to stand on                        
                        WorldGen.PlaceObject(cx, cy , curTileType, true);         
                        
                    }
                    else if (curTileType == TileID.Lamps)
                    {                        
                        WorldGen.PlaceTile(cx, cy + 3, TileID.Dirt, true, true, -1, 0); //dummy tiles to stand on                        
                        WorldGen.PlaceTile(cx, cy + 2, curTileType, true, true, -1, curLampStyle);
                    }
                }
                cx += dir; //could be while inside
            }

        }

        public void DrawALine(DrawLineTask ltask, bool goRightDirec = true)
        {
            List<DrawTask> ldlt = ltask.drawTaskList.ToList();
            if(!goRightDirec) ldlt.Reverse();
            foreach( DrawTask task in ldlt)
            {
                DrawATask(task, true);                
            }
        }

        public void DrawAObject(DrawObjectTask otask, bool goRightDirec = true)
        {
            int startX = cx;
            int startY = cy;
                        
            foreach (DrawLineTask task in otask.drawLineList)
            {
                DrawALine(task, goRightDirec);
                cx = startX;
                cy++;
            }
        }

        public void PlaceAllItems()
        {
            int x = Main.spawnTileX - 20;
            int y = Main.spawnTileY - 20;
           
            int chestNum = NewChest(x, y);

            int ci = 0;
            int xc = 0;
            for (int i = 0; i < ItemLoader.ItemCount; i++)
            {
                Main.chest[chestNum].item[ci].SetDefaults(i);
                Main.chest[chestNum].item[ci].stack = Main.chest[chestNum].item[ci].maxStack;
                ci++;
                if (ci == 40)
                {
                    ci = 0;
                    x += 2;
                    xc++;
                    chestNum = NewChest(x, y);

                    if (xc == 25)
                    {
                        xc = 0;
                        x = Main.spawnTileX - 22;
                        y -= 4;
                    }
                }
            }
            
        }               

        public int NewChest(int x, int y)
        {
            WorldGen.PlaceWall(x, y, curWallType, true);
            WorldGen.PlaceTile(x, y, TileID.Platforms, true, true, -1, curPlatformStyle);
            WorldGen.PlaceTile(x+1, y, TileID.Platforms, true, true, -1, curPlatformStyle);            
            int chestNum = WorldGen.PlaceChest(x, y - 1, TileID.Containers, false, WorldGen.genRand.Next(5, 22));
            return chestNum;
        }
        
        


    }
}
