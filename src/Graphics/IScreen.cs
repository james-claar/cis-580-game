using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace cis_580_game;

/// <summary>
/// A template class for game screens
/// </summary>
public interface IScreen
{
    /// <summary>
    /// Updates everything in the screen
    /// </summary>
    /// <param name="gt">The GameTime</param>
    public virtual void Update(GameTime gt)
    {
        
    }

    /// <summary>
    /// Draws this screen
    /// </summary>
    /// <param name="sb">The SpriteBatch to draw using</param>
    /// <param name="gt">The GameTime</param>
    public virtual void Draw(SpriteBatch sb, GameTime gt)
    {
        
    }
}
