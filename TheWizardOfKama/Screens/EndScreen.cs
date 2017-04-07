using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Content;

namespace TheWizardOfKama
{
    class EndScreen : GameScreen
    {
        string winBackground_file;
        Texture2D winBackground;
        string victoryTitle = "VICTORY";
        string endGameInstructions = "Click 'Esc' to quit or 'R' to restart...";
        string levelStat;
        string monstersKilledStat;
        string playedTimeStat;

        public EndScreen(Game game, ContentManager content, SpriteBatch spriteBatch, string name) : base(game, content, spriteBatch, name)
        {
            Initalize();
            LoadContent();
        }

        private void Initalize()
        {
            winBackground_file = "backgrounds/WinWallpaper";
        }

        private void LoadContent()
        {
            winBackground = content.Load<Texture2D>(winBackground_file);
            spriteFontNormal = content.Load<SpriteFont>("GameText36");
            spriteFontBig = content.Load<SpriteFont>("GameText56");
            spriteFontLarge = content.Load<SpriteFont>("GameText70");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(winBackground, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            textSize = spriteFontLarge.MeasureString(victoryTitle);
            spriteBatch.DrawString(spriteFontLarge, victoryTitle, new Vector2((screenWidth - textSize.X) / 2, 100), Color.DeepPink);
            textSize = spriteFontBig.MeasureString(levelStat);
            spriteBatch.DrawString(spriteFontBig, levelStat, new Vector2((screenWidth - textSize.X) / 2, 400), Color.Lavender);
            textSize = spriteFontBig.MeasureString(monstersKilledStat);
            spriteBatch.DrawString(spriteFontBig, monstersKilledStat, new Vector2((screenWidth - textSize.X) / 2, 500), Color.DarkRed);
            textSize = spriteFontBig.MeasureString(playedTimeStat);
            spriteBatch.DrawString(spriteFontBig, playedTimeStat, new Vector2((screenWidth - textSize.X) / 2, 600), Color.Gold);
            textSize = spriteFontNormal.MeasureString(endGameInstructions);
            spriteBatch.DrawString(spriteFontNormal, endGameInstructions, new Vector2((screenWidth - textSize.X) / 2, 800), Color.GhostWhite);
            spriteBatch.End();
        }

        public void LoadGameStats(GameStats gameStats)
        {
            this.endGameStats = gameStats;
            levelStat = "Reached level: " + endGameStats.Level;
            monstersKilledStat = "Total killed monsters: " + endGameStats.NoOfMonstersKilled;
            playedTimeStat = "Total game time: " + (endGameStats.PlayedTime / 1000 /60).ToString("0.00") + " minutes";
        }
    }
}
