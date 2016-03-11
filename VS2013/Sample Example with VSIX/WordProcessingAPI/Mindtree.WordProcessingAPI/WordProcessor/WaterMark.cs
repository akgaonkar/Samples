﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Vml.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;
using HorizontalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.HorizontalAnchorValues;
using WaterMarkLock = DocumentFormat.OpenXml.Vml.Office.Lock;
using VerticalAnchorValues = DocumentFormat.OpenXml.Vml.Wordprocessing.VerticalAnchorValues;


namespace Mindtree.WordProcessingAPI.WordProcessor
{

    /// <summary>
    /// This class hold the methods to manupulate water marks in a given Document 
    /// </summary>
    public partial class WaterMark
    {


        public WaterMark()
        {
        }

        /// <summary>
        /// This medthod is used to Add water mark in a given Document
        /// </summary>
        /// <param name="doc"></param>
        public void AddWatermark(WordprocessingDocument doc)
        {


            foreach (HeaderPart headerPart in doc.MainDocumentPart.HeaderParts)
            {
                var sdtBlock1 = new SdtBlock();
                var sdtProperties1 = new SdtProperties();
                var sdtId1 = new SdtId() { Val = 87908844 };
                var sdtContentDocPartObject1 = new SdtContentDocPartObject();
                var docPartGallery1 = new DocPartGallery() { Val = "Watermarks" };
                var docPartUnique1 = new DocPartUnique();
                sdtContentDocPartObject1.Append(docPartGallery1);
                sdtContentDocPartObject1.Append(docPartUnique1);
                sdtProperties1.Append(sdtId1);
                sdtProperties1.Append(sdtContentDocPartObject1);

                var sdtContentBlock1 = new SdtContentBlock();
                var paragraph2 = new Paragraph()
                {
                    RsidParagraphAddition = "00656E18",
                    RsidRunAdditionDefault = "00656E18"
                };
                var paragraphProperties2 = new ParagraphProperties();
                var paragraphStyleId2 = new ParagraphStyleId() { Val = "Header" };
                paragraphProperties2.Append(paragraphStyleId2);
                var run1 = new Run();
                var runProperties1 = new RunProperties();
                var noProof1 = new NoProof();
                var languages1 = new Languages() { EastAsia = "zh-TW" };
                runProperties1.Append(noProof1);
                runProperties1.Append(languages1);
                var picture1 = new Picture();
                var shapetype1 = new Shapetype()
                {
                    Id = "_x0000_t136",
                    CoordinateSize = "21600,21600",
                    OptionalNumber = 136,
                    Adjustment = "10800",
                    EdgePath = "m@7,l@8,m@5,21600l@6,21600e"
                };
                var formulas1 = new Formulas();
                var formula1 = new Formula() { Equation = "sum #0 0 10800" };
                var formula2 = new Formula() { Equation = "prod #0 2 1" };
                var formula3 = new Formula() { Equation = "sum 21600 0 @1" };
                var formula4 = new Formula() { Equation = "sum 0 0 @2" };
                var formula5 = new Formula() { Equation = "sum 21600 0 @3" };
                var formula6 = new Formula() { Equation = "if @0 @3 0" };
                var formula7 = new Formula() { Equation = "if @0 21600 @1" };
                var formula8 = new Formula() { Equation = "if @0 0 @2" };
                var formula9 = new Formula() { Equation = "if @0 @4 21600" };
                var formula10 = new Formula() { Equation = "mid @5 @6" };
                var formula11 = new Formula() { Equation = "mid @8 @5" };
                var formula12 = new Formula() { Equation = "mid @7 @8" };
                var formula13 = new Formula() { Equation = "mid @6 @7" };
                var formula14 = new Formula() { Equation = "sum @6 0 @5" };

                formulas1.Append(formula1);
                formulas1.Append(formula2);
                formulas1.Append(formula3);
                formulas1.Append(formula4);
                formulas1.Append(formula5);
                formulas1.Append(formula6);
                formulas1.Append(formula7);
                formulas1.Append(formula8);
                formulas1.Append(formula9);
                formulas1.Append(formula10);
                formulas1.Append(formula11);
                formulas1.Append(formula12);
                formulas1.Append(formula13);
                formulas1.Append(formula14);
                var path1 = new Path()
                {
                    AllowTextPath = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(true),
                    ConnectionPointType = ConnectValues.Custom,
                    ConnectionPoints = "@9,0;@10,10800;@11,21600;@12,10800",
                    ConnectAngles = "270,180,90,0"
                };
                var textPath1 = new TextPath()
                {
                    On = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(true),
                    FitShape = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(true)
                };
                //var shapeHandles1 = new ShapeHandle();

                var shapeHandle1 = new ShapeHandle()
                {
                    Position = "#0,bottomRight",
                    XRange = "6629,14971"
                };

                //  shapeHandles1.Append(shapeHandle1);

                var lock1 = new WaterMarkLock
                {
                    Extension = ExtensionHandlingBehaviorValues.Edit,
                    TextLock = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(true),
                    ShapeType = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(true)
                };

                shapetype1.Append(formulas1);
                shapetype1.Append(path1);
                shapetype1.Append(textPath1);
                shapetype1.Append(shapeHandle1);
                shapetype1.Append(lock1);
                var shape1 = new Shape()
                {
                    Id = "PowerPlusWaterMarkObject357476642",
                    Style = "position:absolute;left:0;text-align:left;margin-left:0;margin-top:0;width:527.85pt;height:250pt;rotation:315;z-index:-251656192;mso-position-horizontal:center;mso-position-horizontal-relative:margin;mso-position-vertical:center;mso-position-vertical-relative:margin",
                    OptionalString = "_x0000_s2049",
                    AllowInCell = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(false),
                    FillColor = "silver",
                    Stroked = DocumentFormat.OpenXml.TrueFalseValue.FromBoolean(false),
                    Type = "#_x0000_t136"
                };


                var fill1 = new Fill() { Opacity = ".5" };
                TextPath textPath2 = new TextPath()
                {
                    Style = "font-family:\"Calibri\";font-size:1pt",
                    String = "DRAFT"
                };

                var textWrap1 = new TextWrap()
                {
                    AnchorX = HorizontalAnchorValues.Margin,
                    AnchorY = VerticalAnchorValues.Margin
                };

                shape1.Append(fill1);
                shape1.Append(textPath2);
                shape1.Append(textWrap1);
                picture1.Append(shapetype1);
                picture1.Append(shape1);
                run1.Append(runProperties1);
                run1.Append(picture1);
                paragraph2.Append(paragraphProperties2);
                paragraph2.Append(run1);
                sdtContentBlock1.Append(paragraph2);
                sdtBlock1.Append(sdtProperties1);
                sdtBlock1.Append(sdtContentBlock1);
                headerPart.Header.Append(sdtBlock1);
                headerPart.Header.Save();
                //break;
            }
        }


    }
}
