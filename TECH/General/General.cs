using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TECH.General
{
  
    public static class Common
    {
        public static string GetStatus(int status)
        {
            if (status == 1)
            {
                return "Đang làm";
            }
            if (status == 2)
            {
                return "Đã hủy";
            }
            if (status == 3)
            {
                return "Đã hoàn thành";
            }           
            return "";
        }
        public static string GetLevelAcademic(int status)
        {
            if (status == 1)
            {
                return " Cao Đẳng";
            }
            if (status == 2)
            {
                return "Đại Học";
            }
            if (status == 3)
            {
                return "Tiến Sĩ";
            }
            if (status == 4)
            {
                return "Giáo Sư";
            }
            return "";
        }
        public static string GetGender(int status)
        {
            if (status == 1)
            {
                return " Nam";
            }
            if (status == 2)
            {
                return "Nữ";
            }
            return "";
        }
        public static string GetEmployStatus(bool status)
        {
            if (!status)
            {
                return " Đang làm";
            }
            else
            {
                return " Đã Nghỉ Việc";
            }
            return "";
        }
        public static string GetBonuesPunishStatus(int status)
        {
            if (status == 1)
            {
                return "Thưởng";
            }
            if (status == 2)
            {
                return "Phạt";
            }
            return "";
        }

    }
}
