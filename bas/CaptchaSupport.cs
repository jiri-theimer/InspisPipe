using System;
using System.Drawing;


public class CaptchaSupport
{
    public string Formula { get; set; }
    public string FormulaHashed { get; set; }
    public byte[] ImageBytes { get; set; }

    public CaptchaSupport()
    {
        var rnd = new Random();

        string f1 = RandomFormula(DateTime.Now.Millisecond);
        string f2 = RandomFormula(DateTime.Now.Millisecond+8900);
        var b1 = ConvertTextToImage(f1, "Tahoma", rnd.Next(17, 22), Color.White, Color.Gray, 140, 100, rnd.Next(-15, 15));
        var b2 = ConvertTextToImage("+ " + Number2Word(f2), "Arial", rnd.Next(17, 22), Color.White, Color.PaleGreen, 260, 100, rnd.Next(-15, 15));
        var b = MergedBitmaps(b1, b2);


        this.Formula = (f1 + " + " + f2);
        this.FormulaHashed = new Crypto().Encrypt(this.Formula);


        ImageConverter converter = new ImageConverter();

        this.ImageBytes = (byte[])converter.ConvertTo(b, typeof(byte[]));
    }



    private string RandomFormula(int seed)
    {
        var rnd = new Random(seed);
        
        string znam = " + ";
        if (rnd.Next(1, 10) > 5)
        {
            znam = " - ";
        }

        return rnd.Next(0, 9).ToString() + znam + rnd.Next(0, 9).ToString();
    }

    private string Number2Word(string num)
    {
        return num.Replace("1", "jedna").Replace("2", "dvě").Replace("3", "tři").Replace("4", "čtyři").Replace("5", "pět").Replace("6", "šest").Replace("7", "sedm").Replace("8", "osm").Replace("9", "devět");
    }
    private Bitmap ConvertTextToImage(string expr, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height, int angle)
    {

        Bitmap bmp = new Bitmap(width, Height);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            Font font = new Font(fontname, fontsize);
            g.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
            g.RotateTransform(angle);
            g.DrawString(expr, font, new SolidBrush(fcolor), 15, 35);
            g.Flush();
            font.Dispose();
            g.Dispose();
        }

        return bmp;
    }

    private Bitmap MergedBitmaps(Bitmap bmp1, Bitmap bmp2)
    {
        Bitmap result = new Bitmap(400, 100);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.DrawImage(bmp1, 0, 0);
            g.DrawImage(bmp2, 100, 0);
        }
        return result;
    }


}
