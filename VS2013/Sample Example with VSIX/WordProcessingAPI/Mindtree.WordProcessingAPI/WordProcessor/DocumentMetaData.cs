using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindtree.WordProcessingAPI.WordProcessor
{

    /// <summary>
    ///  This Class will act as a container for all the metadata for the a given Document Template
    /// </summary>
    public partial class DocumentMetaData
    {
        public DocumentMetaData()
        {
        }

        public DocumentType DocumentType { get; set; }

        public string OpinionId { get; set; }

        public int CompanyId { get; set; }

        public int OpinionDocumentVersion { get; set; }

        public bool IsDocumentPreparedForExecution { get; set; }

        public bool IsDocumentExecuted { get; set; }

        public string CompanyName { get; set; }

        public string JurisdictionName { get; set; }

        public string OpinionName { get; set; }

        public string OpinionDescription { get; set; }

        public string SubmissionDate { get; set; }

        public string SubmittedBy { get; set; }

        public int NumOfQuestions { get; set; }

        public int NumOfQuestionsCompleted { get; set; }

        public int NumOfQuestionsAnswered { get; set; }

    }


    /// <summary>
    /// This enum is used to specify the document types
    /// </summary>
    public enum DocumentType
    {
        INVALID_DOCUMENT = 0,
        GENERIC_QUESTION_TEMPLATE = 1,
        OPINION_SPECIFIC_QUESTION_TEMPLATE = 2,
        ANSWER_TEMPLATE = 3
    }
}
