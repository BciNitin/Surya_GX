
using BarcodeLib;

using System.Drawing;

namespace ELog.Core.BarcodeGeneration
{
    public static class BarcodeGeneratorHelper
    {
        public static void GetBarCode(string CodeNumber, int Length = 1000, int Height = 200, int FontSize = 40)
        {

            using Barcode barcode = new Barcode();
            Color foreColor = Color.Black;
            Color backColor = Color.Transparent;

            barcode.IncludeLabel = true;
            barcode.Alignment = AlignmentPositions.CENTER;
            barcode.LabelFont = new Font(FontFamily.GenericMonospace, FontSize * Barcode.DotsPerPointAt96Dpi, FontStyle.Regular, GraphicsUnit.Pixel);

            var barcodeImage = barcode.Encode(TYPE.CODE39, CodeNumber, Color.Black, Color.White, Length, Height);
            // barcodeImage.Save(@"C:\Barcode.png", ImageFormat.Jpeg);
        }
    }

}

