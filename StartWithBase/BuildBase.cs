﻿using System;
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
        static int curTileType = TileID.HayBlock;
        static int curTileItemType = ItemID.Hay;
        static byte curWallType = WallID.Hay;
        static int curWallItemType = ItemID.HayWall;

        static int curStyle = 17;

        static int curChestStyle = 5;

        static int curPlatformStyle = 17;
        static int curLanternStyle = 13;
        static int curChairStyle = 29;
        static int curWorkBenchStyle = 22;
        static int curLampStyle = 0;
        static int curTorchStyle = 0;
        static int curPlatformItemType = ItemID.PalmWood;
        

        int cx, cy;

        
        static Dictionary<string, StyleSetting> styleTypeDict = new Dictionary<string, StyleSetting>
            {
            {"fpa", new StyleSetting{ PlatformStyle = 17, LanternStyle = 27, ChairStyle = 29, WorkBenchStyle = 22, LampStyle = 18, TorchStyle = 0, PlatformItemType = ItemID.PalmWood } },
            {"fdy", new StyleSetting{ PlatformStyle = 32, LanternStyle = 26, ChairStyle = 27, WorkBenchStyle = 18, LampStyle = 17, TorchStyle = 0, PlatformItemType = ItemID.DynastyWood } },
            {"fwo", new StyleSetting{ PlatformStyle = 0, LanternStyle = 22, ChairStyle = 0, WorkBenchStyle = 0, LampStyle = 0, TorchStyle = 0, PlatformItemType = ItemID.Wood } },
            {"fgr", new StyleSetting{ PlatformStyle = 28, LanternStyle = 35, ChairStyle = 34, WorkBenchStyle = 29, LampStyle = 29, TorchStyle = 0, PlatformItemType = ItemID.GraniteBlock } },
            {"fbo", new StyleSetting{ PlatformStyle = 19, LanternStyle = 29, ChairStyle = 30, WorkBenchStyle = 23, LampStyle = 20, TorchStyle = 0, PlatformItemType = ItemID.BorealWood } },
            {"fri", new StyleSetting{ PlatformStyle = 2, LanternStyle = 16, ChairStyle = 3, WorkBenchStyle = 2, LampStyle = 6, TorchStyle = 0, PlatformItemType = ItemID.RichMahogany} },
         };
        static Dictionary<string, WallSetting> wallTypeDict = new Dictionary<string, WallSetting>
            {
            {"wha", new WallSetting{ WallID = WallID.Hay, ItemIDofWallType = ItemID.HayWall} },
            {"wdg", new WallSetting{ WallID = WallID.DiamondGemspark, ItemIDofWallType = ItemID.DiamondGemsparkWall} },
            {"wdy", new WallSetting{ WallID = WallID.WhiteDynasty, ItemIDofWallType = ItemID.WhiteDynastyWall} },
            {"wwo", new WallSetting{ WallID = WallID.Wood, ItemIDofWallType = ItemID.WoodWall} },
            {"wss", new WallSetting{ WallID = WallID.StoneSlab, ItemIDofWallType = ItemID.StoneSlabWall} },
            {"wdi", new WallSetting{ WallID = WallID.Dirt, ItemIDofWallType = ItemID.DirtWall} },
            {"wri", new WallSetting{ WallID = WallID.RichMaogany, ItemIDofWallType = ItemID.RichMahoganyWall} },
            {"wbo", new WallSetting{ WallID = WallID.BorealWood, ItemIDofWallType = ItemID.BorealWoodWall} },
         };
        static Dictionary<string, TileSetting> TileTypeDict = new Dictionary<string, TileSetting>
            {
            {"tha", new TileSetting{ TileID = TileID.HayBlock, ItemID = ItemID.Hay } },
            {"tob", new TileSetting{ TileID = TileID.ObsidianBrick, ItemID = ItemID.ObsidianBrick} },
            {"tdy", new TileSetting{ TileID = TileID.DynastyWood, ItemID = ItemID.DynastyWood} },
            {"two", new TileSetting{ TileID = TileID.WoodBlock, ItemID = ItemID.Wood} },
            {"tss", new TileSetting{ TileID = TileID.StoneSlab, ItemID = ItemID.StoneSlab} },
            {"tri", new TileSetting{ TileID = TileID.RichMahogany, ItemID = ItemID.RichMahogany} },
            {"tbo", new TileSetting{ TileID = TileID.BorealWood, ItemID = ItemID.BorealWood} },            
         };


        BaseType baseType = BaseType.Base3;
        public enum BaseType { Base2, Base3, Base4, Base6 };
        public class Base
        {
            BaseType baseType;
        }

        public class StyleSetting
        {
            public int PlatformStyle { get; set; }
            public int LanternStyle { get; set; }
            public int ChairStyle { get; set; }
            public int WorkBenchStyle { get; set; }
            public int LampStyle { get; set; }
            public int TorchStyle { get; set; }
            public int PlatformItemType { get; set; }
        }
        public class WallSetting
        {
            public int WallID { get; set; }
            public int ItemIDofWallType { get; set; }            
        }
        public class TileSetting
        {
            public int TileID { get; set; }
            public int ItemID { get; set; }
        }

        public class DrawTask {
            public int type;
            public int count;
            public int spriteStyle;
            public bool placeWall;
            
            public DrawTask(int type, int count, int spriteStyle = -1, bool placeWall = true)
            {
                this.type = type;
                this.count = count;
                this.spriteStyle = spriteStyle;
                this.placeWall = placeWall;                
            }

            public DrawTask(string task)
            {
                char num = task[0];
                char type = task[1];

                string numbers = "0123456789abcdefghijklmnopqrstABCDEFGHIJKLMNOPQRSTZ"; //a = 10, k=20; A=30; k=40; Z=50

                this.placeWall = Char.IsLower(type); // place wall if type is written small letters
                this.count = numbers.IndexOf(num);
                if (this.count < 0) throw new Exception("invalid draw task count "+ num  +" ("+ task[0]+")"+"\n");
                

                type = Char.ToUpper(type);
                this.spriteStyle = -1;

                switch (type)
                {
                    //Blocks
                    //Platform
                    //Storage container
                    //Hanging lantern
                    //Chair
                    //Work bench
                    //tall Gate
                    //Lamp
                    //Empty (or only fill walls)

                    case 'B':
                        //block
                        this.type = curTileType;                        
                        break;
                    case 'P':
                        this.type = TileID.Platforms;
                        this.spriteStyle = curPlatformStyle;
                        break;
                    case 'S':
                        //storage container
                        this.type = TileID.Containers;
                        this.spriteStyle = curChestStyle;
                        break;
                    case 'H':                        
                        this.type = TileID.HangingLanterns;
                        this.spriteStyle = curLanternStyle;
                        break;
                    case 'C':                        
                        this.type = TileID.Chairs;
                        this.spriteStyle = curChairStyle;
                        break;
                    case 'W':                        
                        this.type = TileID.WorkBenches;
                        this.spriteStyle = curWorkBenchStyle;
                        break;
                    case 'G':                        
                        this.type = TileID.TallGateOpen;
                        this.spriteStyle = 0;
                        break;
                    case 'L':                        
                        this.type = TileID.Lamps;
                        this.spriteStyle = curLampStyle;
                        break;
                    case 'E':
                        //empty tile or only place walls
                        this.type = -1;                        
                        break;
                    case 'T':
                        this.type = TileID.Torches;
                        this.spriteStyle = curTorchStyle;
                        break;
                }


            }
        }
        public class DrawLineTask
        {
            public List<DrawTask> drawTaskList;

            public DrawLineTask()
            {
                drawTaskList = new List<DrawTask>();
            }
            public DrawLineTask(string string2Parse)
            {
                drawTaskList = new List<DrawTask>();
                for (int i = 0; i < string2Parse.Length-1; i+=2)
                {
                    drawTaskList.Add(new DrawTask(string2Parse.Substring(i, 2)));
                }
            }
        }
        public class DrawObjectTask
        {
            public List<DrawLineTask> drawLineList;
            public DrawObjectTask()
            {
                drawLineList = new List<DrawLineTask>();
            }
            public DrawObjectTask(string string2Parse)
            {
                drawLineList = new List<DrawLineTask>();
                string[] lines = string2Parse.Split('|');
                for(int i=0; i < lines.Length; i++)
                drawLineList.Add(new DrawLineTask(lines[i]));
            }
        }

        int townNPCcount;
        int allItemCount;
        int chestsNeeded;
        const int yoff = 13;
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {

            tasks.Add(new PassLegacy("Stop Sead search", delegate (GenerationProgress progress)
            {                
                cx = Main.spawnTileX - 20;
                cy = Main.spawnTileY - yoff;

                
                if (Main.worldName.Contains("all") && Main.worldName.Contains("item"))
                {
                    PlaceAllItems();
                    return;
                }
                
                townNPCcount = Main.MaxShopIDs; //vanilla town npc
                int count2 = townNPCcount;               
                for (int i=0; i < NPCLoader.NPCCount; i++)
                {
                    //adds town npc's from mods
                    ModNPC cur = NPCLoader.GetNPC(i);
                    if (cur != null)
                        if (cur.npc.CanTalk && cur.npc.friendly) townNPCcount++;
                    if (cur != null)
                        if (cur.npc.townNPC) count2++;                                    

                }
                //all vanilla 1.3.5.3 npc count: 23
                townNPCcount = Math.Max(townNPCcount, count2);
                

                allItemCount = ItemLoader.ItemCount;
                chestsNeeded = (allItemCount + 39) / 40;

                
                string wname = Main.worldName;
                param = new List<string>();
                //if (!wname.Contains("$"))
                //    wname = wname + "$sy*";
                if (wname.Contains("$"))
                    parsWN(wname);

                if(baseType == BaseType.Base2)
                    Base2(); 
                else if (baseType == BaseType.Base6)
                    Base6(); //up to 90 at small
                else if (baseType == BaseType.Base4)
                    Base4();                
                else 
                    Base3(); //if (param.Contains("ba3")) max npc about 80 at small world


                param = null;
            }));
        }



        List<string> param;
        public void parsWN(string wname)
        {
            int from = wname.IndexOf("$");
            int to = wname.Length;
            extend = false;

            string subs = wname.Substring(from, to - from);

            //rename world file
            string newWN = wname.Substring(0, from);
            Main.worldName = newWN;
            string seed = Main.ActiveWorldFileData.SeedText;
            Main.ActiveWorldFileData = WorldFile.CreateMetadata(Main.worldName, Main.ActiveWorldFileData.IsCloudSave, Main.expertMode);
            Main.ActiveWorldFileData.SetSeed(seed);

            subs = subs.Substring(1);
            subs = subs.ToLower().Normalize();
            if (subs.Length == 3 && (subs.Substring(0,2).Equals("ba") || subs.Equals("b3b")))
                subs = subs + "sy*";
            
            for(int i=0; i < subs.Length/3;i++)
            {
                string pa = subs.Substring(3 * i, 3);
                param.Add(pa);

                if (pa.Substring(0, 2).Equals("ba"))
                {
                    if (pa.Equals("ba2")) baseType = BaseType.Base2;
                    if (pa.Equals("ba3")) baseType = BaseType.Base3;
                    if (pa.Equals("ba4")) baseType = BaseType.Base4;
                    if (pa.Equals("ba6")) baseType = BaseType.Base6;
                }
                if (pa.Equals("b3b"))
                {
                    baseType = BaseType.Base3;
                    extend = true;
                }

                if (pa.Equals("sy*"))
                {
                    //random
                    //int val = WorldGen.genRand.Next(100);
                    //baseType = val<10? BaseType.Base2 : val < 20 ? BaseType.Base6: BaseType.Base3;
                    int dummy = (WorldGen.genRand.Next(Math.Abs(Main.worldName.GetHashCode() - (Main.player.Length>0? Main.player[0].name.GetHashCode():0)+ DateTime.Now.Second+ DateTime.Now.Millisecond)))%100;
                    while (dummy-->0) WorldGen.genRand.Next();
                    SetToFurniture( (styleTypeDict.ElementAt( WorldGen.genRand.Next(styleTypeDict.Count) )).Value);
                    SetToWall((wallTypeDict.ElementAt( WorldGen.genRand.Next(wallTypeDict.Count))).Value);
                    if(curWallType == wallTypeDict["wri"].WallID || curWallType == wallTypeDict["wdg"].WallID) SetToWall((wallTypeDict.ElementAt(WorldGen.genRand.Next(wallTypeDict.Count))).Value);//dont like mahagony reduce chance, digew hard to get
                    SetToTiles((TileTypeDict.ElementAt( WorldGen.genRand.Next(TileTypeDict.Count))).Value);
                    if (curTileType == TileTypeDict["tri"].TileID || curTileType == TileTypeDict["tob"].TileID) SetToWall((wallTypeDict.ElementAt(WorldGen.genRand.Next(wallTypeDict.Count))).Value);//dont like mahagony, obsidian hard to get reduce chance
                }
                if (pa.Equals("sy0"))
                {   
                    SetToFurniture(styleTypeDict["fpa"]);
                    SetToWall(wallTypeDict["wha"]);
                    SetToTiles(TileTypeDict["tha"]);
                    curLanternStyle = 13;
                }
                if (pa.Equals("sy1"))
                {
                    SetToFurniture(styleTypeDict["fwo"]);
                    SetToWall(wallTypeDict["wdi"]);
                    SetToTiles(TileTypeDict["tss"]);
                    curLanternStyle = 26;
                }
                if (pa.Equals("sy2"))
                {
                    SetToFurniture(styleTypeDict["fwo"]);
                    SetToWall(wallTypeDict["wdg"]);
                    SetToTiles(TileTypeDict["tob"]);
                    curLanternStyle = 15;
                }


                if (styleTypeDict.ContainsKey(pa))
                    SetToFurniture(styleTypeDict[pa]);
                else if (wallTypeDict.ContainsKey(pa))
                    SetToWall(wallTypeDict[pa]);
                else if (TileTypeDict.ContainsKey(pa))
                    SetToTiles(TileTypeDict[pa]);
            }
        }
        

        public void SetToFurniture(StyleSetting ss)
        {
            curPlatformStyle = ss.PlatformStyle;
            curLanternStyle = ss.LanternStyle;
            curChairStyle = ss.ChairStyle;
            curWorkBenchStyle = ss.WorkBenchStyle;
            curLampStyle = ss.LampStyle;
            curTorchStyle = ss.TorchStyle;
            curPlatformItemType = ss.PlatformItemType;
        }
        public void SetToWall(WallSetting ws)
        {
            curWallType = (byte)ws.WallID;
            curWallItemType = ws.ItemIDofWallType;
        }

        public void SetToTiles(TileSetting ts)
        {
            curTileType = (ushort)ts.TileID;
            curTileItemType = ts.ItemID;
        }

        const int stackSize = 42;
        public void SetUpChest(int cid)
        {
            Main.chest[cid].item[0].SetDefaults(curPlatformItemType);
            Main.chest[cid].item[0].stack = stackSize;
            Main.chest[cid].item[1].SetDefaults(curTileItemType);
            Main.chest[cid].item[1].stack = stackSize;
            Main.chest[cid].item[2].SetDefaults(curWallItemType);
            Main.chest[cid].item[2].stack = stackSize;
            
        }

        public void Base2()
        {
            int chestsPerFloor = 2;
            int npcsPerFloor = 2;    
           
            int baseChests = 0;
            int basNPCFlats = 0;


            //extra floors needed
            int floorsNeeded = (townNPCcount - basNPCFlats + npcsPerFloor - 1) / npcsPerFloor;
            int newChests = floorsNeeded * chestsPerFloor + baseChests;


            cy -= 23 ;            
            int range = 2 * floorsNeeded+2;
            cx += 20 - 4;

            ClearField(23, range);

            DrawBase2(floorsNeeded);
        }


        public void Base3()
        {
            int chestsPerFloor = 9 ;
            int npcsPerFloor = 3;
            int baseFloor = 3;
            int groundChests = 2 * 10 - 1 + 6 + 2 * 4;
            int storageFloorChest = 2 * 18;
            int goundNPC = 2;
            int baseChests = chestsPerFloor * baseFloor + groundChests + storageFloorChest;
            int basNPCFlats = npcsPerFloor * baseFloor + goundNPC;


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


            

            int floorSpace = (floorsNeeded) / (extraStorageFloor + 1);
            if (floorSpace < 4)
            {
                floorSpace = 4;
                extraStorageFloor = floorsNeeded / floorSpace - 1 ;
                extend = true;
            }
                
            

            if (extend)
            {
                cy -= 7;
                cx -= 7;
            }

            cy -= 4 * floorsNeeded;
            cy -= 7 * extraStorageFloor;
            int groundBaseHight = 29;
            cy -= groundBaseHight;


            ClearField(groundBaseHight + 4 * floorsNeeded + 7 * extraStorageFloor + +(extend ? 7 : 0) , 2 + (extend?7:0));

            DrawBase3(floorsNeeded, extraStorageFloor);
        }

        public void Base4()
        {
            int chestsPerFloor = 12;
            int npcsPerFloor = 6;
            int baseFloor = 0;
            
            int startBaseChests = 80 + baseFloor * chestsPerFloor;
            int startBaseNPC = 23 + baseFloor * npcsPerFloor;
            

            //extra floors needed
            int floorsNeeded = (townNPCcount - startBaseNPC + npcsPerFloor - 1) / npcsPerFloor;
            

            int newChests = floorsNeeded * chestsPerFloor + startBaseChests;

            

            int translateY = 47;
            cy -= translateY;
            int spaceXneeded = floorsNeeded * 8;

            ClearField(translateY, (spaceXneeded+1/2) + 2);
            cx++;

            DrawBase4(floorsNeeded);
        }

        public void Base6()
        {
            int chestsPerFloor = 10;
            int npcsPerFloor = 6;
            int baseFloor = 0;
            int groundChests = 4;            
            int goundNPC = 2;
            int baseChests = chestsPerFloor * baseFloor + groundChests;
            int basNPCFlats = npcsPerFloor * baseFloor + goundNPC;


            //extra floors needed
            int floorsNeeded = (townNPCcount - basNPCFlats + npcsPerFloor - 1) / npcsPerFloor;
            int newChests = floorsNeeded * chestsPerFloor + baseChests;

            int storageExit = floorsNeeded > 6 ? floorsNeeded / 6 : 0;

            int translateY = 8 * floorsNeeded + storageExit * 7;
            cy -= translateY;
            
            
            ClearField(translateY, 2);
            

            DrawBase6(floorsNeeded, storageExit);


        }

        public void ClearField(int yend, int xdimPlus = 0)
        {
            for (int xi = cx- xdimPlus; xi < cx + 40 + xdimPlus; xi++)
            {
                for (int yi = cy; yi < cy + yoff + yend -5; yi++)
                {
                    Main.tile[xi, yi] = new Tile();
                }
            }

        }

        public static void writeDebugFile(string content, bool append=true)
        {
            using (System.IO.StreamWriter file =
             new System.IO.StreamWriter(Main.SavePath + @".\debug.txt", append))
            {
                file.WriteLine(content);
            }
        }


        bool extend = false;
      
        public void DrawBase3(int extraFloors, int extraStorageFloor)
        {
            int startX = cx, startY = cy;

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
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, - 1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
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
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle,  false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 16, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
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

            DrawObjectTask storageRoomExtendTop = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoomExtend = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Lamps, 1, curLampStyle));                    
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoomExtendbase = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend3 = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend4 = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
            }


            int floorSpace = (extraFloors ) / (extraStorageFloor + 1);
            int florCountToStorage = 0;
                        

            if (extend)
            {
                
                DrawAObject(storageRoomExtendTop); cy -= 8; cx = cx + 7;
                DrawAObject(storageRoom); cy -= 8; cx = cx + 38;
                DrawAObject(storageRoomExtendTop, false); cy -= 1;
                
                cx = startX + 7;
                //gates not needed anymore
                Main.tile[cx, cy-1].active(false); Main.tile[cx + 38, cy - 1].active(false);
                Main.tile[cx, cy-2].active(false); Main.tile[cx + 38, cy - 2].active(false);
                Main.tile[cx, cy-3].active(false); Main.tile[cx + 38, cy - 3].active(false);
                Main.tile[cx, cy-4].active(false); Main.tile[cx + 38, cy - 4].active(false);
                Main.tile[cx, cy-5].active(false); Main.tile[cx + 38, cy - 5].active(false);
                Main.tile[cx, cy-6].active(false); Main.tile[cx + 38, cy - 6].active(false);
                WorldGen.PlaceTile(cx, cy-1, TileID.Lamps, true, true, - 1, curLampStyle);
                WorldGen.PlaceTile(cx+38, cy-1, TileID.Lamps, true, true, - 1, curLampStyle);

            }

            int extraFloorsStart = extraFloors;
            int extraStorageFloorStart = extraStorageFloor;
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
                if(florCountToStorage+1 == floorSpace && extraFloors > 0 && extraStorageFloor > 0)
                {
                    DrawAObject(storageRoom); cy--;
                    extraStorageFloor--;
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
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 25));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 14));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 27, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }                
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 34, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Containers, 1, curChestStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 18));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 37));                                        
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
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
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, - 1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 3, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));                    
                    basement.drawLineList.Add(drawLineTask);
                }                
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5, - 1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 6, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5, -1, false));                    
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();                    
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 10, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 10, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }               
            }
            DrawAObject(basement); cy--;
                        
            Main.tile[cx + 7, liquidY].liquid = 255;                        
            Main.tile[cx + 31, liquidY].honey(true);
            Main.tile[cx + 31, liquidY].liquid = 255;

            int cid = Chest.FindChest(cx+17, liquidY-2);
            SetUpChest(cid);

            RopeToGround(cx + 19, liquidY + 1);


            if (extend)
            {
                cx = startX;
                cy = startY + 7;
                int floorsGenToExtend = extraFloorsStart;
                int storageGenToExtend = extraStorageFloorStart;

                while (floorsGenToExtend > 0)
                {
                    floorSpace = Math.Min(floorSpace, floorsGenToExtend);
                    if (storageGenToExtend == 0) floorSpace = floorsGenToExtend;

                    int startWith = floorSpace % 3 + 2;
                    if (startWith != 0)
                    {
                        if (startWith == 3)
                        {
                            DrawAObject(floorExtend3); cy -= 5; cx = cx + 45;
                            DrawAObject(floorExtend3, false); cy -= 1; cx = startX;
                        }
                        if (startWith == 4)
                        {
                            DrawAObject(floorExtend4); cy -= 6; cx = cx + 45;
                            DrawAObject(floorExtend4, false); cy -= 1; cx = startX;
                        }

                    }
                    int extraExtendFloors = (floorSpace *4 ) /3 - (startWith>2?1:0);

                    while (extraExtendFloors > 0)
                    {
                        DrawAObject(floorExtend); cy -= 4; cx = cx + 45;
                        DrawAObject(floorExtend, false); cy -= 1; cx = startX;

                        extraExtendFloors--;
                    }

                    if (storageGenToExtend > 0)
                    {
                        Main.tile[cx + 7, cy + 1].active(false); Main.tile[cx + 45, cy + 1].active(false);
                        Main.tile[cx + 7, cy + 2].active(false); Main.tile[cx + 45, cy + 2].active(false);
                        Main.tile[cx + 7, cy + 3].active(false); Main.tile[cx + 45, cy + 3].active(false);
                        Main.tile[cx + 7, cy + 4].active(false); Main.tile[cx + 45, cy + 4].active(false);
                        Main.tile[cx + 7, cy + 5].active(false); Main.tile[cx + 45, cy + 5].active(false);
                        Main.tile[cx + 7, cy + 6].active(false); Main.tile[cx + 45, cy + 6].active(false);
                        DrawAObject(storageRoomExtend); cy -= 8; cx = cx + 45;
                        DrawAObject(storageRoomExtend, false); cy -= 1; cx = startX;
                        storageGenToExtend--;
                    }

                    floorsGenToExtend -= floorSpace;
                }

                //fill up base and remove gates                
                Main.tile[cx + 7, cy + 1].active(false); Main.tile[cx + 45, cy + 1].active(false);
                Main.tile[cx + 7, cy + 2].active(false); Main.tile[cx + 45, cy + 2].active(false);
                Main.tile[cx + 7, cy + 3].active(false); Main.tile[cx + 45, cy + 3].active(false);
                Main.tile[cx + 7, cy + 4].active(false); Main.tile[cx + 45, cy + 4].active(false);
                Main.tile[cx + 7, cy + 5].active(false); Main.tile[cx + 45, cy + 5].active(false);
                Main.tile[cx + 7, cy + 6].active(false); Main.tile[cx + 45, cy + 6].active(false);
                DrawAObject(storageRoomExtend); cy -= 8; cx = cx + 45;
                DrawAObject(storageRoomExtend, false); cy -= 1; cx = startX;

                int num = 4;
                while (num-- > 0)
                {
                    DrawAObject(floorExtend); cy -= 4; cx = cx + 45;
                    DrawAObject(floorExtend, false); cy -= 1; cx = startX;
                }                
                Main.tile[cx + 7, cy + 1].active(false); Main.tile[cx + 45, cy + 1].active(false);
                Main.tile[cx + 7, cy + 2].active(false); Main.tile[cx + 45, cy + 2].active(false);
                Main.tile[cx + 7, cy + 3].active(false); Main.tile[cx + 45, cy + 3].active(false);
                Main.tile[cx + 7, cy + 4].active(false); Main.tile[cx + 45, cy + 4].active(false);
                Main.tile[cx + 7, cy + 5].active(false); Main.tile[cx + 45, cy + 5].active(false);
                DrawAObject(storageRoomExtendbase); cy -= 7; cx = cx + 45;
                DrawAObject(storageRoomExtendbase, false); cy -= 1;
                

                cx = startX + 7;
            }


        }

        public void RopeToGround(int x, int y)
        {
            int rope = 0;
            while ((!Main.tile[x, y + rope].active() || rope < 5))
            {
                WorldGen.PlaceTile(x, y + rope++, TileID.Rope, true, true, -1, 0);
            }

        }


        public void DrawBase6(int extraFloors, int extraStorageFloor)
        {
            
            DrawObjectTask floorType1 = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 9, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));                    
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 11, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HolidayLights, 1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HolidayLights, 1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 8));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 11, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 7));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 7));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 11, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 11, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 11, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }

            }

            DrawObjectTask storageExit = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.TallGateClosed, 1, 0, false));                    
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1,-1,false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 31, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 9));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 7, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
            }

            int floorSpace = (extraFloors) / (extraStorageFloor + 1);
            int florCountToStorage = 0;

            while (extraFloors > 0)
            {
                DrawAObject(floorType1); cy-=2;
                extraFloors--;
                if (florCountToStorage + 1 == floorSpace && extraFloors > 0 && extraStorageFloor > 0)
                {
                    cy++; DrawAObject(storageExit); cy--;
                    extraStorageFloor--;
                    florCountToStorage = 0;
                }
                else
                {
                    florCountToStorage++;
                }
            }

            //DrawAObject(floorType1); cy--;

            Main.tile[cx + 3,  cy - 5].liquid = 255;
            Main.tile[cx + 29, cy - 5].honey(true);
            Main.tile[cx + 29, cy - 5].liquid = 255;
                       
                        
            RopeToGround(cx + 16, cy + 2);


            WorldGen.PlaceTile(cx + 14, cy, TileID.Containers, true, true, -1, curChestStyle);
            int cid = Chest.FindChest(cx + 14, cy -1);
            SetUpChest(cid);


        }

        public void Fill(int xtl, int ytl, int dimX, int dimY, int wallID, int tileID = -1, int style = 0)
        {
            for(int x= xtl; x< xtl + dimX;x++)
                for (int y = ytl; y < ytl + dimY; y++)
                {
                    if (wallID > 0)
                        WorldGen.PlaceWall(x, y, wallID, true);

                    if(tileID>=0)
                        WorldGen.PlaceTile(x, y, tileID, true, true, -1, style);
                }



        }


        public void DrawBase4(int extraFloors)
        {
            int startX = cx; //top left of inner base
            int startY = cy;

            
            int extraFloorsParam = extraFloors;

            if (extraFloors > 0)
            {
                DrawObjectTask floor = new DrawObjectTask("9B|" +
                                                          "1B1h1b3e1b1h1b|" +
                                                          "1B1e1b3e1b1e1b|" +
                                                          "1B1e1b1e3b1e1b|" +
                                                          "1B1e1b1e1b1w1e1b|" +
                                                          "1B1e1b1e3b1e1b|" +
                                                          "1B1e1b1w1h1b1e1b|" +
                                                          "1B1e3b1e1b1e1b|" +
                                                          "1B1e1w1b1e1b1e1b|" +
                                                          "1B1e3b1c1b1e1b|" +
                                                          "1B2e1b2e1b1e1b|" +
                                                          "1B1c1b1p1e1p1b1c1p|" +
                                                          "1B2e1p1e1p2e1p|" +
                                                          "1B1p1e1p1e1p1e1p1b|" +
                                                          "1E1p1e1p1b1p1e1p1e|" +
                                                          "1E1p1e1p1h1p1e1p1e|" +
                                                          "1E1p1b1p1e1p1b1p1e|" +
                                                          "1E1e1p2e1c1w1e|" +
                                                          "1B2p3e2p1b|" +
                                                          "1E1e4p3e|" +
                                                          "1E1e1p6e|" +
                                                          "1E1e1p6e|" +                                                          
                                                          "1E8p|" +
                                                          "1E8e|" +
                                                          "1E8e|" +
                                                          "1B8b|" +
                                                          "1B3e1b3e1b|" +
                                                          "1P3e1p3e1p|" +
                                                          "1B1b1p1e1p1b1p1e1p|" +
                                                          "1P1c2e1p1c2e1p|" +
                                                          "1P1e1p1b1p1e1p1b1p|" +
                                                          "1P1p1w2p1w1p|" +
                                                          "1E2p1b1e2p1b1e|" +
                                                          "1E1p1e1h1e1p1e1h1e|" +
                                                          "1E1p1p2e1p1p2e|" +
                                                          "1E1p3e1p3e|" +
                                                          "9B");

                


                int stfx = startX;
                int stfy = startY + 13;
                               

                while (extraFloors-- > 0)
                {
                    cy = stfy;
                    if (extraFloors % 2 == 0)
                    {
                        cx = stfx - 8 * (((extraFloorsParam+1)/2) - (extraFloors / 2));
                        DrawAObject(floor);
                    }
                    else
                    {
                        cx = stfx + 30 + 8 * (((extraFloorsParam)/2) - (extraFloors / 2));
                        DrawAObject(floor, false);
                    }
                }
                cx = startX;
                cy = startY;
            }
            
            
            DrawObjectTask innerbase = new DrawObjectTask("1B8P6B9P6B8P1B|" +
                                                          "1B8e1b3e1c1b9e1b3e1h1b8e1B|" +
                                                          "1B8e1h4e1b9e1b4e1c8e1B|" +
                                                          "1B8p4e1p3b5p4b1p1w1e8p1B|" +
                                                          "1B8e4b1w1p6e1b1h2e1p3b8e1B|" +
                                                          "1B8e1b2e4p6e1b2e1c1p2e1b8e1B|" +
                                                          "1B8p1b3e1w1p6e1w2e1p2e1b8p1B|" +
                                                          "1B8e1h1c1e1p4b5p4b1p1e1c1h8e1B|" +
                                                          "1Gbe1p2e1h1b7e1p1wbe1G|" +
                                                          "1E8p3b1p1c2e1b7e3p3b8p1E|" +
                                                          "1E8e1h2e1p2e1w7e1p1w2e1h8e1E|" +
                                                          "1E9e1c1e1p4b5p4b1p1e1c9e1E|" +
                                                          "1E8p1b2e1w1p7e1b1h2e1p2e1b8p1E|" +
                                                          "1B8e3b3p7e1b2e1c1p3b8e1B|" +
                                                          "1B8e1b2e1w1p7e1w2e1p2e1b8e1B|" +
                                                          "1B8p1h1c1e1p4b5p4b1p1e1c1h8p1B|" +
                                                          "1Bbe1p2e1h1b7e1p1wbe1B|" +
                                                          "1B8e3b1p1c2e1b7e3p3b8e1B|" +
                                                          "1B8p1b2e1p2e1w7e1p1w2e1b8p1B|" +
                                                          "1B8e1h1c1e1p4b5p4b1p1e1c1h8e1B|" +
                                                          "1Bbe1w1p7e1b1h2e1pbe1B|" +
                                                          "1B8p3b3p7e1b2e1c1p3b8p1B|" +
                                                          "1B8e1b2e1w1p7e1w2e1p1w1b8e1B|" +
                                                          "1B8e1h2e1p4b5p4b3p1h8e1B|" +
                                                          "1B8p1e1c1e1p1c1e1h1b7e1p1b4e8p1B|" +
                                                          "1B8e1b2e1p3e1b7e1p1c3e1b8e1B|" +
                                                          "1B8e4b1p1e1w7e1p4e1b8e1B|" +
                                                          "1B8p1b3e2b9p6b8p1B");


            DrawAObject(innerbase);
            cy--; //overwrite bot lane

            DrawObjectTask maininnertop = new DrawObjectTask("1B4b4p1b3e4b5p8b4p4b1B|" +
                                                          "1c3e1b4e1pje1p4e1b3e1c|" +
                                                          "4e1h4e1pje1p4e1h4e|" +
                                                          "1b8e1pje1p8e1b|" +
                                                          "1B4btp4b1B");
            DrawAObject(maininnertop);

            
            if (extraFloorsParam == 0)
            {              
                WorldGen.PlaceTile(cx-1, cy-4, curTileType, true, true, -1, 0);
                WorldGen.PlaceTile(cx - 1, cy - 5, curTileType, true, true, -1, 0);
                WorldGen.PlaceTile(cx-3, cy - 2, curTileType, true, true, -1, 0);
                Fill(cx - 3, cy-1, 3, 1, -1, curTileType);
                Fill(cx - 3, cy - 3, 3, 1, -1, curTileType);
                Fill(cx - 2, cy - 2, 2, 1, curWallType);
                WorldGen.PlaceTile(cx - 2, cy - 2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);

                WorldGen.PlaceTile(cx+39, cy - 4, curTileType, true, true, -1, 0);
                WorldGen.PlaceTile(cx + 39, cy - 5, curTileType, true, true, -1, 0);
                WorldGen.PlaceTile(cx+ 39+2, cy - 2, curTileType, true, true, -1, 0);
                Fill(cx+39 , cy - 1, 3, 1, -1, curTileType);
                Fill(cx+39 , cy - 3, 3, 1, -1, curTileType);
                Fill(cx+39 , cy - 2, 2, 1, curWallType);
                WorldGen.PlaceTile(cx + 39, cy - 2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);
            }else
            {
                WorldGen.PlaceTile(cx - 3, cy - 2, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx - 3, cy - 1, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx + 39 + 2, cy - 2, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx + 39 + 2, cy - 1, TileID.Platforms, true, true, -1, curPlatformStyle);
            }


            DrawObjectTask maininnerbot = new DrawObjectTask("1E3e1hte1h3e1E|"+
                                                             "1EHe1E|" +
                                                             "1EHe1E|" +
                                                             "1EHp1E|" +
                                                             "1EHe1E|" +
                                                             "1EHe1E|" +
                                                             "1BHp1B|" +
                                                             "1BHe1B|" +
                                                             "1BHe1B|" +
                                                             "1B6b1p9b5p9b1p1b1p4b1B|" +
                                                             "1B6e1b2e1h1c4e1b5e1b4e1c1h2e1b1e1b4e1B|"+
                                                             "1Bfe1b5e1bfe1B|"+
                                                             "1B8e1w6b5e6b1w8e1B|"+
                                                             "1Bbb4e1h5e1h4ebb1B|"+
                                                             "1E1pFe1p1E|" +
                                                             "1E1pFe1p1E|" +
                                                             "1E1pFe1p1E|" +
                                                             "hB5PhB"
                                                             );
            DrawAObject(maininnerbot);


            RopeToGround(cx+19, cy);            
            int cid = WorldGen.PlaceChest(cx + 18, cy -2 , TileID.Containers, false, curChestStyle);
            if (cid >= 0) SetUpChest(cid);

            //entrances
            if(extraFloorsParam == 0)
            {                
                for(int cyi=cy-2;cyi>cy-7;cyi--)
                    WorldGen.KillTile(cx, cyi, false, false, false);                
                WorldGen.PlaceObject(cx, cy - 6, TileID.TallGateOpen, true);

                for (int cyi = cy - 2; cyi > cy - 7; cyi--)
                    WorldGen.KillTile(cx+38, cyi, false, false, false);
                WorldGen.PlaceObject(cx+38, cy - 6, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx, cy - 17, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx+38, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx + 38, cy - 17, TileID.TallGateOpen, true);
            }
            else
            {
                int diffxl = 8 * ((extraFloorsParam + 1) / 2);
                int diffxr = 8 * ((extraFloorsParam ) / 2);

                WorldGen.PlaceTile(cx - diffxl, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx - diffxl, cy - 17, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx + 38 + diffxr, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx + 38 + diffxr, cy - 17, TileID.TallGateOpen, true);

                for (int cyi = cy - 20; cyi > cy - 26; cyi--)
                    WorldGen.PlaceTile(cx - diffxl, cyi, curTileType, true, true);
                for (int cyi = cy - 20; cyi > cy - 26; cyi--)
                    WorldGen.PlaceTile(cx + 38+ +diffxr, cyi, curTileType, true, true);


                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx - diffxl, cyi, false, false, false);
                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx +38+ diffxr, cyi, false, false, false);


                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx - diffxl, cyi, curTileType, true, true);
                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx + 38 + diffxr, cyi, curTileType, true, true);
                               

                WorldGen.KillTile(cx - diffxl, cy-6, false, false, false);
                WorldGen.KillTile(cx + 38 + diffxr , cy-6, false, false, false);

                WorldGen.PlaceTile(cx - diffxl, cy - 7, curTileType, true, true);
                WorldGen.PlaceTile(cx + 38 + diffxr, cy - 7, curTileType, true, true);

                WorldGen.PlaceObject(cx - diffxl, cy - 6, TileID.TallGateOpen, true);
                WorldGen.PlaceObject(cx + 38 + diffxr, cy - 6, TileID.TallGateOpen, true);


                //for closed flats to inner base bot
                for (int cyi = cy - 8; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx, cyi, false, false, false);
                for (int cyi = cy - 8; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx + 38, cyi, false, false, false);

                for (int cyi = cy - 8; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx , cyi, TileID.Platforms, true, true, -1, curPlatformStyle);
                for (int cyi = cy - 8; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx + 38, cyi, TileID.Platforms, true, true, -1, curPlatformStyle);

                WorldGen.KillTile(cx -2, cy - 5, false, false, false);
                WorldGen.KillTile(cx + 38 +2, cy - 5, false, false, false);

                WorldGen.PlaceTile(cx -2, cy-2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);
                WorldGen.PlaceTile(cx + 37+2, cy-2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);
            }

            Main.tile[cx + 7, cy - 9].liquid = 255;
            Main.tile[cx + 31, cy - 9].liquid = 255;
            Main.tile[cx + 31, cy - 9].honey(true);
            Main.tile[cx + 33, cy - 9].liquid = 255;            
            WorldGen.PlaceTile(cx + 33, cy - 9, TileID.Platforms, true, true, -1, 13);//obsid plat to hold lava
            Main.tile[cx + 33, cy - 9].lava(true);


        }



        public void DrawBase2(int extraFloors)
        {

            DrawObjectTask floorType1 = new DrawObjectTask();
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 5, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));                                        
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));                    
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2));                    
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 4, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 2, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();                    
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 2, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 2, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 2, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask();
                    drawLineTask.drawTaskList.Add(new DrawTask(-1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(curTileType, 5, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
            }



            int startX = cx;
            int startY = cy;
            int count = 0;

            while (extraFloors > 0)
            {
                
                cy = startY;

                if (count % 2 == 0)
                {
                    cx = startX - (count/2) * 4 -3;
                    DrawAObject(floorType1);
                }
                else
                {
                    cx = startX + (count/2+1) * 4+3;
                    DrawAObject(floorType1, false);
                }
                extraFloors--;
                count++;
            }

            Fill(startX+3, startY+1, 4, 21, curWallType);
            Fill(startX+3, startY, 4, 1, 0, TileID.Platforms ,curPlatformStyle);
            Fill(startX+3, startY+11, 4, 1, curWallType, TileID.Platforms , curPlatformStyle);
            Fill(startX+2, startY + 17, 6, 1, curWallType, TileID.Platforms, curPlatformStyle);

            cx = startX+4;
            cy = startY;

            Main.tile[cx - 10, cy + 17].liquid = 255;
            Main.tile[cx - 10, cy + 17].honey(true);
            Main.tile[cx + 11, cy + 17].liquid = 255;


            RopeToGround(startX + 2+2, startY+18 );
            RopeToGround(startX + 3+2, startY + 18);
                        
            int cid = WorldGen.PlaceChest(startX+ +4, startY+16, TileID.Containers, false, curChestStyle);
            if(cid>=0)SetUpChest(cid);            
            
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
                

                if (curTileType == -1)
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


                        //normal chair open to left
                        //right hand tiles (cy+0) unknown, they still need to generate

                        bool changeIt = false;
                        int yd = 0;
                        while (yd++ < 10)
                        {
                            if (Main.tile[cx - 1, cy - yd].active() && Main.tile[cx + 1, cy - yd].active())
                                continue;
                            else
                            {
                                if ((Main.tile[cx - 1, cy - yd].active() && !Main.tile[cx + 1, cy - yd].active())) // && Main.tile[cx - 1, cy - yd].type != TileID.HangingLanterns
                                    changeIt = true;
                                break;
                            }
                        }
                        if (changeIt)
                        {
                            Tile expr_691 = Main.tile[cx, cy + 1];
                            expr_691.frameX += 18;
                            Tile expr_6B2 = Main.tile[cx, cy];
                            expr_6B2.frameX += 18;                            
                        }                       

                        /*
                         * bool changed = false;
                        if (((!Main.tile[cx + 1, cy - 1].active()) && (Main.tile[cx - 1, cy - 1].active())) || (
                            (!Main.tile[cx + 1, cy - 1].active()) && (!Main.tile[cx - 1, cy - 1].active()) &&
                            (!Main.tile[cx + 1, cy - 2].active()) && (Main.tile[cx - 1, cy - 1].active())) || 
                            (Main.tile[cx -1, cy ].active() && !Main.tile[cx + 1, cy-2].active()) )
                        {
                            Tile expr_691 = Main.tile[cx, cy + 1];
                            expr_691.frameX += 18;
                            Tile expr_6B2 = Main.tile[cx, cy];
                            expr_6B2.frameX += 18;
                            changed = true;
                        }      
                        
                        if (Main.tile[cx - 1, cy].type == TileID.WorkBenches && changed && (!Main.tile[cx - 1, cy - 1].active()) )
                        {
                            Tile expr_691 = Main.tile[cx, cy + 1];
                            expr_691.frameX -= 18;
                            Tile expr_6B2 = Main.tile[cx, cy];
                            expr_6B2.frameX -= 18;                            
                        }
                        */

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
                    else
                    {                                               
                        WorldGen.PlaceTile(cx, cy, curTileType, true, true, -1, curStyle);
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
