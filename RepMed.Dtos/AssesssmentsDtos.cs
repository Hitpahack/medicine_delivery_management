using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepMed.Dtos
{
    public class BaseAssessmentsDtos
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Opt1 { get; set; }
        [Required]
        public string Opt2 { get; set; }
        [Required]
        public string Opt3 { get; set; }
        [Required]
        public string Opt4 { get; set; }

    }
    public class AppAssessmentsDtos : BaseAssessmentsDtos
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SelectedAns { get; set; }
        public bool? IsCorrect { get; set; }
        public string Comments { get; set; }
    }
    public class AssessmentsDtos : BaseAssessmentsDtos
    {
        [Required]
        public string CorectAns { get; set; }
    }

    public class EntityAssesssmentsDtos : AssessmentsDtos
    {
        public int Id { get; set; }

    }
    public class EntityAssesssmentsAnsDtos : AssessmentsDtos
    {
        public int Id { get; set; }

    }
    public class AssesssmentsScoreDtos
    {
        public IList<AppAssessmentsDtos> Assessments { get; set; }
        public int TotalQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public double Score { get; set; }
        public int Atttempts { get; set; }


    }


    public class AddAssessmentsDtos : AssessmentsDtos
    {


    }

    public class UpdateAssessmentsDtos : AssessmentsDtos
    {


    }
}
