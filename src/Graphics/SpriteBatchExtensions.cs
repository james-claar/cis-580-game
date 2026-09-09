using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

static class SpriteBatchExtensions
{
    private static Texture2D _blankTexture;

    /// <summary>
    /// Custom extension method to get a 1x1 white rectangle texture, to be stretched and colored at draw time.
    /// </summary>
    /// <param name="s">The SpriteBatch to get the texture from</param>
    /// <returns>A 1x1 white pixel texture</returns>
    public static Texture2D BlankTexture(this SpriteBatch s)
    {
        // https://community.monogame.net/t/whats-the-simplest-way-to-draw-a-rectangular-outline-without-generating-the-texture/7818/5
        if (_blankTexture == null)
        {
            _blankTexture = new Texture2D(s.GraphicsDevice, 1, 1);
            _blankTexture.SetData(new[] {Color.White});
        }
        return _blankTexture;
    }
}