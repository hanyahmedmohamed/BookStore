using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;



namespace BookStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "hany.a.farag1@gmail.com,hany_a_farag@yahoo.com";
        public string MailFromAddress = "hany.a.farag1@gmail.com";
        //SECURE SOCKET LAYAR
        public bool UseSsl = true;
        public string UserName = "hany.a.farag1@gmail.com";
        public string Password = "3268530602Fcih";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        //el file elly hay save feh
        public string FileLocation = @"c:\
_bookstore_emails";

    }

    public class EmailOrderProcessor:IOrderProcessor
    {


        private EmailSettings emailSetting;
        public EmailOrderProcessor (EmailSettings setting)
        {
            emailSetting = setting;
        }
        public void ProcessOrder(Entities.Cart cart, Entities.ShippingDetails shippingDetails)
        {
            using(var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSetting.UseSsl;
                smtpClient.Host = emailSetting.ServerName;
                smtpClient.Port = emailSetting.ServerPort;
                //////////////////////////////////
                //smtpClient.Timeout = 10000;
                //hl ha4ta8l b el72o2 el3adia
                smtpClient.UseDefaultCredentials = false;
                //hay4ta8l 3la dol m4 3 el default
                smtpClient.Credentials = new
                    NetworkCredential(emailSetting.UserName, 
                    emailSetting.Password);
                if(emailSetting.WriteAsFile)
                {
                    //hadek file feh el emails tb3t mno
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    ///////
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    ////
                    smtpClient.PickupDirectoryLocation = emailSetting.FileLocation;
                    //talma feh file yb3t mno m4 haynf3 ykon feh ssl
                    smtpClient.EnableSsl = false;
                }
                //body el message
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("------------")
                .AppendLine("Books: ");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Book.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c} )",
                                                        line.Quantity, line.Book.Title, subtotal);
                }

                body.AppendFormat("Total order value : {0:c}", cart.ComputeTotalValue())
                    .AppendLine("----------")
                    .AppendLine("Ship to")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2)
                    .AppendLine(shippingDetails.State)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("-----------")
                    .AppendFormat("Gift wrap:{0} ", shippingDetails.GiftWrap ? "Yes" : "No");
                //byzbt el message b4akl kamel
                MailMessage mailMessage = new MailMessage(
                    emailSetting.MailFromAddress, emailSetting.MailToAddress,
                    "New order submited",
                    body.ToString()
                    );
                if (emailSetting.WriteAsFile)
                    mailMessage.BodyEncoding = Encoding.ASCII;
                try
                {
                    smtpClient.Send(mailMessage);
                }catch(Exception ex)
                {
                    Debug.Print(ex.Message);
                }

            }

        }

        EFDbContext context = new EFDbContext();
        public IEnumerable<ShippingDetails> Orders
        {
            get
            {
                return context.Orders
                    .Include(o => o.OrderLines
                      .Select(ol => ol.Order));
            }
        }

        public void SaveOrder(ShippingDetails order)
        {

            ShippingDetails dbOrder = context.Orders.Find(order.ShippingID);
            if (dbOrder == null)
                context.Orders.Add(order);

            else
            {
                dbOrder.Name = order.Name;
                dbOrder.Line1 = order.Line1;
                dbOrder.Line2 = order.Line2;

                dbOrder.City = order.City;
                dbOrder.State = order.State;
                dbOrder.GiftWrap = order.GiftWrap;

            }
            context.SaveChanges();

            ///////////////// from book ////////////////////////
            //if (order.ShippingID == 0)
            //{
            //    order = context.Orders.Add(order);
            //    foreach (OrderLine line in order.OrderLines)
            //    {
            //        context.Entry(line.Book).State = EntityState.Modified;

            //        //context.Entry(line.Book).State
            //        //= System.Data.EntityState.Modified;
            //    }
            //}
            //else
            //{
            //    ShippingDetails dbOrder = context.Orders.Find(order.ShippingID);
            //    if (dbOrder != null)
            //    {
            //        dbOrder.Name = order.Name;
            //        dbOrder.Line1 = order.Line1;
            //        dbOrder.Line2 = order.Line2;

            //        dbOrder.City = order.City;
            //        dbOrder.State = order.State;
            //        dbOrder.GiftWrap = order.GiftWrap;
            //    }
            //}
            //context.SaveChanges();
        }




    }
}
