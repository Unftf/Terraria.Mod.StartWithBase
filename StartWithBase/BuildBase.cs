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


using Terraria.GameContent.UI.Elements;




namespace StartWithBase
{
    class BuildBase : ModWorld
    {
       

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            Builder builder = new Builder(mod);

            int tunnelsIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Sand"));
            if(tunnelsIndex == -1)
                tunnelsIndex = Math.Min(2, tasks.Count);
            
            tasks.Insert(tunnelsIndex, new PassLegacy("Create UI", delegate (GenerationProgress progress)
            {
                UIState state = Main.MenuUI.CurrentState;
                builder.Init(state);                    

            }));
            
            int taskCount = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup")) - 1;
            if (taskCount == -1)
                taskCount = Math.Max(0, tasks.Count-2);

            for (int tid = taskCount; tid > tunnelsIndex ; tid--)
            {
                int val = (taskCount - tid);
                tasks.Insert(tid, new PassLegacy("Set counter to", delegate (GenerationProgress progress)
                {
                    if (builder != null)
                    {
                        builder.SetProgress(val);
                        while (builder.swbui != null && builder.swbui.pauseActive) { Thread.Sleep(242); }
                    }

                }));
            }
            

            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (genIndex == -1)
                genIndex = Math.Max(0, tasks.Count-1);
            if (genIndex != -1)
            {                
                tasks.Insert(genIndex, new PassLegacy("Build base", delegate (GenerationProgress progress)
                {                                    
                    builder.Build();                                                
                    builder.EndBuilding();
                    builder = null;                    
                })); 
            }                       
        }
    }
}
