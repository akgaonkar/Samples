using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordProcessingAPI
{
    public partial class DocumentMetaData 
    {
        public DocumentMetaData()
        {           
        }

            public DocumentType DocumentType { get; set; }

            public string OpinionId { get; set; }

            public int LawFirmId { get; set; }

            public int OpinionDocumentVersion { get; set; }

            public bool IsDocumentPreparedForExecution { get; set; }

            public bool IsDocumentExecuted { get; set; }

            public string LawFirmName { get; set; }

            public string JurisdictionName { get; set; }

            public string OpinionName { get; set; }

            public string OpinionDescription { get; set; }

            public string SubmissionDate { get; set; }

            public string SubmittedBy { get; set; }

            //public CoverPageDetails CoverPageDetails { get; set; }
        
            public int NumOfQuestions { get; set; }
        
            public int NumOfQuestionsCompleted { get; set; }
        
            public int NumOfQuestionsAnswered { get; set; }
        
    }

    public enum DocumentType
    {
        INVALID_DOCUMENT = 0,
        GENERIC_QUESTION_TEMPLATE = 1,
        OPINION_SPECIFIC_QUESTION_TEMPLATE = 2,
        ANSWER_TEMPLATE = 3
    }
}
