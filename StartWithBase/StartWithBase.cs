using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terraria.ModLoader;
using Terraria;
using Terraria.UI;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.GameContent.UI.States;
using Microsoft.Xna.Framework;


using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI.Gamepad;
using Terraria.World.Generation;
using Terraria.Map;

using System.IO;
using System.Reflection;
using ReLogic.Graphics;





using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.IO;

using System.Threading;



namespace StartWithBase
{
    class StartWithBase : Mod
    {


        public StartWithBase instance;
        public StartWithBase()
        {
            Properties = new ModProperties()
            {
                Autoload = true
            };
            instance = this;

        }
        public override void Load()
        {

        }
        public override void Unload()
        {

        }

        

    }
}
