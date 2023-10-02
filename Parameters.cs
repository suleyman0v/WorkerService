using System;
using System.Collections.Generic;
using System.Text;

namespace SmartNtsService
{
    class Parameters
    {
        private string PARAMETRNAME;
        public string Parametr_name
        {
            get { return PARAMETRNAME; }
            set { PARAMETRNAME = value; }
        }
        private string PARAMETRVALUE;

        public string Parametr_value
        {
            get { return PARAMETRVALUE; }
            set { PARAMETRVALUE = value; }
        }
    }
}
