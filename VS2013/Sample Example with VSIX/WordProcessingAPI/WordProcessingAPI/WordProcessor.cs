using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Word;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.VariantTypes;
using System.Xml.Linq;
using System.Xml;
//using DocumentFormat.OpenXml.Drawing;


namespace WordProcessingAPI
{
    public class SectionInfo
    {
        public string SectionName { get; set; }
        public int NoOfQuestions { get; set; }
        public int SectionId { get; set; }
    }
    public enum PropertyTypes : int
    {
        YesNo,
        Text,
        DateTime,
        NumberInteger
        //NumberDouble
    }

    public class GetQuestions
    {
        public string subHeadingText { get; set; }
        public Dictionary<string, string> questions;

    }
    public class WordProcessor
    {
        //Temp Physical path for the documents
        //string LegalOpinionInput = @"E:\Legal Opinion Latest\Input_OutPut_Files\IO\QuestionTemplate.docx";
        //string LegalOpinionOutput = @"E:\Legal Opinion Latest\Input_OutPut_Files\IO\AnswerTemplate.docx";

        #region Template Processing API

        public byte[] GetOpinionTemplate(int SubHeadingNo, int NumOfQuestions)
        {
            byte[] DocumentQuestionTemplate = new byte[100];
            byte[] fileBytesQuestionTemplate;
            try
            {
                if (SubHeadingNo > 0 && NumOfQuestions > 0)
                {
                    using (MemoryStream streamQuestionTemplate = new MemoryStream())
                    {
                        streamQuestionTemplate.Write(DocumentQuestionTemplate, 0, DocumentQuestionTemplate.Length);
                        CreateBlankOutputDoc(streamQuestionTemplate, false);
                        using (WordprocessingDocument wordDocumentInput = WordprocessingDocument.Open(streamQuestionTemplate, true))
                        {
                            wordDocumentInput.ChangeDocumentType(WordprocessingDocumentType.Document);
                            GenerateBody(wordDocumentInput, SubHeadingNo, NumOfQuestions);
                        }
                        //string docType=DocumentType.GENERIC_QUESTION_TEMPLATE;
                        fileBytesQuestionTemplate = ReadToEnd(streamQuestionTemplate);
                    }
                    return WDSetCustomProperty(fileBytesQuestionTemplate, "DocumentType", DocumentType.GENERIC_QUESTION_TEMPLATE.ToString(), PropertyTypes.Text);
                }
                else
                {
                    InvalidDataException ex = new InvalidDataException();
                    throw ex;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public byte[] GetOpinionTemplateWithHeader(byte[] NewTemplateDocument, DocumentMetaData metaData)
        {
            try
            {
                if (metaData.OpinionName != null && metaData.OpinionName != "" && metaData.OpinionDescription != null && metaData.OpinionDescription != "")
                {
                    string LegalOpinion = metaData.OpinionName; string DescriptionContent = metaData.OpinionDescription;
                    byte[] finalqTemplate;
                    using (MemoryStream streamQuestionTemplate = new MemoryStream())
                    {
                        streamQuestionTemplate.Write(NewTemplateDocument, 0, NewTemplateDocument.Length);
                        using (WordprocessingDocument wordDocumentInput = WordprocessingDocument.Open(streamQuestionTemplate, true))
                        {

                            Body inputDocBody = wordDocumentInput.MainDocumentPart.Document.Body;
                            List<SdtBlock> firstSection = inputDocBody.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.Equals("Section_" + 1)).ToList();
                            //Legal Opinion Names Start
                            List<SdtBlock> legalBlock = wordDocumentInput.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value == "LegalOpinion").ToList();
                            if (legalBlock.Count == 0)
                            {
                                Run runLegalOpinionBlock = new Run();
                                RunProperties runPropsLegalOpinionBlock = new RunProperties();
                                Bold boldLegalOpinionBlock = new Bold();
                                Underline ulLegalOpinionBlock = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };

                                runPropsLegalOpinionBlock.Append(boldLegalOpinionBlock);
                                runPropsLegalOpinionBlock.Append(ulLegalOpinionBlock);
                                RunFonts runFontLegalOpinionBlock = new RunFonts();           // Create font
                                runFontLegalOpinionBlock.Ascii = "Times New Roman";
                                runPropsLegalOpinionBlock.Append(runFontLegalOpinionBlock);
                                FontSize fontSizeLegalOpinionBlock = new FontSize() { Val = "30" };
                                runPropsLegalOpinionBlock.Append(fontSizeLegalOpinionBlock);
                                runLegalOpinionBlock.AppendChild(new RunProperties(runPropsLegalOpinionBlock));
                                runLegalOpinionBlock.AppendChild(new Text(LegalOpinion));
                                Paragraph paragraphLegalOpinionBlock = new Paragraph(runLegalOpinionBlock);
                                SdtProperties sdtprLegalOpinionBlock = new SdtProperties(
                                        new SdtAlias { Val = "LegalOpinion" },
                                        new Tag { Val = "LegalOpinion" });
                                SdtContentBlock sdtCBlockLegalOpinionBlock = new SdtContentBlock(paragraphLegalOpinionBlock);
                                SdtBlock sdtBlockLegalOpinionBlock = new SdtBlock(sdtprLegalOpinionBlock, sdtCBlockLegalOpinionBlock);
                                firstSection[0].InsertBeforeSelf<SdtBlock>(sdtBlockLegalOpinionBlock);
                                //Legal Opinion Names End
                            }
                            //--
                            List<SdtBlock> DescBlock = wordDocumentInput.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value == "DescriptionTag").ToList();

                            if (DescBlock.Count == 0)
                            {
                                //Description Tag Start
                                Run runDescriptionBlock = new Run();
                                RunProperties runPropsDescriptionBlock = new RunProperties();
                                Bold boldDescriptionBlock = new Bold();
                                runPropsDescriptionBlock.Append(boldDescriptionBlock);
                                FontSize fontSizeDescriptionBlock = new FontSize() { Val = "25" };
                                runPropsDescriptionBlock.Append(fontSizeDescriptionBlock);
                                RunFonts runFontDescriptionBlock = new RunFonts();           // Create font
                                runFontDescriptionBlock.Ascii = "Times New Roman";
                                runPropsDescriptionBlock.Append(runFontDescriptionBlock);
                                runDescriptionBlock.AppendChild(new RunProperties(runPropsDescriptionBlock));
                                runDescriptionBlock.AppendChild(new Text("Description"));
                                Paragraph paragraphDescriptionBlock = new Paragraph(runDescriptionBlock);
                                SdtProperties sdtprDescriptionBlock = new SdtProperties(
                                        new SdtAlias { Val = "DescriptionTitle" },
                                        new Tag { Val = "DescriptionTag" });

                                SdtContentBlock sdtCBlockDescriptionBlock = new SdtContentBlock(paragraphDescriptionBlock);
                                SdtBlock sdtBlockDescriptionBlock = new SdtBlock(sdtprDescriptionBlock, sdtCBlockDescriptionBlock);

                                firstSection[0].InsertBeforeSelf<SdtBlock>(sdtBlockDescriptionBlock);
                                //Description Tag End
                            }
                            List<SdtBlock> DescContentBlock = wordDocumentInput.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value == "DescriptionContentTag").ToList();

                            if (DescContentBlock.Count == 0)
                            {

                                //Description Content  Start
                                Run runDescriptionContentBlock = new Run();
                                RunProperties runPropsDescriptionContentBlock = new RunProperties();
                                FontSize fontSizeDescriptionContentBlock = new FontSize() { Val = "25" };
                                runPropsDescriptionContentBlock.Append(fontSizeDescriptionContentBlock);
                                RunFonts runFontDescriptionContentBlock = new RunFonts();           // Create font
                                runFontDescriptionContentBlock.Ascii = "Times New Roman";
                                runPropsDescriptionContentBlock.Append(runFontDescriptionContentBlock);
                                runDescriptionContentBlock.AppendChild(new RunProperties(runPropsDescriptionContentBlock));
                                runDescriptionContentBlock.AppendChild(new Text(DescriptionContent));
                                Paragraph paragraphDescriptionContentBlock = new Paragraph(runDescriptionContentBlock);
                                SdtProperties sdtprDescriptionContentBlock = new SdtProperties(
                                        new SdtAlias { Val = "DescriptionContentTitle" },
                                        new Tag { Val = "DescriptionContentTag" });

                                SdtContentBlock sdtCBlockDescriptionContentBlock = new SdtContentBlock(paragraphDescriptionContentBlock);
                                SdtBlock sdtBlockDescriptionContentBlock = new SdtBlock(sdtprDescriptionContentBlock, sdtCBlockDescriptionContentBlock);
                                firstSection[0].InsertBeforeSelf<SdtBlock>(sdtBlockDescriptionContentBlock);
                                //Description Content End

                            }
                        }
                        string opinionId = metaData.OpinionId;
                        if (opinionId == "" || opinionId == null)
                        {
                            opinionId = "EMPTY";
                        }
                        byte[] qTemplateHeader = ReadToEnd(streamQuestionTemplate);
                        byte[] qTemplateMetaDataDocType = WDSetCustomProperty(qTemplateHeader, "DocumentType", DocumentType.OPINION_SPECIFIC_QUESTION_TEMPLATE.ToString(), PropertyTypes.Text);
                        byte[] qTemplateMetaDataOpinionId = WDSetCustomProperty(qTemplateMetaDataDocType, "OpinionId", opinionId, PropertyTypes.Text);
                        byte[] qTemplateMetaDataOpinionDocVersion = WDSetCustomProperty(qTemplateMetaDataOpinionId, "OpinionDocumentVersion", 1, PropertyTypes.NumberInteger);
                        byte[] qTemplateOpinionName = WDSetCustomProperty(qTemplateMetaDataOpinionDocVersion, "OpinionName", LegalOpinion, PropertyTypes.Text);
                        byte[] qTemplateDescription = WDSetCustomProperty(qTemplateOpinionName, "OpinionDescription", DescriptionContent, PropertyTypes.Text);
                        finalqTemplate = CreateDeletedQuestionOutputOpinionTemplate(qTemplateDescription);
                    }
                    return finalqTemplate;
                }
                else
                {
                    InvalidDataException ex = new InvalidDataException();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method updates the opinion document version  ont he Opinion Question Template and returns the
        /// updated Opinion Question Template
        /// </summary>
        /// <param name="NewTemplateDocument"></param>
        /// <param name="opinionDocumentVersion"></param>
        /// <returns></returns>
        public byte[] UpdateOpinionTemplate(byte[] NewTemplateDocument, int opinionDocumentVersion)
        {
            try
            {

                byte[] updatedTemplateDocument = CreateDeletedQuestionOutputOpinionTemplate(NewTemplateDocument);

                //To Do: This method updates the opinion document version  on the Opinion Question Template and returns the
                //updated Opinion Question Template                
                byte[] updatedTemplateDocumentWithProperty = WDSetCustomProperty(updatedTemplateDocument, "OpinionDocumentVersion", opinionDocumentVersion, PropertyTypes.NumberInteger);
                return updatedTemplateDocumentWithProperty;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public byte[] GetDistributableDocument(byte[] DocumentQuestionTemplate, DocumentMetaData metaData)
        {

            Dictionary<string, string> metaDataDictionary = WDGetCustomProperty(DocumentQuestionTemplate, "OpinionId", "OpinionDocumentVersion", "OpinionName", "OpinionDescription");

            string opinionId = metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionId").Value;
            string opinionDocVersion = metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionDocumentVersion").Value;
            string opinionName = metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionName").Value;
            string opinionDescription = metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionDescription").Value;
            //Copying the "Legal Opinion Name" and "Legal Opinion description" from "Question Template"
            //to avoid mismatch
            metaData.OpinionName = opinionName;
            metaData.OpinionDescription = opinionDescription;
            byte[] DocumentAnswerTemplate = new byte[100];

            WordProcessor objWordProcessing = new WordProcessor();


            byte[] fileBytesAnswerTemplate = CreateLegalOpinionOutput(DocumentQuestionTemplate, DocumentAnswerTemplate, metaData, false);

            byte[] AnswerTemplateWithDeletedQuestions = CreateDeletedQuestionOutputAnswerTemplate(DocumentQuestionTemplate, fileBytesAnswerTemplate);
            byte[] AnswerTemplatewithOpinionId = WDSetCustomProperty(AnswerTemplateWithDeletedQuestions, "OpinionId", opinionId, PropertyTypes.Text); ;
            byte[] AnswerTemplatewithOpinionDocVersion = WDSetCustomProperty(AnswerTemplatewithOpinionId, "OpinionDocumentVersion", Convert.ToInt32(opinionDocVersion), PropertyTypes.NumberInteger);
            byte[] AnswerTemplatewithLawFirmid = WDSetCustomProperty(AnswerTemplatewithOpinionDocVersion, "LawFirmId", metaData.LawFirmId, PropertyTypes.NumberInteger);
            byte[] AnswerTemplatewithDocType = WDSetCustomProperty(AnswerTemplatewithLawFirmid, "DocumentType", DocumentType.ANSWER_TEMPLATE.ToString(), PropertyTypes.Text);

            return AnswerTemplatewithDocType;
            //return OutputDocEnableTracking(AnswerTemplatewithDocType);
        }

        public DocumentMetaData GetDocumentMetadata(byte[] documentAnsTemplate)
        {
            //need to call our original functions and return OpinionSubRecord

            DocumentMetaData _metaDataRecords = new DocumentMetaData();
            try
            {

                byte[] DocumentAnswerTemplateWithProprty = documentAnsTemplate;
                Dictionary<string, string> metaDataDictionary = WDGetCustomProperty(DocumentAnswerTemplateWithProprty,
                                                                                    "OpinionId",
                                                                                    "OpinionDocumentVersion",
                                                                                    "DocumentType", "LawFirmId",
                                                                                    "docPreparedforExecution",
                                                                                    "docExecuted");

                string opinionId = metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionId").Value;
                string opinionDocVersion =
                    metaDataDictionary.FirstOrDefault(x => x.Key == "OpinionDocumentVersion").Value;
                string documentType = metaDataDictionary.FirstOrDefault(x => x.Key == "DocumentType").Value;
                string lawFirmid = metaDataDictionary.FirstOrDefault(x => x.Key == "LawFirmId").Value;
                string isDocumentPreparedForExecution = metaDataDictionary.FirstOrDefault(x => x.Key == "docPreparedforExecution").Value;
                string isDocumentExecuted = metaDataDictionary.FirstOrDefault(x => x.Key == "docExecuted").Value;


                using (MemoryStream streamAnsTemplate = new MemoryStream())
                {
                    streamAnsTemplate.Write(documentAnsTemplate, 0, documentAnsTemplate.Length);

                    using (WordprocessingDocument outPutDoc = WordprocessingDocument.Open(streamAnsTemplate, true))
                    {
                        MainDocumentPart mainPartinputDoc = outPutDoc.MainDocumentPart;
                        DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                        Body bodyoutputDoc = documentinputDoc.Body;

                        List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();
                        int questionCompleted = 0;
                        int questionAnswered = 0;
                        int totalNumberofQuestions = 0;
                        int numOfUnchangedQuestions = 0;
                        string dateSubmitted = "";
                        string submittedBy = "";
                        string LawFirmName = "";
                        string JurisdictionName = "";
                        string opinionName = "";
                        string opinionDescription = "";

                        foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                        {

                            if (sdtoutputDoc.GetType().Name == "Table")
                            {
                                if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                                {
                                    foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                                    {
                                        foreach (TableCell tc in tr.Elements<TableCell>())
                                        {
                                            string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            string ContentInnerText = tc.InnerText;
                                            SdtContentCheckBox SCCB = tc
                                            .Descendants<SdtContentCheckBox>().FirstOrDefault();


                                            if ((ContentTagName.Contains("CheckBoxSection_") || ContentTagName.Contains("ChkBox_Del_Section_")) && (
                                                ContentInnerText.Contains("☒") || ContentInnerText.Contains("Completed")))
                                            {
                                                if (SCCB.Checked.Val == OnOffValues.One)
                                                {
                                                    questionCompleted = questionCompleted + 1;
                                                }
                                            }
                                            if (ContentTagName.Contains("_QuestionNo_") &&
                                                (!ContentTagName.Contains("CheckBox")))
                                            {
                                                totalNumberofQuestions = totalNumberofQuestions + 1;
                                            }

                                            if (ContentTagName.Contains("DateSubmittedValueTag"))
                                            {
                                                dateSubmitted = ContentInnerText;
                                            }
                                            if (ContentTagName.Contains("SubmittedByValueTag"))
                                            {
                                                submittedBy = ContentInnerText;
                                            }
                                            if (ContentTagName.Contains("FirmNameValueTag"))
                                            {
                                                LawFirmName = ContentInnerText;
                                            }
                                            if (ContentTagName.Contains("JurisdictionValueTag"))
                                            {
                                                JurisdictionName = ContentInnerText;
                                            }
                                            if (ContentTagName.Contains("LegalOpinionNameValueTag"))
                                            {
                                                opinionName = ContentInnerText;
                                            }
                                            if (ContentTagName.Contains("DescriptionValueTag"))
                                            {
                                                opinionDescription = ContentInnerText;
                                            }


                                        }
                                    }
                                }


                            }
                            if (sdtoutputDoc.GetType().Name == "SdtBlock")
                            {
                                if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                                {
                                    string ContentTagName =
                                        sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                    string ContentInnerText = sdtoutputDoc.InnerText;
                                    if (ContentTagName.Contains("_Answer_") && ContentInnerText.Length != 0 && (
                                        (ContentInnerText != "Enter new Answer") && (ContentInnerText != " Enter new Answer ") && (ContentInnerText != "")))
                                    {
                                        questionAnswered = questionAnswered + 1;
                                    }
                                    if ((ContentTagName.Contains("Section_") && ContentTagName.Contains("_Question_")) && (ContentInnerText == "" || ContentInnerText == " Enter new question "))
                                    {
                                        numOfUnchangedQuestions = numOfUnchangedQuestions + 1;
                                    }
                                }
                            }

                        }
                        if (documentType == "GENERIC_QUESTION_TEMPLATE" || documentType == "OPINION_SPECIFIC_QUESTION_TEMPLATE")
                        {
                            _metaDataRecords.NumOfQuestions = totalNumberofQuestions - (numOfUnchangedQuestions + questionCompleted);
                            _metaDataRecords.NumOfQuestionsCompleted = 0;
                        }
                        else
                        {
                            _metaDataRecords.NumOfQuestions = totalNumberofQuestions;
                            _metaDataRecords.NumOfQuestionsCompleted = questionCompleted;
                        }
                        _metaDataRecords.NumOfQuestionsAnswered = questionAnswered;

                        _metaDataRecords.LawFirmName = LawFirmName;
                        _metaDataRecords.JurisdictionName = JurisdictionName;
                        _metaDataRecords.SubmissionDate = dateSubmitted;
                        _metaDataRecords.SubmittedBy = submittedBy;
                        _metaDataRecords.DocumentType = (DocumentType)Enum.Parse(typeof(DocumentType), documentType);
                        _metaDataRecords.LawFirmId = Convert.ToInt32(lawFirmid);
                        _metaDataRecords.OpinionDocumentVersion = Convert.ToInt32(opinionDocVersion);
                        _metaDataRecords.OpinionId = opinionId;
                        _metaDataRecords.OpinionName = opinionName;
                        _metaDataRecords.OpinionDescription = opinionDescription;
                        _metaDataRecords.IsDocumentPreparedForExecution = isDocumentPreparedForExecution == "true";
                        _metaDataRecords.IsDocumentExecuted = isDocumentExecuted == "true";
                        if (_metaDataRecords.IsDocumentPreparedForExecution)
                        {
                            _metaDataRecords.NumOfQuestionsCompleted = _metaDataRecords.NumOfQuestions;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                _metaDataRecords.DocumentType = DocumentType.INVALID_DOCUMENT;
            }
            return _metaDataRecords;
        }

        public byte[] GetMergedDocumentWithAnswers(byte[] NewTemplateDocument, byte[] SubRecordDocumentWithAnswers)
        {
            //To Do: perm start and perm end needs to be removed from before and after check boxes in this document. 
            //This will restrict the law firm users from checking the complete box. 
            //The law firm user should only be able to edit the answers and nothing else.
            //If opinionDocument version in Question template and answer template are the same, then only call MarkCompletionAnswerTemplate.
            Dictionary<string, string> metaDataDictionaryQuesTemplate = WDGetCustomProperty(NewTemplateDocument, "OpinionDocumentVersion");
            string OpinionDocumentVersionQuesTemplate = metaDataDictionaryQuesTemplate.FirstOrDefault(x => x.Key == "OpinionDocumentVersion").Value;

            Dictionary<string, string> metaDataDictionaryAnsTemplate = WDGetCustomProperty(SubRecordDocumentWithAnswers, "OpinionDocumentVersion", "docPreparedforExecution");
            string OpinionDocumentVersionAnsTemplate = metaDataDictionaryAnsTemplate.FirstOrDefault(x => x.Key == "OpinionDocumentVersion").Value;
            string docPreparedforExecution = metaDataDictionaryAnsTemplate.FirstOrDefault(x => x.Key == "docPreparedforExecution").Value;

            byte[] outputDocument;
            if (docPreparedforExecution != "true")
            {
                byte[] documentWithQuestionsToBeMarkedComplete = SubRecordDocumentWithAnswers;
                if (Convert.ToInt32(OpinionDocumentVersionQuesTemplate) != Convert.ToInt32(OpinionDocumentVersionAnsTemplate))
                {
                    DocumentMetaData metaData = new DocumentMetaData();
                    byte[] fileBytesAnswerTemplate = CreateLegalOpinionOutput(NewTemplateDocument, SubRecordDocumentWithAnswers, metaData, true);

                    byte[] filebyteAfterdeletion = CreateDeletedQuestionOutputAnswerTemplate(NewTemplateDocument, fileBytesAnswerTemplate);

                    documentWithQuestionsToBeMarkedComplete = WDSetCustomProperty(filebyteAfterdeletion, "OpinionDocumentVersion", Convert.ToInt32(OpinionDocumentVersionQuesTemplate), PropertyTypes.NumberInteger);
                }
                outputDocument = MarkCompletionAnswerTemplate(documentWithQuestionsToBeMarkedComplete);
            }
            else
            {
                outputDocument = SubRecordDocumentWithAnswers;
            }
            return outputDocument;
        }

        //Function for MarkCompletion
        public byte[] MarkCompletionAnswerTemplate(byte[] LegalOpinionOutput)
        {
            WordProcessor objWordPrcessor = new WordProcessor();
            byte[] markCompleteDocument;
            int ansPermId = 0;
            using (MemoryStream streamAnswerTemplate = new MemoryStream())
            {
                streamAnswerTemplate.Write(LegalOpinionOutput, 0, LegalOpinionOutput.Length);
                using (WordprocessingDocument ansDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                {


                    MainDocumentPart mainPartansDoc = ansDoc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Document documentansDoc = mainPartansDoc.Document;
                    Body bodyansDoc = documentansDoc.Body;

                    List<OpenXmlElement> blocksansDoc = bodyansDoc.Elements<OpenXmlElement>().ToList();

                    string ansMarked = "";
                    foreach (OpenXmlElement sdtansDoc in blocksansDoc)
                    {
                        //Removing all Permissions
                        PermStart objPermStart = sdtansDoc.Descendants<PermStart>().FirstOrDefault();
                        PermEnd objPermEnd = sdtansDoc.Descendants<PermEnd>().FirstOrDefault();
                        if (objPermStart != null)
                            objPermStart.Remove();
                        if (objPermEnd != null)
                            objPermEnd.Remove();

                        //Removing permission for last row of cover page if called with Execution document
                        PermStart objPermStart1 = sdtansDoc.Descendants<PermStart>().LastOrDefault();
                        PermEnd objPermEnd1 = sdtansDoc.Descendants<PermEnd>().LastOrDefault();
                        if (objPermStart1 != null)
                            objPermStart1.Remove();
                        if (objPermEnd1 != null)
                            objPermEnd1.Remove();

                        //Selecting the mark complete questions
                        if (sdtansDoc.GetType().Name == "Table")
                        {

                            foreach (TableRow tr in sdtansDoc.Elements<TableRow>())
                            {
                                foreach (TableCell tc in tr.Elements<TableCell>())
                                {
                                    string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                    string ContentInnerText = tc.InnerText;
                                    if (ContentTagName.Contains("CheckBoxSection_") && (ContentInnerText.Contains("☒") || ContentInnerText.Contains("Completed")))
                                    {
                                        sdtansDoc.AcceptRevision();

                                        SdtBlock SPS1 = tc.GetFirstChild<SdtBlock>(); //SR.SdtProperties;
                                        SdtProperties SPS = SPS1.GetFirstChild<SdtProperties>();
                                        SdtContentCheckBox SCCB = tc
                                            .Descendants<SdtContentCheckBox>().FirstOrDefault();
                                        if (SCCB.Checked.Val == OnOffValues.One)
                                            ansMarked = "Section_" + ContentTagName.Split('_')[1] + "_Answer_" + ContentTagName.Split('_')[3];

                                        if (SCCB.Checked.Val == OnOffValues.Zero)
                                        {
                                            Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                            T.Text = " ";//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                                        }
                                        else if (SCCB.Checked.Val == OnOffValues.One)
                                        {
                                            Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                            T.Text = "Completed ✓ ";
                                        }
                                        ansDoc.MainDocumentPart.Document.Save();

                                    }
                                    //Making the unchecked boxes blank
                                    else if (ContentTagName.Contains("CheckBoxSection_") && !(ContentInnerText.Contains("☒")) && ((ContentInnerText.Contains("☐")) || (ContentInnerText.Contains("MarkComplete"))))
                                    {
                                        SdtBlock SPS1 = tc.GetFirstChild<SdtBlock>(); //SR.SdtProperties;
                                        SdtProperties SPS = SPS1.GetFirstChild<SdtProperties>();
                                        SdtContentCheckBox SCCB = tc
                                            .Descendants<SdtContentCheckBox>().FirstOrDefault();

                                        if (SCCB.Checked.Val == OnOffValues.Zero)
                                        {
                                            Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                            T.Text = " ";//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                                            tc.AcceptRevision();

                                        }
                                        ansDoc.MainDocumentPart.Document.Save();

                                    }
                                }
                            }


                        }
                        if (sdtansDoc.GetType().Name == "SdtBlock")
                        {
                            string ContentTagName = sdtansDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                            string ContentInnerText = sdtansDoc.InnerText;
                            if (ContentTagName == ansMarked)
                            {
                                sdtansDoc.AcceptRevision();

                            }
                            //giving permissions back to all unchecked Answers

                            if (ContentTagName != ansMarked && ContentTagName.Contains("Section_") && ContentTagName.Contains("_Answer_"))
                            {

                                Run objFirstRun = new Run();
                                objFirstRun.AppendChild(new Text(" "));

                                Run objLastRun = new Run();
                                objLastRun.AppendChild(new Text(" "));

                                Paragraph objFirstPara = sdtansDoc.Descendants<Paragraph>().FirstOrDefault();
                                Paragraph objLastPara = sdtansDoc.Descendants<Paragraph>().LastOrDefault();
                                #region permstart and permend

                                ansPermId = ansPermId + 1;
                                int id = ansPermId;
                                PermStart permStart = new PermStart();
                                permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                                permStart.Id = id;
                                PermEnd permEnd = new PermEnd();
                                permEnd.Id = id;

                                #endregion
                                objFirstPara.PrependChild<Run>(objFirstRun);
                                objLastPara.AppendChild<Run>(objLastRun);
                                objFirstPara.InsertAfter<PermStart>(permStart, objFirstRun);
                                objLastPara.InsertBefore<PermEnd>(permEnd, objLastRun);
                            }
                        }
                    }
                    ansDoc.MainDocumentPart.Document.Save();
                    ansDoc.Dispose();
                }

                markCompleteDocument = objWordPrcessor.ReadToEnd(streamAnswerTemplate);
            }
            return markCompleteDocument;


        }

        //Function for Marked Question Deletion in AnswerTemplate
        public byte[] CreateDeletedQuestionOutputAnswerTemplate(byte[] LegalOpinionInput, byte[] LegalOpinionOutput)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] outputDocument;
            using (MemoryStream streamQuestionTemplate = new MemoryStream())
            {
                streamQuestionTemplate.Write(LegalOpinionInput, 0, LegalOpinionInput.Length);

                using (MemoryStream streamAnswerTemplate = new MemoryStream())
                {
                    streamAnswerTemplate.Write(LegalOpinionOutput, 0, LegalOpinionOutput.Length);


                    using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamQuestionTemplate, true))
                    {
                        MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;
                        var inputDocFootNote = mainPartinputDoc.FootnotesPart;

                        DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                        Body bodyinputDoc = documentinputDoc.Body;

                        List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

                        WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true);
                        //if (inputDocFootNote != null)
                        //{
                        //    var footNotePart = outputDoc.MainDocumentPart.FootnotesPart;//.Footnotes;// 

                        //    if (footNotePart == null)
                        //        outputDoc.MainDocumentPart.AddPart<FootnotesPart>(inputDocFootNote);
                        //    else
                        //    {
                        //        footNotePart.Footnotes.InnerXml = footNotePart.Footnotes.InnerXml + inputDocFootNote.Footnotes.InnerXml;

                        //    }


                        //}
                        MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;
                        DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                        Body bodyoutputDoc = documentoutputDoc.Body;

                        List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();



                        var selectedQuestions = new List<string>();
                        var selectedQuestuionNumbers = new List<string>();
                        var selectedCheckBoxNumbers = new List<string>();
                        var selectedAnswerNumbers = new List<string>();
                        var selectedAnswerHeaderNumbers = new List<string>();

                        foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                        {
                            string SectionNumber = string.Empty;
                            string QuestionNumber = string.Empty;
                            if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {

                                if (sdtinputDoc.GetType().Name == "Table")
                                {

                                    //Selecting the questions which are marked for deletion
                                    foreach (TableRow tr in sdtinputDoc.Elements<TableRow>())
                                    {
                                        foreach (TableCell tc in tr.Elements<TableCell>())
                                        {
                                            string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            string ContentInnerText = tc.InnerText;
                                            if (ContentTagName.Contains("ChkBox_Del_Section") && ContentInnerText.Contains("☒"))
                                            {
                                                SectionNumber = ContentTagName.Split('_')[3];
                                                QuestionNumber = ContentTagName.Split('_')[5];
                                                selectedQuestions.Add("Section_" + SectionNumber + "_Question_" + QuestionNumber);
                                                selectedQuestuionNumbers.Add("Section_" + SectionNumber + "_QuestionNo_" + QuestionNumber);
                                                selectedCheckBoxNumbers.Add("CheckBoxSection_" + SectionNumber + "_QuestionNo_" + QuestionNumber);
                                                selectedAnswerNumbers.Add("Section_" + SectionNumber + "_Answer_" + QuestionNumber);
                                                selectedAnswerHeaderNumbers.Add("Section_" + SectionNumber + "_AnswerHeader_" + QuestionNumber);
                                            }
                                        }
                                    }



                                }

                                if (sdtinputDoc.GetType().Name == "SdtBlock")
                                {
                                    string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                    string ContentInnerText = sdtinputDoc.InnerText;

                                    //Selecting the Questions which are empty or nothing added
                                    if ((ContentTagName.Contains("Section_") && ContentTagName.Contains("_Question_")) && (ContentInnerText == "" || ContentInnerText == " Enter new question "))
                                    {
                                        SectionNumber = ContentTagName.Split('_')[1];
                                        QuestionNumber = ContentTagName.Split('_')[3];
                                        selectedQuestions.Add("Section_" + SectionNumber + "_Question_" + QuestionNumber);
                                        selectedQuestuionNumbers.Add("Section_" + SectionNumber + "_QuestionNo_" + QuestionNumber);
                                        selectedCheckBoxNumbers.Add("CheckBoxSection_" + SectionNumber + "_QuestionNo_" + QuestionNumber);
                                        selectedAnswerNumbers.Add("Section_" + SectionNumber + "_Answer_" + QuestionNumber);
                                        selectedAnswerHeaderNumbers.Add("Section_" + SectionNumber + "_AnswerHeader_" + QuestionNumber);
                                    }


                                }
                            }
                        }

                        selectedQuestions = selectedQuestions.Distinct().ToList();
                        selectedQuestuionNumbers = selectedQuestuionNumbers.Distinct().ToList();
                        selectedCheckBoxNumbers = selectedCheckBoxNumbers.Distinct().ToList();
                        selectedAnswerNumbers = selectedAnswerNumbers.Distinct().ToList();
                        selectedAnswerHeaderNumbers = selectedAnswerHeaderNumbers.Distinct().ToList();

                        foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                        {

                            if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {

                                if (sdtoutputDoc.GetType().Name == "Table")
                                {
                                    foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                                    {
                                        foreach (TableCell tc in tr.Elements<TableCell>())
                                        {
                                            string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            string ContentInnerText = tc.InnerText;
                                            if (selectedQuestuionNumbers.Contains(ContentTagName))
                                            {
                                                if (sdtoutputDoc.Parent != null)
                                                    sdtoutputDoc.Remove();
                                            }
                                            if (selectedCheckBoxNumbers.Contains(ContentTagName))
                                            {
                                                if (sdtoutputDoc.Parent != null)
                                                    sdtoutputDoc.Remove();
                                            }

                                        }
                                    }


                                }
                                if (sdtoutputDoc.GetType().Name == "SdtBlock")
                                {
                                    string ContentTagName = sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                    string ContentInnerText = sdtoutputDoc.InnerText;
                                    if (selectedQuestions.Contains(ContentTagName))
                                    {
                                        if (sdtoutputDoc.Parent != null)
                                            sdtoutputDoc.Remove();

                                    }
                                    if (selectedAnswerNumbers.Contains(ContentTagName))
                                    {
                                        if (sdtoutputDoc.Parent != null)
                                            sdtoutputDoc.Remove();

                                    }


                                    if (selectedAnswerHeaderNumbers.Contains(ContentTagName))
                                    {
                                        if (sdtoutputDoc.Parent != null)
                                            sdtoutputDoc.Remove();

                                    }
                                    if (ContentTagName == null || ContentTagName == "")
                                    {
                                        if (sdtoutputDoc.Parent != null)
                                            sdtoutputDoc.Remove();
                                    }

                                }

                            }
                        }

                        //Section to delete SubHeading if no question exist under that Subheading
                        foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                        {

                            if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {
                                string ContentTagName = sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                int sectionNo = 0;

                                if (ContentTagName.Contains("_"))
                                {
                                    sectionNo = Convert.ToInt16(ContentTagName.Split('_')[1]);
                                }

                                List<SdtBlock> sectionExistList = outputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + sectionNo)).ToList();
                                if (sectionExistList.Count == 1)
                                {
                                    if (sdtoutputDoc.Parent != null)
                                        sdtoutputDoc.Remove();
                                }
                            }
                        }


                        outputDoc.MainDocumentPart.Document.Save();
                        outputDoc.Dispose();
                    }
                    outputDocument = objWordProcessor.ReadToEnd(streamAnswerTemplate);
                }
                //return outputDocument;
            }
            return outputDocument;

        }

        //Function for Marked Question Deletion in Question Template
        public byte[] CreateDeletedQuestionOutputOpinionTemplate(byte[] LegalOpinionInput)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] inputDocument;
            using (MemoryStream streamQuestionTemplate = new MemoryStream())
            {
                streamQuestionTemplate.Write(LegalOpinionInput, 0, LegalOpinionInput.Length);
                using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamQuestionTemplate, true))
                {
                    MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                    Body bodyinputDoc = documentinputDoc.Body;

                    List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

                    string SectionNumber = "";
                    string QuestionNumber = "";

                    ArrayList selectedQuestions = new ArrayList();
                    //Below section to get the list of selected questions
                    foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                    {

                        if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
                        {
                            if (sdtinputDoc.GetType().Name == "Table")
                            {

                                foreach (TableRow tr in sdtinputDoc.Elements<TableRow>())
                                {
                                    foreach (TableCell tc in tr.Elements<TableCell>())
                                    {
                                        string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                        string ContentInnerText = tc.InnerText;
                                        if (ContentTagName.Contains("ChkBox_Del_Section") && ContentInnerText.Contains("☒"))
                                        {
                                            SectionNumber = ContentTagName.Split('_')[3];
                                            QuestionNumber = ContentTagName.Split('_')[5];
                                            selectedQuestions.Add(SectionNumber + "." + QuestionNumber);

                                            Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                            if (T.Text == "☒")
                                                T.Text = "Deleted ☒";
                                            else if (T.Text == "Deleted ")
                                                T.Text = "Deleted ";
                                            else
                                                T.Text = "Deleted ☒";

                                        }
                                    }
                                }

                            }



                        }
                    }

                    //--Below section to delete the selected questions
                    foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                    {

                        if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
                        {
                            string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                            foreach (string selected in selectedQuestions)
                            {
                                string[] sectionNo = selected.Split('.');
                                if (ContentTagName == ("Section_" + sectionNo[0] + "_Question_" + sectionNo[1]))
                                {

                                    foreach (SdtContentBlock sc in sdtinputDoc.Elements<SdtContentBlock>())
                                    {
                                        foreach (Paragraph p in sc.Elements<Paragraph>())
                                        {
                                            var tps = p.GetFirstChild<PermStart>();
                                            var tpe = p.GetFirstChild<PermEnd>();
                                            if (tps != null)
                                            {
                                                tps.Remove();
                                            }
                                            if (tpe != null)
                                            {
                                                tpe.Remove();
                                            }
                                        }

                                    }
                                    List<Run> runQuestion = sdtinputDoc.Descendants<Run>().ToList();


                                    //In case if Prperties are not set in the input document Questions because if user remove the properties by mistake,appending new RunProperty
                                    foreach (Run run in runQuestion)
                                    {
                                        RunProperties runQuestionProp = run.Descendants<RunProperties>().FirstOrDefault();
                                        if (runQuestionProp == null)
                                        {
                                            runQuestionProp = new RunProperties();
                                            runQuestionProp.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                            run.RunProperties = runQuestionProp;

                                        }
                                    }
                                    //Modfifying the RunProperty for existing RunProprty

                                    List<RunProperties> runQuestionPropsList = sdtinputDoc.Descendants<RunProperties>().ToList();

                                    foreach (RunProperties runQuestionProps in runQuestionPropsList)
                                    {

                                        if (runQuestionProps != null)
                                        {

                                            runQuestionProps.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                        }
                                    }

                                }
                                if (ContentTagName == ("Section_" + sectionNo[0] + "_QuestionNo_" + sectionNo[1]))
                                {

                                    if (sdtinputDoc.GetType().Name == "Table")
                                    {
                                        if (sdtinputDoc.Parent != null)
                                        {
                                            foreach (TableRow tr in sdtinputDoc.Elements<TableRow>())
                                            {
                                                foreach (TableCell tc in tr.Elements<TableCell>())
                                                {
                                                    var tps = tc.GetFirstChild<PermStart>();
                                                    var tpe = tc.GetFirstChild<PermEnd>();
                                                    if (tps != null)
                                                    {
                                                        tps.Remove();
                                                    }


                                                }
                                            }

                                            List<Run> runQuestion = sdtinputDoc.Descendants<Run>().ToList();


                                            //In case if Prperties are not set in the input document Questions because if user remove the properties by mistake,appending new RunProperty
                                            foreach (Run run in runQuestion)
                                            {
                                                RunProperties runQuestionProp = run.Descendants<RunProperties>().FirstOrDefault();
                                                if (runQuestionProp == null)
                                                {
                                                    runQuestionProp = new RunProperties();
                                                    runQuestionProp.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                                    RunFonts runFontQuestion = new RunFonts();           // Create font
                                                    runFontQuestion.Ascii = "Times New Roman";
                                                    runQuestionProp.Append(runFontQuestion);

                                                    FontSize fontSizeQuestion = new FontSize() { Val = "24" };
                                                    runQuestionProp.Append(fontSizeQuestion);

                                                    Italic italicFontQuestion = new Italic();
                                                    runQuestionProp.Append(italicFontQuestion);

                                                    run.RunProperties = runQuestionProp;

                                                }
                                            }
                                            //Modfifying the RunProperty for existing RunProprty

                                            List<RunProperties> runQuestionPropsList = sdtinputDoc.Descendants<RunProperties>().ToList();

                                            foreach (RunProperties runQuestionProps in runQuestionPropsList)
                                            {

                                                if (runQuestionProps != null)
                                                {
                                                    //light blue=0EBFE9
                                                    RunFonts runFontQuestion = new RunFonts();           // Create font
                                                    runFontQuestion.Ascii = "Times New Roman";
                                                    runQuestionProps.Append(runFontQuestion);

                                                    FontSize fontSizeQuestion = new FontSize() { Val = "24" };
                                                    runQuestionProps.Append(fontSizeQuestion);

                                                    Italic italicFont = new Italic();
                                                    runQuestionProps.Append(italicFont);
                                                    runQuestionProps.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                                }
                                            }

                                        }
                                    }

                                }

                            }


                        }
                    }


                    inputDoc.MainDocumentPart.Document.Save();

                    inputDoc.Dispose();

                }
                inputDocument = objWordProcessor.ReadToEnd(streamQuestionTemplate);
            }
            return inputDocument;


        }


        public byte[] ConvertAnswerTemplateForLegalUser(byte[] DocumentFromLawFirm)
        {
            //byte[] documentForLegalUser = new byte[100];
            //To Do: Add perm start and perm end needs to be added before and after the check boxes in this document. 
            //This will allow the legal users to check the complete box in addition to editing answers. 
            //The legal user should not be able to edit anything else.
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] AnswerTemplateForLegalUser;
            try
            {
                using (MemoryStream streamAnswerTemplate = new MemoryStream())
                {
                    streamAnswerTemplate.Write(DocumentFromLawFirm, 0, DocumentFromLawFirm.Length);
                    using (WordprocessingDocument ansDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                    {

                        MainDocumentPart mainPartansDoc = ansDoc.MainDocumentPart;
                        DocumentFormat.OpenXml.Wordprocessing.Document documentansDoc = mainPartansDoc.Document;
                        Body bodyansDoc = documentansDoc.Body;
                        List<OpenXmlElement> blocksansDoc = bodyansDoc.Elements<OpenXmlElement>().ToList();
                        int chkPermid = 1000;
                        foreach (OpenXmlElement sdtansDoc in blocksansDoc)
                        {
                            if (sdtansDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {
                                if (sdtansDoc.GetType().Name == "Table")
                                {

                                    foreach (TableRow tr in sdtansDoc.Elements<TableRow>())
                                    {
                                        foreach (TableCell tc in tr.Elements<TableCell>())
                                        {
                                            string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            string ContentInnerText = tc.InnerText;
                                            //CheckBoxSection_1_QuestionNo_2
                                            if (ContentTagName.Contains("CheckBoxSection_") && ContentTagName.Contains("_QuestionNo_"))
                                            {
                                                SdtBlock SPS1 = tc.GetFirstChild<SdtBlock>();
                                                SdtProperties SPS = SPS1.GetFirstChild<SdtProperties>();
                                                SdtContentCheckBox SCCB = tc.Descendants<SdtContentCheckBox>().FirstOrDefault();

                                                if (SCCB.Checked == null)
                                                {
                                                    SCCB.Checked = new DocumentFormat.OpenXml.Office2010.Word.Checked();
                                                    SCCB.Checked.Val = OnOffValues.Zero;
                                                    Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                                    T.Text = "MarkComplete[_]";
                                                    List<RunProperties> runPropsList = tc.Descendants<RunProperties>().ToList();

                                                    foreach (RunProperties runProps in runPropsList)
                                                    {

                                                        if (runProps != null)
                                                        {
                                                            Italic italicFont = new Italic();
                                                            runProps.Append(italicFont);
                                                            RunFonts runFont = new RunFonts();           // Create font
                                                            runFont.Ascii = "Times New Roman";
                                                            runProps.Append(runFont);
                                                        }
                                                    }

                                                }

                                                else if (SCCB.Checked.Val == OnOffValues.One)
                                                {
                                                    Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                                    T.Text = "Completed ✓ ";
                                                }
                                                else if (SCCB.Checked.Val == OnOffValues.Zero)
                                                {
                                                    Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                                    T.Text = "MarkComplete[_]";
                                                    List<RunProperties> runPropsList = tc.Descendants<RunProperties>().ToList();
                                                    foreach (RunProperties runProps in runPropsList)
                                                    {
                                                        if (runProps != null)
                                                        {
                                                            Italic italicFont = new Italic();
                                                            runProps.Append(italicFont);
                                                            RunFonts runFont = new RunFonts();// Create font
                                                            runFont.Ascii = "Times New Roman";
                                                            runProps.Append(runFont);
                                                        }
                                                    }


                                                }

                                                ansDoc.MainDocumentPart.Document.Save();

                                                Run objRun = tc.Descendants<Run>().FirstOrDefault();
                                                SdtContentRun objSdtContentRun = tc.Descendants<SdtContentRun>().FirstOrDefault();
                                                #region permstart and permend
                                                chkPermid = chkPermid + 1;
                                                int id = chkPermid;
                                                PermStart permStart = new PermStart();
                                                permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                                                permStart.Id = id;
                                                PermEnd permEnd = new PermEnd();
                                                permEnd.Id = id;

                                                #endregion

                                                // Code to make only "mark complete" checkbox section editable
                                                // But that will impact Mark Complete funtionality.
                                                objSdtContentRun.InsertBefore<PermStart>(permStart, objRun);
                                                objSdtContentRun.InsertAfter<PermEnd>(permEnd, objRun);


                                            }
                                        }
                                    }


                                }
                            }
                        }
                        ansDoc.MainDocumentPart.Document.Save();
                        ansDoc.Dispose();
                    }
                    AnswerTemplateForLegalUser = objWordProcessor.ReadToEnd(streamAnswerTemplate);

                }

                using (MemoryStream streamAnswerTemplate = new MemoryStream())
                {
                    streamAnswerTemplate.Write(AnswerTemplateForLegalUser, 0, AnswerTemplateForLegalUser.Length);
                    using (WordprocessingDocument ansDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                    {
                        //
                        var trackRevisionSetting = ansDoc.MainDocumentPart.DocumentSettingsPart.Settings.Descendants<TrackRevisions>();
                        if (!(trackRevisionSetting.Any()))
                        {
                            //trackRevisionSetting.First().Remove();
                            return OutputDocEnableTracking(AnswerTemplateForLegalUser);
                        }
                        else
                            return AnswerTemplateForLegalUser;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return AnswerTemplateForLegalUser;

        }

        //API to add Submitted by and Submitted fields but the value should be empty and editable
        public byte[] PrepareDocumentForExecution(byte[] InputDocumentForExecution)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            Dictionary<string, string> metaDataDictionary = WDGetCustomProperty(InputDocumentForExecution, "docPreparedforExecution");
            string docPrepared = metaDataDictionary.FirstOrDefault(x => x.Key == "docPreparedforExecution").Value;
            if (docPrepared == "true")
            {
                return InputDocumentForExecution;
            }
            string[] ExecutionData = new string[2];
            ExecutionData[0] = "Submitted By";
            ExecutionData[1] = "Date Submitted";
            byte[] OutputDocumentForExecution;
            string ContentTagName = "";
            bool documentPrepared = false;
            using (MemoryStream streamAnswerTemplate = new MemoryStream())
            {
                streamAnswerTemplate.Write(InputDocumentForExecution, 0, InputDocumentForExecution.Length);

                //--
                using (WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                {

                    MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;


                    DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                    Body bodyoutputDoc = documentoutputDoc.Body;

                    List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();

                    foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                    {

                        PermStart objPermStart = sdtoutputDoc.Descendants<PermStart>().FirstOrDefault();
                        PermEnd objPermEnd = sdtoutputDoc.Descendants<PermEnd>().FirstOrDefault();
                        if (objPermStart != null)
                            objPermStart.Remove();
                        if (objPermEnd != null)
                            objPermEnd.Remove();
                        if (sdtoutputDoc.GetType().Name == "Table")
                        {
                            if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {
                                ContentTagName = sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                            }

                            //--Cover Page Entry starts
                            if (ContentTagName == "LegalOpinionNameKeyTag")
                            {
                                foreach (string Data in ExecutionData)
                                {
                                    //--Entry for the Meta Data Key starts
                                    Run RunMetaData = new Run();
                                    RunProperties runPropsMetaData = new RunProperties();
                                    Bold boldMetaData = new Bold();
                                    FontSize fontSizeMetaData = new FontSize() { Val = "28" };
                                    runPropsMetaData.Append(boldMetaData);
                                    runPropsMetaData.Append(fontSizeMetaData);
                                    RunFonts runFontMetaData = new RunFonts();           // Create font
                                    runFontMetaData.Ascii = "Times New Roman";
                                    runPropsMetaData.Append(runFontMetaData);

                                    RunMetaData.AppendChild(new RunProperties(runPropsMetaData));
                                    RunMetaData.AppendChild(new Text(Data + ":"));
                                    Paragraph paragraphMetaData = new Paragraph(RunMetaData);
                                    SdtProperties sdtPrparagraphMetaData = new SdtProperties(
                                                   new SdtAlias { Val = Data.Replace(" ", "") + "KeyTitle" },
                                                   new Tag { Val = Data.Replace(" ", "") + "KeyTag" },
                                                   new Lock { Val = LockingValues.ContentLocked },
                                                   new Lock { Val = LockingValues.SdtLocked });
                                    SdtContentBlock sdtCBlockparagraphMetaData = new SdtContentBlock(paragraphMetaData);
                                    SdtBlock sdtBlockparagraphMetaData = new SdtBlock(sdtPrparagraphMetaData, sdtCBlockparagraphMetaData);
                                    //-- Entry for the Titles ends
                                    string metaDataValue = "Enter " + Data;
                                    //--Entry for Meta Data Values starts

                                    //
                                    Run runBeforeMetaDataValue = new Run();
                                    runBeforeMetaDataValue.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                                    Run runAfterMetaDataValue = new Run();
                                    runAfterMetaDataValue.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT
                                    //
                                    Run RunMetaDataValue = new Run();
                                    RunProperties runPropsMetaDataValue = new RunProperties();
                                    FontSize fontSizeMetaDataValue = new FontSize() { Val = "28" };
                                    runPropsMetaDataValue.Append(fontSizeMetaDataValue);

                                    RunFonts runFontMetaDataValue = new RunFonts();           // Create font
                                    runFontMetaDataValue.Ascii = "Times New Roman";
                                    runPropsMetaDataValue.Append(runFontMetaDataValue);

                                    RunMetaDataValue.AppendChild(new RunProperties(runPropsMetaDataValue));
                                    RunMetaDataValue.AppendChild(new Text(metaDataValue));
                                    Paragraph paragraphMetaDataValue = new Paragraph();//RunMetaDataValue);

                                    paragraphMetaDataValue.Append(runBeforeMetaDataValue);
                                    paragraphMetaDataValue.Append(RunMetaDataValue);
                                    paragraphMetaDataValue.Append(runAfterMetaDataValue);

                                    //#region permstart and permend

                                    int id = 1;
                                    PermStart permStart = new PermStart();
                                    permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                                    permStart.Id = id;
                                    PermEnd permEnd = new PermEnd();
                                    permEnd.Id = id;

                                    //#endregion
                                    paragraphMetaDataValue.InsertBefore<PermStart>(permStart, RunMetaDataValue);
                                    paragraphMetaDataValue.InsertAfter<PermEnd>(permEnd, RunMetaDataValue);

                                    SdtProperties sdtPrparagraphMetaDataValue = new SdtProperties(
                                                   new SdtAlias { Val = Data.Replace(" ", "") + "ValueTitle" },
                                                   new Tag { Val = Data.Replace(" ", "") + "ValueTag" },
                                        //new Lock { Val = LockingValues.ContentLocked },
                                                   new Lock { Val = LockingValues.SdtLocked });
                                    SdtContentBlock sdtCBlockparagraphMetaDataValue = new SdtContentBlock(paragraphMetaDataValue);

                                    SdtBlock sdtBlockparagraphMetaDataValue = new SdtBlock(sdtPrparagraphMetaDataValue, sdtCBlockparagraphMetaDataValue);

                                    //-- Entry for Meta Data Values

                                    TableRow trLegalOpinionCover = new TableRow();
                                    TableCell tcLegalOpinionCover = new TableCell();
                                    tcLegalOpinionCover.Append(sdtBlockparagraphMetaData);
                                    TableCell tcLegalOpinionNameCover = new TableCell();
                                    tcLegalOpinionNameCover.Append(sdtBlockparagraphMetaDataValue);
                                    trLegalOpinionCover.Append(tcLegalOpinionCover);
                                    trLegalOpinionCover.Append(tcLegalOpinionNameCover);
                                    sdtoutputDoc.Append(trLegalOpinionCover);

                                }
                                documentPrepared = true;

                            }
                            //Removing the CheckBox content
                            foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                            {
                                foreach (TableCell tc in tr.Elements<TableCell>())
                                {
                                    string tagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                    string innerText = tc.InnerText;
                                    if (tagName.Contains("CheckBoxSection_"))// && (innerText.Contains("☒") || innerText.Contains("Completed")))
                                    {
                                        sdtoutputDoc.AcceptRevision();

                                        SdtBlock SPS1 = tc.GetFirstChild<SdtBlock>(); //SR.SdtProperties;
                                        SdtProperties SPS = SPS1.GetFirstChild<SdtProperties>();
                                        SdtContentCheckBox SCCB = tc
                                            .Descendants<SdtContentCheckBox>().FirstOrDefault();
                                        if (SCCB.Checked != null)
                                        {
                                            if (SCCB.Checked.Val == OnOffValues.Zero)
                                            {
                                                Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                                T.Text = " ";//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                                            }
                                            else if (SCCB.Checked.Val == OnOffValues.One)
                                            {
                                                Text T = tc.GetFirstChild<SdtBlock>().Descendants<Text>().FirstOrDefault();
                                                //T.Text = "Completed ✓ ";
                                                T.Text = " ";
                                                SCCB.Checked.Val = OnOffValues.Zero;
                                            }
                                        }
                                        //Removing Water Mark
                                        foreach (HeaderPart headerPart in outputDoc.MainDocumentPart.HeaderParts)
                                        {
                                            foreach (SdtBlock sdt in headerPart.Header.Elements<SdtBlock>())
                                            {
                                                sdt.Remove();
                                            }

                                        }
                                        
                                        //
                                        outputDoc.MainDocumentPart.Document.Save();

                                    }
                                }
                            }
                        }
                    }

                    //--Cover Page Entry ends
                    outputDoc.MainDocumentPart.Document.Save();
                    outputDoc.Dispose();
                }
                byte[] OutputDocument = objWordProcessor.ReadToEnd(streamAnswerTemplate);
                OutputDocumentForExecution = WDSetCustomProperty(OutputDocument, "docPreparedforExecution", documentPrepared, PropertyTypes.YesNo);
                OutputDocumentForExecution = DisableTracking(OutputDocumentForExecution);
            }
            return OutputDocumentForExecution;

        }
        //API to make the entire document non editable
        public byte[] ExecuteDocument(byte[] InputExecuteDocument)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            Dictionary<string, string> metaDataDictionary = WDGetCustomProperty(InputExecuteDocument, "docPreparedforExecution");

            byte[] OutputExecuteDocument;
            using (MemoryStream streamAnswerTemplate = new MemoryStream())
            {
                streamAnswerTemplate.Write(InputExecuteDocument, 0, InputExecuteDocument.Length);

                //--
                using (WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                {

                    MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                    Body bodyoutputDoc = documentoutputDoc.Body;
                    List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();
                    foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                    {
                        //Removing Permission from "Submitted By"
                        PermStart objPermStart1 = sdtoutputDoc.Descendants<PermStart>().FirstOrDefault();
                        PermEnd objPermEnd1 = sdtoutputDoc.Descendants<PermEnd>().FirstOrDefault();
                        if (objPermStart1 != null)
                            objPermStart1.Remove();
                        if (objPermEnd1 != null)
                            objPermEnd1.Remove();
                        //Removing permissions from "Data Submitted"
                        PermStart objPermStart2 = sdtoutputDoc.Descendants<PermStart>().FirstOrDefault();
                        PermEnd objPermEnd2 = sdtoutputDoc.Descendants<PermEnd>().FirstOrDefault();
                        if (objPermStart2 != null)
                            objPermStart2.Remove();
                        if (objPermEnd2 != null)
                            objPermEnd2.Remove();

                    }

                }
                OutputExecuteDocument = objWordProcessor.ReadToEnd(streamAnswerTemplate);
                OutputExecuteDocument = WDSetCustomProperty(OutputExecuteDocument, "docExecuted", true, PropertyTypes.YesNo);
            }
            return OutputExecuteDocument;
        }

        //API to accept the revisions based on the the Track Changes
        public byte[] AcceptRevisions(byte[] fileName)
        {
            // Given a document , accept revisions. 
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] acceptRevisionDocument;
            using (MemoryStream streamfileName = new MemoryStream())
            {
                streamfileName.Write(fileName, 0, fileName.Length);
                using (WordprocessingDocument wdDoc = WordprocessingDocument.Open(streamfileName, true))
                {
                    Body body = wdDoc.MainDocumentPart.Document.Body;
                    body.AcceptRevision();

                    var footnotePart = wdDoc.MainDocumentPart.FootnotesPart;
                    if (footnotePart != null)
                    {
                        if (footnotePart.Footnotes != null)
                        {
                            footnotePart.Footnotes.AcceptRevision();
                        }
                    }
                    // Handle the formatting changes.                    
                }
                acceptRevisionDocument = objWordProcessor.ReadToEnd(streamfileName);
            }
            return acceptRevisionDocument;
        }

        //API for creating Comparison Report
        public byte[] CreateComparisonReportDocument(Dictionary<string, byte[]> LawFirmsDocument, List<string> questionNumbers)
        {
            byte[] ComparisonReportOutput = new byte[100];

            List<string> QuestionSectionTag = new List<string>();


            foreach (string question in questionNumbers)
            {
                QuestionSectionTag.Add(getTheQuestionTag(question));

            }


            using (MemoryStream streamOutputDoc = new MemoryStream())
            {
                streamOutputDoc.Write(ComparisonReportOutput, 0, ComparisonReportOutput.Length);
                //Calling method to create a blank document
                CreateBlankOutputDoc(streamOutputDoc, false);

                //Calling method to create the cover Page
                CreateComparisonReportCoverPage(streamOutputDoc, LawFirmsDocument);
                //
                WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamOutputDoc, true);
                outputDoc.ChangeDocumentType(WordprocessingDocumentType.Document);
                MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;
                DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                Body bodyoutputDoc = documentoutputDoc.Body;
                List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();
                int max = 1;
                int commentsMax = 0;
                List<Footnote> comparisionFootnotes = new List<Footnote>();
                List<Comment> comparisionComments = new List<Comment>();
                Dictionary<string, byte[]> updatedLawFirmsDocument = new Dictionary<string, byte[]>();
                foreach (KeyValuePair<string, byte[]> firmDoc in LawFirmsDocument)
                {
                    using (MemoryStream streamInputDoc = new MemoryStream())
                    {
                        streamInputDoc.Write(firmDoc.Value, 0, firmDoc.Value.Length);
                        using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamInputDoc, true))
                        {
                            MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;
                            
                            FootnotesPart inputDocFootNote = inputDoc.MainDocumentPart.FootnotesPart;
                            //
                            if (inputDocFootNote != null)
                            {
                                max = max + 1000;//1000 is a random number we chose assuming max 1000 foot note per document

                                var footnotereferences = (IEnumerable<FootnoteEndnoteReferenceType>)mainPartinputDoc.Document.Descendants<FootnoteReference>();
                                foreach (var footnotereference in footnotereferences)
                                {
                                    var footnotes = inputDocFootNote.Footnotes.Descendants<Footnote>();
                                    foreach (var footnote in footnotes)
                                    {
                                        var footnoteEndnote = (FootnoteEndnoteType)footnote;
                                        if (footnote.Id.ToString() == footnotereference.Id.ToString())
                                        {
                                            footnotereference.Id = max;
                                            footnote.Id = max;
                                            max++;
                                            comparisionFootnotes.Add(footnote);

                                        }
                                    }
                                }
                            }

                            
                            WordprocessingCommentsPart inputDocCommentsPart = inputDoc.MainDocumentPart.WordprocessingCommentsPart;
                            //
                            if (inputDocCommentsPart != null)
                            {
                                commentsMax = commentsMax + 1000;//1000 is a random number we chose assuming max 1000 foot note per document

                                var commentreferences = (IEnumerable<CommentReference>)mainPartinputDoc.Document.Descendants<CommentReference>();
                                var commentrangestarts = (IEnumerable<CommentRangeStart>)mainPartinputDoc.Document.Descendants<CommentRangeStart>();
                                var commentrangeends = (IEnumerable<CommentRangeEnd>)mainPartinputDoc.Document.Descendants<CommentRangeEnd>();
                                foreach (var commentreference in commentreferences)
                                {
                                    var comments = inputDocCommentsPart.Comments.Descendants<Comment>();
                                    foreach (var comment in comments)
                                    {

                                        if (comment.Id.ToString() == commentreference.Id.ToString())
                                        {
                                            var commentrangestart = commentrangestarts.Where(x => x.Id.ToString() == comment.Id.ToString()).First();
                                            var commentrangeend = commentrangeends.Where(x => x.Id.ToString() == comment.Id.ToString()).First();
                                            commentreference.Id = commentsMax.ToString();
                                            comment.Id = commentsMax.ToString();
                                            commentrangestart.Id = commentsMax.ToString();
                                            commentrangeend.Id = commentsMax.ToString();
                                            commentsMax++;
                                            comparisionComments.Add(comment);

                                        }
                                    }
                                }
                            }
                            inputDoc.MainDocumentPart.Document.Save();
                            inputDoc.Dispose();
                            updatedLawFirmsDocument.Add(firmDoc.Key, ReadToEnd(streamInputDoc));
                        }
                    }
                }

                //

                //Header Footer Section Starts
                ApplyHeader(outputDoc, "... Legal Opinion Comparison Report",false, false);
                ApplyFooter(outputDoc);
                //Header Footer Section Ends
                foreach (var item in QuestionSectionTag)
                {
                    foreach (KeyValuePair<string, byte[]> firmDoc in updatedLawFirmsDocument)
                    {
                        using (MemoryStream streamInputDoc = new MemoryStream())
                        {
                            streamInputDoc.Write(firmDoc.Value, 0, firmDoc.Value.Length);
                            WDGetCustomProperty(firmDoc.Value, "JurisdictionName");

                            Dictionary<string, string> customPropsDictionary = WDGetCustomProperty(firmDoc.Value, "LegalOpinionName", "LegalOpinionDesc", "LawFirmName", "JurisdictionName");

                            string juridictionName = customPropsDictionary.FirstOrDefault(x => x.Key == "JurisdictionName").Value;

                            using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamInputDoc, true))
                            {
                                MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;

                                DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                                Body bodyinputDoc = documentinputDoc.Body;

                                List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

                                

                                foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                                {

                                    //Removing existing Permissions
                                    PermStart objPermStart = sdtinputDoc.Descendants<PermStart>().FirstOrDefault();
                                    PermEnd objPermEnd = sdtinputDoc.Descendants<PermEnd>().FirstOrDefault();
                                    if (objPermStart != null)
                                        objPermStart.Remove();
                                    if (objPermEnd != null)
                                        objPermEnd.Remove();

                                    if (sdtinputDoc.GetType().Name == "SdtBlock")
                                    {
                                        //Question Entry starts
                                        if (sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == item)
                                        {

                                            string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            int sectionNo = 0;
                                            int questionNo = 0;
                                            if (ContentTagName.Contains("_"))
                                            {
                                                sectionNo = Convert.ToInt16(ContentTagName.Split('_')[1]);
                                                questionNo = Convert.ToInt16(ContentTagName.Split('_')[3]);
                                            }
                                            int GetSectionNo = sectionNo;// +1;
                                            List<SdtBlock> sectionExistList = outputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value == ("Section_" + GetSectionNo + "_Question_" + questionNo)).ToList();
                                            //Question Number Entry Section
                                            Run runQuestionHeader = new Run();

                                            RunProperties runPropsQuestionHeader = new RunProperties();

                                            RunFonts runFontQuestionHeader = new RunFonts();           // Create font
                                            runFontQuestionHeader.Ascii = "Times New Roman";
                                            runPropsQuestionHeader.Append(runFontQuestionHeader);

                                            FontSize fontSizeQuestionHeader = new FontSize() { Val = "24" };
                                            runPropsQuestionHeader.Append(fontSizeQuestionHeader);

                                            Italic italicQuestionHeader = new Italic();
                                            runPropsQuestionHeader.Append(italicQuestionHeader);

                                            Underline ulQuestionHeader = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };
                                            runPropsQuestionHeader.Append(ulQuestionHeader);

                                            Bold boldQuestionHeader = new Bold();
                                            runPropsQuestionHeader.Append(boldQuestionHeader);

                                            runQuestionHeader.AppendChild(new RunProperties(runPropsQuestionHeader));
                                            runQuestionHeader.AppendChild(new Text("Question :" + sectionNo + "." + questionNo));
                                            Paragraph paragraphQuestionHeader = new Paragraph(runQuestionHeader);
                                            SdtProperties sdtPrQuestionHeader = new SdtProperties(
                                                    new SdtAlias { Val = "Section_" + sectionNo + "_QuestionNo_" + questionNo },
                                                    new Tag { Val = "Section_" + sectionNo + "_QuestionNo_" + questionNo },
                                                    new Lock { Val = LockingValues.SdtLocked });
                                            SdtContentBlock sdtCBlockQuestionHeader = new SdtContentBlock(paragraphQuestionHeader);
                                            SdtBlock sdtBlockQuestionHeader = new SdtBlock(sdtPrQuestionHeader, sdtCBlockQuestionHeader);

                                            //

                                            if (sectionExistList.Count == 0)
                                            {
                                                bodyoutputDoc.Append(sdtBlockQuestionHeader);

                                                //
                                                List<Run> runQuestion = sdtinputDoc.Descendants<Run>().ToList();

                                                #region italic and blue color
                                                //In case if Prperties are not set in the input document Questions because if user remove the properties by mistake,appending new RunProperty
                                                foreach (Run run in runQuestion)
                                                {
                                                    RunProperties runQuestionProp = run.Descendants<RunProperties>().FirstOrDefault();
                                                    if (runQuestionProp == null)
                                                    {
                                                        runQuestionProp = new RunProperties();
                                                        runQuestionProp.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                                        RunFonts runFontQuestion = new RunFonts();           // Create font
                                                        runFontQuestion.Ascii = "Times New Roman";
                                                        runQuestionProp.Append(runFontQuestion);

                                                        FontSize fontSizeQuestion = new FontSize() { Val = "24" };
                                                        runQuestionProp.Append(fontSizeQuestion);

                                                        Italic italicFontQuestion = new Italic();
                                                        runQuestionProp.Append(italicFontQuestion);

                                                        //runQuestion.AppendChild(new RunProperties(runQuestionProps));
                                                        run.RunProperties = runQuestionProp;

                                                    }
                                                }
                                                //Modfifying the RunProperty for existing RunProprty
                                                List<RunProperties> runQuestionPropsList = sdtinputDoc.Descendants<RunProperties>().ToList();

                                                foreach (RunProperties runQuestionProps in runQuestionPropsList)
                                                {

                                                    if (runQuestionProps != null)
                                                    {
                                                        //light blue=0EBFE9
                                                        Italic italicFont = new Italic();
                                                        runQuestionProps.Append(italicFont);
                                                        runQuestionProps.Color = new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "0070C0" };

                                                    }
                                                }
                                                #endregion
                                                bodyoutputDoc.Append((OpenXmlElement)sdtinputDoc.Clone());
                                            }


                                        }

                                        //Question Entry Ends
                                        //section_1_question_1                                        
                                        string answerTag = "Section_" + item.Split('_')[1] + "_Answer_" + item.Split('_')[3];
                                        if (sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == answerTag && sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
                                        {

                                            string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            int sectionNo = 0;
                                            int questionNo = 0;
                                            if (ContentTagName.Contains("_"))
                                            {
                                                sectionNo = Convert.ToInt16(ContentTagName.Split('_')[1]);
                                                questionNo = Convert.ToInt16(ContentTagName.Split('_')[3]);
                                            }
                                            int GetSectionNo = sectionNo + 1;
                                            List<SdtBlock> sectionExistList = outputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + GetSectionNo)).ToList();
                                            //LawFirm
                                            Run runLawFirm = new Run();

                                            RunProperties runPropsLawFirm = new RunProperties();

                                            RunFonts runFontLawFirm = new RunFonts();           // Create font
                                            runFontLawFirm.Ascii = "Times New Roman";
                                            runPropsLawFirm.Append(runFontLawFirm);

                                            FontSize fontSizeLawFirm = new FontSize() { Val = "27" };
                                            runPropsLawFirm.Append(fontSizeLawFirm);

                                            Bold boldLawFirm = new Bold();
                                            runPropsLawFirm.Append(boldLawFirm);

                                            runLawFirm.AppendChild(new RunProperties(runPropsLawFirm));
                                            runLawFirm.AppendChild(new Text(firmDoc.Key + "-" + juridictionName));
                                            Paragraph paragraphLawFirm = new Paragraph(runLawFirm);
                                            SdtProperties sdtPrLawFirm = new SdtProperties(
                                                    new SdtAlias { Val = "Section_" + sectionNo + "_Answer_" + questionNo + "_From_" + firmDoc.Key },
                                                    new Tag { Val = "Section_" + sectionNo + "_Answer_" + questionNo },
                                                    new Lock { Val = LockingValues.SdtLocked });
                                            SdtContentBlock sdtCBlockLawFirm = new SdtContentBlock(paragraphLawFirm);
                                            SdtBlock sdtBlockLawFirm = new SdtBlock(sdtPrLawFirm, sdtCBlockLawFirm);
                                            //

                                            //AnswerHeader Section Starts
                                            Run runAnswerHeader = new Run();

                                            RunProperties runPropsAnswerHeader = new RunProperties();

                                            RunFonts runFontAnswerHeader = new RunFonts();           // Create font
                                            runFontAnswerHeader.Ascii = "Times New Roman";
                                            runPropsAnswerHeader.Append(runFontAnswerHeader);

                                            FontSize fontSizeAnswerHeader = new FontSize() { Val = "24" };
                                            runPropsAnswerHeader.Append(fontSizeAnswerHeader);

                                            Italic italicAnswerHeader = new Italic();
                                            runPropsAnswerHeader.Append(italicAnswerHeader);

                                            Underline ulAnswerHeader = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };
                                            runPropsAnswerHeader.Append(ulAnswerHeader);

                                            Bold boldAnswerHeader = new Bold();
                                            runPropsAnswerHeader.Append(boldAnswerHeader);

                                            runAnswerHeader.AppendChild(new RunProperties(runPropsAnswerHeader));
                                            runAnswerHeader.AppendChild(new Text("Answer to Question :" + sectionNo + "." + questionNo));
                                            Paragraph paragraphAnswerHeader = new Paragraph(runAnswerHeader);
                                            SdtProperties sdtPrAnswerHeader = new SdtProperties(
                                                    new SdtAlias { Val = "Section_" + sectionNo + "_AnswerNo_" + questionNo + "_From_" + firmDoc.Key },
                                                    new Tag { Val = "Section_" + sectionNo + "_AnswerNo_" + questionNo },
                                                    new Lock { Val = LockingValues.SdtLocked });
                                            SdtContentBlock sdtCBlockAnswerHeader = new SdtContentBlock(paragraphAnswerHeader);
                                            SdtBlock sdtBlockAnswerHeader = new SdtBlock(sdtPrAnswerHeader, sdtCBlockAnswerHeader);

                                            //AnswerHeader Section Ends 
                                            if (sectionExistList.Count == 0)
                                            {
                                                bodyoutputDoc.Append(sdtBlockLawFirm);
                                                bodyoutputDoc.Append(sdtBlockAnswerHeader);
                                                bodyoutputDoc.Append((OpenXmlElement)sdtinputDoc.Clone());
                                            }
                                            else
                                            {
                                                sectionExistList[0].InsertBeforeSelf<OpenXmlElement>(sdtBlockLawFirm);
                                                sectionExistList[0].InsertBeforeSelf<OpenXmlElement>(sdtBlockAnswerHeader);
                                                sectionExistList[0].InsertBeforeSelf<OpenXmlElement>((OpenXmlElement)sdtinputDoc.Clone());
                                            }

                                        }

                                    }
                                }



                                var inputStyleDefinitions = inputDoc.MainDocumentPart.StyleDefinitionsPart;
                                var inputStyleWithEffects = inputDoc.MainDocumentPart.StylesWithEffectsPart;
                                var outputStyleDefinitions = outputDoc.MainDocumentPart.StyleDefinitionsPart;
                                var outputStyleWithEffects = outputDoc.MainDocumentPart.StylesWithEffectsPart;

                                if (inputStyleDefinitions != null)
                                {
                                    if (outputDoc.MainDocumentPart.StyleDefinitionsPart == null)
                                    {
                                        outputDoc.MainDocumentPart.AddPart<StyleDefinitionsPart>(inputStyleDefinitions);
                                    }
                                    else
                                    {
                                        var inputStyles = inputStyleDefinitions.Styles.Descendants<Style>();
                                        var outputStyles = outputStyleDefinitions.Styles.Descendants<Style>();
                                        var stylesToBeAdded = inputStyles.Where(s => !outputStyles.Any(os => os.StyleId.ToString() == s.StyleId.ToString()));
                                        foreach (var style in stylesToBeAdded)
                                        {
                                            var clonedStyle = (Style)style.CloneNode(true);
                                            outputStyleDefinitions.Styles.AppendChild<Style>(clonedStyle);
                                        }

                                    }
                                }

                                if (outputStyleWithEffects != null)
                                {
                                    if (outputDoc.MainDocumentPart.StylesWithEffectsPart == null)
                                    {
                                        outputDoc.MainDocumentPart.AddPart<StylesWithEffectsPart>(inputStyleWithEffects);
                                    }
                                    else
                                    {
                                        var inputStyles = inputStyleWithEffects.Styles.Descendants<Style>();
                                        var outputStyles = outputStyleWithEffects.Styles.Descendants<Style>();
                                        var stylesToBeAdded = inputStyles.Where(s => !outputStyles.Any(os => os.StyleId.ToString() == s.StyleId.ToString()));
                                        foreach (var style in stylesToBeAdded)
                                        {
                                            var clonedStyle = (Style)style.CloneNode(true);
                                            outputStyleWithEffects.Styles.AppendChild<Style>(clonedStyle);
                                        }

                                    }
                                }                               
                                outputDoc.MainDocumentPart.Document.Save();

                            }
                        }
                    }

                }
                outputDoc.MainDocumentPart.AddNewPart<FootnotesPart>();//inputDocFootNote);
                outputDoc.MainDocumentPart.FootnotesPart.Footnotes = new Footnotes();
                foreach (var footnote in comparisionFootnotes)
                {
                    var clonedFootnote = (Footnote)footnote.CloneNode(true);
                    outputDoc.MainDocumentPart.FootnotesPart.Footnotes.AppendChild<Footnote>(clonedFootnote);
                }

                outputDoc.MainDocumentPart.AddNewPart<WordprocessingCommentsPart>();//inputDocFootNote);
                outputDoc.MainDocumentPart.WordprocessingCommentsPart.Comments = new Comments();
                foreach (var comment in comparisionComments)
                {
                    var clonedComment = (Comment)comment.CloneNode(true);
                    outputDoc.MainDocumentPart.WordprocessingCommentsPart.Comments.AppendChild<Comment>(clonedComment);
                }

                outputDoc.MainDocumentPart.Document.Save();
                outputDoc.Dispose();
                ComparisonReportOutput = ReadToEnd(streamOutputDoc);
            }


            return ComparisonReportOutput;
        }

        //API to revert back the Execute Document to Distributed Document
        public byte[] CreateRevertedExecuteDocument(byte[] ExecutedDocument)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] OutputDocumentForExecution;
            string ContentTagName = "";
            bool documentPrepared = false;
            using (MemoryStream streamAnswerTemplate = new MemoryStream())
            {
                streamAnswerTemplate.Write(ExecutedDocument, 0, ExecutedDocument.Length);

                using (WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true))
                {

                    MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;


                    DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                    Body bodyoutputDoc = documentoutputDoc.Body;

                    List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();

                    foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                    {
                        if (sdtoutputDoc.GetType().Name == "Table")

                            if (sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {
                                ContentTagName = sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                if (ContentTagName == "LegalOpinionNameKeyTag")
                                {
                                    foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                                    {
                                        //SubmittedByKeyTag
                                        //DateSubmittedKeyTag
                                        ContentTagName = tr.Descendants<Tag>().FirstOrDefault().Val.Value;
                                        if (ContentTagName.Contains("SubmittedBy"))
                                            tr.Remove();
                                    }
                                    foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                                    {
                                        //SubmittedByKeyTag
                                        //DateSubmittedKeyTag
                                        ContentTagName = tr.Descendants<Tag>().FirstOrDefault().Val.Value;
                                        if (ContentTagName.Contains("DateSubmitted"))
                                            tr.Remove();
                                    }
                                }

                            }

                    }

                    //--Cover Page Entry ends

                    //Adding WaterMarks back to the document
                    WaterMark objWaterMark = new WaterMark();
                    objWaterMark.AddWatermark(outputDoc);
                    //

                    outputDoc.MainDocumentPart.Document.Save();
                    outputDoc.Dispose();
                }
                byte[] OutputDocument = objWordProcessor.ReadToEnd(streamAnswerTemplate);
                documentPrepared = false;
                OutputDocumentForExecution = WDSetCustomProperty(OutputDocument, "docPreparedforExecution", documentPrepared, PropertyTypes.YesNo);
                OutputDocumentForExecution = WDSetCustomProperty(OutputDocumentForExecution, "docExecuted", false, PropertyTypes.YesNo);
                //OutputDocumentForExecution = OutputDocEnableTracking(OutputDocumentForExecution);
                OutputDocumentForExecution = MarkCompletionAnswerTemplate(OutputDocumentForExecution);
                OutputDocumentForExecution = ConvertAnswerTemplateForLegalUser(OutputDocumentForExecution);
            }
            return OutputDocumentForExecution;

        }

        //API to modify the cover PAGE
        public byte[] ModifyCoverPage(byte[] LegalOpinionOutput, DocumentMetaData metaData)
        {
            WordProcessor objWordProc = new WordProcessor();
            byte[] outPutDocument;
            Dictionary<string, string> metaDataDictionary = WDGetCustomProperty(LegalOpinionOutput,
                                                                                "docPreparedforExecution");

            bool isDocumentPreparedForExecution = metaDataDictionary.FirstOrDefault(x => x.Key == "docPreparedforExecution").Value == "true";
            
            using (MemoryStream streamAnswerTemplate = new MemoryStream())
            {
                streamAnswerTemplate.Write(LegalOpinionOutput, 0, LegalOpinionOutput.Length);
                WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true);
                MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;
                DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                Body bodyoutputDoc = documentoutputDoc.Body;

                List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();

                ApplyHeader(outputDoc, (metaData.LawFirmName + "/" + metaData.OpinionName),true, !isDocumentPreparedForExecution);

                foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                {
                    if (sdtoutputDoc.GetType().Name == "Table")
                    {
                        foreach (TableRow tr in sdtoutputDoc.Elements<TableRow>())
                        {
                            foreach (TableCell tc in tr.Elements<TableCell>())
                            {
                                Tag coverContentTag = tc.Descendants<Tag>().FirstOrDefault();
                                string tagString = coverContentTag.Val.Value;
                                Text existingText = tc.Descendants<Text>().FirstOrDefault();
                                List<Text> existingTextList = tc.Descendants<Text>().ToList();

                                if (tagString == "LegalOpinionNameValueTag" && metaData.OpinionName != "" && metaData.OpinionName != null)
                                {
                                    int i = 0;
                                    //[0] position has the title "Legal Opinion Name :", so starting from [1]
                                    for (i = 0; i < existingTextList.Count; i++)
                                    {
                                        existingText.Text = existingText.Text + existingTextList[i].Text;
                                        existingTextList[i].Text = "";
                                    }
                                    existingText.Text = metaData.OpinionName;
                                }
                                if (tagString == "DescriptionValueTag" && metaData.OpinionDescription != "" && metaData.OpinionDescription != null)
                                {
                                    int i = 0;
                                    //[0] position has the title "Description :", so starting from [1]
                                    for (i = 0; i < existingTextList.Count; i++)
                                    {
                                        existingText.Text = existingText.Text + existingTextList[i].Text;
                                        existingTextList[i].Text = "";
                                    }

                                    existingText.Text = metaData.OpinionDescription;
                                }
                                if (tagString == "JurisdictionValueTag" && metaData.JurisdictionName != "" && metaData.JurisdictionName != null)
                                {
                                    int i = 0;
                                    //[0] position has the title "Jurisdiction Name :", so starting from [1]
                                    for (i = 0; i < existingTextList.Count; i++)
                                    {
                                        existingText.Text = existingText.Text + existingTextList[i].Text;
                                        existingTextList[i].Text = "";
                                    }
                                    existingText.Text = metaData.JurisdictionName;
                                }
                                if (tagString == "FirmNameValueTag" && metaData.LawFirmName != "" && metaData.LawFirmName != null)
                                {
                                    int i = 0;
                                    //[0] position has the title "LAW FIRM Name :", so starting from [1]
                                    for (i = 0; i < existingTextList.Count; i++)
                                    {
                                        existingText.Text = existingText.Text + existingTextList[i].Text;
                                        existingTextList[i].Text = "";
                                    }
                                    existingText.Text = metaData.LawFirmName;
                                }
                                if (tagString == "SubmittedByValueTag" && metaData.SubmittedBy != "" && metaData.SubmittedBy != null)
                                {
                                    //Due to PermStart/PermEnd the SubmittedBy Value is in index no 2 because we have 3 Run where 1st 
                                    //and 3rd has "" and 2nd has the actual value which need to be modified
                                    Text existingText1 = tc.Descendants<Text>().ElementAtOrDefault<Text>(1);
                                    //existingText1.Text = metaData.SubmittedBy;
                                    int i = 0;
                                    //[0] position has the title "Submitted By :", so starting from [1]
                                    for (i = 1; i < (existingTextList.Count - 1); i++)
                                    {
                                        existingTextList[i].Text = "";
                                    }

                                    existingText1.Text = metaData.SubmittedBy;

                                }
                                if (tagString == "DateSubmittedValueTag" && metaData.SubmissionDate != "" && metaData.SubmissionDate != null)
                                {
                                    //Due to PermStart/PermEnd the Date Submitted Value is in index no 2 because we have 3 Run where 1st 
                                    //and 3rd has "" and 2nd has the actual value which need to be modified
                                    Text existingText1 = tc.Descendants<Text>().ElementAtOrDefault<Text>(1);
                                    int i = 0;
                                    //[0] position has the title "Date Submitted :", so starting from [1]
                                    for (i = 1; i < (existingTextList.Count - 1); i++)
                                    {
                                        existingTextList[i].Text = "";
                                    }

                                    existingText1.Text = metaData.SubmissionDate;

                                }
                            }
                        }


                    }
                }
                //ApplyHeader(outputDoc, (metaData.LawFirmName + "/" + metaData.OpinionName));
                outputDoc.MainDocumentPart.Document.Save();
                outputDoc.Dispose();

                outPutDocument = objWordProc.ReadToEnd(streamAnswerTemplate);
            }
            return outPutDocument;
        }

        //
        //API to get List of all the SubheadingName and corresponding question numbers present in the document

        public Dictionary<string, List<string>> getQuestionNumbersFromDoc(byte[] documentContents)
        {
            Dictionary<string, List<string>> quesDictionary = new Dictionary<string, List<string>>();
            using (MemoryStream streamInputDoc = new MemoryStream())
            {
                streamInputDoc.Write(documentContents, 0, documentContents.Length);
                using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamInputDoc, true))
                {
                    MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;
                    List<SdtBlock> sectionList = mainPartinputDoc.Document.Body.Descendants<SdtBlock>().Where(r => (r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_") && r.SdtProperties.GetFirstChild<Tag>().Val.Value.Split('_').Length == 2)).ToList();
                    foreach (SdtBlock section in sectionList)
                    {
                        string sectionTagName = section.Descendants<Tag>().FirstOrDefault().Val.Value;
                        List<TableCell> questionListAlongWithCheckbox = mainPartinputDoc.Document.Body.Descendants<TableCell>().ToList().Where(x => x.Descendants<Tag>().FirstOrDefault().Val.Value.StartsWith(sectionTagName + "_") || x.Descendants<Tag>().FirstOrDefault().Val.Value.StartsWith("ChkBox_Del_" + sectionTagName + "_")).ToList();
                        List<string> questionList = new List<string>();
                        foreach (TableCell question in questionListAlongWithCheckbox)
                        {
                            string questionTagName = question.Descendants<Tag>().FirstOrDefault().Val.Value;
                            List<SdtBlock> ActualQuestionInfoBasedOnQuestionNo = mainPartinputDoc.Document.Body.Descendants<SdtBlock>().Where(r => (r.SdtProperties.GetFirstChild<Tag>().Val.Value == questionTagName.Replace("QuestionNo", "Question"))).ToList();
                            List<SdtBlock> ActualQuestionInfoBasedOnChkBoxDel = mainPartinputDoc.Document.Body.Descendants<SdtBlock>().Where(r => (r.SdtProperties.GetFirstChild<Tag>().Val.Value == questionTagName.Replace("ChkBox_Del_", ""))).ToList();
                            if (!(ActualQuestionInfoBasedOnQuestionNo[0].InnerText.Trim() == "" || ActualQuestionInfoBasedOnQuestionNo[0].InnerText == " Enter new question ") && questionTagName.Contains("ChkBox_Del_Section") && !(question.InnerText.Contains("☒")))
                            {
                                if (!(ActualQuestionInfoBasedOnChkBoxDel[0].InnerText.Trim() == "" || ActualQuestionInfoBasedOnChkBoxDel[0].InnerText == " Enter new question "))
                                {
                                    string questioNumber = questionListAlongWithCheckbox.Where(x => x.Descendants<Tag>().FirstOrDefault().Val.Value.StartsWith(questionTagName.Replace("ChkBox_Del_", "").Replace("Question", "QuestionNo"))).ToList()[0].InnerText;
                                    questionList.Add(questioNumber);
                                }
                            }
                        }
                        if (questionList.Count > 0)
                        {
                            quesDictionary.Add(sectionTagName.Split('_')[1] + ":" + section.InnerText, questionList);
                        }
                    }
                }
            }
            return quesDictionary;
        }

       
        //API to get List of all the question numbers present in the document
        //public List<GetQuestions> getQuestionNumbersFromDocWithHTML(byte[] documentContents)
        //{

        //    Dictionary<string, string> quesDictionary = new Dictionary<string, string>();

        //    List<string> validQuestions = new List<string>();
        //    string subHeadingName = "";

        //    //lstQuestionNumbers = {String subHeadingText;Dictionary<string, string> questions;}
        //    GetQuestions objQuestions = new GetQuestions();

        //    List<GetQuestions> objQuestionList = new List<GetQuestions>();
        //    using (MemoryStream streamInputDoc = new MemoryStream())
        //    {
        //        streamInputDoc.Write(documentContents, 0, documentContents.Length);
        //        using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamInputDoc, true))
        //        {
        //            MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;

        //            DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
        //            Body bodyinputDoc = documentinputDoc.Body;
        //            List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

        //            string sectionNumber = "";
        //            string questionNumber = "";

        //            //Listing the questions except the deleted ones
        //            foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
        //            {
        //                if (sdtinputDoc.GetType().Name == "Table")
        //                {

        //                    foreach (TableRow tr in sdtinputDoc.Elements<TableRow>())
        //                    {
        //                        foreach (TableCell tc in tr.Elements<TableCell>())
        //                        {
        //                            string ContentTagName = tc.Descendants<Tag>().FirstOrDefault().Val.Value;
        //                            string ContentInnerText = tc.InnerText;
        //                            if (ContentTagName.Contains("ChkBox_Del_Section") && !(ContentInnerText.Contains("☒")))
        //                            {
        //                                sectionNumber = ContentTagName.Split('_')[3];
        //                                questionNumber = ContentTagName.Split('_')[5];
        //                                validQuestions.Add(sectionNumber + "." + questionNumber);

        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
        //            {

        //                foreach (string validQ in validQuestions)
        //                {
        //                    string validSectionNo = validQ.Split('.')[0];
        //                    string validQuestionNo = validQ.Split('.')[1];
        //                    if (sdtinputDoc.GetType().Name == "SdtBlock")
        //                    {
        //                        string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
        //                        string ContentInnerText = sdtinputDoc.InnerText;
        //                        if ((ContentTagName.Contains("Section_" + validSectionNo)))
        //                        {
        //                            if ((ContentTagName.Contains("Section_" + validSectionNo)) && (ContentTagName.Split('_').Length == 2))
        //                            {
        //                                subHeadingName = validSectionNo + ":" + ContentInnerText;
        //                                break;


        //                            }
        //                            objQuestionList.Add(new GetQuestions { subHeadingText = subHeadingName, questions = new Dictionary<string, string>() });

        //                        }

        //                    }

        //                }



        //            }
        //            foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
        //            {
        //                if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
        //                {
        //                    string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
        //                    string ContentInnerText = sdtinputDoc.InnerText;
        //                    if (sdtinputDoc.GetType().Name == "SdtBlock")
        //                    {
        //                        foreach (string validQ in validQuestions)
        //                        {
        //                            string validSectionNo = validQ.Split('.')[0];
        //                            string validQuestionNo = validQ.Split('.')[1];

        //                            if ((ContentTagName.Contains("Section_" + validSectionNo)) && (ContentTagName.Contains("_Question_" + validQuestionNo)))
        //                            {
        //                                //string htmlQuestion = GetHTMLforQuestion(sdtinputDoc);
        //                                quesDictionary.Add(("Question " + validQ), ContentInnerText);

        //                            }

        //                        }
        //                    }

        //                }

        //            }

        //            foreach (var item in objQuestionList)
        //            {
        //                string subHedaingNo = item.subHeadingText.Split(':')[0];
        //                foreach (var dicItem in quesDictionary)
        //                {
        //                    if (dicItem.Key.Split(' ')[1].Split('.')[0] == subHedaingNo)
        //                    {
        //                        item.questions[dicItem.Key] = dicItem.Value;

        //                    }
        //                }
        //            }

        //        }
        //    }

        //    List<GetQuestions> removeMe = new List<GetQuestions>();
        //    string key = "";
        //    foreach (var item in objQuestionList)
        //    {
        //        if (item.subHeadingText == key)
        //        {
        //            removeMe.Add(item);
        //        }
        //        key = item.subHeadingText;
        //    }

        //    foreach (var item in removeMe)
        //    {
        //        objQuestionList.Remove(item);
        //    }
        //    removeMe.Clear();
        //    return objQuestionList.Distinct<GetQuestions>().ToList();

        //}

        //public string GetHTMLforQuestion(OpenXmlElement sdtinputDoc)
        //{
        //    string HTMLforQuestion = "";
        //    byte[] eachQuestionDoc = new byte[100];
        //    byte[] output;
        //    using (MemoryStream streamFile = new MemoryStream())
        //    {
        //        streamFile.Write(eachQuestionDoc, 0, eachQuestionDoc.Length);
        //        CreateBlankOutputDoc(streamFile, true);
        //        using (WordprocessingDocument workingDoc = WordprocessingDocument.Open(streamFile, true))
        //        {

        //            MainDocumentPart mainPartinputDoc = workingDoc.MainDocumentPart;

        //            DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
        //            Body bodyinputDoc = documentinputDoc.Body;

        //            bodyinputDoc.Append((OpenXmlElement)sdtinputDoc.Clone());
        //            mainPartinputDoc.Document.Save();
        //        }
        //        output = ReadToEnd(streamFile);
        //    }
        //    HTMLforQuestion = GetHTMLFromDoc(output);
        //    return HTMLforQuestion;
        //}
        #endregion


        #region Private Methods



        //Functions not included so far
        public byte[] ReorderQuestionNumbers(byte[] LegalOpinionDocument)
        {

            using (MemoryStream streamFile = new MemoryStream())
            {
                streamFile.Write(LegalOpinionDocument, 0, LegalOpinionDocument.Length);
                using (WordprocessingDocument workingDoc = WordprocessingDocument.Open(streamFile, true))
                {

                    MainDocumentPart mainPartinputDoc = workingDoc.MainDocumentPart;

                    DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                    Body bodyinputDoc = documentinputDoc.Body;

                    List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

                    List<SdtBlock> sectionExistList = workingDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => (r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_") && r.SdtProperties.GetFirstChild<Tag>().Val.Value.Split('_').Length == 2)).ToList();

                    for (int i = 0; i < sectionExistList.Count; i++)
                    {
                        string SectionTag = sectionExistList[i].Descendants<Tag>().FirstOrDefault().Val.Value;
                        int SectionNumber = Convert.ToInt32(SectionTag.Split('_')[1]);
                        List<SdtBlock> questionExistList = workingDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => (r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + SectionNumber) && r.SdtProperties.GetFirstChild<Tag>().Val.Value.Contains("_QuestionNo_"))).ToList();
                        for (int j = 0; j < questionExistList.Count; j++)
                        {
                            string questionTag = questionExistList[j].Descendants<Tag>().FirstOrDefault().Val.Value;
                            int questionNumber = Convert.ToInt32(questionTag.Split('_')[3]);
                            if (questionExistList[j].Descendants<Tag>().FirstOrDefault().Val.Value == "Section_" + SectionNumber + "_QuestionNo_" + questionNumber)
                            {
                                List<Text> textList = workingDoc.MainDocumentPart.Document.Body.Descendants<Text>().ToList();
                                foreach (Text T in textList)
                                {
                                    if (T.Text == "Question " + SectionNumber + "." + questionNumber)
                                        T.Text = "Question " + (i + 1) + "." + (j + 1);
                                }
                            }
                        }


                    }
                    mainPartinputDoc.Document.Save();
                }
                LegalOpinionDocument = ReadToEnd(streamFile);
            }
            return LegalOpinionDocument;
        }
        //Function to correct the document if user do a copy past of the entire content control
        //this function is not in use so far
        public byte[] DocumentCorrection(byte[] LegalOpinionDocument)
        {
            byte[] correctedOutput = new byte[100];
            using (MemoryStream streamFile = new MemoryStream())
            {
                streamFile.Write(LegalOpinionDocument, 0, LegalOpinionDocument.Length);
                using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamFile, true))
                {
                    MainDocumentPart mainDoc = inputDoc.MainDocumentPart;
                    DocumentFormat.OpenXml.Wordprocessing.Document document = mainDoc.Document;
                    Body bodyInputDoc = document.Body;
                    List<OpenXmlElement> blocksInputDoc = bodyInputDoc.Descendants<OpenXmlElement>().ToList();

                    foreach (OpenXmlElement sdtInput in blocksInputDoc)
                    {
                        if (sdtInput.GetType().Name == "SdtBlock")
                        {
                            string contentTagName = sdtInput.Descendants<Tag>().FirstOrDefault().Val.Value;

                            if (contentTagName.Contains("Section_") && contentTagName.Contains("_Answer_"))
                            {
                                List<SdtBlock> innerSdtBlockList = sdtInput.Descendants<SdtBlock>().ToList();
                                if (innerSdtBlockList.Count > 0)
                                {

                                    Paragraph para = sdtInput.Descendants<Paragraph>().FirstOrDefault();

                                    Run firstRun = new Run();
                                    firstRun.AppendChild(new Text(" "));

                                    Run lastRun = new Run();
                                    lastRun.AppendChild(new Text(" "));

                                    Run middleRun = new Run();

                                    Text dataText = sdtInput.Descendants<Text>().FirstOrDefault();
                                    Text textNew = new Text();
                                    textNew.Text = "";
                                    List<Text> textList = sdtInput.Descendants<Text>().ToList();
                                    foreach (Text t in textList)
                                    {
                                        textNew.Text = textNew.Text + t.Text;
                                    }

                                    dataText.Text = textNew.Text;
                                    foreach (SdtBlock sdtBlockNew in innerSdtBlockList)
                                    {
                                        if (sdtBlockNew != null)
                                        {
                                            sdtBlockNew.Remove();
                                        }
                                    }
                                    //
                                    middleRun.AppendChild(new Text(dataText.Text));
                                    dataText.Remove();
                                    List<Run> runList = sdtInput.Descendants<Run>().ToList();
                                    foreach (Run run in runList)
                                    {
                                        run.Remove();
                                    }

                                    para.Append(firstRun);
                                    para.Append(middleRun);
                                    para.Append(lastRun);
                                    #region permstart and permend

                                    int id = 1;
                                    PermStart permStart = new PermStart();
                                    permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                                    permStart.Id = id;
                                    PermEnd permEnd = new PermEnd();
                                    permEnd.Id = id;

                                    #endregion
                                    para.InsertAfter<PermStart>(permStart, middleRun);
                                    para.InsertBefore<PermEnd>(permEnd, middleRun);


                                    //
                                }

                            }


                        }
                    }
                    mainDoc.Document.Save();
                }
                correctedOutput = ReadToEnd(streamFile);

            }

            return correctedOutput;
        }

        //
        //Function to collect all the Tags for the given list of Questions from given list of documents
        //for Comparision Report
        private List<String> GetQuestionTagList(Dictionary<string, byte[]> LawFirmsDocument, List<string> questionNumbers)
        {
            List<string> QuestionSectionTag = new List<string>();
            foreach (KeyValuePair<string, byte[]> firmDoc in LawFirmsDocument)
            {
                using (MemoryStream streamInputDoc = new MemoryStream())
                {
                    streamInputDoc.Write(firmDoc.Value, 0, firmDoc.Value.Length);
                    using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamInputDoc, true))
                    {
                        MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;

                        DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                        Body bodyinputDoc = documentinputDoc.Body;

                        List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();

                        foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                        {
                            if (sdtinputDoc.GetType().Name == "Table")
                            {
                                string contentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                if (contentTagName.Contains("Section_") && contentTagName.Contains("_QuestionNo_"))
                                {
                                    Text T = sdtinputDoc.Descendants<Text>().FirstOrDefault();
                                    foreach (string question in questionNumbers)
                                    {
                                        if (T.Text.Contains(question))
                                        {
                                            //Finding the tag name
                                            string sectionNo = contentTagName.Split('_')[1];
                                            string questionNo = contentTagName.Split('_')[3];
                                            QuestionSectionTag.Add("Section_" + sectionNo + "_Question_" + questionNo);
                                        }
                                    }
                                }


                            }
                        }
                    }
                }
            }

            return QuestionSectionTag.Distinct().ToList();
        }

        //Function to create a blank document for the Templates       

        private void CreateBlankOutputDoc(Stream file, bool isThisOutputFile)
        {
            using (WordprocessingDocument myDoc = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {

                MainDocumentPart mainPart = myDoc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body body = new Body();
                Paragraph paragraph = new Paragraph();
                Run run_paragraph = new Run();
                mainPart.Document.Append(body);
                mainPart.Document.Save();

                //--OutputDocument Permission start
                DocumentProtection docProtection = new DocumentProtection();
                docProtection.Edit = DocumentProtectionValues.ReadOnly;
                if (mainPart.DocumentSettingsPart == null) mainPart.AddNewPart<DocumentSettingsPart>();
                if (mainPart.DocumentSettingsPart.Settings == null) mainPart.DocumentSettingsPart.Settings = new Settings();
                mainPart.DocumentSettingsPart.Settings.AppendChild(docProtection);
                mainPart.DocumentSettingsPart.Settings.Save();
                UserPermissions _userPermissions = new UserPermissions();
                //_userPermissions.ApplyDocumentProtection(myDoc, ConfigurationHelper.DocumentProtectionPassword);
                _userPermissions.ApplyDocumentProtection(myDoc,"1234");
                //--Output Document Permission ends

            }
        }
        //Function to Generate the Opinion Template
        private void GenerateBody(WordprocessingDocument inputDoc, int SectionCount, int SectionQuestionCount)
        {

            //--Appending to SectionInfo List
            List<SectionInfo> sectionInfo = new List<SectionInfo>();
            for (int i = 0; i < SectionCount; i++)
            {
                sectionInfo.Add(new SectionInfo() { SectionName = "Enter Sub Heading Name", NoOfQuestions = SectionQuestionCount, SectionId = i + 1 });
            }
            //--
            int numOfQuestions = 0;
            Body inputDocBody = inputDoc.MainDocumentPart.Document.Body;

            foreach (SectionInfo _sectionInfo in sectionInfo)
            {
                int sectionCount = _sectionInfo.SectionId;
                List<SdtBlock> sectionList = inputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value == "Section_" + _sectionInfo.SectionId.ToString()).ToList();

                if (sectionList.Count == 0)
                {
                    //Section Names Start
                    //
                    Run runBeforeSection = new Run();
                    runBeforeSection.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                    Run runAfterSection = new Run();
                    runAfterSection.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT
                    //
                    Run runSection = new Run();
                    RunProperties runPropsSection = new RunProperties();

                    Bold boldSection = new Bold();
                    runPropsSection.Append(boldSection);

                    Underline ulSection = new Underline();
                    runPropsSection.Append(ulSection);

                    RunFonts runFontSection = new RunFonts();           // Create font
                    runFontSection.Ascii = "Times New Roman";
                    runPropsSection.Append(runFontSection);

                    FontSize fontSizeSection = new FontSize() { Val = "27" };
                    runPropsSection.Append(fontSizeSection);
                    runSection.AppendChild(new RunProperties(runPropsSection));
                    runSection.AppendChild(new Text(_sectionInfo.SectionName));
                    Paragraph paragraphSection = new Paragraph();
                    paragraphSection.Append(runBeforeSection);
                    paragraphSection.Append(runSection);
                    paragraphSection.Append(runAfterSection);


                    //--
                    #region permstart and permend

                    int id = 1;
                    PermStart permStart = new PermStart();
                    permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                    permStart.Id = id;
                    PermEnd permEnd = new PermEnd();
                    permEnd.Id = id;

                    #endregion
                    paragraphSection.InsertBefore<PermStart>(permStart, runSection);
                    paragraphSection.InsertAfter<PermEnd>(permEnd, runSection);
                    //--
                    SdtProperties sdtprSection = new SdtProperties(
                            new SdtAlias { Val = "Section_" + sectionCount },
                            new Tag { Val = "Section_" + sectionCount });

                    SdtContentBlock sdtCBlockSection = new SdtContentBlock(paragraphSection);


                    SdtBlock sdtBlockSection = new SdtBlock(sdtprSection, sdtCBlockSection);
                    inputDocBody.AppendChild(sdtBlockSection);
                    //Section Names End



                }

                numOfQuestions = Convert.ToInt16(sectionInfo.FirstOrDefault(x => x.SectionName == _sectionInfo.SectionName).NoOfQuestions);
                for (int i = 1; i <= numOfQuestions; i++)
                {
                    List<SdtBlock> QuestionsList = inputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + sectionCount + "_QuestionNo_")).ToList();
                    if (QuestionsList.Count != numOfQuestions)
                    {
                        //Question Header Start
                        i = QuestionsList.Count + 1;
                        Run RunQuestionNo = new Run();
                        RunProperties runPropsQuestionNo = new RunProperties();

                        Bold boldQuestionNo = new Bold();
                        runPropsQuestionNo.Append(boldQuestionNo);

                        Underline ulQuestionNo = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };
                        runPropsQuestionNo.Append(ulQuestionNo);

                        RunFonts runFontQuestionNo = new RunFonts();           // Create font
                        runFontQuestionNo.Ascii = "Times New Roman";
                        runPropsQuestionNo.Append(runFontQuestionNo);

                        FontSize fontSizeQuestionNo = new FontSize() { Val = "24" };
                        runPropsQuestionNo.Append(fontSizeQuestionNo);

                        Italic italicQuestionNo = new Italic();
                        runPropsQuestionNo.Append(italicQuestionNo);

                        RunQuestionNo.AppendChild(new RunProperties(runPropsQuestionNo));
                        RunQuestionNo.AppendChild(new Text("Question " + sectionCount + "." + i));
                        Paragraph paragraphQuestionNo = new Paragraph(RunQuestionNo);
                        SdtProperties sdtPrparagraphQuestionNo = new SdtProperties(
                                new SdtAlias { Val = "Section_" + sectionCount + "_QuestionNo_" + i },
                                new Tag { Val = "Section_" + sectionCount + "_QuestionNo_" + i });

                        SdtContentBlock sdtCBlockparagraphQuestionNo = new SdtContentBlock(paragraphQuestionNo);
                        SdtBlock sdtBlockparagraphQuestionNo = new SdtBlock(sdtPrparagraphQuestionNo, sdtCBlockparagraphQuestionNo);
                        //Question Header End                       


                        //Deletion check box start
                        Paragraph paragraphCheckBox = new Paragraph();
                        Run runCheckBox = new Run();
                        RunProperties runPropsCheckBox = new RunProperties();

                        RunFonts runFontCheckBox = new RunFonts();           // Create font
                        runFontCheckBox.Ascii = "Times New Roman";
                        runPropsCheckBox.Append(runFontCheckBox);

                        FontSize fontSizeCheckBox = new FontSize() { Val = "24" };
                        runPropsCheckBox.Append(fontSizeCheckBox);

                        Italic italicCheckBox = new Italic();
                        runPropsCheckBox.Append(italicCheckBox);

                        runCheckBox.AppendChild(new RunProperties(runPropsCheckBox));

                        Text textCheckBox = new Text();

                        textCheckBox.Text = "Delete:☐";

                        runCheckBox.Append(textCheckBox);
                        paragraphCheckBox.Append(CreateCheckBoxContentControl(runCheckBox, inputDoc, true));


                        SdtProperties sdtPrparagraphQuestionNoPrCheckBox = new SdtProperties(
                                new SdtAlias { Val = "ChkBox_Del_Section_" + sectionCount + "_Question_" + i },
                                new Tag { Val = "ChkBox_Del_Section_" + sectionCount + "_Question_" + i });

                        SdtContentBlock sdtCBlockCheckBox = new SdtContentBlock(paragraphCheckBox);

                        SdtBlock sdtBlockCheckBox = new SdtBlock(sdtPrparagraphQuestionNoPrCheckBox, sdtCBlockCheckBox);
                        //Deletion check box End


                        //Question creation start
                        //
                        Run runBeforeEnterQuestion = new Run();
                        runBeforeEnterQuestion.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT

                        Run runAfterEnterQuestion = new Run();
                        runAfterEnterQuestion.AppendChild(new Text(" "));//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT
                        //
                        Run runEnterQuestion = new Run();

                        RunProperties runPropsEnterQuestion = new RunProperties();

                        RunFonts runFontEnterQuestion = new RunFonts();           // Create font
                        runFontEnterQuestion.Ascii = "Times New Roman";
                        runPropsEnterQuestion.Append(runFontEnterQuestion);

                        FontSize fontSizeEnterQuestion = new FontSize() { Val = "22" };
                        runPropsEnterQuestion.Append(fontSizeEnterQuestion);

                        runEnterQuestion.AppendChild(new RunProperties(runPropsEnterQuestion));

                        runEnterQuestion.AppendChild(new Text("Enter new question"));

                        Paragraph paragraphEnterQuestion = new Paragraph();//runEnterQuestion);

                        paragraphEnterQuestion.Append(runBeforeEnterQuestion);
                        paragraphEnterQuestion.Append(runEnterQuestion);
                        paragraphEnterQuestion.Append(runAfterEnterQuestion);

                        //--
                        #region permstart and permend

                        int id3 = 3;
                        PermStart permStart2 = new PermStart();
                        permStart2.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                        permStart2.Id = id3;
                        PermEnd permEnd2 = new PermEnd();
                        permEnd2.Id = id3;

                        #endregion
                        paragraphEnterQuestion.InsertBefore<PermStart>(permStart2, runEnterQuestion);
                        paragraphEnterQuestion.InsertAfter<PermEnd>(permEnd2, runEnterQuestion);
                        //--
                        SdtProperties sdtPrEnterQuestion = new SdtProperties(
                                new SdtAlias { Val = "Section_Question_" + i },
                                new Tag { Val = "Section_" + sectionCount + "_Question_" + i });
                        //,
                        //new Lock { Val = LockingValues.SdtLocked });
                        SdtContentBlock sdtCBlockEnterQuestion = new SdtContentBlock(paragraphEnterQuestion);

                        SdtBlock sdtBlockEnterQuestion = new SdtBlock(sdtPrEnterQuestion, sdtCBlockEnterQuestion);
                        //Question creation End



                        int GetSectionNo = sectionCount + 1;
                        List<SdtBlock> QuestionsSectionExist = inputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + GetSectionNo)).ToList();

                        Table TblQuestionHeader = new Table();
                        TableRow trQuestionHeader = new TableRow();
                        TableCell tcQuestionHeader = new TableCell();
                        TableCellProperties tcQuestionHeaderProps = new TableCellProperties();
                        TableCellWidth tcWidthQuestionHeader = new TableCellWidth() { Width = "3800", Type = TableWidthUnitValues.Pct };
                        tcQuestionHeaderProps.Append(tcWidthQuestionHeader);
                        tcQuestionHeader.Append(tcQuestionHeaderProps);
                        tcQuestionHeader.Append(sdtBlockparagraphQuestionNo);
                        TableProperties tablePropQuestionHeader = new TableProperties();
                        TableWidth tableWidthQuestionHeader = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                        tablePropQuestionHeader.Append(tableWidthQuestionHeader);
                        TableCell tcStatusDropDown = new TableCell();
                        tcStatusDropDown.Append(sdtBlockCheckBox);
                        trQuestionHeader.Append(tcQuestionHeader);
                        TblQuestionHeader.AppendChild(tablePropQuestionHeader);
                        trQuestionHeader.Append(tcStatusDropDown);
                        TblQuestionHeader.Append(trQuestionHeader);

                        if (QuestionsSectionExist.Count == 0)
                        {
                            inputDocBody.Append(TblQuestionHeader);
                            inputDocBody.Append(sdtBlockEnterQuestion);
                        }
                        else
                        {
                            QuestionsSectionExist[0].InsertBeforeSelf<Table>(TblQuestionHeader);
                            QuestionsSectionExist[0].InsertBeforeSelf<SdtBlock>(sdtBlockEnterQuestion);
                        }


                    }
                }
                sectionCount = sectionCount + 1;
            }

            inputDoc.MainDocumentPart.Document.Save();
            inputDoc.Dispose();
        }

        //Function to create check box control in document
        private SdtRun CreateCheckBoxContentControl(Run run, WordprocessingDocument outDoc, bool isQuesTemplate)
        {
            SdtId sdtId = new SdtId() { Val = -524013019 };
            SdtContentCheckBox sdtContentCheckBox = new SdtContentCheckBox();
            // sdtContentCheckBox.Checked.Val = OnOffValues.Zero;
            SdtProperties sdtProperties = new SdtProperties();
            SdtContentRun sdtContentRun = new SdtContentRun();
            SdtRun sdtRun = new SdtRun();
            sdtProperties.Append(sdtId);
            sdtProperties.Append(sdtContentCheckBox);
            sdtContentRun.Append(run);
            sdtRun.Append(sdtProperties);
            sdtRun.Append(sdtContentRun);

            //--
            if (isQuesTemplate)
            {

                #region permstart and permend

                int id = 2;
                PermStart permStart1 = new PermStart();
                permStart1.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                permStart1.Id = id;
                PermEnd permEnd1 = new PermEnd();
                permEnd1.Id = id;


                #endregion
                sdtRun.InsertBefore<PermStart>(permStart1, sdtContentRun);
                sdtRun.InsertAfter<PermEnd>(permEnd1, sdtContentRun);


            }
            //
            return sdtRun;
        }

        //private byte[] CreateLegalOpinionOutput(byte[] LegalOpinionInput, byte[] LegalOpinionOutput, string Jurisdictionname, string LawFirmName, bool isMargeCalled)
        private byte[] CreateLegalOpinionOutput(byte[] LegalOpinionInput, byte[] LegalOpinionOutput, DocumentMetaData metaData, bool isMargeCalled)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] mergedDocument;
            int ansPermId = 0;
            using (MemoryStream streamQuestionTemplate = new MemoryStream())
            {
                streamQuestionTemplate.Write(LegalOpinionInput, 0, LegalOpinionInput.Length);
                //streamQuestionTemplate.Position = 0;

                using (MemoryStream streamAnswerTemplate = new MemoryStream())
                {
                    streamAnswerTemplate.Write(LegalOpinionOutput, 0, LegalOpinionOutput.Length);
                    //streamAnswerTemplate.Position = 0;
                    WordProcessor objWordProc = new WordProcessor();

                    if (isMargeCalled == false)
                    {
                        //Calling function for Creating a blank document
                        CreateBlankOutputDoc(streamAnswerTemplate, true);

                        //calling function to create the cover page

                        CreateOutputDocCoverPage(streamAnswerTemplate, metaData);
                    }



                    using (WordprocessingDocument inputDoc = WordprocessingDocument.Open(streamQuestionTemplate, true))
                    {
                        //Footnote inputDocFootNote = new Footnote();

                        MainDocumentPart mainPartinputDoc = inputDoc.MainDocumentPart;
                        var inputDocFootNote = mainPartinputDoc.FootnotesPart;
                        DocumentFormat.OpenXml.Wordprocessing.Document documentinputDoc = mainPartinputDoc.Document;
                        Body bodyinputDoc = documentinputDoc.Body;

                        List<OpenXmlElement> blocksinputDoc = bodyinputDoc.Elements<OpenXmlElement>().ToList();
                        WordprocessingDocument outputDoc = WordprocessingDocument.Open(streamAnswerTemplate, true);
                        if (isMargeCalled == false)
                        {
                            //ApplyHeader(outputDoc, metaData.LawFirmName);
                            ApplyHeader(outputDoc, (metaData.LawFirmName + "/" + metaData.OpinionName),false,false);
                            ApplyFooter(outputDoc);
                        }

                        MainDocumentPart mainPartoutputDoc = outputDoc.MainDocumentPart;
                        DocumentFormat.OpenXml.Wordprocessing.Document documentoutputDoc = mainPartoutputDoc.Document;
                        Body bodyoutputDoc = documentoutputDoc.Body;

                        if (inputDocFootNote != null)// && isMargeCalled == false)
                        {
                            long footnoterefid = 0;
                            FootnotesPart footNotePart = outputDoc.MainDocumentPart.FootnotesPart;//.Footnotes;//
                            footnoterefid = ((IEnumerable<FootnoteEndnoteType>)inputDocFootNote.Footnotes.Descendants<Footnote>()).Select(fn => fn.Id.Value).Max();
                            if (footNotePart != null)// && isMargeCalled == false)
                            {
                                var outputMaxfootnoterefid = ((IEnumerable<FootnoteEndnoteType>)footNotePart.Footnotes.Descendants<Footnote>()).Select(fn => fn.Id.Value).Max();
                                footnoterefid = footnoterefid > outputMaxfootnoterefid ? footnoterefid : outputMaxfootnoterefid;
                                footnoterefid++;
                                var footnotereferences = (IEnumerable<FootnoteEndnoteReferenceType>)mainPartoutputDoc.Document.Descendants<FootnoteReference>();
                                foreach (var footnotereference in footnotereferences)
                                {
                                    var footnotes = (IEnumerable<FootnoteEndnoteType>)footNotePart.Footnotes.Descendants<Footnote>();
                                    foreach (var footnote in footnotes)
                                    {
                                        if (footnote.Id.ToString() == footnotereference.Id.ToString())
                                        {
                                            footnotereference.Id = footnoterefid;
                                            footnote.Id = footnoterefid;
                                            footnoterefid++;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        List<OpenXmlElement> blocksoutputDoc = bodyoutputDoc.Elements<OpenXmlElement>().ToList();


                        //Paragraph para = new Paragraph(new Run((new Break() { Type = BreakValues.Page })));
                        //mainPartoutputDoc.Document.Body.InsertAfter(para, mainPartoutputDoc.Document.Body.LastChild);

                        foreach (OpenXmlElement sdtinputDoc in blocksinputDoc)
                        {
                            if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null)
                            {
                                if (sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == "LegalOpinion" || sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == "DescriptionTag" || sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == "DescriptionContentTag")
                                {
                                }
                                else
                                {
                                    bool isContentBlockExist = false;

                                    foreach (OpenXmlElement sdtoutputDoc in blocksoutputDoc)
                                    {
                                        if (sdtinputDoc.Descendants<Tag>().FirstOrDefault() != null && sdtoutputDoc.Descendants<Tag>().FirstOrDefault() != null)
                                        {
                                            if (sdtoutputDoc.GetType().Name == "SdtBlock")
                                            {
                                                if (sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value)
                                                {
                                                    isContentBlockExist = true;

                                                    foreach (SdtContentBlock sc in sdtinputDoc.Elements<SdtContentBlock>())
                                                    {
                                                        foreach (Paragraph p in sc.Elements<Paragraph>())
                                                        {
                                                            var tps = p.GetFirstChild<PermStart>();
                                                            var tpe = p.GetFirstChild<PermEnd>();
                                                            if (tps != null)
                                                            {
                                                                tps.Remove();
                                                            }
                                                            if (tpe != null)
                                                            {
                                                                tpe.Remove();
                                                            }

                                                        }

                                                    }

                                                    sdtoutputDoc.InnerXml = sdtinputDoc.InnerXml;

                                                }
                                            }
                                            if (sdtoutputDoc.GetType().Name == "Table")
                                            {
                                                if (sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value == sdtoutputDoc.Descendants<Tag>().FirstOrDefault().Val.Value)
                                                {
                                                    isContentBlockExist = true;
                                                }
                                            }
                                        }
                                    }
                                    if (isContentBlockExist == false)
                                    {
                                        if (sdtinputDoc.GetType().Name == "SdtBlock")
                                        {
                                            foreach (SdtContentBlock sc in sdtinputDoc.Elements<SdtContentBlock>())
                                            {
                                                foreach (Paragraph p in sc.Elements<Paragraph>())
                                                {
                                                    var tps = p.GetFirstChild<PermStart>();
                                                    var tpe = p.GetFirstChild<PermEnd>();
                                                    if (tps != null)
                                                    {
                                                        tps.Remove();
                                                    }
                                                    if (tpe != null)
                                                    {
                                                        tpe.Remove();
                                                    }
                                                }

                                            }

                                            string ContentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            int sectionNo = 0;
                                            if (ContentTagName.Contains("_"))
                                            {
                                                sectionNo = Convert.ToInt16(ContentTagName.Split('_')[1]);
                                            }
                                            int GetSectionNo = sectionNo + 1;
                                            List<SdtBlock> sectionExistList = outputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<Tag>().Val.Value.StartsWith("Section_" + GetSectionNo)).ToList();

                                            if (sectionExistList.Count == 0)
                                            {
                                                bodyoutputDoc.Append((OpenXmlElement)sdtinputDoc.Clone());
                                            }
                                            else
                                            {
                                                sectionExistList[0].InsertBeforeSelf<OpenXmlElement>((OpenXmlElement)sdtinputDoc.Clone());
                                            }
                                            
                                            if (sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.StartsWith("Section_Question_"))
                                            {
                                                Run runAnswerHeader = new Run();

                                                RunProperties runPropsAnswerHeader = new RunProperties();

                                                RunFonts runFontAnswerHeader = new RunFonts();           // Create font
                                                runFontAnswerHeader.Ascii = "Times New Roman";
                                                runPropsAnswerHeader.Append(runFontAnswerHeader);

                                                FontSize fontSizeAnswerHeader = new FontSize() { Val = "24" };
                                                runPropsAnswerHeader.Append(fontSizeAnswerHeader);

                                                Bold boldAnswerHeader = new Bold();
                                                runPropsAnswerHeader.Append(boldAnswerHeader);

                                                Italic italicAnswerHeader = new Italic();
                                                runPropsAnswerHeader.Append(italicAnswerHeader);

                                                Underline ulAnswerHeader = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };
                                                runPropsAnswerHeader.Append(ulAnswerHeader);

                                                runAnswerHeader.AppendChild(new RunProperties(runPropsAnswerHeader));
                                                runAnswerHeader.AppendChild(new Text("Answer"));
                                                Paragraph paragraphAnswerHeader = new Paragraph(runAnswerHeader);
                                                string title = sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.Replace("Question", "AnswerHeader");
                                                string tag = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value.Replace("Question", "AnswerHeader");
                                                SdtProperties sdtPrAnswerHeader = new SdtProperties(
                                                        new SdtAlias { Val = title },
                                                        new Tag { Val = tag },
                                                        new Lock { Val = LockingValues.SdtLocked });
                                                SdtContentBlock sdtCBlockAnswerHeader = new SdtContentBlock(paragraphAnswerHeader);
                                                SdtBlock sdtBlockAnswerHeader = new SdtBlock(sdtPrAnswerHeader, sdtCBlockAnswerHeader);

                                                if (sectionExistList.Count == 0)
                                                {
                                                    bodyoutputDoc.Append(sdtBlockAnswerHeader);

                                                }
                                                else
                                                {
                                                    sectionExistList[0].InsertBeforeSelf<SdtBlock>(sdtBlockAnswerHeader);

                                                }
                                            }
                                           

                                            if (sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.StartsWith("Section_Question_"))
                                            {

                                               
                                                Run runBeforeEnterAnswer = new Run();
                                                runBeforeEnterAnswer.AppendChild(new Text(" "));

                                                Run runAfterEnterAnswer = new Run();
                                                runAfterEnterAnswer.AppendChild(new Text(" "));
                                                
                                                Run runEnterAnswer = new Run();

                                                RunProperties runPropsEnterAnswer = new RunProperties();

                                                RunFonts runFontEnterAnswer = new RunFonts();           // Create font
                                                runFontEnterAnswer.Ascii = "Times New Roman";
                                                runPropsEnterAnswer.Append(runFontEnterAnswer);

                                                FontSize fontSizeEnterAnswer = new FontSize() { Val = "24" };
                                                runPropsEnterAnswer.Append(fontSizeEnterAnswer);

                                                runEnterAnswer.AppendChild(new RunProperties(runPropsEnterAnswer));
                                                runEnterAnswer.AppendChild(new Text("Enter new Answer"));
                                                Paragraph paragraphEnterAnswer = new Paragraph();//runEnterAnswer);
                                                paragraphEnterAnswer.Append(runBeforeEnterAnswer);
                                                paragraphEnterAnswer.Append(runEnterAnswer);
                                                paragraphEnterAnswer.Append(runAfterEnterAnswer);
                                                #region permstart and permend
                                                ansPermId = ansPermId + 1;
                                                int id = ansPermId;
                                                PermStart permStart = new PermStart();
                                                permStart.EditorGroup = RangePermissionEditingGroupValues.Everyone;

                                                permStart.Id = id;
                                                PermEnd permEnd = new PermEnd();
                                                permEnd.Id = id;

                                                #endregion
                                                paragraphEnterAnswer.InsertBefore<PermStart>(permStart, runEnterAnswer);
                                                paragraphEnterAnswer.InsertAfter<PermEnd>(permEnd, runEnterAnswer);

                                                SdtProperties sdtPrEnterAnswer = new SdtProperties(
                                                        new SdtAlias { Val = sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.Replace("Question", "Answer") },
                                                        new Tag { Val = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value.Replace("Question", "Answer") },
                                                        new Lock { Val = LockingValues.SdtLocked });
                                                SdtContentBlock sdtCBlockEnterAnswer = new SdtContentBlock(paragraphEnterAnswer);
                                                SdtBlock sdtBlockEnterAnswer = new SdtBlock(sdtPrEnterAnswer, sdtCBlockEnterAnswer);

                                                if (sectionExistList.Count == 0)
                                                {

                                                    bodyoutputDoc.Append(sdtBlockEnterAnswer);
                                                }
                                                else
                                                {

                                                    sectionExistList[0].InsertBeforeSelf<SdtBlock>(sdtBlockEnterAnswer);
                                                }

                                            }
                                        }
                                        if (sdtinputDoc.GetType().Name == "Table")
                                        {
                                            Run runQuestionNo = new Run();
                                            RunProperties runPropsQuestionNo = new RunProperties();

                                            RunFonts runFont = new RunFonts();           // Create font
                                            runFont.Ascii = "Times New Roman";
                                            runPropsQuestionNo.Append(runFont);

                                            FontSize fontSizeQuestionNo = new FontSize() { Val = "24" };
                                            runPropsQuestionNo.Append(fontSizeQuestionNo);

                                            Bold boldQuestionNo = new Bold();
                                            runPropsQuestionNo.Append(boldQuestionNo);

                                            Italic italicQuestionNo = new Italic();
                                            runPropsQuestionNo.Append(italicQuestionNo);

                                            Underline ulLegalOpinionBlock = new Underline() { Val = DocumentFormat.OpenXml.Wordprocessing.UnderlineValues.Single };
                                            runPropsQuestionNo.Append(ulLegalOpinionBlock);

                                            runQuestionNo.AppendChild(new RunProperties(runPropsQuestionNo));

                                            //Input check box value
                                            string chkBox = sdtinputDoc.Descendants<SdtAlias>().ElementAtOrDefault<SdtAlias>(1).Val.Value;

                                            string QueNo = sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.Split('_')[3];
                                            string SecNo = sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value.Split('_')[1];
                                            runQuestionNo.AppendChild(new Text("Question " + SecNo + "." + QueNo));
                                            Paragraph paragraphQuestionNo = new Paragraph(runQuestionNo);
                                            SdtProperties sdtPrparagraphQuestionNo = new SdtProperties(
                                                    new SdtAlias { Val = sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value },
                                                    new Tag { Val = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value },
                                                    new Lock { Val = LockingValues.SdtContentLocked },
                                                    new Lock { Val = LockingValues.SdtLocked });
                                            SdtContentBlock sdtCBlockparagraphQuestionNo = new SdtContentBlock(paragraphQuestionNo);
                                            SdtBlock sdtBlockparagraphQuestionNo = new SdtBlock(sdtPrparagraphQuestionNo, sdtCBlockparagraphQuestionNo);
                                            //Question Header End

                                            Paragraph paragraphCheckBox = new Paragraph();
                                            Run runCheckBox = new Run();

                                            RunProperties runPropsCheckBox = new RunProperties();

                                            RunFonts runFontCheckBox = new RunFonts();           // Create font
                                            runFontCheckBox.Ascii = "Times New Roman";
                                            runPropsCheckBox.Append(runFontCheckBox);

                                            FontSize fontSizeCheckBox = new FontSize() { Val = "24" };
                                            runPropsCheckBox.Append(fontSizeCheckBox);

                                            //Bold boldCheckBox = new Bold();
                                            // runPropsCheckBox.Append(boldCheckBox);

                                            Italic italicCheckBox = new Italic();
                                            runPropsCheckBox.Append(italicCheckBox);

                                            runCheckBox.AppendChild(new RunProperties(runPropsCheckBox));

                                            Text textCheckBox = new Text();

                                            //textCheckBox.Text = "MarkComplete:☐";

                                            textCheckBox.Text = " ";//for this blank character,HOLD ALT then hit "255" on the number pad, then release ALT
                                            // "Not Completed";
                                            SdtProperties checkBoxDisabled = new SdtProperties(
                                                     new Lock { Val = LockingValues.SdtLocked });
                                            runCheckBox.Append(checkBoxDisabled);

                                            runCheckBox.Append(textCheckBox);
                                            paragraphCheckBox.Append(CreateCheckBoxContentControl(runCheckBox, outputDoc, false));


                                            SdtProperties sdtPrparagraphQuestionNoPrCheckBox = new SdtProperties(
                                                    new SdtAlias { Val = "CheckBox" + sdtinputDoc.Descendants<SdtAlias>().FirstOrDefault().Val.Value },
                                                    new Tag { Val = "CheckBox" + sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value },
                                                    new Lock { Val = LockingValues.SdtLocked });
                                            SdtContentBlock sdtCBlockCheckBox = new SdtContentBlock(paragraphCheckBox);
                                            SdtBlock sdtBlockCheckBox = new SdtBlock(sdtPrparagraphQuestionNoPrCheckBox, sdtCBlockCheckBox);

                                            //Get Section Number
                                            string contentTagName = sdtinputDoc.Descendants<Tag>().FirstOrDefault().Val.Value;
                                            //"LegalName";
                                            int sectionNo = 0;
                                            if (contentTagName.Contains("_"))
                                            {
                                                sectionNo = Convert.ToInt16(contentTagName.Split('_')[1]);
                                            }
                                            int GetSectionNo = sectionNo + 1;

                                            List<SdtBlock> SectionExist = outputDoc.MainDocumentPart.Document.Body.Descendants<SdtBlock>().Where(r => r.Descendants<Tag>().FirstOrDefault().Val.Value.StartsWith("Section_" + GetSectionNo)).ToList();

                                            Table TblQuestionHeader = new Table();
                                            TableRow trQuestionHeader = new TableRow();
                                            TableCell tcQuestionHeader = new TableCell();
                                            TableCellProperties tcQuestionHeaderProps = new TableCellProperties();
                                            TableCellWidth tcWidthQuestionHeader = new TableCellWidth() { Width = "3800", Type = TableWidthUnitValues.Pct };
                                            tcQuestionHeaderProps.Append(tcWidthQuestionHeader);
                                            tcQuestionHeader.Append(tcQuestionHeaderProps);
                                            tcQuestionHeader.Append(sdtBlockparagraphQuestionNo);
                                            TableProperties tablePropQuestionHeader = new TableProperties();
                                            // Make the table width 100% of the page width.
                                            TableWidth tableWidthQuestionHeader = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                                            tablePropQuestionHeader.Append(tableWidthQuestionHeader);
                                            TableCell tcCheckBox = new TableCell();

                                            tcCheckBox.Append(sdtBlockCheckBox);
                                            trQuestionHeader.Append(tcQuestionHeader);
                                            TblQuestionHeader.AppendChild(tablePropQuestionHeader);
                                            trQuestionHeader.Append(tcCheckBox);
                                            TblQuestionHeader.Append(trQuestionHeader);

                                            if (SectionExist.Count == 0)
                                            {
                                                bodyoutputDoc.Append(TblQuestionHeader);
                                            }
                                            else
                                            {
                                                SectionExist[0].InsertBeforeSelf<Table>(TblQuestionHeader);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        WaterMark objWaterMark = new WaterMark();
                        if (!isMargeCalled)
                        {
                            objWaterMark.AddWatermark(outputDoc);
                        }



                        if (inputDocFootNote != null)// && isMargeCalled == false)
                        {
                            FootnotesPart footNotePart = outputDoc.MainDocumentPart.FootnotesPart;//.Footnotes;// 

                            if (footNotePart == null || footNotePart.Footnotes == null)
                            {
                                outputDoc.MainDocumentPart.AddNewPart<FootnotesPart>();//inputDocFootNote);
                                outputDoc.MainDocumentPart.FootnotesPart.Footnotes = new Footnotes();
                                if (inputDocFootNote.Footnotes != null && inputDocFootNote.Footnotes.Any())
                                {
                                    foreach (var footnote in inputDocFootNote.Footnotes.Descendants<Footnote>())
                                    {
                                        var clonedFootnote = (Footnote) footnote.CloneNode(true);
                                        outputDoc.MainDocumentPart.FootnotesPart.Footnotes.AppendChild<Footnote>(clonedFootnote);
                                    }
                                }
                            }
                            else
                            {
                                if (inputDocFootNote.Footnotes != null && inputDocFootNote.Footnotes.Any())
                                {
                                    foreach (var footnote in inputDocFootNote.Footnotes.Descendants<Footnote>().Where(fn => fn.Id > 0))
                                    {
                                        var clonedFootnote = (Footnote)footnote.CloneNode(true);
                                        outputDoc.MainDocumentPart.FootnotesPart.Footnotes.AppendChild<Footnote>(clonedFootnote);
                                    }
                                }
                            }                            
                        }

                        //This is to handle hyperlinks in the document
                        var inputHyperlinkRelationships = inputDoc.MainDocumentPart.HyperlinkRelationships;
                        var outputHyperlinkRelationships = outputDoc.MainDocumentPart.HyperlinkRelationships;
                        if (inputHyperlinkRelationships != null)
                        {
                            foreach (var hyperlinkRelationship in inputHyperlinkRelationships)
                            {
                                if (outputHyperlinkRelationships != null && outputHyperlinkRelationships.Any())
                                {
                                    var matchingHyperLinks = outputHyperlinkRelationships.Where(hl => hl.Id == hyperlinkRelationship.Id);
                                    if (matchingHyperLinks == null || !matchingHyperLinks.Any())
                                    {
                                        outputDoc.MainDocumentPart.AddHyperlinkRelationship(hyperlinkRelationship.Uri, hyperlinkRelationship.IsExternal, hyperlinkRelationship.Id);
                                    }
                                }
                            }                            
                        }

                        var inputStyleDefinitions = inputDoc.MainDocumentPart.StyleDefinitionsPart;
                        var inputStyleWithEffects = inputDoc.MainDocumentPart.StylesWithEffectsPart;
                        var outputStyleDefinitions = outputDoc.MainDocumentPart.StyleDefinitionsPart;
                        var outputStyleWithEffects = outputDoc.MainDocumentPart.StylesWithEffectsPart;

                        if (inputStyleDefinitions != null)
                        {
                            if (outputDoc.MainDocumentPart.StyleDefinitionsPart == null)
                            {
                                outputDoc.MainDocumentPart.AddPart<StyleDefinitionsPart>(inputStyleDefinitions);
                            }
                            else
                            {
                                var inputStyles = inputStyleDefinitions.Styles.Descendants<Style>();
                                var outputStyles = outputStyleDefinitions.Styles.Descendants<Style>();
                                var stylesToBeAdded = inputStyles.Where(s => !outputStyles.Any(os => os.StyleId.ToString() == s.StyleId.ToString()));
                                foreach (var style in stylesToBeAdded)
                                {
                                    var clonedStyle = (Style)style.CloneNode(true);
                                    outputStyleDefinitions.Styles.AppendChild<Style>(clonedStyle);
                                }

                            }
                        }

                        if (outputStyleWithEffects != null)
                        {
                            if (outputDoc.MainDocumentPart.StylesWithEffectsPart == null)
                            {
                                outputDoc.MainDocumentPart.AddPart<StylesWithEffectsPart>(inputStyleWithEffects);
                            }
                            else
                            {
                                var inputStyles = inputStyleWithEffects.Styles.Descendants<Style>();
                                var outputStyles = outputStyleWithEffects.Styles.Descendants<Style>();
                                var stylesToBeAdded = inputStyles.Where(s => !outputStyles.Any(os => os.StyleId.ToString() == s.StyleId.ToString()));
                                foreach (var style in stylesToBeAdded)
                                {
                                    var clonedStyle = (Style)style.CloneNode(true);
                                    outputStyleWithEffects.Styles.AppendChild<Style>(clonedStyle);
                                }

                            }
                        }

                        #region Image Handling - Commented
                        //This is to handle images in the questions - Needs to be cleaned up and tested. To be included when it is asked for.

                        //var inputImageParts = inputDoc.MainDocumentPart.ImageParts;
                        //var outputImageParts = outputDoc.MainDocumentPart.ImageParts;
                        //if (inputImageParts != null && inputImageParts.Any())// && isMargeCalled == false)
                        //{
                        //    if (outputImageParts == null || !outputImageParts.Any())
                        //    {
                        //        foreach (var inputImagePart in inputImageParts)
                        //        {
                        //            outputDoc.MainDocumentPart.AddPart<ImagePart>(inputImagePart, inputDoc.MainDocumentPart.GetIdOfPart(inputImagePart));
                        //        }                                
                        //    }
                        //    else
                        //    {
                        //        foreach (var inputImagePart in inputImageParts)
                        //        {
                        //            if (outputDoc.MainDocumentPart.ImageParts.Where(ip => outputDoc.MainDocumentPart.GetIdOfPart(ip) == inputDoc.MainDocumentPart.GetIdOfPart(inputImagePart)) == null)
                        //            {
                        //                outputDoc.MainDocumentPart.AddPart<ImagePart>(inputImagePart, inputDoc.MainDocumentPart.GetIdOfPart(inputImagePart));
                        //            }
                        //            else
                        //            {
                                        
                        //            }
                                    
                        //        }  
                        //    }
                        //}

                    #endregion

                        outputDoc.MainDocumentPart.Document.Save();
                        outputDoc.Dispose();
                    }
                    byte[] outPutDocument = objWordProc.ReadToEnd(streamAnswerTemplate);
                    byte[] outPutDocumentwithProps1 = WDSetCustomProperty(outPutDocument, "LegalOpinionName", metaData.OpinionName, PropertyTypes.Text);
                    byte[] outPutDocumentwithProps2 = WDSetCustomProperty(outPutDocumentwithProps1, "LegalOpinionDesc", metaData.OpinionDescription, PropertyTypes.Text);
                    byte[] outPutDocumentwithProps3 = WDSetCustomProperty(outPutDocumentwithProps2, "LawFirmName", metaData.LawFirmName, PropertyTypes.Text);
                    byte[] outPutDocumentwithProps4 = WDSetCustomProperty(outPutDocumentwithProps3, "JurisdictionName", metaData.JurisdictionName, PropertyTypes.Text);

                    mergedDocument = outPutDocumentwithProps4;
                }
            }
            return mergedDocument;
        }
        //Function to create the cover page        
        private void CreateOutputDocCoverPage(MemoryStream LegalOpinionOutput, DocumentMetaData metaData)
        {

            //--
            string dateSubmitted = System.DateTime.Today.ToShortDateString();
            Dictionary<string, string> metaDataDictionary = new Dictionary<string, string>();


            metaDataDictionary.Add("Legal Opinion Name", metaData.OpinionName);
            metaDataDictionary.Add("Description", metaData.OpinionDescription);
            metaDataDictionary.Add("Jurisdiction", metaData.JurisdictionName);
            metaDataDictionary.Add("Firm Name", metaData.LawFirmName);
            //--
            using (WordprocessingDocument outputDoc = WordprocessingDocument.Open(LegalOpinionOutput, true))
            {

                MainDocumentPart mainPart = outputDoc.MainDocumentPart;
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                Body bodyoutputDoc = new Body();
                Paragraph paragraph = new Paragraph();
                Run run_paragraph = new Run();
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                run_paragraph.Append(new Break());
                paragraph.Append(run_paragraph);
                bodyoutputDoc.Append(paragraph);

                mainPart.Document.Append(bodyoutputDoc);
                mainPart.Document.Save();


                //--Cover Page Entry starts
                Table TblMetaDataInfo = new Table();

                // Set the style and width for the table.
                TableProperties tableProp = new TableProperties();
                TableWidth tableWidthLegalOpinionCover = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                tableProp.Append(tableWidthLegalOpinionCover);


                TblMetaDataInfo.AppendChild(tableProp);

                foreach (string metaDataKey in metaDataDictionary.Keys)
                {
                    //--Entry for the Meta Data Key starts
                    Run RunMetaData = new Run();
                    RunProperties runPropsMetaData = new RunProperties();
                    Bold boldMetaData = new Bold();
                    FontSize fontSizeMetaData = new FontSize() { Val = "28" };
                    runPropsMetaData.Append(boldMetaData);
                    runPropsMetaData.Append(fontSizeMetaData);

                    RunFonts runFontMetaData = new RunFonts();           // Create font
                    runFontMetaData.Ascii = "Times New Roman";
                    runPropsMetaData.Append(runFontMetaData);

                    RunMetaData.AppendChild(new RunProperties(runPropsMetaData));
                    RunMetaData.AppendChild(new Text(metaDataKey + ":"));
                    Paragraph paragraphMetaData = new Paragraph(RunMetaData);
                    SdtProperties sdtPrparagraphMetaData = new SdtProperties(
                                   new SdtAlias { Val = metaDataKey.Replace(" ", "") + "KeyTitle" },
                                   new Tag { Val = metaDataKey.Replace(" ", "") + "KeyTag" },
                                   new Lock { Val = LockingValues.ContentLocked },
                                   new Lock { Val = LockingValues.SdtLocked });
                    SdtContentBlock sdtCBlockparagraphMetaData = new SdtContentBlock(paragraphMetaData);
                    SdtBlock sdtBlockparagraphMetaData = new SdtBlock(sdtPrparagraphMetaData, sdtCBlockparagraphMetaData);
                    //-- Entry for the Titles ends
                    string metaDataValue = metaDataDictionary.FirstOrDefault(x => x.Key == metaDataKey).Value;
                    //--Entry for Meta Data Values starts
                    Run RunMetaDataValue = new Run();
                    RunProperties runPropsMetaDataValue = new RunProperties();
                    FontSize fontSizeMetaDataValue = new FontSize() { Val = "28" };
                    runPropsMetaDataValue.Append(fontSizeMetaDataValue);

                    RunFonts runFontMetaDataValue = new RunFonts();           // Create font
                    runFontMetaDataValue.Ascii = "Times New Roman";
                    runPropsMetaDataValue.Append(runFontMetaDataValue);

                    RunMetaDataValue.AppendChild(new RunProperties(runPropsMetaDataValue));
                    RunMetaDataValue.AppendChild(new Text(metaDataValue));
                    Paragraph paragraphMetaDataValue = new Paragraph(RunMetaDataValue);
                    SdtProperties sdtPrparagraphMetaDataValue = new SdtProperties(
                                   new SdtAlias { Val = metaDataKey.Replace(" ", "") + "ValueTitle" },
                                   new Tag { Val = metaDataKey.Replace(" ", "") + "ValueTag" },
                                   new Lock { Val = LockingValues.ContentLocked },
                                   new Lock { Val = LockingValues.SdtLocked });
                    SdtContentBlock sdtCBlockparagraphMetaDataValue = new SdtContentBlock(paragraphMetaDataValue);
                    SdtBlock sdtBlockparagraphMetaDataValue = new SdtBlock(sdtPrparagraphMetaDataValue, sdtCBlockparagraphMetaDataValue);
                    //-- Entry for Meta Data Values

                    TableRow trLegalOpinionCover = new TableRow();
                    TableCell tcLegalOpinionCover = new TableCell();
                    TableCellProperties tcLegalOpinionCoverProps = new TableCellProperties();
                    TableCellWidth tcWidthtcLegalOpinionCover = new TableCellWidth() { Width = "2000", Type = TableWidthUnitValues.Pct };
                    tcLegalOpinionCoverProps.Append(tcWidthtcLegalOpinionCover);
                    tcLegalOpinionCover.Append(tcLegalOpinionCoverProps);
                    tcLegalOpinionCover.Append(sdtBlockparagraphMetaData);
                    TableCell tcLegalOpinionNameCover = new TableCell();
                    tcLegalOpinionNameCover.Append(sdtBlockparagraphMetaDataValue);
                    trLegalOpinionCover.Append(tcLegalOpinionCover);
                    trLegalOpinionCover.Append(tcLegalOpinionNameCover);
                    TblMetaDataInfo.Append(trLegalOpinionCover);


                }

                bodyoutputDoc.Append(TblMetaDataInfo);

                outputDoc.MainDocumentPart.Document.Save();
            }
            using (WordprocessingDocument outputDocbreak = WordprocessingDocument.Open(LegalOpinionOutput, true))
            {
                MainDocumentPart mainPartoutputDocbreak = outputDocbreak.MainDocumentPart;
                Paragraph para = new Paragraph(new Run((new Break() { Type = BreakValues.Page })));
                mainPartoutputDocbreak.Document.Body.InsertAfter(para, mainPartoutputDocbreak.Document.Body.LastChild);
            }

        }

        //Function to convert stream to byte[]
        private byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        //Method to set the custom properties of document
        private byte[] WDSetCustomProperty(byte[] fileName, string propertyName, object propertyValue, PropertyTypes propertyType)
        {

            string returnValue = null;

            var newProp = new CustomDocumentProperty();
            bool propSet = false;

            try
            {
                // Calculate the correct type:
                switch (propertyType)
                {
                    case PropertyTypes.DateTime:
                        // Verify that you were passed a real date, 
                        // and if so, format in the correct way. 
                        // The date/time value passed in should 
                        // represent a UTC date/time.
                        if ((propertyValue) is DateTime)
                        {
                            newProp.VTFileTime = new VTFileTime(string.Format(
                              "{0:s}Z", Convert.ToDateTime(propertyValue)));
                            propSet = true;
                        }

                        break;
                    case PropertyTypes.NumberInteger:
                        if ((propertyValue) is int)
                        {
                            newProp.VTInt32 = new VTInt32(propertyValue.ToString());
                            propSet = true;
                        }

                        break;
                    //case PropertyTypes.NumberDouble:
                    //    if (propertyValue is double)
                    //    {
                    //        newProp.VTFloat = new VTFloat(propertyValue.ToString());
                    //        propSet = true;
                    //    }

                    //    break;
                    case PropertyTypes.Text:
                        newProp.VTLPWSTR = new VTLPWSTR(propertyValue.ToString());
                        propSet = true;

                        break;
                    case PropertyTypes.YesNo:
                        if (propertyValue is bool)
                        {
                            // Must be lowercase.
                            newProp.VTBool = new VTBool(
                              Convert.ToBoolean(propertyValue).ToString().ToLower());
                            propSet = true;
                        }
                        break;
                }

                if (!propSet)
                {
                    // If the code could not convert the 
                    // property to a valid value, throw an exception:
                    throw new InvalidDataException("propertyValue");
                }

                // Now that you have handled the parameters,
                // work on the document.
                newProp.FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}";
                newProp.Name = propertyName;
                byte[] newByte;
                using (MemoryStream streamDocument = new MemoryStream())
                {
                    streamDocument.Write(fileName, 0, fileName.Length);
                    using (var document = WordprocessingDocument.Open(streamDocument, true))
                    {
                        var customProps = document.CustomFilePropertiesPart;
                        if (customProps == null)
                        {
                            // No custom properties? Add the part, and the
                            // collection of properties now.
                            customProps = document.AddCustomFilePropertiesPart();
                            customProps.Properties = new DocumentFormat.OpenXml.
                              CustomProperties.Properties();
                        }

                        var props = customProps.Properties;
                        if (props != null)
                        {
                            var prop = props.
                              Where(p => ((CustomDocumentProperty)p).
                                Name.Value == propertyName).FirstOrDefault();
                            // Does the property exist? If so, get the return value, 
                            // and then delete the property.
                            if (prop != null)
                            {
                                returnValue = prop.InnerText;
                                prop.Remove();
                            }

                            // Append the new property, and 
                            // fix all the property ID values. 
                            // The PropertyId value must start at 2.
                            props.AppendChild(newProp);
                            int pid = 2;
                            foreach (CustomDocumentProperty item in props)
                            {
                                item.PropertyId = pid++;
                            }
                            props.Save();
                        }
                        //document.MainDocumentPart.Document.Save();
                    }
                    WordProcessor obj = new WordProcessor();
                    streamDocument.Position = 0;
                    newByte = obj.ReadToEnd(streamDocument);
                }
                //streamDocument.Close();
                fileName = newByte;
                return newByte;
            }
            catch (Exception e)
            {
                returnValue = "Invalid Input";
                //return returnValue;
                return fileName;
            }
        }

        //Method to retrieve the custom properties
        private Dictionary<string, string> WDGetCustomProperty(byte[] fileName, params string[] listDocProperty)
        {
            string returnValue = null;
            Dictionary<string, string> metaDataDictionary = new Dictionary<string, string>();
            using (MemoryStream streamfileName = new MemoryStream(fileName))
            {

                streamfileName.Read(fileName, 0, fileName.Length);
                streamfileName.Position = 0;
                using (WordprocessingDocument document = WordprocessingDocument.Open(streamfileName, false))
                {
                    var customProps = document.CustomFilePropertiesPart;
                    var props = customProps.Properties;
                    if (props != null)
                    {
                        for (int i = 0; i < listDocProperty.Length; i++)
                        {
                            var prop = props.
                              Where(p => ((CustomDocumentProperty)p).
                                Name.Value == listDocProperty[i]).FirstOrDefault();
                            // Does the property exist? If so, get the return value, 
                            // and then delete the property.
                            if (prop != null)
                            {
                                returnValue = prop.InnerText;
                                metaDataDictionary.Add(listDocProperty[i], prop.InnerText);

                            }
                        }
                    }


                }
                //streamfileName.Close();
                //return returnValue;
            }
            return metaDataDictionary;
        }

        //Method to set Tracking to a document
        private byte[] OutputDocEnableTracking(byte[] fileName)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] OutputTackingDocument;
            //MemoryStream ms = new MemoryStream(500);
            using (MemoryStream streamfileName = new MemoryStream(fileName.Length))
            {
                streamfileName.Write(fileName, 0, fileName.Length);
                // ms = streamfileName;
                //streamfileName.CopyTo(ms);

                using (WordprocessingDocument docPart = WordprocessingDocument.Open(streamfileName, true))
                {

                    TrackRevisions newrevision = new TrackRevisions();

                    newrevision.Val = new DocumentFormat.OpenXml.OnOffValue(true);

                    docPart.MainDocumentPart.DocumentSettingsPart.Settings.AppendChild(newrevision);

                    docPart.MainDocumentPart.DocumentSettingsPart.Settings.Save();

                }

                OutputTackingDocument = objWordProcessor.ReadToEnd(streamfileName);

            }
            return OutputTackingDocument;
        }
        //Method to disable Tracking to a document
        private byte[] DisableTracking(byte[] fileName)
        {
            WordProcessor objWordProcessor = new WordProcessor();
            byte[] OutputTackingDocument;
            //MemoryStream ms = new MemoryStream(500);
            using (MemoryStream streamfileName = new MemoryStream(fileName.Length))
            {
                streamfileName.Write(fileName, 0, fileName.Length);
                // ms = streamfileName;
                //streamfileName.CopyTo(ms);

                using (WordprocessingDocument docPart = WordprocessingDocument.Open(streamfileName, true))
                {

                    TrackRevisions newrevision = new TrackRevisions();

                    newrevision.Val = new DocumentFormat.OpenXml.OnOffValue(true);

                    var trackRevisionSetting = docPart.MainDocumentPart.DocumentSettingsPart.Settings.Descendants<TrackRevisions>();
                    if (trackRevisionSetting != null && trackRevisionSetting.Any())
                    {
                        trackRevisionSetting.First().Remove();
                    }

                    docPart.MainDocumentPart.DocumentSettingsPart.Settings.Save();

                }

                OutputTackingDocument = objWordProcessor.ReadToEnd(streamfileName);

            }
            return OutputTackingDocument;
        }
        //Metohod to enter Header
        private void ApplyHeader(WordprocessingDocument doc, string inputText, bool isUpdate, bool shouldAddWatermark)
        {
            
            // Get the main document part.
            MainDocumentPart mainDocPart = doc.MainDocumentPart;
            //mainDocPart.DeleteParts<HeaderPart>(mainDocPart.HeaderParts);
            mainDocPart.DeleteParts(mainDocPart.HeaderParts);
            HeaderPart headerPart1 = mainDocPart.AddNewPart<HeaderPart>("r97");
            string rId = mainDocPart.GetIdOfPart(headerPart1);
            Header header1 = new Header();
            Paragraph paragraphText = new Paragraph(
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        )
                        );
            Paragraph paragraphLine = new Paragraph(
                new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        )
                        );
            string text1st = "";
            string text2nd = "";
            if (inputText.Split('/').Length > 1)
            {
                text1st = inputText.Split('/')[0];
                text2nd = inputText.Split('/')[1];
            }
            else
            {
                text1st = inputText;
            }
            Run run1 = new Run();
            RunProperties runPropsSection = new RunProperties();
            RunFonts runFont = new RunFonts();           // Create font
            runFont.Ascii = "Times New Roman";
            runPropsSection.Append(runFont);
            FontSize fontSizeSection = new FontSize() { Val = "16" };

            runPropsSection.Append(fontSizeSection);
            Bold boldSection = new Bold();
            runPropsSection.Append(boldSection);
            run1.AppendChild(new RunProperties(runPropsSection));

            Text text1 = new Text();
            text1.Text = text1st;
            run1.Append(text1);

            //Break br1 = new Break();
            //run1.Append(br);




            paragraphText.Append(run1);


            //
            Run runLglName = new Run();
            RunProperties runPropsLglName = new RunProperties();
            RunFonts runFontLglName = new RunFonts();           // Create font
            runFontLglName.Ascii = "Times New Roman";
            runPropsLglName.Append(runFontLglName);

            FontSize fontSizeLglName = new FontSize() { Val = "16" };
            runPropsLglName.Append(fontSizeLglName);
            Bold boldLglName = new Bold();
            runPropsLglName.Append(boldLglName);
            runLglName.AppendChild(new RunProperties(runPropsLglName));
            Break br2 = new Break();
            Text text2 = new Text();
            text2.Text = text2nd;
            runLglName.Append(text2, br2);


            Run runLine = new Run();
            RunProperties runPropsLine = new RunProperties();
            RunFonts runFontLine = new RunFonts();           // Create font
            runFontLine.Ascii = "Broadway";
            runPropsLine.Append(runFontLine);

            FontSize fontSizeLine = new FontSize() { Val = "16" };
            runPropsLine.Append(fontSizeLine);

            runLine.AppendChild(new RunProperties(runPropsLine));


            runLine.AppendChild(new Text("---------------------------------------------------------------------------------------------------------------------------------------------------"));



            paragraphLine.Append(runLglName, runLine);
            //paragraphText.Append(br2, runLine);

            //
            header1.Append(paragraphText);
            header1.Append(paragraphLine);
            headerPart1.Header = header1;
            if (isUpdate)
            {
                IEnumerable<DocumentFormat.OpenXml.Wordprocessing.SectionProperties> sectPrs = mainDocPart.Document.Body.Elements<SectionProperties>();

                foreach (var sectPr in sectPrs)
                {
                    //Delete existing references to headers.
                    sectPr.RemoveAllChildren<HeaderReference>();

                    // Create the new header reference node.
                    sectPr.PrependChild<HeaderReference>(new HeaderReference() { Id = rId });


                }
                if (shouldAddWatermark)
                {
                    WaterMark objWaterMark = new WaterMark();
                    objWaterMark.AddWatermark(doc);
                }
            }
            else
            {
                SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
                if (sectionProperties1 == null)
                {
                    sectionProperties1 = new SectionProperties(new PageMargin()
                    {
                        Top = 1440,
                        Right = (UInt32Value)2000UL,
                        Bottom = 1440,
                        Left = (UInt32Value)2000UL,
                        Header = (UInt32Value)720UL,
                        Footer = (UInt32Value)720UL,
                        Gutter = (UInt32Value)0UL
                    }) { };
                    mainDocPart.Document.Body.Append(sectionProperties1);
                    
                }
                HeaderReference headerReference1 = new HeaderReference() { Type = HeaderFooterValues.Default, Id = "r97" };
                sectionProperties1.InsertAt(headerReference1, 0);
            }            

        }
        //Method to enter the Footer
        private void ApplyFooter(WordprocessingDocument doc)
        {
            // Get the main document part.
            MainDocumentPart mainDocPart = doc.MainDocumentPart;
            mainDocPart.DeleteParts(mainDocPart.FooterParts);
            FooterPart footerPart1 = mainDocPart.AddNewPart<FooterPart>("r98");
            Footer footer1 = new Footer();
            //
            ParagraphProperties paragraphProperties = new ParagraphProperties(
                   new ParagraphStyleId() { Val = "Footer" },
                   new Tabs(
                       new TabStop() { Val = TabStopValues.Clear, Position = 4320 },
                       new TabStop() { Val = TabStopValues.Clear, Position = 8640 },
                       new TabStop() { Val = TabStopValues.Center, Position = 4820 },
                       new TabStop() { Val = TabStopValues.Right, Position = 9639 }));

            Paragraph paragraph = new Paragraph(

                        paragraphProperties,
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        ),
                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Right, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        ),
                        new Run(
                            new Text("") { }
                        ),
                        new SimpleField(
                            new Run(
                                new RunProperties(
                                    new NoProof()),
                                new Text("1")
                            )
                        ) { Instruction = " PAGE   \\* MERGEFORMAT " }
                    );
            //


            Paragraph paragraphText = new Paragraph(
                new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        )
                        );
            Paragraph paragraphLine = new Paragraph(
                new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        )
                        );

            Run runTextBold = new Run();
            RunProperties runPropsTextBold = new RunProperties();
            RunFonts runFontTextBold = new RunFonts();           // Create font
            runFontTextBold.Ascii = "Times New Roman";
            runPropsTextBold.Append(runFontTextBold);

            Bold boldSection = new Bold();
            runPropsTextBold.Append(boldSection);
            FontSize fontSizeTextBold = new FontSize() { Val = "16" };
            runPropsTextBold.Append(fontSizeTextBold);


            runTextBold.AppendChild(new RunProperties(runPropsTextBold));
            runTextBold.AppendChild(new Text("Classification"));


            //
            Run runText = new Run();
            RunProperties runPropsSectionText = new RunProperties();
            RunFonts runFontText = new RunFonts();           // Create font
            runFontText.Ascii = "Times New Roman";
            runPropsSectionText.Append(runFontText);

            FontSize fontSizeText = new FontSize() { Val = "16" };
            runPropsSectionText.Append(fontSizeText);

            runText.AppendChild(new RunProperties(runPropsSectionText));
            runText.AppendChild(new Text(": Confidential Information"));
            //

            //
            Run runLine = new Run();
            RunProperties runPropsLine = new RunProperties();
            RunFonts runFontLine = new RunFonts();           // Create font
            runFontLine.Ascii = "Broadway";
            runPropsLine.Append(runFontLine);

            FontSize fontSizeLine = new FontSize() { Val = "16" };
            runPropsLine.Append(fontSizeLine);

            runLine.AppendChild(new RunProperties(runPropsLine));
            runLine.AppendChild(new Text("---------------------------------------------------------------------------------------------------------------------------------------------------"));
            paragraphLine.Append(runLine);
            //

            paragraphText.Append(runTextBold);
            paragraphText.Append(runText);

            footer1.Append(paragraphLine);
            footer1.Append(paragraph);
            footer1.Append(paragraphText);
            footerPart1.Footer = footer1;

            SectionProperties sectionProperties1 = mainDocPart.Document.Body.Descendants<SectionProperties>().FirstOrDefault();
            if (sectionProperties1 == null)
            {
                sectionProperties1 = new SectionProperties() { };
                mainDocPart.Document.Body.Append(sectionProperties1);
            }
            FooterReference footerReference1 = new FooterReference() { Type = DocumentFormat.OpenXml.Wordprocessing.HeaderFooterValues.Default, Id = "r98" };


            sectionProperties1.InsertAt(footerReference1, 0);

        }


        //
        private string getAnswerTag(string questionNumber)
        {
            try
            {
                //var data = questionNumber.Split(' ');
                //var questionData = data[1].Split('.');
                var questionData = questionNumber.Split('.');
                return ("Section_" + questionData[0] + "_Answer_" + questionData[1]);

            }
            catch (Exception)
            {

                throw;
            }
        }
        //
        private string getTheQuestionTag(string questionNumber)
        {
            try
            {
                //var data = questionNumber.Split(' ');
                //var questionData = data[1].Split('.');
                var questionData = questionNumber.Split('.');
                return ("Section_" + questionData[0] + "_Question_" + questionData[1]);

            }
            catch (Exception)
            {

                throw;
            }
        }

        //
        private void CreateComparisonReportCoverPage(MemoryStream LegalOpinionOutput, Dictionary<string, byte[]> LawFirmsDocument)
        {
            Dictionary<string, string> lawfirmDictionary = new Dictionary<string, string>();
            lawfirmDictionary.Add("Law Firms selected for comparison:", "");
            lawfirmDictionary.Add("Law Firm", "Jurisdiction");
            Dictionary<string, string> metaDataDictionary = new Dictionary<string, string>();
            string LegalOpinionName = "";
            string LegalOpinionDesc = "";
            try
            {
                foreach (KeyValuePair<string, byte[]> firmDoc in LawFirmsDocument)
                {
                    Dictionary<string, string> customPropsDictionary = WDGetCustomProperty(firmDoc.Value, "LegalOpinionName", "LegalOpinionDesc", "LawFirmName", "JurisdictionName");

                    LegalOpinionName = customPropsDictionary.FirstOrDefault(x => x.Key == "LegalOpinionName").Value;
                    LegalOpinionDesc = customPropsDictionary.FirstOrDefault(x => x.Key == "LegalOpinionDesc").Value;
                    string LawFirmName = customPropsDictionary.FirstOrDefault(x => x.Key == "LawFirmName").Value;
                    string JurisdictionName = customPropsDictionary.FirstOrDefault(x => x.Key == "JurisdictionName").Value;
                    lawfirmDictionary.Add(LawFirmName, JurisdictionName);
                }
                //--
                string dateSubmitted = System.DateTime.Today.ToLongDateString();//.ToString();
                metaDataDictionary.Add("Report Title:", "... Legal Opinion Comparison Report");
                metaDataDictionary.Add("Date Created:", dateSubmitted);
                metaDataDictionary.Add("Report Purpose:", "Comparison multiple Law Firm answers for a single question from a ... Legal Opinion.");
                metaDataDictionary.Add("Legal Opinion:", LegalOpinionName);
                metaDataDictionary.Add("", LegalOpinionDesc);

                //--
                using (WordprocessingDocument outputDoc = WordprocessingDocument.Open(LegalOpinionOutput, true))
                {

                    MainDocumentPart mainPart = outputDoc.MainDocumentPart;
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    Body bodyoutputDoc = new Body();
                    Paragraph paragraph = new Paragraph();
                    Run run_paragraph = new Run();
                    paragraph.Append(run_paragraph);
                    bodyoutputDoc.Append(paragraph);

                    mainPart.Document.Append(bodyoutputDoc);
                    mainPart.Document.Save();
                    #region GenericReport Details
                    //--Cover Page Entry starts
                    Table TblGenericDataInfo = new Table();
                    // Set the style and width for the table.
                    TableProperties tableProp = new TableProperties();


                    foreach (string metaDataKey in metaDataDictionary.Keys)
                    {
                        //--Entry for the Meta Data Key starts
                        Run RunMetaData = new Run();
                        RunProperties runPropsMetaData = new RunProperties();
                        Bold boldMetaData = new Bold();
                        FontSize fontSizeMetaData = new FontSize() { Val = "28" };
                        runPropsMetaData.Append(boldMetaData);
                        runPropsMetaData.Append(fontSizeMetaData);

                        RunFonts runFontMetaData = new RunFonts();           // Create font
                        runFontMetaData.Ascii = "Times New Roman";
                        runPropsMetaData.Append(runFontMetaData);

                        RunMetaData.AppendChild(new RunProperties(runPropsMetaData));
                        RunMetaData.AppendChild(new Text(metaDataKey));
                        Paragraph paragraphMetaData = new Paragraph(RunMetaData);
                        SdtProperties sdtPrparagraphMetaData = new SdtProperties(
                                       new SdtAlias { Val = metaDataKey + "KeyTitle" },
                                       new Tag { Val = metaDataKey + "KeyTag" },
                                       new Lock { Val = LockingValues.ContentLocked },
                                       new Lock { Val = LockingValues.SdtLocked });
                        SdtContentBlock sdtCBlockparagraphMetaData = new SdtContentBlock(paragraphMetaData);
                        SdtBlock sdtBlockparagraphMetaData = new SdtBlock(sdtPrparagraphMetaData, sdtCBlockparagraphMetaData);
                        //-- Entry for the Titles ends
                        string metaDataValue = metaDataDictionary.FirstOrDefault(x => x.Key == metaDataKey).Value;
                        //--Entry for Meta Data Values starts
                        Run RunMetaDataValue = new Run();
                        RunProperties runPropsMetaDataValue = new RunProperties();
                        FontSize fontSizeMetaDataValue = new FontSize() { Val = "28" };
                        runPropsMetaDataValue.Append(fontSizeMetaDataValue);

                        RunFonts runFontMetaDataValue = new RunFonts();           // Create font
                        runFontMetaDataValue.Ascii = "Times New Roman";
                        runPropsMetaDataValue.Append(runFontMetaDataValue);

                        RunMetaDataValue.AppendChild(new RunProperties(runPropsMetaDataValue));
                        RunMetaDataValue.AppendChild(new Text(metaDataValue));
                        Paragraph paragraphMetaDataValue = new Paragraph(RunMetaDataValue);
                        SdtProperties sdtPrparagraphMetaDataValue = new SdtProperties(
                                       new SdtAlias { Val = metaDataKey + "ValueTitle" },
                                       new Tag { Val = metaDataKey + "ValueTag" },
                                       new Lock { Val = LockingValues.ContentLocked },
                                       new Lock { Val = LockingValues.SdtLocked });
                        SdtContentBlock sdtCBlockparagraphMetaDataValue = new SdtContentBlock(paragraphMetaDataValue);
                        SdtBlock sdtBlockparagraphMetaDataValue = new SdtBlock(sdtPrparagraphMetaDataValue, sdtCBlockparagraphMetaDataValue);
                        //-- Entry for Meta Data Values

                        TableRow trLegalOpinionCover = new TableRow();
                        TableCell tcLegalOpinionCover = new TableCell();

                        TableCellProperties tcLegalOpinionCoverProps = new TableCellProperties();
                        TableCellWidth tcWidthtcLegalOpinionCover = new TableCellWidth() { Width = "1500", Type = TableWidthUnitValues.Pct };
                        tcLegalOpinionCoverProps.Append(tcWidthtcLegalOpinionCover);
                        tcLegalOpinionCover.Append(tcLegalOpinionCoverProps);

                        tcLegalOpinionCover.Append(sdtBlockparagraphMetaData);
                        TableProperties tablePropLegalOpinionCover = new TableProperties();
                        TableWidth tableWidthLegalOpinionCover = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                        tablePropLegalOpinionCover.Append(tableWidthLegalOpinionCover);
                        TableCell tcLegalOpinionNameCover = new TableCell();
                        tcLegalOpinionNameCover.Append(sdtBlockparagraphMetaDataValue);
                        trLegalOpinionCover.Append(tcLegalOpinionCover);
                        TblGenericDataInfo.AppendChild(tablePropLegalOpinionCover);
                        trLegalOpinionCover.Append(tcLegalOpinionNameCover);
                        TblGenericDataInfo.Append(trLegalOpinionCover);


                    }
                    #endregion
                    #region Given LawFirms Details
                    Table TblLawFirmsDataInfo = new Table();
                    // Set the style and width for the table.
                    TableProperties tableLawFirmsProp = new TableProperties();


                    foreach (string lawfirmDataKey in lawfirmDictionary.Keys)
                    {
                        //--Entry for the LawFirm Data Key starts
                        Run RunLawFirmData = new Run();
                        RunProperties runPropsLawFirmData = new RunProperties();
                        Bold boldLawFirmData = new Bold();
                        FontSize fontSizeLawFirmData = new FontSize() { Val = "28" };
                        if (lawfirmDataKey == "Law Firms selected for comparison:" || lawfirmDataKey == "Law Firm")
                            runPropsLawFirmData.Append(boldLawFirmData);
                        runPropsLawFirmData.Append(fontSizeLawFirmData);

                        RunFonts runFontLawFirmData = new RunFonts();           // Create font
                        runFontLawFirmData.Ascii = "Times New Roman";
                        runPropsLawFirmData.Append(runFontLawFirmData);

                        RunLawFirmData.AppendChild(new RunProperties(runPropsLawFirmData));
                        RunLawFirmData.AppendChild(new Text(lawfirmDataKey));
                        Paragraph paragraphLawFirmData = new Paragraph(RunLawFirmData);
                        SdtProperties sdtPrparagraphLawFirmData = new SdtProperties(
                                       new SdtAlias { Val = lawfirmDataKey + "KeyTitle" },
                                       new Tag { Val = lawfirmDataKey + "KeyTag" },
                                       new Lock { Val = LockingValues.ContentLocked },
                                       new Lock { Val = LockingValues.SdtLocked });
                        SdtContentBlock sdtCBlockparagraphLawFirmData = new SdtContentBlock(paragraphLawFirmData);
                        SdtBlock sdtBlockparagraphLawFirmData = new SdtBlock(sdtPrparagraphLawFirmData, sdtCBlockparagraphLawFirmData);
                        //-- Entry for the Titles ends
                        string lawfirmDataValue = lawfirmDictionary.FirstOrDefault(x => x.Key == lawfirmDataKey).Value;
                        //--Entry for LawFirm Data Values starts
                        Run RunLawFirmDataValue = new Run();
                        RunProperties runPropsLawFirmDataValue = new RunProperties();

                        Bold boldLawFirmDataValue = new Bold();
                        if (lawfirmDataValue == "Jurisdiction")
                            runPropsLawFirmDataValue.Append(boldLawFirmDataValue);

                        FontSize fontSizeLawFirmDataValue = new FontSize() { Val = "28" };
                        runPropsLawFirmDataValue.Append(fontSizeLawFirmDataValue);

                        RunFonts runFontLawFirmDataValue = new RunFonts();           // Create font
                        runFontLawFirmDataValue.Ascii = "Times New Roman";
                        runPropsLawFirmDataValue.Append(runFontLawFirmDataValue);

                        RunLawFirmDataValue.AppendChild(new RunProperties(runPropsLawFirmDataValue));
                        RunLawFirmDataValue.AppendChild(new Text(lawfirmDataValue));
                        Paragraph paragraphLawFirmDataValue = new Paragraph(RunLawFirmDataValue);
                        SdtProperties sdtPrparagraphLawFirmDataValue = new SdtProperties(
                                       new SdtAlias { Val = lawfirmDataKey + "ValueTitle" },
                                       new Tag { Val = lawfirmDataKey + "ValueTag" },
                                       new Lock { Val = LockingValues.ContentLocked },
                                       new Lock { Val = LockingValues.SdtLocked });
                        SdtContentBlock sdtCBlockparagraphLawFirmDataValue = new SdtContentBlock(paragraphLawFirmDataValue);
                        SdtBlock sdtBlockparagraphLawFirmDataValue = new SdtBlock(sdtPrparagraphLawFirmDataValue, sdtCBlockparagraphLawFirmDataValue);
                        //-- Entry for Meta Data Values

                        TableRow trLegalOpinionCover = new TableRow();
                        TableCell tcLegalOpinionCover = new TableCell();

                        TableCellProperties tcLegalOpinionCoverProps = new TableCellProperties();
                        TableCellWidth tcWidthtcLegalOpinionCover = new TableCellWidth() { Width = "2500", Type = TableWidthUnitValues.Pct };
                        tcLegalOpinionCoverProps.Append(tcWidthtcLegalOpinionCover);
                        tcLegalOpinionCover.Append(tcLegalOpinionCoverProps);

                        tcLegalOpinionCover.Append(sdtBlockparagraphLawFirmData);
                        TableProperties tablePropLegalOpinionCover = new TableProperties();
                        TableWidth tableWidthLegalOpinionCover = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };
                        tablePropLegalOpinionCover.Append(tableWidthLegalOpinionCover);
                        TableCell tcLegalOpinionNameCover = new TableCell();
                        tcLegalOpinionNameCover.Append(sdtBlockparagraphLawFirmDataValue);
                        trLegalOpinionCover.Append(tcLegalOpinionCover);
                        TblLawFirmsDataInfo.AppendChild(tablePropLegalOpinionCover);
                        trLegalOpinionCover.Append(tcLegalOpinionNameCover);
                        TblLawFirmsDataInfo.Append(trLegalOpinionCover);


                    }
                    #endregion

                    bodyoutputDoc.Append(TblGenericDataInfo);
                    bodyoutputDoc.Append(TblLawFirmsDataInfo);
                    //--Cover Page Entry ends

                }
                using (WordprocessingDocument outputDocbreak = WordprocessingDocument.Open(LegalOpinionOutput, true))
                {
                    MainDocumentPart mainPartoutputDocbreak = outputDocbreak.MainDocumentPart;
                    Paragraph para = new Paragraph(new Run((new Break() { Type = BreakValues.Page })));
                    mainPartoutputDocbreak.Document.Body.InsertAfter(para, mainPartoutputDocbreak.Document.Body.LastChild);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }

    public static class OpenXMLExtensions
    {
        public static OpenXmlElement AcceptRevision(this OpenXmlElement element)
        {
            List<OpenXmlElement> changes =
                        element.Descendants<ParagraphPropertiesChange>().Cast<OpenXmlElement>().ToList();

            foreach (OpenXmlElement change in changes)
            {
                change.Remove();
            }

            // Handle the deletions.
            List<OpenXmlElement> deletions =
                element.Descendants<Deleted>().Cast<OpenXmlElement>().ToList();

            deletions.AddRange(element.Descendants<DeletedRun>().Cast<OpenXmlElement>().ToList());

            deletions.AddRange(element.Descendants<DeletedMathControl>().Cast<OpenXmlElement>().ToList());

            foreach (OpenXmlElement deletion in deletions)
            {
                deletion.Remove();
            }

            // Handle the insertions.
            List<OpenXmlElement> insertions =
                element.Descendants<Inserted>().Cast<OpenXmlElement>().ToList();

            insertions.AddRange(element.Descendants<InsertedRun>().Cast<OpenXmlElement>().ToList());

            insertions.AddRange(element.Descendants<InsertedMathControl>().Cast<OpenXmlElement>().ToList());

            foreach (OpenXmlElement insertion in insertions)
            {
                // Found new content.
                // Promote them to the same level as node, and then delete the node.
                OpenXmlElement curreElement = insertion;
                foreach (var run in insertion.Elements<Run>())
                {
                    curreElement = curreElement.InsertAfterSelf(new Run(run.OuterXml));
                }
                insertion.RemoveAttribute("rsidR",
                    "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                insertion.RemoveAttribute("rsidRPr",
                    "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
                insertion.Remove();
            }
            return element;
        }
    }
}
