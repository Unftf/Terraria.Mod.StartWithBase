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


using Terraria.GameContent.UI.Elements;

namespace StartWithBase
{
    class Builder
    {


        public int curTileType = TileID.HayBlock;
        public int curTileItemType = ItemID.Hay;
        public byte curWallType = WallID.Hay;
        public int curWallItemType = ItemID.HayWall;
        public int curLanternItemTyp = ItemID.ChainLantern;

        int curStyle = 17;

        int curChestStyle = 5;

        public int curPlatformStyle = 17;
        public int curLanternStyle = 13;
        int curChairStyle = 29;
        public int curWorkBenchStyle = 22;
        int curLampStyle = 0;
        int curTorchStyle = 0;
        int curFurnitureCraftingItemType = ItemID.PalmWood;
        int curPlatformCraftingItemType = ItemID.PalmWood;


        int cx, cy;


        public Dictionary<string, StyleSetting> styleTypeDict = new Dictionary<string, StyleSetting>
            {
            {"fpa", new StyleSetting{ PlatformStyle = 17, LanternStyle = 27, ChairStyle = 29, WorkBenchStyle = 22, LampStyle = 18, TorchStyle = 0, FurnitureCraftingItem = ItemID.PalmWood, ItemID = ItemID.PalmWoodWorkBench } },
            {"fdy", new StyleSetting{ PlatformStyle = 32, LanternStyle = 26, ChairStyle = 27, WorkBenchStyle = 18, LampStyle = 17, TorchStyle = 0, FurnitureCraftingItem = ItemID.DynastyWood, ItemID = ItemID.DynastyWorkBench } },
            {"fwo", new StyleSetting{ PlatformStyle = 0, LanternStyle = 22, ChairStyle = 0, WorkBenchStyle = 0, LampStyle = 0, TorchStyle = 0, FurnitureCraftingItem = ItemID.Wood, ItemID = ItemID.WorkBench } },
            {"fri", new StyleSetting{ PlatformStyle = 2, LanternStyle = 16, ChairStyle = 3, WorkBenchStyle = 2, LampStyle = 6, TorchStyle = 0, FurnitureCraftingItem = ItemID.RichMahogany, ItemID = ItemID.RichMahoganyWorkBench} },            
            {"fbo", new StyleSetting{ PlatformStyle = 19, LanternStyle = 29, ChairStyle = 30, WorkBenchStyle = 23, LampStyle = 20, TorchStyle = 0, FurnitureCraftingItem = ItemID.BorealWood, ItemID = ItemID.BorealWoodWorkBench } },
            {"fgr", new StyleSetting{ PlatformStyle = 28, LanternStyle = 35, ChairStyle = 34, WorkBenchStyle = 29, LampStyle = 29, TorchStyle = 0, FurnitureCraftingItem = ItemID.GraniteBlock, ItemID = ItemID.GraniteWorkBench } },
            {"fmb", new StyleSetting{ PlatformStyle = 29, LanternStyle = 36, ChairStyle = 35, WorkBenchStyle = 30, LampStyle = 30, TorchStyle = 0, FurnitureCraftingItem = ItemID.MarbleBlock, ItemID = ItemID.MarbleWorkBench} },
            {"fme", new StyleSetting{ PlatformStyle = 27, LanternStyle = 34, ChairStyle = 33, WorkBenchStyle = 28, LampStyle = 28, TorchStyle = 0, FurnitureCraftingItem = ItemID.MeteoriteBrick, ItemID = ItemID.MeteoriteWorkBench} },
            {"fcr", new StyleSetting{ PlatformStyle = 30, LanternStyle = 37, ChairStyle = 36, WorkBenchStyle = 31, LampStyle = 31, TorchStyle = 0, FurnitureCraftingItem = ItemID.CrystalBlock, ItemID = ItemID.CrystalWorkbench} },            
            {"fma", new StyleSetting{ PlatformStyle = 26, LanternStyle = 33, ChairStyle = 32, WorkBenchStyle = 27, LampStyle = 27, TorchStyle = 0, FurnitureCraftingItem = ItemID.MartianConduitPlating, ItemID = ItemID.MartianWorkBench} },
         };
        public Dictionary<string, LanternSetting> lanternTypeDict = new Dictionary<string, LanternSetting>
            {
            {"lho", new LanternSetting{LanternStyle = 13, ItemID = ItemID.HoneyLantern } },
            {"lpa", new LanternSetting{LanternStyle = 27, ItemID = ItemID.PalmWoodLantern } },
            {"ldy", new LanternSetting{LanternStyle = 26, ItemID = ItemID.DynastyLantern } },
            {"lwo", new LanternSetting{LanternStyle = 22, ItemID = ItemID.LivingWoodLantern } },
            {"lgl", new LanternSetting{LanternStyle = 15, ItemID = ItemID.GlassLantern } },
            {"lsk", new LanternSetting{LanternStyle = 20, ItemID = ItemID.SkywareLantern } },
            {"lst", new LanternSetting{LanternStyle = 14, ItemID = ItemID.SteampunkLantern } },
            {"lri", new LanternSetting{LanternStyle = 16, ItemID = ItemID.RichMahoganyLantern } },            
            {"lbo", new LanternSetting{LanternStyle = 29, ItemID = ItemID.BorealWoodLantern } },                        
            {"lgr", new LanternSetting{LanternStyle = 35, ItemID = ItemID.GraniteLantern } },
            {"lca", new LanternSetting{LanternStyle = 10, ItemID = ItemID.CactusLantern } },            
            {"lmb", new LanternSetting{LanternStyle = 36, ItemID = ItemID.MarbleLantern } },
            {"lsl", new LanternSetting{LanternStyle = 30, ItemID = ItemID.SlimeLantern } },
            {"lme", new LanternSetting{LanternStyle = 34, ItemID = ItemID.MeteoriteLantern } },
            {"lcr", new LanternSetting{LanternStyle = 37, ItemID = ItemID.CrystalLantern } },
            {"lma", new LanternSetting{LanternStyle = 33, ItemID = ItemID.MartianLantern } },            
            {"lic", new LanternSetting{LanternStyle = 18, ItemID = ItemID.FrozenLantern } },
            
         };

       
        public Dictionary<string, PlatformSetting> platformTypeDict = new Dictionary<string, PlatformSetting>
            {
            {"ppa", new PlatformSetting{ PlatformStyle = 17, ItemID = ItemID.PalmWoodPlatform } },
            {"pdy", new PlatformSetting{ PlatformStyle = 32, ItemID = ItemID.DynastyPlatform } },            
            {"pwo", new PlatformSetting{ PlatformStyle = 0,  ItemID = ItemID.WoodPlatform } },
            {"pri", new PlatformSetting{ PlatformStyle = 2,  ItemID = ItemID.RichMahoganyPlatform} },
            {"pbo", new PlatformSetting{ PlatformStyle = 19, ItemID = ItemID.BorealWoodPlatform } },
            {"pgr", new PlatformSetting{ PlatformStyle = 28, ItemID = ItemID.GranitePlatform } },
            {"pmb", new PlatformSetting{ PlatformStyle = 29, ItemID = ItemID.MarblePlatform} },
            {"pme", new PlatformSetting{ PlatformStyle = 27, ItemID = ItemID.MeteoritePlatform} },
            {"pcr", new PlatformSetting{ PlatformStyle = 30, ItemID = ItemID.CrystalPlatform} },
            {"pma", new PlatformSetting{ PlatformStyle = 26, ItemID = ItemID.MartianPlatform} },
            {"pic", new PlatformSetting{ PlatformStyle = 35, ItemID = ItemID.FrozenPlatform} },
         };

        public Dictionary<string, WallSetting> wallTypeDict = new Dictionary<string, WallSetting>
            {
            {"wdi", new WallSetting{ WallID = WallID.Dirt, ItemIDofWallType = ItemID.DirtWall} },
            {"wdg", new WallSetting{ WallID = WallID.DiamondGemspark, ItemIDofWallType = ItemID.DiamondGemsparkWall} },
            {"wha", new WallSetting{ WallID = WallID.Hay, ItemIDofWallType = ItemID.HayWall} },
            {"wpa", new WallSetting{ WallID = WallID.PalmWood, ItemIDofWallType = ItemID.PalmWoodWall} },
            {"wdy", new WallSetting{ WallID = WallID.WhiteDynasty, ItemIDofWallType = ItemID.WhiteDynastyWall} },
            {"wwo", new WallSetting{ WallID = WallID.Wood, ItemIDofWallType = ItemID.WoodWall} },
            {"wri", new WallSetting{ WallID = WallID.RichMaogany, ItemIDofWallType = ItemID.RichMahoganyWall} },
            {"wbo", new WallSetting{ WallID = WallID.BorealWood, ItemIDofWallType = ItemID.BorealWoodWall} },
            {"wgr", new WallSetting{ WallID = WallID.GraniteBlock, ItemIDofWallType = ItemID.GraniteBlockWall} },
            {"wmb", new WallSetting{ WallID = WallID.MarbleBlock, ItemIDofWallType = ItemID.MarbleBlockWall} },
            {"wme", new WallSetting{ WallID = WallID.MeteoriteBrick, ItemIDofWallType = ItemID.MeteoriteBrickWall} },
            {"wcr", new WallSetting{ WallID = WallID.Crystal, ItemIDofWallType = ItemID.CrystalBlockWall} },
            {"wma", new WallSetting{ WallID = WallID.MartianConduit, ItemIDofWallType = ItemID.MartianConduitWall} },
            {"wic", new WallSetting{ WallID = WallID.IceBrick, ItemIDofWallType = ItemID.IceBrickWall} },
            {"wss", new WallSetting{ WallID = WallID.StoneSlab, ItemIDofWallType = ItemID.StoneSlabWall} },
            {"wsb", new WallSetting{ WallID = WallID.SandstoneBrick, ItemIDofWallType = ItemID.SandstoneBrickWall} },                        
            {"wob", new WallSetting{ WallID = WallID.ObsidianBrick, ItemIDofWallType = ItemID.ObsidianBrickWall} },
         };
        public Dictionary<string, TileSetting> TileTypeDict = new Dictionary<string, TileSetting>
            {
            {"tha", new TileSetting{ TileID = TileID.HayBlock, ItemID = ItemID.Hay } },
            {"tpa", new TileSetting{ TileID = TileID.PalmWood, ItemID = ItemID.PalmWood } },
            {"tdy", new TileSetting{ TileID = TileID.DynastyWood, ItemID = ItemID.DynastyWood} },
            {"two", new TileSetting{ TileID = TileID.WoodBlock, ItemID = ItemID.Wood} },            
            {"tri", new TileSetting{ TileID = TileID.RichMahogany, ItemID = ItemID.RichMahogany} },
            {"tbo", new TileSetting{ TileID = TileID.BorealWood, ItemID = ItemID.BorealWood} },
            {"tgr", new TileSetting{ TileID = TileID.GraniteBlock, ItemID = ItemID.GraniteBlock} },
            {"tmb", new TileSetting{ TileID = TileID.MarbleBlock, ItemID = ItemID.MarbleBlock} },
            {"tir", new TileSetting{ TileID = TileID.Dirt, ItemID = ItemID.DirtBlock} },
            {"tst", new TileSetting{ TileID = TileID.Stone, ItemID = ItemID.StoneBlock} },
            {"tme", new TileSetting{ TileID = TileID.MeteoriteBrick, ItemID = ItemID.MeteoriteBrick} },
            {"tcr", new TileSetting{ TileID = TileID.CrystalBlock, ItemID = ItemID.CrystalBlock} },
            {"tma", new TileSetting{ TileID = TileID.MartianConduitPlating, ItemID = ItemID.MartianConduitPlating} },
            {"tic", new TileSetting{ TileID = TileID.IceBrick, ItemID = ItemID.IceBrick} },
            {"tss", new TileSetting{ TileID = TileID.StoneSlab, ItemID = ItemID.StoneSlab} },
            {"tsb", new TileSetting{ TileID = TileID.SandstoneBrick, ItemID = ItemID.SandstoneBrick} },            
            {"tob", new TileSetting{ TileID = TileID.ObsidianBrick, ItemID = ItemID.ObsidianBrick} },
         };


        public BaseType baseType = BaseType.Base3;
        public enum BaseType { Base2, Base3, Base3ext, Base4, Base6 };

        public Dictionary<string, BaseType> baseTypeDict = new Dictionary<string, BaseType>
        {
            {"ba2", BaseType.Base2 },
            {"ba3", BaseType.Base3 },
            {"b3b", BaseType.Base3ext },
            {"ba4", BaseType.Base4 },
            {"ba6", BaseType.Base6 },
         };

        public class StyleSetting
        {
            public int PlatformStyle { get; set; }
            public int LanternStyle { get; set; }
            public int ChairStyle { get; set; }
            public int WorkBenchStyle { get; set; }
            public int LampStyle { get; set; }
            public int TorchStyle { get; set; }
            public int FurnitureCraftingItem { get; set; }
            public int ItemID { get; set; }
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

        public class LanternSetting
        {
            public int LanternStyle { get; set; }
            public int ItemID { get; set; }
        }

        public class PlatformSetting
        {
            public int PlatformStyle { get; set; }
            public int ItemID { get; set; }
        }


        Mod mod;
        public Builder(Mod mod)
        {
            this.mod = mod;
            
            
        }

        public void Init(UIState uistate)
        {
            string wname = Main.worldName;
            if (wname.Contains("$"))
                parsWN(wname);

            //Main.MenuUI.CurrentState.RemoveAllChildren();

            swbui = new StartWithBaseUI(this, uistate, mod);

        }

        public void SetProgress(int val)
        {
            if (swbui != null)
            {
                //swbui.counterText.SetText(""+ (Int32.Parse(swbui.counterText.Text)-1) );
                swbui.counterText.SetText("" + val);

            }
        }

        public void Build()
        {
            if (swbui != null && !swbui.doNotBuildBase)
            {

                cx = Main.spawnTileX - 20;
                cy = Main.spawnTileY;
                findSweetSpot(); // increases cy if spawn under surface
                cy -= yoff;


                townNPCcount = Main.MaxShopIDs; //vanilla town npc
                int count2 = townNPCcount;
                for (int i = 0; i < NPCLoader.NPCCount; i++)
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


                if (swbui != null)
                {
                    string winfo = swbui.getValues();
                    parsWN(winfo, false);                   
                }


                //if (!wname.Contains("$"))
                //    wname = wname + "$sy*";


                if (baseType == BaseType.Base2)
                    Base2();
                else if (baseType == BaseType.Base6)
                    Base6(); //up to 90 at small
                else if (baseType == BaseType.Base4)
                    Base4();
                else
                    Base3(); //if (param.Contains("ba3")) max npc about 80 at small world

                if (Main.worldName.Contains("all") && Main.worldName.Contains("item") && (Main.worldName.Contains("world") || Main.worldName.Contains("map")))
                {
                    PlaceAllItems();
                }
            }
        }

        public void EndBuilding()
        {
            if (swbui != null)
            {               
                swbui.free();
            }

        }

        public class DrawTask
        {
            public int type;
            public int count;
            public int spriteStyle;
            public bool placeWall;
            Builder builder;

            public DrawTask(Builder builder, int type, int count, int spriteStyle = -1, bool placeWall = true)
            {
                this.builder = builder;
                this.type = type;
                this.count = count;
                this.spriteStyle = spriteStyle;
                this.placeWall = placeWall;
            }

            public DrawTask(Builder builder, string task)
            {
                this.builder = builder;
                char num = task[0];
                char type = task[1];

                string numbers = "0123456789abcdefghijklmnopqrstABCDEFGHIJKLMNOPQRSTZ"; //a = 10, k=20; A=30; k=40; Z=50

                this.placeWall = Char.IsLower(type); // place wall if type is written small letters
                this.count = numbers.IndexOf(num);
                if (this.count < 0) throw new Exception("invalid draw task count " + num + " (" + task[0] + ")" + "\n");


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
                        this.type = builder.curTileType;
                        break;
                    case 'P':
                        this.type = TileID.Platforms;
                        this.spriteStyle = builder.curPlatformStyle;
                        break;
                    case 'S':
                        //storage container
                        this.type = TileID.Containers;
                        this.spriteStyle = builder.curChestStyle;
                        break;
                    case 'H':
                        this.type = TileID.HangingLanterns;
                        this.spriteStyle = builder.curLanternStyle;
                        break;
                    case 'C':
                        this.type = TileID.Chairs;
                        this.spriteStyle = builder.curChairStyle;
                        break;
                    case 'W':
                        this.type = TileID.WorkBenches;
                        this.spriteStyle = builder.curWorkBenchStyle;
                        break;
                    case 'G':
                        this.type = TileID.TallGateOpen;
                        this.spriteStyle = 0;
                        break;
                    case 'L':
                        this.type = TileID.Lamps;
                        this.spriteStyle = builder.curLampStyle;
                        break;
                    case 'E':
                        //empty tile or only place walls
                        this.type = -1;
                        break;
                    case 'T':
                        this.type = TileID.Torches;
                        this.spriteStyle = builder.curTorchStyle;
                        break;
                }


            }
        }
        public class DrawLineTask
        {
            public List<DrawTask> drawTaskList;
            Builder builder;
            public DrawLineTask(Builder builder)
            {
                this.builder = builder;
                drawTaskList = new List<DrawTask>();
            }
            public DrawLineTask(Builder builder, string string2Parse)
            {
                this.builder = builder;
                drawTaskList = new List<DrawTask>();
                for (int i = 0; i < string2Parse.Length - 1; i += 2)
                {
                    drawTaskList.Add(new DrawTask(builder, string2Parse.Substring(i, 2)));
                }
            }
        }
        public class DrawObjectTask
        {
            public List<DrawLineTask> drawLineList;
            Builder builder;
            public DrawObjectTask(Builder builder)
            {
                this.builder = builder;
                drawLineList = new List<DrawLineTask>();
            }
            public DrawObjectTask(Builder builder , string string2Parse)
            {
                this.builder = builder;
                drawLineList = new List<DrawLineTask>();
                string[] lines = string2Parse.Split('|');
                for (int i = 0; i < lines.Length; i++)
                    drawLineList.Add(new DrawLineTask(builder, lines[i]));
            }
        }

        int townNPCcount;
        int allItemCount;
        int chestsNeeded;
        const int yoff = 13;
        public StartWithBaseUI swbui = null;









        public void findSweetSpot()
        {
            const int checkSizex = 20;

            for (; cy > 200; cy--)
            {
                bool decre = false;

                for (int xi = 0; xi < checkSizex; xi++)
                {
                    if ((Main.tile[cx + xi, cy].active() || Main.tile[cx - xi, cy].active()) && Main.tile[cx - xi, cy].wall != WallID.None)
                    {
                        decre = true;
                        break;
                    }
                }
                if (!decre)
                    break;
            }
        }

        public void parsWN(string wname, bool setwn = true)
        {
            int from = wname.IndexOf("$");
            int to = wname.Length;
            extend = false;

            string subs = wname.Substring(from, to - from);

            //rename world file
            if (setwn)
            {

                string newWN = wname.Substring(0, from);
                Main.worldName = newWN;
                string seed = Main.ActiveWorldFileData.SeedText;
                Main.ActiveWorldFileData = WorldFile.CreateMetadata(Main.worldName, Main.ActiveWorldFileData.IsCloudSave, Main.expertMode);
                Main.ActiveWorldFileData.SetSeed(seed);
            }

            subs = subs.Substring(1);
            subs = subs.ToLower().Normalize();
            if (subs.Length == 3 && (subs.Substring(0, 2).Equals("ba") || subs.Equals("b3b")))
                subs = subs + "sy*";

            for (int i = 0; i < subs.Length / 3; i++)
            {
                string pa = subs.Substring(3 * i, 3);


                if (pa.Substring(0, 2).Equals("ba"))
                {
                    if (pa.Equals("ba2")) baseType = BaseType.Base2;
                    if (pa.Equals("ba3")) baseType = BaseType.Base3;
                    if (pa.Equals("ba4")) baseType = BaseType.Base4;
                    if (pa.Equals("ba6")) baseType = BaseType.Base6;
                }
                if (pa.Equals("b3b"))
                {
                    //baseType = BaseType.Base3;
                    baseType = BaseType.Base3ext;
                    extend = true;
                }

                if (pa.Equals("sy*"))
                {
                    //random
                    //int val = WorldGen.genRand.Next(100);
                    //baseType = val<10? BaseType.Base2 : val < 20 ? BaseType.Base6: BaseType.Base3;
                    int dummy = (WorldGen.genRand.Next(Math.Abs(Main.worldName.GetHashCode() - (Main.player.Length > 0 ? Main.player[0].name.GetHashCode() : 0) + DateTime.Now.Second + DateTime.Now.Millisecond))) % 100;
                    while (dummy-- > 0) WorldGen.genRand.Next();
                    SetToFurniture((styleTypeDict.ElementAt(WorldGen.genRand.Next(styleTypeDict.Count))).Value);
                    SetToWall((wallTypeDict.ElementAt(WorldGen.genRand.Next(wallTypeDict.Count))).Value);
                    if (curWallType == wallTypeDict["wri"].WallID || curWallType == wallTypeDict["wdg"].WallID || curWallType == wallTypeDict["wgr"].WallID) SetToWall((wallTypeDict.ElementAt(WorldGen.genRand.Next(wallTypeDict.Count))).Value);//dont like mahagony reduce chance, digew hard to get
                    SetToTiles((TileTypeDict.ElementAt(WorldGen.genRand.Next(TileTypeDict.Count))).Value);
                    if (curTileType == TileTypeDict["tri"].TileID || curTileType == TileTypeDict["tob"].TileID) SetToTiles((TileTypeDict.ElementAt(WorldGen.genRand.Next(TileTypeDict.Count))).Value);//dont like mahagony, obsidian hard to get reduce chance
                }
                if (pa.Equals("sy0"))
                {
                    SetToFurniture(styleTypeDict["fpa"]);
                    SetToWall(wallTypeDict["wha"]);
                    SetToTiles(TileTypeDict["tha"]);
                    SetToLantern(lanternTypeDict["lho"]);
                }
                if (pa.Equals("sy1"))
                {
                    SetToFurniture(styleTypeDict["fwo"]);
                    SetToWall(wallTypeDict["wdi"]);
                    SetToTiles(TileTypeDict["tss"]);
                    SetToLantern(lanternTypeDict["ldy"]);
                }
                if (pa.Equals("sy2"))
                {
                    SetToFurniture(styleTypeDict["fwo"]);
                    SetToWall(wallTypeDict["wdg"]);
                    SetToTiles(TileTypeDict["tob"]);
                    SetToLantern(lanternTypeDict["lgl"]);
                }
                if (pa.Substring(0, 2).Equals("cx"))
                {
                    //custom values
                    CheckGenerate(pa.Substring(2, 1));
                    ReadAndSetConfig(pa.Substring(2, 1));
                }

                if (styleTypeDict.ContainsKey(pa))
                    SetToFurniture(styleTypeDict[pa]);
                else if (wallTypeDict.ContainsKey(pa))
                    SetToWall(wallTypeDict[pa]);
                else if (TileTypeDict.ContainsKey(pa))
                    SetToTiles(TileTypeDict[pa]);
                else if (lanternTypeDict.ContainsKey(pa))
                    SetToLantern(lanternTypeDict[pa]);
                else if (platformTypeDict.ContainsKey(pa))
                    SetToPlatform(platformTypeDict[pa]);
            }
        }


        public bool ReadAndSetConfig(string numbs)
        {
            string modpath = @"/StartWithBase";
            if (!System.IO.Directory.Exists(Main.SavePath + modpath))
                return false;

            string filePath = Main.SavePath + modpath + @"/config" + numbs + ".txt";
            if (!System.IO.File.Exists(filePath))
                return false;

            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                line = line.Normalize();

                if (line.Length < 3)
                    continue;
                if (line[0] == '#')
                    continue;

                string[] nameValue = line.Split('=');
                if (nameValue.Length != 2)
                    continue;

                string name = nameValue[0].Trim();
                string value = nameValue[1].Trim();


                if (name.Equals("basetype"))
                {
                    baseType = (BaseType)Int32.Parse(value);
                    if (baseType == BaseType.Base3ext)
                    {
                        //baseType = BaseType.Base3;
                        baseType = BaseType.Base3ext;
                        extend = true;
                    }
                }
                else if (name.Equals("TileID"))
                    curTileType = Int32.Parse(value);
                else if (name.Equals("ItemIDoftileType"))
                    curTileItemType = Int32.Parse(value);


                else if (name.Equals("WallID"))
                    curWallType = (byte)Int32.Parse(value);
                else if (name.Equals("ItemIDofWallType"))
                    curWallItemType = Int32.Parse(value);


                else if (name.Equals("PlatformStyle"))
                    curPlatformStyle = Int32.Parse(value);
                else if (name.Equals("LanternStyle"))
                    curLanternStyle = Int32.Parse(value);
                else if (name.Equals("ChairStyle"))
                    curChairStyle = Int32.Parse(value);
                else if (name.Equals("WorkBenchStyle"))
                    curWorkBenchStyle = Int32.Parse(value);
                else if (name.Equals("LampStyle"))
                    curLampStyle = Int32.Parse(value);
                else if (name.Equals("TorchStyle"))
                    curTorchStyle = Int32.Parse(value);
                else if (name.Equals("FurnitureCraftingItemType"))
                    curFurnitureCraftingItemType = Int32.Parse(value);
                else if (name.Equals("PlatformCraftingItemType"))
                    curPlatformCraftingItemType = Int32.Parse(value);

            }

            return true;
        }

        public void CheckGenerate(string numbs, bool overwrite = false)
        {
            string modpath = @"/StartWithBase";
            if (!System.IO.Directory.Exists(Main.SavePath + modpath))
                System.IO.Directory.CreateDirectory(Main.SavePath + modpath);

            string filePath = Main.SavePath + modpath + @"/config" + numbs + ".txt";
            if (System.IO.File.Exists(filePath) && !overwrite)
                return;

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filePath, false))
            {
                file.WriteLine("# Config file of Start with Base mod");
                file.WriteLine("# here you can setup custom values");
                file.WriteLine("# lines start with '#' get ignored");
                file.WriteLine("");
                file.WriteLine("# basetype values ba2=0 ba3=1 b3b=2 ba4=3 ba6=4 currently supported");
                file.WriteLine("basetype = " + (int)((baseType == BaseType.Base3 && extend) ? BaseType.Base3ext : baseType));


                file.WriteLine("");
                file.WriteLine("# In following part you can change each type of placed objects.");
                file.WriteLine("# For tile ID's look e.g. at https://terraria.gamepedia.com/Tile_IDs");
                file.WriteLine("# For items types look at their wiki pages. Those are placed in a starting chest.");

                file.WriteLine("");
                file.WriteLine("# Tiles used to build base");
                file.WriteLine("TileID = " + curTileType);
                file.WriteLine("ItemIDoftileType = " + curTileItemType);

                file.WriteLine("");
                file.WriteLine("# Wall");
                file.WriteLine("WallID = " + curWallType);
                file.WriteLine("ItemIDofWallType = " + curWallItemType);

                file.WriteLine("");
                file.WriteLine("# Furniture");
                file.WriteLine("PlatformStyle = " + curPlatformStyle);
                file.WriteLine("LanternStyle = " + curLanternStyle);
                file.WriteLine("ChairStyle = " + curChairStyle);
                file.WriteLine("WorkBenchStyle = " + curWorkBenchStyle);
                file.WriteLine("LampStyle = " + curLampStyle);
                file.WriteLine("TorchStyle = " + curTorchStyle);
                file.WriteLine("FurnitureCraftingItemType = " + curFurnitureCraftingItemType);
                file.WriteLine("PlatformCraftingItemType = " + curPlatformCraftingItemType);
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
            curFurnitureCraftingItemType = ss.FurnitureCraftingItem;


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
        public void SetToLantern(LanternSetting ls)
        {
            curLanternStyle = (ushort)ls.LanternStyle;
        }
        public void SetToPlatform(PlatformSetting ps)
        {
            curPlatformStyle = (ushort)ps.PlatformStyle;
            curPlatformCraftingItemType = ps.ItemID;
        }

        const int stackSize = 42;
        public void SetUpChest(int cid)
        {
            Main.chest[cid].item[0].SetDefaults(curFurnitureCraftingItemType);
            Main.chest[cid].item[0].stack = stackSize;
            if (Main.chest[cid].item[0].value > 99) Main.chest[cid].item[0].stack = 0;
            Main.chest[cid].item[1].SetDefaults(curTileItemType);
            Main.chest[cid].item[1].stack = stackSize;
            if (Main.chest[cid].item[1].value > 99) Main.chest[cid].item[1].stack = 0;
            Main.chest[cid].item[2].SetDefaults(curWallItemType);
            Main.chest[cid].item[2].stack = stackSize;
            if (Main.chest[cid].item[2].value > 99) Main.chest[cid].item[2].stack = 0;
            Main.chest[cid].item[3].SetDefaults(curPlatformCraftingItemType);
            Main.chest[cid].item[3].stack = stackSize;
            if (Main.chest[cid].item[3].value > 99) Main.chest[cid].item[3].stack = 0;

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


            cy -= 23;
            int range = 2 * floorsNeeded + 2;
            cx += 20 - 4;

            ClearField(23, range);

            DrawBase2(floorsNeeded);
        }


        public void Base3()
        {
            int chestsPerFloor = 9;
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
                extraStorageFloor = floorsNeeded / floorSpace - 1;
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


            ClearField(groundBaseHight + 4 * floorsNeeded + 7 * extraStorageFloor + (extend ? 7 : 0), 2 + (extend ? 7 + 4 : 0));

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

            ClearField(translateY, ((spaceXneeded + 1) / 2) + 2);
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
            for (int xi = cx - xdimPlus; xi < cx + 40 + xdimPlus; xi++)
            {
                for (int yi = cy; yi < cy + yoff + yend - 5; yi++)
                {
                    Main.tile[xi, yi] = new Tile();
                }
            }

        }

        public void writeDebugFile(string content, bool append = true)
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

            DrawObjectTask floorType1 = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 17, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 17, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 10));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoom = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 18, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    storageRoom.drawLineList.Add(drawLineTask);
                }
            }

            DrawObjectTask storageRoomExtendTop = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendTop.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoomExtend = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Lamps, 1, curLampStyle));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtend.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask storageRoomExtendbase = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    storageRoomExtendbase.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend3 = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend3.drawLineList.Add(drawLineTask);
                }
            }
            DrawObjectTask floorExtend4 = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 6, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorExtend4.drawLineList.Add(drawLineTask);
                }
            }


            int floorSpace = (extraFloors) / (extraStorageFloor + 1);
            int florCountToStorage = 0;


            if (extend)
            {

                DrawAObject(storageRoomExtendTop); cy -= 8; cx = cx + 7;
                DrawAObject(storageRoom); cy -= 8; cx = cx + 38;
                DrawAObject(storageRoomExtendTop, false); cy -= 1;

                cx = startX + 7;
                //gates not needed anymore
                Main.tile[cx, cy - 1].active(false); Main.tile[cx + 38, cy - 1].active(false);
                Main.tile[cx, cy - 2].active(false); Main.tile[cx + 38, cy - 2].active(false);
                Main.tile[cx, cy - 3].active(false); Main.tile[cx + 38, cy - 3].active(false);
                Main.tile[cx, cy - 4].active(false); Main.tile[cx + 38, cy - 4].active(false);
                Main.tile[cx, cy - 5].active(false); Main.tile[cx + 38, cy - 5].active(false);
                Main.tile[cx, cy - 6].active(false); Main.tile[cx + 38, cy - 6].active(false);
                WorldGen.PlaceTile(cx, cy - 1, TileID.Lamps, true, true, -1, curLampStyle);
                WorldGen.PlaceTile(cx + 38, cy - 1, TileID.Lamps, true, true, -1, curLampStyle);

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
                if (florCountToStorage + 1 == floorSpace && extraFloors > 0 && extraStorageFloor > 0)
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

            DrawObjectTask groundFloor = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 16, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, 7));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 25));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Lamps, 1, curLampStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 14));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 27, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 33));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 34, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Containers, 1, curChestStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 18));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 37));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 16));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    groundFloor.drawLineList.Add(drawLineTask);
                }
            }
            DrawAObject(groundFloor); cy--;

            int liquidY = cy;
            DrawObjectTask basement = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 9));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 9));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 15));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 3, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 6, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 10, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 10, -1, false));
                    basement.drawLineList.Add(drawLineTask);
                }
            }
            DrawAObject(basement); cy--;

            Main.tile[cx + 7, liquidY].liquid = 255;
            Main.tile[cx + 31, liquidY].honey(true);
            Main.tile[cx + 31, liquidY].liquid = 255;

            int cid = Chest.FindChest(cx + 17, liquidY - 2);
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
                    int extraExtendFloors = (floorSpace * 4) / 3 - (startWith > 2 ? 1 : 0);

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

        int ropeX, ropeY;

        public void RopeToGround(int x, int y)
        {
            ropeX = x;
            ropeY = y;
            int rope = 0;
            while ((!Main.tile[x, y + rope].active() || (Main.tile[x, y + rope].active() && Main.tile[x, y + rope].type == TileID.Trees) || rope < 5))
            {
                WorldGen.PlaceTile(x, y + rope++, TileID.Rope, true, true, -1, 0);
            }

        }


        public void DrawBase6(int extraFloors, int extraStorageFloor)
        {

            DrawObjectTask floorType1 = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 9, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 11, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 5));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HolidayLights, 1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HolidayLights, 1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 8));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Torches, 1, curTorchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 11, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 7));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 7));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 4));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 11, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 11, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 11, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }

            }

            DrawObjectTask storageExit = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.TallGateClosed, 1, 0, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 31, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 31));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 9));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 5, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 7, -1, false));
                    storageExit.drawLineList.Add(drawLineTask);
                }
            }

            int floorSpace = (extraFloors) / (extraStorageFloor + 1);
            int florCountToStorage = 0;

            while (extraFloors > 0)
            {
                DrawAObject(floorType1); cy -= 2;
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

            Main.tile[cx + 3, cy - 5].liquid = 255;
            Main.tile[cx + 29, cy - 5].honey(true);
            Main.tile[cx + 29, cy - 5].liquid = 255;


            RopeToGround(cx + 16, cy + 2);


            WorldGen.PlaceTile(cx + 14, cy, TileID.Containers, true, true, -1, curChestStyle);
            int cid = Chest.FindChest(cx + 14, cy - 1);
            SetUpChest(cid);


        }

        public void Fill(int xtl, int ytl, int dimX, int dimY, int wallID, int tileID = -1, int style = 0)
        {
            for (int x = xtl; x < xtl + dimX; x++)
                for (int y = ytl; y < ytl + dimY; y++)
                {
                    if (wallID > 0)
                        WorldGen.PlaceWall(x, y, wallID, true);

                    if (tileID >= 0)
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
                DrawObjectTask floor = new DrawObjectTask(this, "9B|" +
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
                    //if (extraFloors % 2 == 0)
                    //{
                    cx = stfx - 8 * (((extraFloorsParam + 1) / 2) - (extraFloors / 2));
                    DrawAObject(floor);
                    //}
                    // else
                    //{
                    cy = stfy;// added // for sym
                    cx = stfx + 30 + 8 * (((extraFloorsParam + 1) / 2) - (extraFloors / 2));//+1 for sym
                    DrawAObject(floor, false);
                    //}
                    extraFloors--;// added // for sym
                }
                cx = startX;
                cy = startY;
            }


            DrawObjectTask innerbase = new DrawObjectTask(this, "1B8P6B9P6B8P1B|" +
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
                                                          (extraFloorsParam == 0 ? "1B3e1b4p1b3e4b5p8b4p1b3e1B" : "1B8p1b3e2b9p6b8p1B"));//it does not overwrite platform with empty /todo remove


            DrawAObject(innerbase);


            DrawObjectTask maininnertop;

            if (extraFloorsParam == 0)
            {
                cy -= 4; //overwrite bot floor

                maininnertop = new DrawObjectTask(this, "1B4b4p3e1p3e1b7e1p1b4e4p4b1B|" +
                                                          "1B1h1w1b4e1b2e1p3e1b7e1p4e1b4e1b1w1h1B|" +
                                                          "1B1e3b4e4b1pae1p4e1b4e3b1e1B|" +
                                                          "1B3e1b4p1b3e4b5p8b4p1b3e1B|" +
                                                          "1B3e1b4e1pje1p4e1b3e1B|" +
                                                          "1B3e1c4e1pje1p4e1c3e1B|" +
                                                          "1B8e1pje1p8e1B|" +
                                                          "1B4btp4b1B");
            }
            else
            {
                cy--; //overwrite bot lane
                maininnertop = new DrawObjectTask(this, "1B4b4p1b3e4b5p8b4p4b1B|" +
                                                          "1c3e1b4e1pje1p4e1b3e1c|" +
                                                          "4e1h4e1pje1p4e1h4e|" +
                                                          "1b8e1pje1p8e1b|" +
                                                          "1B4btp4b1B");
            }
            DrawAObject(maininnertop);


            if (extraFloorsParam == 0)
            {
                /*
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
                WorldGen.PlaceTile(cx + 39, cy - 2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);*/
            }
            else
            {
                WorldGen.PlaceTile(cx - 3, cy - 2, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx - 3, cy - 1, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx + 39 + 2, cy - 2, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(cx + 39 + 2, cy - 1, TileID.Platforms, true, true, -1, curPlatformStyle);
            }


            DrawObjectTask maininnerbot = new DrawObjectTask(this, "1E3e1hte1h3e1E|" +
                                                             "1EHe1E|" +
                                                             "1EHe1E|" +
                                                             "1EHp1E|" +
                                                             "1EHe1E|" +
                                                             "1EHe1E|" +
                                                             "1BHp1B|" +
                                                             "1BHe1B|" +
                                                             "1BHe1B|" +
                                                             "1B6b1p9b5p9b1p1b1p4b1B|" +
                                                             "1B6e1b2e1h1c4e1b5e1b4e1c1h2e1b1e1b4e1B|" +
                                                             "1Bfe1b5e1bfe1B|" +
                                                             "1B8e1w6b1p3e1p6b1w8e1B|" +
                                                             "1Bbb4e1h5e1h4ebb1B|" +
                                                             "1E1pFe1p1E|" +
                                                             "1E1pFe1p1E|" +
                                                             "1E1pFe1p1E|" +
                                                             "hB5PhB"
                                                             );
            DrawAObject(maininnerbot);


            RopeToGround(cx + 19, cy);
            int cid = WorldGen.PlaceChest(cx + 18, cy - 2, TileID.Containers, false, curChestStyle);
            if (cid >= 0) SetUpChest(cid);

            //entrances
            if (extraFloorsParam == 0)
            {
                for (int cyi = cy - 2; cyi > cy - 7; cyi--)
                    WorldGen.KillTile(cx, cyi, false, false, false);
                WorldGen.PlaceObject(cx, cy - 6, TileID.TallGateOpen, true);

                for (int cyi = cy - 2; cyi > cy - 7; cyi--)
                    WorldGen.KillTile(cx + 38, cyi, false, false, false);
                WorldGen.PlaceObject(cx + 38, cy - 6, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx, cy - 17, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx + 38, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx + 38, cy - 17, TileID.TallGateOpen, true);
            }
            else
            {
                int diffxl = 8 * ((extraFloorsParam + 1) / 2);
                int diffxr = 8 * ((extraFloorsParam + 1) / 2);//+1 for sym

                WorldGen.PlaceTile(cx - diffxl, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx - diffxl, cy - 17, TileID.TallGateOpen, true);

                WorldGen.PlaceTile(cx + 38 + diffxr, cy - 18, curTileType, true, true);
                WorldGen.PlaceObject(cx + 38 + diffxr, cy - 17, TileID.TallGateOpen, true);

                for (int cyi = cy - 20; cyi > cy - 26; cyi--)
                    WorldGen.PlaceTile(cx - diffxl, cyi, curTileType, true, true);
                for (int cyi = cy - 20; cyi > cy - 26; cyi--)
                    WorldGen.PlaceTile(cx + 38 + +diffxr, cyi, curTileType, true, true);


                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx - diffxl, cyi, false, false, false);
                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.KillTile(cx + 38 + diffxr, cyi, false, false, false);


                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx - diffxl, cyi, curTileType, true, true);
                for (int cyi = cy - 7; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx + 38 + diffxr, cyi, curTileType, true, true);


                WorldGen.KillTile(cx - diffxl, cy - 6, false, false, false);
                WorldGen.KillTile(cx + 38 + diffxr, cy - 6, false, false, false);

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
                    WorldGen.PlaceTile(cx, cyi, TileID.Platforms, true, true, -1, curPlatformStyle);
                for (int cyi = cy - 8; cyi > cy - 11; cyi--)
                    WorldGen.PlaceTile(cx + 38, cyi, TileID.Platforms, true, true, -1, curPlatformStyle);

                WorldGen.KillTile(cx - 2, cy - 5, false, false, false);
                WorldGen.KillTile(cx + 38 + 2, cy - 5, false, false, false);

                WorldGen.PlaceTile(cx - 2, cy - 2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);
                WorldGen.PlaceTile(cx + 37 + 2, cy - 2, TileID.WorkBenches, true, true, -1, curWorkBenchStyle);
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

            DrawObjectTask floorType1 = new DrawObjectTask(this);
            {
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 5, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 4, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 2, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 2, curPlatformStyle, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 2, curPlatformStyle));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 2, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.HangingLanterns, 1, curLanternStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Platforms, 1, curPlatformStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 3));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 2));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.Chairs, 1, curChairStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, TileID.WorkBenches, 1, curWorkBenchStyle));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 1, -1, false));
                    floorType1.drawLineList.Add(drawLineTask);
                }
                {
                    DrawLineTask drawLineTask = new DrawLineTask(this);
                    drawLineTask.drawTaskList.Add(new DrawTask(this, -1, 1, -1, false));
                    drawLineTask.drawTaskList.Add(new DrawTask(this, curTileType, 5, -1, false));
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
                    cx = startX - (count / 2) * 4 - 3;
                    DrawAObject(floorType1);
                }
                else
                {
                    cx = startX + (count / 2 + 1) * 4 + 3;
                    DrawAObject(floorType1, false);
                }
                extraFloors--;
                count++;
            }

            Fill(startX + 3, startY + 1, 4, 21, curWallType);
            Fill(startX + 3, startY, 4, 1, 0, TileID.Platforms, curPlatformStyle);
            Fill(startX + 3, startY + 11, 4, 1, curWallType, TileID.Platforms, curPlatformStyle);
            Fill(startX + 2, startY + 17, 6, 1, curWallType, TileID.Platforms, curPlatformStyle);

            cx = startX + 4;
            cy = startY;

            Main.tile[cx - 10, cy + 17].liquid = 255;
            Main.tile[cx - 10, cy + 17].honey(true);
            Main.tile[cx + 11, cy + 17].liquid = 255;


            RopeToGround(startX + 2 + 2, startY + 18);
            RopeToGround(startX + 3 + 2, startY + 18);

            int cid = WorldGen.PlaceChest(startX + +4, startY + 16, TileID.Containers, false, curChestStyle);
            if (cid >= 0) SetUpChest(cid);

        }




        public void DrawATask(DrawTask task, bool goRightDirec = true)
        {
            int dir = goRightDirec ? 1 : -1;

            int curTileType = task.type;
            int curStyle = task.spriteStyle;
            int count = task.count;
            bool placeWall = task.placeWall;

            while (count > 0)
            {
                count--;
                if (placeWall) WorldGen.PlaceWall(cx, cy, curWallType, true);


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
                        int chestNum = WorldGen.PlaceChest(cx, cy + 1, TileID.Containers, false, curStyle);
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
                        WorldGen.PlaceTile(cx, cy + 1, TileID.Dirt, true, true, -1, 0); //dummy tiles to stand on
                        WorldGen.PlaceTile(cx + 1, cy + 1, TileID.Dirt, true, true, -1, 0);
                        WorldGen.PlaceTile(cx, cy, TileID.WorkBenches, true, true, -1, curStyle);
                        cx += dir > 0 ? dir : 0;
                        if (placeWall) WorldGen.PlaceWall(cx, cy, curWallType, true);
                    }
                    else if (curTileType == TileID.Chairs)
                    {
                        //they are drawn at bottom instead of top left
                        WorldGen.PlaceTile(cx, cy + 2, TileID.Dirt, true, true, -1, 0);
                        WorldGen.PlaceTile(cx, cy + 1, 15, true, false, -1, curStyle);


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
                        WorldGen.PlaceObject(cx, cy, curTileType, true);

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
            if (!goRightDirec) ldlt.Reverse();
            foreach (DrawTask task in ldlt)
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


        private bool canPlaceC(int x, int y)
        {
            y--;

            if (Main.tile[x, y].active() || Main.tile[x + 1, y].active() || Main.tile[x, y - 1].active() || Main.tile[x + 1, y - 1].active())
                return false;

            if (Main.tile[x, y + 1].type == TileID.Platforms && Main.tile[x + 1, y + 1].type == TileID.Platforms)
                return true;

            if ((Main.tile[x, y + 1].type != TileID.Platforms && Main.tile[x, y + 1].type != curTileType) ||
                (Main.tile[x + 1, y + 1].type != TileID.Platforms && Main.tile[x + 1, y + 1].type != curTileType))
                return false;

            if ((Main.tile[x - 1, y].type == TileID.Chairs || Main.tile[x - 1, y].type == TileID.WorkBenches || Main.tile[x - 1, y].type == TileID.Lamps)
                  && (Main.tile[x - 1, y - 1].type != curTileType && Main.tile[x - 1, y - 1].type != TileID.Platforms))
                return false;

            if ((Main.tile[x + 2, y].type == TileID.Chairs || Main.tile[x + 2, y].type == TileID.WorkBenches || Main.tile[x + 2, y].type == TileID.Lamps)
                 && (Main.tile[x + 2, y - 1].type != curTileType && Main.tile[x + 2, y - 1].type != TileID.Platforms))
                return false;

            if ((Main.tile[x - 2, y].type == TileID.Chairs || Main.tile[x - 2, y].type == TileID.WorkBenches)
                 && (!Main.tile[x - 1, y].active() && Main.tile[x - 1, y].type != TileID.HangingLanterns && Main.tile[x - 1, y].type != TileID.Platforms))
                return false;
            if ((Main.tile[x + 3, y].type == TileID.Chairs || Main.tile[x + 3, y].type == TileID.WorkBenches)
                && (!Main.tile[x + 2, y].active() && Main.tile[x + 2, y].type != TileID.HangingLanterns && Main.tile[x + 2, y].type != TileID.Platforms))
                return false;

            if (Main.tile[x, y + 2].type == TileID.HolidayLights || Main.tile[x + 1, y + 2].type == TileID.HolidayLights)
                return false;

            return true;
        }


        public void PlaceAllItems()
        {
            int x = ropeX;
            int y = ropeY - 5;
            for (; (Main.tile[x, y - 2].active() || Main.tile[x, y - 1].wall == curWallType) && y > 75; y--)
            {

            }
            for (; (Main.tile[x - 1, y].active() || Main.tile[x - 1, y].wall == curWallType) && x > 75; x--)
            {

            }
            x++;
            int startX = x;


            int i = 0;
            bool endIt = false;

            for (; !endIt && (Main.tile[x, y].active() || Main.tile[x, y].wall == curWallType || Main.tile[x, y - 1].active() || Main.tile[x, y - 1].wall == curWallType) && y < Main.maxTilesY - 75; y++)
            {
                for (; !endIt && (Main.tile[x, y].active() || Main.tile[x, y].wall == curWallType || Main.tile[x, y - 1].active() || Main.tile[x, y - 1].wall == curWallType) && x < Main.maxTilesX - 75; x++)
                {
                    if (canPlaceC(x, y))
                    {
                        int chestNum = NewChest(x, y, false);
                        if (chestNum > 0)
                        {
                            for (int ci = 0; ci < 40 && i < ItemLoader.ItemCount; ci++)
                            {
                                Main.chest[chestNum].item[ci].SetDefaults(i++);
                                Main.chest[chestNum].item[ci].stack = Main.chest[chestNum].item[ci].maxStack;
                            }
                            x++;
                        }
                        if (chestNum == Main.maxChests || i == ItemLoader.ItemCount)
                        {
                            endIt = true;
                            break;
                        }

                    }
                }
                x = startX;
            }

        }


        public int NewChest(int x, int y, bool doGround = true)
        {
            if (doGround)
            {
                WorldGen.PlaceWall(x, y, curWallType, true);
                WorldGen.PlaceTile(x, y, TileID.Platforms, true, true, -1, curPlatformStyle);
                WorldGen.PlaceTile(x + 1, y, TileID.Platforms, true, true, -1, curPlatformStyle);
            }
            int chestNum = WorldGen.PlaceChest(x, y - 1, TileID.Containers, false, WorldGen.genRand.Next(5, 22));
            return chestNum;
        }




    }
}
