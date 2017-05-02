using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace QuidMind.CloRoFeel.LcdManagerModule.LcdPaginator
{

    public delegate void LcdRefreshDelegate(string line1, string line2);

    public class LcdPager
    {
        bool defaultText = true;



        SortedList<string, LcdPage> pages = new SortedList<string, LcdPage>();

        public LcdPager()
        {
            this.PausePerPage = 2000;
            pages.Add("00.00", new LcdPage() { Num = "00.00", Line1 = "CloRoFeel V2K12", Line2 = "(c)QuidMind 2012" });
            defaultText = true;
        }

        Thread th = null;
        public void Start()
        {
            th = new Thread(new ThreadStart(RunPager));
            th.IsBackground = true;
            th.Priority = ThreadPriority.BelowNormal;
            th.Start();
        }

        public int PausePerPage { get; set; }

        int currentPageIndex = 0;
        int skipStep = 0;
        void RunPager()
        {
            while (true)
            {
                if (skipStep > 0)
                {
                    skipStep--;
                    Thread.Sleep(PausePerPage);
                    continue;
                }
                if (pages.Count != 0)
                {
                    currentPageIndex = (currentPageIndex + 1) % pages.Count;
                }
                ShowPage(currentPageIndex);
                Thread.Sleep(PausePerPage);
            }
        }

        void ShowPage(int pageIndex)
        {
            if (pages.Count < 1)
                return;
            LcdPage page = pages[pages.Keys[currentPageIndex]];
            if (LcdRefreshed != null)
                LcdRefreshed(page.Line1, page.Line2);
        }

        public event LcdRefreshDelegate LcdRefreshed;

        public void RegisterPage(Action<string, string, string> action, string pageNum, string line1 = "", string line2 = "")
        {
            if (defaultText)
            {
                pages.Clear();
                defaultText = false;
            }
            LcdPage lp = null;
            if (pages.Keys.Contains(pageNum))
            {
                lp = pages[pageNum];
                pages.Remove(pageNum);
            }
            else
            {
                lp = new LcdPage();
            }
            lp.AttachedAction = action;
            lp.Num = pageNum;
            lp.Line1 = (line1 + "                ").Substring(0, 16);

            if (action != null)
                lp.Line2 = (line2 + "                ").Substring(0, 15) + "*";
            else
                lp.Line2 = (line2 + "                ").Substring(0, 16);

            pages.Add(pageNum, lp);
        }

        public void RegisterPage(string pageNum, string line1 = "", string line2 = "")
        {
            RegisterPage(null, pageNum, line1, line2);
        }

        public void NextPage()
        {
            skipStep = 2;
            currentPageIndex = (currentPageIndex + 1) % pages.Count;
            ShowPage(currentPageIndex);
        }

        public void PreviousPage()
        {
            skipStep = 2;
            if (currentPageIndex == 0)
                currentPageIndex = pages.Count;
            currentPageIndex = (currentPageIndex - 1) % pages.Count;

            ShowPage(currentPageIndex);
        }

        public void MoveToPage(string pageNum)
        {
            if (pages.Keys.Contains(pageNum))
            {
                skipStep = 2;
                currentPageIndex = pages.IndexOfKey(pageNum);
                ShowPage(currentPageIndex);
            }
        }

        public void DeletePages(string startPageNum)
        {
            List<string> lstKey = pages.Keys.Where((k) => k.StartsWith(startPageNum)).ToList();
            foreach (string key in lstKey)
                pages.Remove(key);
            if (pages.Count == 0)
            {
                pages.Add("00.00", new LcdPage() { Num = "00.00", Line1 = "CloRoFeel V2K12", Line2 = "(c)QuidMind 2012" });
                defaultText = true;
            }
        }

        public void ExecuteAction()
        {
            LcdPage page = pages[pages.Keys[currentPageIndex]];
            try
            {
                if (page.AttachedAction != null)
                    page.AttachedAction(page.Num, page.Line1, page.Line2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while execution action for page " + page.Num + "\r\n" + ex.ToString());
            }
        }
    }

}
