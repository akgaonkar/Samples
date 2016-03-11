using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace ValueExtractor
{
    public partial class MainUI : Form
    {
        CookieContainer container;

        public MainUI()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            InputFileDialog.ShowDialog();
            txtInputFile.Text = InputFileDialog.FileName;
                     
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnBrowse.Enabled = false;
                lblMessage.Text = "";

                //validate and parse the file
                IEnumerable<string> inputValues = ValidateAndParseInputvalues();

                if (inputValues != null)
                {
                    //Start task for each value
                    List<List<string>> outputValues = new List<List<string>>();
                                    
                    
                    SetSessionCookieContainer();
                    

                    string content, authorisedCapital, paidUpCapital;
                    var data = "taskID=9412&method=find&cmpnyname=&cmpnyID=#CIN#";
                    //var message = "# out of " + inputValues.Where(v => v != null).Count() + ": ##...";
                    //int count = 1;
                    foreach (var cinNumber in inputValues.Where(v => v != null))
                    {
                        //message = message.Replace("#", count.ToString()).Replace("##", cinNumber);
                        //lblMessage.Text = message;
                        

                        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data.Replace("#CIN#",cinNumber));
                        HttpWebRequest postRequest = (HttpWebRequest)WebRequest.Create(@"http://mca.gov.in/DCAPortalWeb/dca/CompanyMaster.do");
                        postRequest.Method = "POST";
                        postRequest.ContentType = "application/x-www-form-urlencoded"; 
                        postRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                        postRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";
                        postRequest.Referer = @"http://mca.gov.in/DCAPortalWeb/dca/MyMCALogin.do?method=setDefaultProperty&mode=31";                        
                        postRequest.ContentLength = buffer.Length;

                        postRequest.CookieContainer = container;

                        Stream PostData = postRequest.GetRequestStream();
                        PostData.Write(buffer, 0, buffer.Length);
                        PostData.Close();

                        HttpWebResponse postResponse = (HttpWebResponse)postRequest.GetResponse();
                        content = new StreamReader(postResponse.GetResponseStream()).ReadToEnd();

                        authorisedCapital = GetValueOf("Authorised Capital(in Rs.)", content);
                        paidUpCapital = GetValueOf("Paid up capital(in Rs.)", content);

                        outputValues.Add(new List<string>() { cinNumber, authorisedCapital, paidUpCapital });
                        //count++;                        
                    }

                    lblMessage.Text = "Writing to output file...";
                    //write to output file
                    WriteToOutputFile(outputValues);
                }
                
            }
            catch (FileNotFoundException)
            {
                lblMessage.Text = "Selected file no longer exists or is not accessible.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnBrowse.Enabled = true;
            }

        }

        private string GetValueOf(string key, string content)
        {
            int index = content.IndexOf(key) + 31;
            content = content.Remove(0, index + 1);
            index = content.IndexOf("</td>");
            content = content.Substring(0, index);
            index = content.LastIndexOf(";");
            content = content.Remove(0, index + 1);

            return content.Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }
        private void SetSessionCookieContainer()
        {
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(@"http://mca.gov.in/DCAPortalWeb/dca/MyMCALogin.do?method=setDefaultProperty&mode=31");
            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();

            var cookie = new Cookie(); //TODO: can be created in on statement. that way might not need this faunction
            cookie.Name = "JSESSIONID";
            cookie.Path = "/";
            cookie.Domain = "mca.gov.in";
            cookie.Value = getResponse.Headers["Set-Cookie"].Split(';')[0].Replace("JSESSIONID=","");
            cookie.Expires = DateTime.MaxValue;

            CookieContainer cnt = new CookieContainer();
            cnt.Add(cookie);
            container = cnt;            
        }

        private void WriteToOutputFile(List<List<string>> outputValues)
        {
            var fileName = InputFileDialog.FileName;
            fileName = fileName.Replace(Path.GetExtension(fileName),".txt");
            
            foreach (var item in outputValues)
            {
                File.AppendAllLines(fileName, item);
            }

            lblMessage.Text = "Done, check file - " + fileName;
            
        }

        private IEnumerable<string> ValidateAndParseInputvalues()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(InputFileDialog.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            var inputValues = ((object[,])xlWorkSheet.get_Range("A1", "A200").Value2).Cast<string>(); // TODO: MOve A1 & A200 to config.

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            return inputValues;
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }        
    }
}
