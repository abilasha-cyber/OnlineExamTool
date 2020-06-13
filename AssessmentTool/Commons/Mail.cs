using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AssessmentTool.Entities;
using System.Net.Mail;
using System.Net;

namespace AssessmentTool.Commons
{
    public class Mail
    {
        public static string SendMailToRecuritmentManager(List<AttemptedQuestion> attemptedQuestions, string user, int count)
        {
            decimal cal = Calculator.CalculateStudentQuizScore(attemptedQuestions);

            using (MailMessage mm = new MailMessage())
            {
                mm.Subject = "Candidate Score";
                //mm.Body = "hi";
                MailAddress fromaddress = new MailAddress("sabilasha50@gmail.com");
                mm.From = fromaddress;
                //Adding email id of receiver link
                mm.To.Add("hanuabilasha@gmail.com");
                //mm.IsBodyHtml = false;
                //             mm.Body = "<br/><br/>We are excited to tell you that your account is" +
                //" successfully created. Please click on the below link to verify your account" +
                //" <br/><br/><a href='" + verifyUrl + "'>" + verifyUrl + "</a> ";

                //string body = "Dear " + "Abilasha" + ",";
                string body = "Dear " + "Team" + ",";
                body += "<br /><br />Please find the Candidate Result";
                body += "<br />UserID : " + user + "";
                body += "<br />Score : " + cal + " / " + count;
                body += "<br /><br />Regards,";
                body += "<br />Innova Recruitment team";
                mm.Body = body;
                mm.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sabilasha50@gmail.com", "Lordshiva@70");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    return "sentmail";

                }

            }


        }



    }
}