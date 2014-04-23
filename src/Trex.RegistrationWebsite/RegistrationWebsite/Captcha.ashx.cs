using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace RegistrationWebsite
{
    public class Captcha : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["rand"] != null)
            {
                var cText = context.Session["rand"].ToString();

                const int iHeight = 100;
                const int iWidth = 312;

                var oRandom = new Random();

                var aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

                var aFontNames = new string[]
                                     {
                                         "Comic Sans MS",
                                         "Arial",
                                         "Times New Roman",
                                         "Georgia",
                                         "Verdana",
                                         "Geneva"
                                     };

                var aFontStyles = new FontStyle[]
                                      {
                                          FontStyle.Bold,
                                          FontStyle.Strikeout,
                                          FontStyle.Strikeout,
                                          FontStyle.Italic,
                                          FontStyle.Regular,
                                          FontStyle.Underline
                                      };


                var outputImage = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
                var graphics = Graphics.FromImage(outputImage);
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

                var oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
                var oBrush = default(Brush);

                graphics.FillRectangle(Brushes.White, oRectangleF);

                var matrix = new Matrix();

                var i = 0;
                for (i = 0; i <= cText.Length - 1; i++)
                {
                    matrix.Reset();
                    var iChars = cText.Length;
                    var x = iWidth / (iChars + 1) * i;
                    var y = iHeight / 2;

                    matrix.RotateAt(oRandom.Next(-60, 60), new PointF(x, y));
                    graphics.Transform = matrix;

                    graphics.DrawString
                        (
                            cText.Substring(i, 1),
                            new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                                     aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                                     aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                            new SolidBrush(Color.Black),
                            x + 20,
                            oRandom.Next(10, 40)
                        );
                    graphics.ResetTransform();
                }

                var points = new Point[15];

                for (var e = 0; e < 15; e++)
                {
                    points[e] = new Point(oRandom.Next(0, 310), oRandom.Next(0, 100));
                }

                graphics.DrawCurve(Pens.Black, points, 12);



                var pen = new Pen(Color.FromArgb(187, 187, 187));

                graphics.DrawRectangle(pen, 0, 0, 311, 99);

                pen.Dispose();

                var memoryStream = new MemoryStream();
                outputImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                var bytes = memoryStream.GetBuffer();

                outputImage.Dispose();
                memoryStream.Close();

                context.Response.BinaryWrite(bytes);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}