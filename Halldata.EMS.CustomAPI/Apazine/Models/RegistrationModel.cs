using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;

namespace Halldata.EMS.CustomAPI.apazine.Models
{
    public class RegistrationModel
    {
        public RegistrationInfo Lookup(string emailaddress, string password, bool debug = false)
        {
            RegistrationInfo registrationinfo = new RegistrationInfo();

            string accountnumber = new DragonRegistrationInfo().Lookup(25, 4, emailaddress, password);

            if (accountnumber == "")
            {
                registrationinfo.Success = "False";
            }
            else
            {
                if (debug == true)
                {
                    DebugRegistrationInfo debugregistrationinfo = new DebugRegistrationInfo();
                    debugregistrationinfo = (DebugRegistrationInfo)new MasterFileRegistrationInfo().Lookup(accountnumber, debug);
                    debugregistrationinfo.AccountNumber = accountnumber;
                    return debugregistrationinfo;
                }

                registrationinfo = new MasterFileRegistrationInfo().Lookup(accountnumber, debug);
            }



            return registrationinfo;
        }
    }

    public class MasterFileRegistrationInfo
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        public RegistrationInfo Lookup(string acctnbr, bool debug = false)
        {
            int recordstatus = 0;
            int paidstatus = 0;

            string verdate = "";

            DateTime startissuedate = DateTime.Now;
            DateTime expireissuedate = DateTime.Now.AddDays(1);

            DateTime verificationdate;
            RegistrationInfo registrationinfo = new RegistrationInfo() { Success = "True" };

            StringBuilder sql = new StringBuilder(string.Format("SELECT P00180001 RECORDSTATUS, P03020001 PAYSTAT, P03060004 VERDATE, mf.FIRSTNAME, mf.LASTNAME, si.ISSUE_DATE STARTISSUEDATE, ei.ISSUE_DATE EXPIREISSUEDATE FROM IJFILIB.ZMPSHORTMF mf LEFT JOIN (SELECT * FROM ZHFILIB2.PUBISSTBL WHERE PUBLICATION_CODE='IJ') si on mf.CURSTRTISS=si.ISSUE_NUMBER LEFT JOIN (SELECT * FROM ZHFILIB2.PUBISSTBL WHERE PUBLICATION_CODE='IJ') ei on mf.CUREXPRISS=ei.ISSUE_NUMBER WHERE mf.ACCTNBR='{0}'", acctnbr));
            using (OleDbConnection cn = new OleDbConnection(Properties.Settings.Default.db2connectionstring))
            {
                using (OleDbCommand cm = new OleDbCommand(sql.ToString(), cn))
                {
                    cn.Open();
                    OleDbDataReader dr = cm.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            recordstatus = Convert.ToInt32(dr[dr.GetOrdinal("RECORDSTATUS")]);
                            paidstatus = Convert.ToInt32(dr[dr.GetOrdinal("PAYSTAT")]);
                            verdate = dr[dr.GetOrdinal("VERDATE")].ToString();
                            registrationinfo.FirstName = dr[dr.GetOrdinal("FIRSTNAME")].ToString().Trim(' ');
                            registrationinfo.LastName = dr[dr.GetOrdinal("LASTNAME")].ToString().Trim(' '); 
                            if (!dr.IsDBNull(dr.GetOrdinal("STARTISSUEDATE")))
                            {
                                startissuedate = Convert.ToDateTime(dr[dr.GetOrdinal("STARTISSUEDATE")]);
                            }
                            if (!dr.IsDBNull(dr.GetOrdinal("EXPIREISSUEDATE")))
                            {
                                expireissuedate = Convert.ToDateTime(dr[dr.GetOrdinal("EXPIREISSUEDATE")]);
                            }
                        }
                    }
                    cn.Close();
                }
            }

            if (Convert.ToInt32(verdate.Substring(0, 2)) < 0)
            {
                verificationdate = new DateTime(1900 + Convert.ToInt32(verdate.Substring(0, 2)), Convert.ToInt32(verdate.Substring(2, 2)), 1);
            }
            else
            {
                verificationdate = new DateTime(2000 + Convert.ToInt32(verdate.Substring(0, 2)), Convert.ToInt32(verdate.Substring(2, 2)), 1);
            }

            if (recordstatus == 1) // normal active record
            {
                switch (paidstatus)
                {
                    case 0:
                        startissuedate = verificationdate;
                        expireissuedate = verificationdate.AddMonths(11);
                        break;
                    case 2:
                        if (paidstatus == 0) // paid grace
                        {
                            expireissuedate = expireissuedate.AddMonths(3);
                        }
                        break;
                }

            }
            else
            { // not an active record
                switch (paidstatus)
                {
                    case 0:
                        startissuedate = verificationdate;
                        expireissuedate = verificationdate;
                        break;
                }
            }

            registrationinfo.StartDate = Convert.ToString(TimeSpan.FromDays(startissuedate.Subtract(UnixEpoch).Days).TotalSeconds);
            registrationinfo.StopDate = Convert.ToString(TimeSpan.FromDays(expireissuedate.Subtract(UnixEpoch).Days).TotalSeconds);

            if (debug == true)
            {
                DebugRegistrationInfo debugregistrationinfo = new DebugRegistrationInfo();
                debugregistrationinfo.ExpireIssueDate = expireissuedate;
                debugregistrationinfo.ExpireIssueDateValue = expireissuedate.ToString("yyyyMMdd");
                debugregistrationinfo.FirstName = registrationinfo.FirstName;
                debugregistrationinfo.LastName = registrationinfo.LastName;
                debugregistrationinfo.PaidStatus = paidstatus;
                debugregistrationinfo.RecordStatus = recordstatus;
                debugregistrationinfo.StartDate = registrationinfo.StartDate;
                debugregistrationinfo.StartIssueDate = startissuedate;
                debugregistrationinfo.StartIssueDateValue = startissuedate.ToString("yyyyMMdd");
                debugregistrationinfo.StopDate = registrationinfo.StopDate;
                debugregistrationinfo.Success = registrationinfo.Success;
                debugregistrationinfo.VerificationDate = verificationdate;
                debugregistrationinfo.VerDate = verdate;
                debugregistrationinfo.VerificationDateValue = verificationdate.ToString("yyyyMMdd");
                return debugregistrationinfo;
            }

            return registrationinfo;
        }
    }

    public class DragonRegistrationInfo
    {
        public string Lookup(int companyid, int productid, string emailaddress, string password)
        {

            //StringBuilder sql = new StringBuilder(string.Format("SELECT '379172018' as ACCTNBR"));
            StringBuilder sql = new StringBuilder(string.Format("CALL get_account_number_email_password('{0}', '{1}', {2}, {3})", emailaddress.Replace("'", "''"), password.Replace("'", "''"), companyid.ToString(), productid.ToString()));
            string acctnbr = "";
            using (MySqlConnection cn = new MySqlConnection(Properties.Settings.Default.mysqlconnectionstring))
            {
                cn.Open();
                using (MySqlCommand cm = new MySqlCommand(sql.ToString(), cn))
                {
                    MySqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        acctnbr = (string)dr[0];
                    }
                    dr.Close();
                }
            }
            return acctnbr;
        }
    }

    public class RegistrationInfo
    {
        public string Success { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class DebugRegistrationInfo : RegistrationInfo
    {
        public string AccountNumber { get; set; }
        public int RecordStatus { get; set; }
        public int PaidStatus { get; set; }
        public DateTime StartIssueDate { get; set; }
        public DateTime ExpireIssueDate { get; set; }
        public DateTime VerificationDate { get; set; }

        public string StartIssueDateValue { get; set; }
        public string ExpireIssueDateValue { get; set; }
        public string VerDate { get; set; }
        public string VerificationDateValue { get; set; }
    }

}
