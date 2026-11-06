
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace cis_580_game;

public static class Texture2DExtensions
{
    extension(Texture2D source)
    {
        public static Texture2D HorizontalConcatenation(params Texture2D[] textures)
        {
            int numTextures = textures.Count();
            if (numTextures <= 0) throw new ArgumentException("Cannot concatenate 0 textures");
            if (numTextures == 1) return textures[0];
            if (textures.Any(texture => texture.Height != textures[0].Height)) throw new ArgumentException("Concatenated textures must have the same height");

            GraphicsDevice graphics = textures[0].GraphicsDevice;
            int width = textures.Sum(texture => texture.Width);
            int height = textures[0].Height;
            int largeTextureCount = width*height;

            Texture2D result = new(graphics, width, height);

            Color[] data = new Color[largeTextureCount];
            int dataStartIndex = 0;
            int columnOffset = 0;
            for (int t = 0; t < numTextures; t++)
            {
                Texture2D texture = textures[t];
                Color[] colorData = texture.GetColorData();

                dataStartIndex = columnOffset;
                for (int row = 0; row < height; row++)
                {
                    Array.Copy(colorData, row*texture.Width, data, dataStartIndex, texture.Width);
                    dataStartIndex += width;
                }
                columnOffset += texture.Width;
            }
            result.SetData(data);

            return result;
        }

        /// <summary>
        /// Copy of the internal method GetColorData().
        /// </summary>
        /// <returns>An array of color data</returns>
        /// <exception cref="Exception"></exception>
        public Color[] GetColorData()
        {
            int num = source.Width * source.Height;
            Color[] array = new Color[num];
            switch (source.Format)
            {
                case SurfaceFormat.Single:
                {
                    float[] array3 = new float[num];
                    source.GetData(array3);
                    for (int j = 0; j < num; j++)
                    {
                        float num2 = array3[j];
                        array[j] = new Color(num2, num2, num2);
                    }

                    break;
                }
                case SurfaceFormat.Color:
                    source.GetData(array);
                    break;
                case SurfaceFormat.Alpha8:
                {
                    Alpha8[] array14 = new Alpha8[num];
                    source.GetData(array14);
                    for (int num9 = 0; num9 < num; num9++)
                    {
                        array[num9] = new Color(array14[num9].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Bgr565:
                {
                    Bgr565[] array7 = new Bgr565[num];
                    source.GetData(array7);
                    for (int n = 0; n < num; n++)
                    {
                        array[n] = new Color(array7[n].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Bgra4444:
                {
                    Bgra4444[] array16 = new Bgra4444[num];
                    source.GetData(array16);
                    for (int num11 = 0; num11 < num; num11++)
                    {
                        array[num11] = new Color(array16[num11].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Bgra5551:
                {
                    Bgra5551[] array12 = new Bgra5551[num];
                    source.GetData(array12);
                    for (int num7 = 0; num7 < num; num7++)
                    {
                        array[num7] = new Color(array12[num7].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.HalfSingle:
                {
                    HalfSingle[] array9 = new HalfSingle[num];
                    source.GetData(array9);
                    for (int num4 = 0; num4 < num; num4++)
                    {
                        array[num4] = new Color(array9[num4].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.HalfVector2:
                {
                    HalfVector2[] array5 = new HalfVector2[num];
                    source.GetData(array5);
                    for (int l = 0; l < num; l++)
                    {
                        array[l] = new Color(array5[l].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.HalfVector4:
                {
                    HalfVector4[] array17 = new HalfVector4[num];
                    source.GetData(array17);
                    for (int num12 = 0; num12 < num; num12++)
                    {
                        array[num12] = new Color(array17[num12].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.NormalizedByte2:
                {
                    NormalizedByte2[] array15 = new NormalizedByte2[num];
                    source.GetData(array15);
                    for (int num10 = 0; num10 < num; num10++)
                    {
                        array[num10] = new Color(array15[num10].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.NormalizedByte4:
                {
                    NormalizedByte4[] array13 = new NormalizedByte4[num];
                    source.GetData(array13);
                    for (int num8 = 0; num8 < num; num8++)
                    {
                        array[num8] = new Color(array13[num8].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Rg32:
                {
                    Rg32[] array11 = new Rg32[num];
                    source.GetData(array11);
                    for (int num6 = 0; num6 < num; num6++)
                    {
                        array[num6] = new Color(array11[num6].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Rgba64:
                {
                    Rgba64[] array10 = new Rgba64[num];
                    source.GetData(array10);
                    for (int num5 = 0; num5 < num; num5++)
                    {
                        array[num5] = new Color(array10[num5].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Rgba1010102:
                {
                    Rgba1010102[] array8 = new Rgba1010102[num];
                    source.GetData(array8);
                    for (int num3 = 0; num3 < num; num3++)
                    {
                        array[num3] = new Color(array8[num3].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Srgb8Etc2:
                {
                    Srgb8Etc2[] array6 = new Srgb8Etc2[num];
                    source.GetData(array6);
                    for (int m = 0; m < num; m++)
                    {
                        array[m] = new Color(array6[m].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.Srgb8A1Etc2:
                {
                    Srgb8A1Etc2[] array4 = new Srgb8A1Etc2[num];
                    source.GetData(array4);
                    for (int k = 0; k < num; k++)
                    {
                        array[k] = new Color(array4[k].ToVector4());
                    }

                    break;
                }
                case SurfaceFormat.SRgb8A8Etc2:
                {
                    SRgb8A8Etc2[] array2 = new SRgb8A8Etc2[num];
                    source.GetData(array2);
                    for (int i = 0; i < num; i++)
                    {
                        array[i] = new Color(array2[i].ToVector4());
                    }

                    break;
                }
                default:
                    throw new Exception("Texture surface format not supported");
            }

            return array;
        }
    }
}
