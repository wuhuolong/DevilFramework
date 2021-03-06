﻿using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TableGenerater
{
    using ExcelApp = Microsoft.Office.Interop.Excel.Application;
    using ExcelBook = Microsoft.Office.Interop.Excel.Workbook;
    using ExcelSheet = Microsoft.Office.Interop.Excel.Worksheet;
    using ExcelRange = Microsoft.Office.Interop.Excel.Range;

    public class ExcelReader
    {
        private bool visible;
        private bool opened;
        private string currExcel;
        private string fileName;
        private ExcelApp app;
        private ExcelBook book;
        private ExcelSheet sheet;

        public string GetCell(int row,int col)
        {
            ExcelRange range = sheet.Cells[row, col];
            return range.Text ?? "";
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                this.visible = value;
                if (app != null && opened)
                    app.Visible = true;
            }
        }

        public string FileName { get { return fileName; } }

        public int Rows { get { return sheet.UsedRange.Rows.Count; } }

        public string CurrentExcel { get { return currExcel; } }

        public Errors OpenExcel(string filePath)
        {
            visible = false;
            if (!File.Exists(filePath))
            {
                if(currExcel == filePath)
                {
                    currExcel = null;
                    opened = false;
                    if (app != null)
                        app.Visible = false;
                }
                return Errors.no_file;
            }
            if (currExcel == filePath)
            {
                return 0;
            }
            if (app == null)
            {
                app = new ExcelApp();
            }
            currExcel = filePath;
            fileName = Path.GetFileNameWithoutExtension(filePath);
            //if (app.Worksheets.Count > 0)
            //    app.Workbooks.Close();
            object miss = Missing.Value;
            book = app.Workbooks.Open(filePath, miss, true, miss, miss);
            sheet = book.Worksheets.Item[1];
            opened = sheet != null;
            if (opened)
                app.Visible = visible;
            return 0;
        }

        public Errors Validate()
        {
            return 0;
        }

        public void Close()
        {
            if (app != null)
            {
                if (app.Worksheets.Count > 0)
                    app.Workbooks.Close();
                app.Quit();
                app = null;
            }
            Process[] procs = Process.GetProcessesByName("excel");
            foreach (Process pro in procs)
            {
                pro.Kill();//没有更好的方法,只有杀掉进程
            }
        }
    }
}
