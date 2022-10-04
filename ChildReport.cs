using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SceneManagement;

public class ChildReport : MonoBehaviour
{
    public GameObject[] questionGroupArr;
    public QAClass[] qaArr;
    public GameObject AnswerPanel;

    // Start is called before the first frame update
    void Start()
    {
        qaArr = new QAClass[questionGroupArr.Length];
    }

    public void SubmitAnswer()
    {
        for (int i = 0; i < qaArr.Length; i++)
        {
            qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
        }

        for (int i = 0; i < qaArr.Length; i++)
        {
            //added debug log to check the content of qaarray and result after each question
            Debug.Log(qaArr[i].Question+qaArr[i].Answer);
        }

        EmailSender(qaArr);
        SceneManager.LoadScene("Menu");

    }
    public void HomeButton()
    {
        SceneManager.LoadScene("Menu");
    }

    QAClass ReadQuestionAndAnswer(GameObject questionGroup)
    {
        QAClass result = new QAClass();

       GameObject q = questionGroup.transform.Find("Question").gameObject;
       GameObject a = questionGroup.transform.Find("Answer").gameObject;

        result.Question = q.GetComponent<Text>().text;

        if (a.GetComponent<ToggleGroup>() != null)
        {
            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    result.Answer = a.transform.GetChild(i)
                                               .Find("Label")
                                               .GetComponent<Text>().text;
                    break;
                }
            }
        }
        else if(a.GetComponent<InputField>() != null)
        {
            result.Answer = a.transform.Find("Text").GetComponent<Text>().text;
        }
        else if(a.GetComponent<ToggleGroup>() == null
                && a.GetComponent<InputField> () == null)
        {
            string s = "";
            int counter = 0;
            for (int i = 0; i < a.transform.childCount; i++)
            {
                if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
                {
                    if (counter != 0)
                    {
                        s = s + ", ";
                    }
                    s = s + a.transform.GetChild(i).
                        Find("Label").GetComponent<Text>().text;
                    counter++;
                }
                if (i == a.transform.childCount - 1)
                {
                    s = s + ".";
                    break;
                }
            }
            result.Answer = s;
        }
        return result;
    }

    public void EmailSender(QAClass[] qaArr)
    {
        string recipient = "";
        String mailMsg = "Dear Parent/Guardian" + "<br/>"  + "Please find below the report on emotions of your child today." + "<br/>";    
        for (int i=0; i<qaArr.Length; i++)  
        {
            if (qaArr[i].Answer.Contains("@") && ValidateEmail(qaArr[i].Answer))
            {
                recipient = qaArr[i].Answer.Trim();
            }else
            {
                mailMsg = mailMsg + "<br/>" + "Q. " + qaArr[i].Question + "<br/>" + qaArr[i].Answer;
            }
        }

        string _sender = "childemotassessment@outlook.com";
        string _password = "gamefirsttry1$";
        if (recipient != null)
        {
            string subject = "Your child's emotion today!!";
            SmtpClient client = new SmtpClient("smtp.office365.com");
            client.Timeout = 10000;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_sender, _password);
            client.EnableSsl = true;
            client.Credentials = (System.Net.ICredentialsByHost)credentials;
            var mail = new MailMessage(_sender.Trim(), recipient.Trim());
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = mailMsg;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try {
                client.Send(mail);
            } catch(SmtpException e)
            {
                Console.Write("Error occurred.");
            }
            
            Debug.Log("Done");
        }
    }

    public bool ValidateEmail(String email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}


[System.Serializable]
public class QAClass
{
    public string Question = "";
    public string Answer = "";
}