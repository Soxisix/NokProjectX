using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace NokProjectX.Wpf.Reports
{
    public partial class AllTransactionReport : DevExpress.XtraReports.UI.XtraReport
    {
        public AllTransactionReport()
        {
            InitializeComponent();
        }

//        int iRowCount = 0;
//        int equalRow = 20;
//        private void xrTable3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
//        {
////            iRowCount++;
////            var detail = (XRTable)sender;
////            if (iRowCount == equalRow * detail.Rows.Count)
////            {
//////                XRPageBreak xrPageBreak = new XRPageBreak();
////////                xrPageBreak.Location = new Point(40, 350);
//////                Detail.Controls.Add(xrPageBreak);
////                xrPageBreak1.Visible = true;
////                iRowCount = 0;
////                equalRow += 20;
////            }
////            else
////            {
////                xrPageBreak1.Visible = false;
////            }
//        }

//        private void xrTable3_AfterPrint(object sender, EventArgs e)
//        {
//            
//            
////            for (int i = 0; i > ; i++)
////            {
////                if (detail.Rows.Count == 10)
////                {
////                    XRPageBreak xrPageBreak = new XRPageBreak();
////                    //                xrPageBreak.Location = new Point(40, 350);
////                    Detail.Controls.Add(xrPageBreak);
////                }
////                if (detail.Rows.Count == 20)
////                {
////                    XRPageBreak xrPageBreak = new XRPageBreak();
////                    //                xrPageBreak.Location = new Point(40, 350);
////                    Detail.Controls.Add(xrPageBreak);
////                }
////            }
//        }
    }
}
