using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project16_MailRegisterAndConfirmaiton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Db16ProjectEntities context = new Db16ProjectEntities();
        private void btnRegister_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            int confirmCode = random.Next(100000, 999999);
            TblUser user = new TblUser();
            user.Name = txtName.Text;
            user.Surname = txtSurname.Text;
            user.Email = txtEmail.Text;
            user.Password = txtPassword.Text;
            user.IsConfirmed = false;
            user.ConfirmCode = confirmCode.ToString();


            context.TblUsers.Add(user);
            context.SaveChanges();



            #region MailKodlari
                 MimeMessage mimeMessage = new MimeMessage();
                 MailboxAddress mailboxAddressFrom = new MailboxAddress("AdminRegister", "yolarhuseyin@gmail.com");
                mimeMessage.From.Add(mailboxAddressFrom); //dogrulama kodunu göderen adminin adresini yazdık ve adminden gönderileceğini beliirttik

                MailboxAddress mailboxAddressTo = new MailboxAddress("User",txtEmail.Text); 
                //kime mail gönderileceğini yazdık 
                mimeMessage.To.Add(mailboxAddressTo);

                //emailin içeriğini yazıcaz bodybuilder ile body 

                var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Email adresinizin konfiramsyon kodu: " + confirmCode;
            mimeMessage.Body=bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "Email Konfirmasyon Kodu";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("yolarhuseyin@gmail.com", "wxryeyykidfypwqv");
            smtpClient.Send(mimeMessage);   
            smtpClient.Disconnect(true);

            MessageBox.Show("Doğrulama kodu mesaj kutunuza gönderilmiştir");

            FrmMailConfirm frmMailConfirm = new FrmMailConfirm();

            frmMailConfirm.email=txtEmail.Text; 
            frmMailConfirm.ShowDialog();

            #endregion
        }
    }
}
