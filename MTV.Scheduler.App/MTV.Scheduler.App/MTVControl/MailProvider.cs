﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using MTV.Scheduler.App.UI;
using System.Net.Mime;

namespace MTV.Scheduler.App.MTVControl
{
    public static class MailProvider
    {
        public static bool IsValidEmail(string email)
        {
            var message = new MailMessage();
            bool f = false;
            try
            {
                message.To.Add(email);//use built in validator
            }
            catch
            {
                f = true;
            }
            message.Dispose();
            return !f;
        }


        public static bool Send(string to, string subject, string body)
        {
            return Send(to, subject, body, null);
        }

        public static bool Send(string to, string subject, string body, byte[] attach)
        {
            bool success = true;
            try
            {
                to = to.Replace(",", ";");

                string[] addrs = to.Split(';');

                var sendFrom = new MailAddress(MainForm.Conf.SMTPFromAddress.Trim());
                var sendTo = new MailAddress(addrs[0].Trim());

                string _LogoPath = string.Format(@"{0}\{1}", System.Windows.Forms.Application.StartupPath, @"Templates\images\Logo.jpg");
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (body, null, MediaTypeNames.Text.Html);

                // Create a LinkedResource object for each embedded image
                LinkedResource _companylogo = new LinkedResource(_LogoPath, MediaTypeNames.Image.Jpeg);
                _companylogo.ContentId = "companylogo";
                avHtml.LinkedResources.Add(_companylogo);

                var myMessage = new MailMessage(sendFrom, sendTo)
                {
                    Subject = subject.Replace(Environment.NewLine, " ").Trim(),
                    Body = body,
                    IsBodyHtml = true,
                };

                myMessage.AlternateViews.Add(avHtml);

                if (addrs.Length > 1)
                {
                    for (int i = 1; i < addrs.Length && i < 5; i++)
                    {
                        if (IsValidEmail(addrs[i].Trim()))
                            myMessage.Bcc.Add(new MailAddress(addrs[i].Trim()));
                    }
                }

                if (attach != null && attach.Length > 0)
                {
                    var attachFile = new Attachment(new MemoryStream(attach), "Screenshot.jpg");

                    myMessage.Attachments.Add(attachFile);
                }

                var emailClient = new SmtpClient(MainForm.Conf.SMTPServer, MainForm.Conf.SMTPPort)
                {
                    UseDefaultCredentials = false,
                    Credentials =
                        new NetworkCredential(MainForm.Conf.SMTPUsername, MainForm.Conf.SMTPPassword),
                    EnableSsl = MainForm.Conf.SMTPSSL
                };

                emailClient.Send(myMessage);

                myMessage.Dispose();
                myMessage = null;
                emailClient.Dispose();
                emailClient = null;

            }
            catch (Exception ex)
            {
                success = false;
                MainForm.LogExceptionToFile(ex);
            }
            return success;
        }

    }
}
