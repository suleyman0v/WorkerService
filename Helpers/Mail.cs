using System;
using System.Collections.Generic;
using System.Text;

namespace SmartNtsService.Helpers
{
    public class Mail
    {
        public string SendMail(string to, string cc, string subject, string message, string files = "")
        {
            try
            {
                string sql = $@"EXEC msdb.dbo.sp_send_dbmail @profile_name='OCTOS',
                            @recipients = '{to}',
                            @copy_recipients = '{cc}',
                            @subject = '{subject}',
                            @body = '{message}'  ,
                            @body_format = 'HTML',
                            @file_attachments='{files}';";
                DbHelper.dbRun(sql);
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}

