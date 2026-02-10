using Aspose.BarCode;
using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebReportHightJump
{
    /// <summary>
    /// Summary description for barcodegen
    /// </summary>
    public class barcodegen : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string symbology = context.Request.QueryString["symbology"];
            string code = context.Request.QueryString["code"];

            if (!string.IsNullOrEmpty(symbology) || !string.IsNullOrEmpty(symbology))
            {
                context.Response.ContentType = "image";

                context.Response.BinaryWrite(Encode(symbology, code));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        //Code128
        //Code39Standard
        //Code39Extended
        //Code93Standard
        //Code93Extended
        //EAN13
        //EAN8
        //Datamatrix
        //QR

        public static byte[] Encode(string type, string data)
        {
            // Instantiate an instance of license and set the license file through its path
            License license = new License();
            license.SetLicense("Aspose.BarCode.lic");

            //ExStart:EncodeQRCode
            // The path to the documents directory.
            // string dataDir = RunExamples.GetDataDir_CreateAndManage2DBarCodes();
            //string dataDir = "";

            //Aspose.BarCode.Generation.EncodeTypes = EncodeTypes.QR;

            //barcode type
            var encodeTypes = EncodeTypes.Code39Standard;
            switch (type)
            {
                case "Code128":
                    encodeTypes = EncodeTypes.Code128;
                    break;
                case "Code39":
                    encodeTypes = EncodeTypes.Code39Standard;
                    break;
                case "Code39Standard":
                    encodeTypes = EncodeTypes.Code39Standard;
                    break;
                case "Code39Extended":
                    encodeTypes = EncodeTypes.Code39Extended;
                    break;
                case "Code93":
                    encodeTypes = EncodeTypes.Code93Standard;
                    break;
                case "Code93Standard":
                    encodeTypes = EncodeTypes.Code93Standard;
                    break;
                case "Code93Extended":
                    encodeTypes = EncodeTypes.Code93Extended;
                    break;
                case "EAN13":
                    encodeTypes = EncodeTypes.EAN13;
                    break;
                case "EAN8":
                    encodeTypes = EncodeTypes.EAN8;
                    break;
                case "Datamatrix":
                    encodeTypes = EncodeTypes.DataMatrix;
                    break;
                case "QR":
                    encodeTypes = EncodeTypes.QR;
                    break;

                default:
                    encodeTypes = EncodeTypes.Code39Standard;
                    break;
            }


            // Initialize a BarcodeGenerator  class object and Set CodeText & Symbology Type
            //BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.QR, qr_data);
            BarCodeBuilder builder = new BarCodeBuilder(data, encodeTypes);
            switch (type)
            {
                case "QR":
                    builder.QREncodeMode = QREncodeMode.Auto;
                    builder.QREncodeType = QREncodeType.ForceQR;
                    builder.QRErrorLevel = QRErrorLevel.LevelQ;
                    builder.ECIEncoding = ECIEncodings.UTF8;
                    //builder.CodeText = 
                    builder.CodeLocation = CodeLocation.None;
                    builder.AspectRatio = 1;

                    //builder.GraphicsUnit = GraphicsUnit.Pixel;

                    builder.Margins.Bottom = 1;
                    builder.Margins.Right = 1;
                    builder.Margins.Top = 1;
                    builder.Margins.Left = 1;
                    builder.BorderVisible = false;
                    builder.BorderWidth = 0;
                    builder.AutoSize = true;

                    //builder.ImageWidth = 500;
                    //builder.ImageHeight = 500;
                    builder.Resolution = new Resolution(300, 300, ResolutionMode.Graphics);
                    //builder.ImageHeight = 500;
                    //builder.BarHeight = 500 - 1;
                    builder.xDimension = 1.6f;
                    builder.yDimension = 1.6f;

                    break;
                default:
                    builder.Resolution = new Resolution(300, 300, ResolutionMode.Graphics);
                    break;
            }

            // Get barcode image Bitmap and Save QR code
            Bitmap lBmp = builder.GenerateBarCodeImage();
            //lBmp.Save(dataDir + "EncodeQA_out.bmp", ImageFormat.Bmp);
            ////ExEnd:EncodeQRCode
            //Console.WriteLine(Environment.NewLine + "Barcode saved at " + dataDir + "EncodeQA_out.bmp");

            return ImageToByte(lBmp);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}