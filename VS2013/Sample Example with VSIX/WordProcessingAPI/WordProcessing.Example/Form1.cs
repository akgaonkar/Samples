using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mindtree.WordProcessingAPI;
using System.IO;
using Mindtree.WordProcessingAPI.WordProcessor;

namespace WordProcessing.Example
{
    public partial class frmWordProcessing : Form
    {
        public frmWordProcessing()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (txtOutput.Text != "")
            {
                if (txtSHCount.Text != "" && txtQcount.Text != "")
                {
                    int SubHeadingNo = Convert.ToInt32(txtSHCount.Text);
                    int NumOfQuestions = Convert.ToInt32(txtQcount.Text);
                    //WordProcessing\WordProcessing.Tests\TestFiles
                    string LegalOpinionInput = txtOutput.Text + @"\QuestionTemplate.docx";//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                    bool isFileExit = File.Exists(LegalOpinionInput);

                    if (isFileExit)
                    {
                        File.Delete(LegalOpinionInput);
                    }
                    WordProcessor _wordProcessingService = new WordProcessor();
                    byte[] QuestionTemplate = _wordProcessingService.GetOpinionTemplate(SubHeadingNo, NumOfQuestions);

                    File.WriteAllBytes(LegalOpinionInput, QuestionTemplate);
                    if (isFileExit)
                    {
                        MessageBox.Show("Opinion Template Created", "Mindtree");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter the numbers", "Mindtree");
                }
            }
            else
            {
                MessageBox.Show("Please select the output location", "Mindtree");
            }
        }

        private void btnGenHeader_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {

                DocumentMetaData docMetaRecord = new DocumentMetaData(); ;
                docMetaRecord.OpinionName = txtName.Text;
                docMetaRecord.OpinionDescription = txtDesc.Text;

                string LegalOpinionInput = txtInput.Text;//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                string output = txtOutput.Text + @"\QuestionTemplateHeader.docx";

                //string LegalOpinionInput = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\2_GetOpinionTemplateWithHeader\Input\QuestionTemplate.docx";
                //string output = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\2_GetOpinionTemplateWithHeader\Output\QuestionTemplate.docx";
                bool isFileExit = File.Exists(output);

                if (isFileExit)
                {
                    File.Delete(output);
                }

                FileStream stream = File.OpenRead(LegalOpinionInput);
                byte[] DocumentQuestionTemplate = new byte[stream.Length];
                stream.Read(DocumentQuestionTemplate, 0, DocumentQuestionTemplate.Length);
                stream.Close();

                WordProcessor _wordProcessingService = new WordProcessor();
                byte[] QuestionTemplate = _wordProcessingService.GetOpinionTemplateWithHeader(DocumentQuestionTemplate, docMetaRecord);

                File.WriteAllBytes(output, QuestionTemplate);
                if (isFileExit)
                {
                    MessageBox.Show("Opinion Template with Header Created", "Mindtree");
                    txtName.Text = "";
                    txtDesc.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {

                DocumentMetaData docMetaRecord = new DocumentMetaData(); ;

                string LegalOpinionInput = txtInput.Text;//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                string output = txtOutput.Text + @"\QuestionTemplateUpdated.docx";

                //string LegalOpinionInput = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\3_CreateDeletedQuestionOutputOpinionTemplate\Input\QuestionTemplate.docx";
                //string output = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\3_CreateDeletedQuestionOutputOpinionTemplate\Output\QuestionTemplate.docx";

                bool isFileExit = File.Exists(output);
                if (isFileExit)
                {
                    File.Delete(output);
                }

                FileStream stream = File.OpenRead(LegalOpinionInput);
                byte[] DocumentQuestionTemplate = new byte[stream.Length];
                stream.Read(DocumentQuestionTemplate, 0, DocumentQuestionTemplate.Length);
                stream.Close();

                WordProcessor _wordProcessingService = new WordProcessor();
                int version = 2;
                byte[] QuestionTemplate = _wordProcessingService.UpdateOpinionTemplate(DocumentQuestionTemplate, version);
                File.WriteAllBytes(output, QuestionTemplate);
                if (isFileExit)
                {
                    MessageBox.Show("Document Updated", "Mindtree");

                }
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }


        }

        private void btnDistribute_Click(object sender, EventArgs e)
        {

            if (txtInput.Text != "" && txtOutput.Text != "")
            {

                string LegalOpinionInput = txtInput.Text;//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                string LegalOpinionOutput = txtOutput.Text + @"\AnswerTemplate.docx";

                //string LegalOpinionInput = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\5_GetDistributableDocument\Input\QuestionTemplate.docx";
                //string LegalOpinionOutput = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\5_GetDistributableDocument\Output\AnswerTemplate.docx";

                bool isFileExit = File.Exists(LegalOpinionOutput);
                if (isFileExit)
                {
                    File.Delete(LegalOpinionOutput);
                }
                DocumentMetaData metaData = new DocumentMetaData();
                metaData.CompanyName = txtCompany.Text;
                metaData.JurisdictionName = txtJurisdiction.Text;
                FileStream stream = File.OpenRead(LegalOpinionInput);
                byte[] DocumentQuestionTemplate = new byte[stream.Length];

                stream.Read(DocumentQuestionTemplate, 0, DocumentQuestionTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();

                byte[] fileBytesAnswerTemplate = _wordProcessingService.GetDistributableDocument(DocumentQuestionTemplate, metaData);
                File.WriteAllBytes(LegalOpinionOutput, fileBytesAnswerTemplate);
                if (isFileExit)
                {
                    MessageBox.Show("Document Created for Distribution", "Mindtree");
                    txtCompany.Text = "";
                    txtJurisdiction.Text = "";

                }
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {

                string LegalOpinionOutput = txtInput.Text;//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                string output = txtOutput.Text + @"\AnswerTemplate.docx";


                //string LegalOpinionOutput = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\6_ConvertAnswerTemplateForLegalUser\Input\AnswerTemplate.docx";

                // string output = @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\6_ConvertAnswerTemplateForLegalUser\Output\AnswerTemplate.docx";

                if (File.Exists(output))
                {
                    File.Delete(output);
                }
                FileStream stream = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream.Length];

                stream.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();
                byte[] fileBytesAnswerTemplate = _wordProcessingService.ConvertAnswerTemplateForSurveyUser(DocumentAnswerTemplate);
                File.WriteAllBytes(output, fileBytesAnswerTemplate);
                if (File.Exists(output))
                {
                    MessageBox.Show("Document converted successfully", "Mindtree");
                }
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "" && txtMerge.Text!="")
            {

                //string LegalOpinionOutput = txtInput.Text;

                //string output = txtOutput.Text + @"\AnswerTemplate.docx";

                string LegalOpinionInput = txtInput.Text;// @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\7_GetMergedDocumentWithAnswers\Input\QuestionTemplate.docx";
                string LegalOpinionOutput = txtMerge.Text;// @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\7_GetMergedDocumentWithAnswers\Input\AnswerTemplate.docx";
                string Output = txtOutput.Text + @"\AnswerTemplate.docx";// @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\7_GetMergedDocumentWithAnswers\Output\AnswerTemplate.docx";

                if (File.Exists(Output))
                {
                    File.Delete(Output);
                }
                FileStream stream1 = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream1.Length];
                stream1.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream1.Close();

                FileStream stream = File.OpenRead(LegalOpinionInput);
                byte[] DocumentQuestionTemplate = new byte[stream.Length];

                stream.Read(DocumentQuestionTemplate, 0, DocumentQuestionTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();
                byte[] fileBytesAnswerTemplate = _wordProcessingService.GetMergedDocumentWithAnswers(DocumentQuestionTemplate, DocumentAnswerTemplate);

                File.WriteAllBytes(Output, fileBytesAnswerTemplate);
                if (File.Exists(Output))
                {
                    MessageBox.Show("Document merged successfully", "Mindtree");
                }
            }
            else
            {
                MessageBox.Show("Please select the input file, output location and Merge document", "Mindtree");
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "")
            {

                DocumentMetaData obj = new DocumentMetaData();
                string LegalOpinionOutput = txtInput.Text;// @"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\10_GetDocumentMetadata\AnswerTemplate.docx";

                FileStream stream1 = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream1.Length];
                stream1.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream1.Close();
                WordProcessor _wordProcessingService = new WordProcessor();
                obj = _wordProcessingService.GetDocumentMetadata(DocumentAnswerTemplate);
                MessageBox.Show("Number of Question=" + obj.NumOfQuestions, "Mindtree");
                MessageBox.Show("Number of Completed Question=" + obj.NumOfQuestionsCompleted, "Mindtree");
                MessageBox.Show("Number of Question Answered=" + obj.NumOfQuestionsAnswered, "Mindtree");
                MessageBox.Show("Description=" + obj.OpinionDescription, "Mindtree");
                MessageBox.Show("Lawfirm Name=" + obj.CompanyName, "Mindtree");
                MessageBox.Show("Jurisdiction Name=" + obj.JurisdictionName, "Mindtree");
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }

        }

        private void btnPrepare_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {
                string LegalOpinionOutput = txtInput.Text;//"C:\SANJAY\R&D\WordProcessingAPI\TestFiles\11_PrepareDocumentForExecution\Input\AnswerTemplate.docx";
                string output = txtOutput.Text + @"\AnswerTemplate.docx";
                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                FileStream stream = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream.Length];

                stream.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();

                byte[] fileBytesAnswerTemplate = _wordProcessingService.PrepareDocumentForExecution(DocumentAnswerTemplate);
                File.WriteAllBytes(output, fileBytesAnswerTemplate);
                if (File.Exists(output))
                {
                    MessageBox.Show("Document Prepared successfully", "Mindtree");
                    txtInput.Text = "";
                    txtOutput.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Please select the input file and output location", "Mindtree");
            }


        }

        private void frmWordProcessing_Load(object sender, EventArgs e)
        {

        }

        private void btnBrowsPrep_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                txtInput.Text = file;
            }
        }

        private void btnExc_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {
                string LegalOpinionOutput = txtInput.Text;

                string output = txtOutput.Text + @"\AnswerTemplate.docx";
                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                FileStream stream = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream.Length];

                stream.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();
                //WordProcessor _wordProcessingService = new WordProcessor();
                byte[] fileBytesAnswerTemplate = _wordProcessingService.ExecuteDocument(DocumentAnswerTemplate);
                File.WriteAllBytes(output, fileBytesAnswerTemplate);
                if (File.Exists(output))
                {
                    MessageBox.Show("Executable Document generated successfully", "Mindtree");
                }
            }
            else
                MessageBox.Show("Please select the input file and output location", "Mindtree");
        }

        private void btnBrowseExc_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();// Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = folderBrowserDialog1.SelectedPath;
                txtOutput.Text = file;
            }

        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (txtInput.Text != "" && txtOutput.Text != "")
            {
                string LegalOpinionOutput = txtInput.Text;
                string output = txtOutput.Text + @"\AnswerTemplate.docx";
                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                FileStream stream = File.OpenRead(LegalOpinionOutput);
                byte[] DocumentAnswerTemplate = new byte[stream.Length];

                stream.Read(DocumentAnswerTemplate, 0, DocumentAnswerTemplate.Length);
                stream.Close();
                WordProcessor _wordProcessingService = new WordProcessor();
                //WordProcessor _wordProcessingService = new WordProcessor();
                byte[] fileBytesAnswerTemplate = _wordProcessingService.CreateRevertedExecuteDocument(DocumentAnswerTemplate);
                File.WriteAllBytes(output, fileBytesAnswerTemplate);
                if (File.Exists(output))
                {
                    MessageBox.Show("Document Reverted successfully", "Mindtree");
                }
            }
            else
                MessageBox.Show("Please select the input file and output location", "Mindtree");
        }

        private void btnBrowsMerg_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                txtMerge.Text = file;
            }
        }
        private void tab_Changed(object sender, EventArgs e)
        {
            string x = tabMenu.SelectedTab.Text;
            if (x == "Report")
            {
                txtInput.Enabled = false;
                btnBrowsPrep.Enabled = false;

            }
            else
            {
                txtInput.Enabled = true;
                btnBrowsPrep.Enabled = true;
            }
        }
        private void btn_Click(object sender, EventArgs e)
        {

            if (txtOutput.Text != "")
            {
                string output = txtOutput.Text + @"\Report.docx";
                ArrayList fileList = new ArrayList();
                List<string> questionList = new List<string>();
                List<string> companyList = new List<string>();
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                WordProcessor _wordProcessingService = new WordProcessor();
                Dictionary<string, byte[]> testData = new Dictionary<string, byte[]>();

                if (result == DialogResult.OK) // Test result.
                {

                    foreach (string selectedQ in checkedListBox.CheckedItems)
                    {
                        questionList.Add(selectedQ);
                    }

                    int i = 0;
                    foreach (string file in openFileDialog1.FileNames)
                    {
                        FileStream stream1 = File.OpenRead(file);
                        byte[] CompanyDoc = new byte[stream1.Length];
                        stream1.Read(CompanyDoc, 0, CompanyDoc.Length);
                        stream1.Close();
                        DocumentMetaData obj = new DocumentMetaData();

                        obj = _wordProcessingService.GetDocumentMetadata(CompanyDoc);
                        //checkedListCompany.Items.Insert(i,obj.LawFirmName);
                        companyList.Add(obj.CompanyName);
                        i++;
                    }
                    foreach (string cmpanyName in companyList.Distinct<string>())
                    {
                        checkedListCompany.Items.Add(cmpanyName);
                    }
                    //checkedListBox.Items.Add(questionList);


                }
            }
            else
            {
                MessageBox.Show("Please select the output location", "Mindtree");
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (txtOutput.Text != "")
            {
                WordProcessor _wordProcessingService = new WordProcessor();
                Dictionary<string, byte[]> testData = new Dictionary<string, byte[]>();

                List<string> questionList = new List<string>();
                List<string> companyList = new List<string>();
                foreach (string questions in checkedListBox.CheckedItems)
                {
                    questionList.Add(questions);
                }
                foreach (string company in checkedListCompany.CheckedItems)
                {
                    companyList.Add(company);
                }
                string output = txtOutput.Text + @"\Report.docx";
                if (File.Exists(output))
                {
                    File.Delete(output);
                }

                if (openFileDialog1.FileNames.Count<string>() == companyList.Count)
                {
                    foreach (string file in openFileDialog1.FileNames)
                    {
                        foreach (string companyName in companyList)
                        {
                            FileStream stream1 = File.OpenRead(file);
                            byte[] CompanyDoc = new byte[stream1.Length];
                            stream1.Read(CompanyDoc, 0, CompanyDoc.Length);
                            stream1.Close();
                            testData.Add(companyName, CompanyDoc);
                        }

                    }
                    byte[] fileBytesAnswerTemplateReport = _wordProcessingService.CreateComparisonReportDocument(testData, questionList);
                    File.WriteAllBytes(output, fileBytesAnswerTemplateReport);
                    if (File.Exists(output))
                    {
                        MessageBox.Show("Report Created successfully", "Mindtree");
                        checkedListCompany.Items.Clear();
                        checkedListBox.ClearSelected();
                    }
                }
                else
                {
                    MessageBox.Show("Please select equal number of  file and Company names", "Mindtree");
                    checkedListCompany.Items.Clear();
                    checkedListBox.ClearSelected();
                }

            }
            else
                MessageBox.Show("Please select the output location for Report generation", "Mindtree");

        }






    }
}
