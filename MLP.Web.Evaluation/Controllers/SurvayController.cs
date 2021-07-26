using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MLP.DAL;
using MLP.Web.Evaluation.Models;

namespace MLP.Web.Evaluation.Controllers
{
  
    public class SurvayController : Controller
    {
        private MLPDB01Entities db = new MLPDB01Entities();
        //[Route("../Survay/SurvayPage/{CustomerID}")]
        // GET: Customers

       
        public ActionResult SurvayPage(string Token)
        {
            var Customer = db.LoginLogs.FirstOrDefault(s => s.Token == Token);
           
           
            if (Customer != null)
            {
                int CustomerID = Customer.FK_CustomerID;
                var dbCustomer = db.Customers.FirstOrDefault(s => s.ID == CustomerID);

                SurvayViewModel NewModel = new SurvayViewModel();
                SurvayEvaluation Survay = new SurvayEvaluation() { FK_CustomerID = CustomerID, EvaluationDate = DateTime.Now, EvaluationTime = DateTime.Now.TimeOfDay, Email = dbCustomer.Email!=null? dbCustomer.Email:"",Name= dbCustomer.FirstName+" "+ dbCustomer.LastName,Telephone= dbCustomer.Mobile,CustomerMobile= dbCustomer.Mobile };
                NewModel.Questions = db.SurvayQuestions.Where(s => s.IsActive == true).ToList();
                foreach (var item in NewModel.Questions)
                {
                    SurvayAnswer CurrentAnswer = new SurvayAnswer();
                    CurrentAnswer.FK_QuestionID = item.ID;
                    CurrentAnswer.AnswerDate = DateTime.Now;
                    Survay.SurvayAnswers.Add(CurrentAnswer);
                }
                db.SurvayEvaluations.Add(Survay);
                db.SaveChanges();
                NewModel.Evaluation = Survay;
                return View(NewModel);
            }
            else
            {
                return View("Errorpage");
            }
        }

        [HttpPost]
        public ActionResult SurvayPage(SurvayViewModel NewModel)
        {
            var CurrentEvaluation = db.SurvayEvaluations.OrderByDescending(s=>s.EvaluationDate).OrderByDescending(p => p.EvaluationTime).FirstOrDefault(s => s.FK_CustomerID == NewModel.Evaluation.FK_CustomerID);
            CurrentEvaluation.EvaluationDate = DateTime.Now;
            CurrentEvaluation.EvaluationTime = DateTime.Now.TimeOfDay;
            CurrentEvaluation.Email = NewModel.Evaluation.Email;
            CurrentEvaluation.CustomerMobile = db.Customers.FirstOrDefault(s => s.ID == NewModel.Evaluation.FK_CustomerID).Mobile;
            CurrentEvaluation.Name = NewModel.Evaluation.Name;
            CurrentEvaluation.Nationality = NewModel.Evaluation.Nationality;
            CurrentEvaluation.Telephone = NewModel.Evaluation.Telephone;
            CurrentEvaluation.TypeOfCar = NewModel.Evaluation.TypeOfCar;
            db.SaveChanges();
            var EmptyList= db.SurvayEvaluations.Where(s => s.FK_CustomerID == NewModel.Evaluation.FK_CustomerID && s.Nationality == null).ToList();
            foreach (var item in EmptyList)
            {
                db.SurvayAnswers.RemoveRange(db.SurvayAnswers.Where(s=>s.FK_EvaluaionID==item.ID).ToList());

                db.SurvayEvaluations.Remove(item);
                db.SaveChanges();
            }
            return View("Thanks");
        }

        public JsonResult SaveAnswers(int QuestionID, int EvaluationID, string Value)
        {
            var CurrentQuestion = db.SurvayQuestions.FirstOrDefault(s => s.ID == QuestionID);
            if (CurrentQuestion!=null)
            {
                var CurrentAnswer = db.SurvayAnswers.FirstOrDefault(s => s.FK_EvaluaionID == EvaluationID && s.FK_QuestionID == QuestionID);
                if (CurrentQuestion.ValueType==1)
                {
                    CurrentAnswer.AnswerValue = int.Parse(Value);
                }
                else
                {
                    CurrentAnswer.AnswerTxtValue = Value;
                }
                CurrentAnswer.AnswerDate = DateTime.Now;
                db.SaveChanges();

                return Json(db.SurvayAnswers.Count(s=>s.FK_EvaluaionID==EvaluationID&&s.FK_QuestionID==QuestionID&&s.AnswerTxtValue==null&&s.AnswerValue==null), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveComments(int QuestionID, int EvaluationID, string Value)
        {
            var CurrentQuestion = db.SurvayQuestions.FirstOrDefault(s => s.ID == QuestionID);
            if (CurrentQuestion != null)
            {
                var CurrentAnswer = db.SurvayAnswers.FirstOrDefault(s => s.FK_EvaluaionID == EvaluationID && s.FK_QuestionID == QuestionID);
                CurrentAnswer.Comments = Value;
                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
