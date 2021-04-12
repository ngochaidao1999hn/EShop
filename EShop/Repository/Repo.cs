using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EShop.Repository
{
    public class Repo
    {
        public bool check(string extension, string[] format)
        {
            foreach (string exten in format)
            {
                if (extension.Contains(exten))
                {
                    return true;
                }
            }
            return false;
        }
    }
}