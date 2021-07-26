using MLP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLP.Web.Evaluation.Models
{
    public class SurvayViewModel
    {
        public SurvayEvaluation Evaluation { get; set; }
       // public List<SurvayAnswer> Answers { get; set; }

        public List<SurvayQuestion> Questions { get; set; }


    }
}