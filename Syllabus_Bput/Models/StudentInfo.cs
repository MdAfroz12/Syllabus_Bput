using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentRepo.Models
{
    public class StudentInfo
    {
        public string COR_TITLE { get; set; }
        public string BR_TITLE { get; set; }
        public string Semester { get; set; }
        public string SubjectCODE { get; set; }
        public string SubjectTP { get; set; }
        public string Reg_Le_Code { get; set; }
        public string SubType { get; set; }
        public string Credits { get; set; }
        public string MaxInternalMarks { get; set; }
        public string InternalPassMarks { get; set; }
        public string MaxExternalMarks { get; set; }
        public string ExternalPassMarks { get; set; }
        public string MaxPracticalMarks { get; set; }
        public string PracticalPassMarks { get; set; }
        public string PracticalCredits { get; set; }
        public string TotalCredits { get; set; }
        public string BatchFrom { get; set; }
        public string SubjectName { get; set; }

    }
}