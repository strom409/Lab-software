using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EasioCore.Models;

namespace EasioCore.BLL
{
    public class DashBoardBLL
    {
        public static List<LeftMenu> LeftMenuList(string masterIds)
        {
            var lm = new List<LeftMenu>();
            try
            {
                var http = NitsAPI.apiConnection();
                var response = http.GetAsync("menu/" + masterIds).Result;
                if (response.IsSuccessStatusCode)
                {
                    lm = response.Content.ReadAsAsync<IEnumerable<LeftMenu>>().Result?.ToList() ?? new List<LeftMenu>();
                }
            }
            catch { }

            if (lm.Count == 0)
            {
                lm.Add(new LeftMenu
                {
                    ID = "0",
                    liClass = "nav-item",
                    aHref = "#",
                    aClass = "nav-link active",
                    iClass = "nav-icon fas fa-exclamation",
                    pVal = "No Data Available!"
                });
            }
            return lm;
        }

        public static List<ActiveUsers> ActiveUserList()
        {
            var au = new List<ActiveUsers>();
            for (int i = 1; i <= 8; i++)
            {
                au.Add(new ActiveUsers
                {
                    ID = "",
                    photoPath = "user" + i + ".jpg",
                    UserName = "user" + i,
                    aHref = "#",
                    aClass = "users-list-name",
                    spnClass = "users-list-date",
                    Day = i + "-May"
                });
            }
            return au;
        }

        public static List<UserMessageBox> UserMessageBoxList()
        {
            var au = new List<UserMessageBox>();
            for (int i = 1; i <= 3; i++)
            {
                au.Add(new UserMessageBox
                {
                    ID = "",
                    photoPath = "user" + i + ".jpg",
                    UserName = "user" + i,
                    aHref = "#",
                    div1 = "direct-chat-msg",
                    div2 = "direct-chat-infos clearfix",
                    div3 = "direct-chat-text",
                    aClass = "users-list-name",
                    spnClass = "users-list-date",
                    imgClass = "direct-chat-img",
                    msg = i + ". User message!",
                    Day = i + "-May"
                });
            }
            return au;
        }

        public static List<DashBNotice> DashBoardNoticeList()
        {
            var dbn = new List<DashBNotice>();
            for (int i = 1; i <= 4; i++)
            {
                dbn.Add(new DashBNotice
                {
                    ID = i.ToString(),
                    UserName = "User " + i,
                    photoPath = "User" + i + ".jpg",
                    Title = "Title " + i,
                    NoticeDesc = "Notice Description "
                });
            }
            return dbn;
        }
    }
}
