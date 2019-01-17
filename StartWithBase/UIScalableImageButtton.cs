using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Terraria;
using Terraria.UI;
using Terraria.World.Generation;
using Terraria.GameContent.UI.Elements;
using Terraria.UI.Gamepad;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.ModLoader;

namespace StartWithBase
{
    class UIScalableImageButtton : UIImageButton
    {

        Texture2D btexture;
        public bool isClicked;
        public string content;
        const float visNotClick = 0.4f;

        public UIScalableImageButtton(Texture2D tex, string content = "",  bool isClicked = false): base(tex)
        {
            btexture = tex;
            this.isClicked = isClicked;
            this.content = content;

            
        }
        bool pd = false;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //Vector2 pos = new Vector2(this.MarginLeft, this.MarginTop);
            CalculatedStyle dimensions = base.GetDimensions();
            float scaling = dimensions.Width / btexture.Width;

            if (!pd && dimensions.Width > 0)
            {                
                pd = true;
            }

            spriteBatch.Draw(this.btexture, dimensions.Position(),
                                new Rectangle(0, 0, btexture.Width, btexture.Height),
                                Color.White*(base.IsMouseHovering ? ((isClicked ? 0.8f : 0.6f)) : (isClicked? 1.0f: visNotClick)), 0.0f, Vector2.Zero, scaling, SpriteEffects.None, 0);
        }

    }
}
