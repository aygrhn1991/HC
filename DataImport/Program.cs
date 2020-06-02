using Maticsoft.DBUtility;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            //excel2db("E:/abc.xls");
            string url1 = "http://www.weather.com.cn/data/sk/101050404.html";
            string url2 = "http://www.weather.com.cn/data/cityinfo/101050404.html";
            string url3 = "http://m.weather.com.cn/data/101010100.html";
            string result = getHtmlStr(url1, Encoding.UTF8);
            Debug.WriteLine(result);

            Console.ReadKey();
        }
        public static string getHtmlStr(string url, Encoding enc)
        {
            string htmlStr = "";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, enc);
            htmlStr = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            response.Close();
            return htmlStr;
        }
        public static void excel2db(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workBook = null;
                workBook = new HSSFWorkbook(fs);
                for (int s = 0; s < workBook.NumberOfSheets; s++)
                {
                    ISheet currentSheet = workBook.GetSheetAt(s);
                    //for (int r = 0; r < currentSheet.LastRowNum; r++)
                    for (int r = 1; r <= currentSheet.LastRowNum; r++)
                    {
                        IRow currentRow = currentSheet.GetRow(r);
                        string name = currentRow.GetCell(0).StringCellValue;
                        string key = currentRow.GetCell(1).StringCellValue;
                        double phone = currentRow.GetCell(2).NumericCellValue;
                        string sql = string.Format("insert into t_user(name,phone,`key`,count_scan,count_question,count_duration,del) values('{0}','{1}','{2}',0,0,0,0)", name, phone, key);
                        int count = DbHelperMySQL.ExecuteSql(sql);
                        //for (int c = 0; c < currentRow.LastCellNum; c++)
                        //{
                        //    try
                        //    {
                        //        ICell cell = currentRow.GetCell(c);
                        //        if (cell == null)
                        //        {
                        //            Debug.WriteLine(string.Format("sheet:{0},row:{1},cell:{2}，空值", s, r, c));
                        //        }
                        //        CellType cellType = cell.CellType;
                        //        switch (cellType)
                        //        {
                        //            case CellType.Numeric:
                        //                break;
                        //            case CellType.String:
                        //                break;
                        //            case CellType.Boolean:
                        //                break;
                        //        }
                        //        DateTime date = cell.DateCellValue;
                        //        double num = cell.NumericCellValue;
                        //        string str = cell.StringCellValue;
                        //        bool bl = cell.BooleanCellValue;

                        //    }
                        //    catch (Exception e)
                        //    {
                        //        Debug.WriteLine(e);
                        //    }
                        //}
                    }
                }
                fs.Close();
                workBook.Close();
                fs.Dispose();
            }
        }
    }
}
