using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WPFPro.DAL;
using WPFPro.Models;

namespace WPFPro.BLL
{
    public class BLLContactUs
    {
        DALContactUs dal = new DALContactUs();
        public bool InsertContactus(string contactName, string contactEmail, string mobilePhone, string note)
        {
            Contactus contact = new Contactus();            
            contact.contactName = contactName;
            contact.contactEmail = contactEmail;
            contact.mobilePhone = mobilePhone;
            contact.note = note;
            contact.createtime = DateTime.Now;
            bool res = dal.InsertContactus(contact);
            return res;
        }
    }
}