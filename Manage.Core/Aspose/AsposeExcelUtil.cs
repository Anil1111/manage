using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace Manage.Core.Aspose
{
    /// <summary>
    /// AsposeExcel 帮助类
    /// </summary>
    public class AsposeExcelUtil
    {
        private const int BufferSize = 0x1000;
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="fullFilename">文件地址</param>
        /// <returns></returns>
        public static DataTable ReadExcel(String fullFilename)
        {
            return ReadExcel(fullFilename, new LoadOptions(LoadFormat.Xlsx));
        }

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="fullFilename">文件地址</param>
        /// <param name="loadOptions">参数信息【如：读取包含密码的excel】</param>
        /// <returns></returns>
        public static DataTable ReadExcel(String fullFilename, LoadOptions loadOptions)
        {
            using (var fstream = new FileStream(fullFilename, FileMode.Open))
            {
                var workbook = new Workbook(fstream, loadOptions);

                Worksheet sheet = workbook.Worksheets[0];

                Cells cells = sheet.Cells;
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            }
        }

        /// <summary>
        /// 根据 datatable 导出文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="response"></param>
        /// <param name="exportFileName"></param>
        public static void Export(DataTable data, HttpResponse response, String exportFileName)
        {
            response.Clear();
            response.Buffer = true;
            response.Charset = "UTF8";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString(CultureInfo.InvariantCulture));
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            Stream outputStream = DatatableToExcelStream(data);
            var buffer = new byte[BufferSize];
            int bytesRead = 0;
            while ((bytesRead = outputStream.Read(buffer, 0, BufferSize)) > 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
            outputStream.Close();
            response.Flush();
            response.End();
        }

        /// <summary>
        /// 根据 datatable 导出文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="response"></param>
        /// <param name="exportFileName"></param>
        public static void Export(DataTable data, HttpResponseBase response, String exportFileName)
        {
            response.Clear();
            response.Buffer = true;
            response.Charset = "UTF8";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString(CultureInfo.InvariantCulture));
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            Stream outputStream = DatatableToExcelStream(data);
            var buffer = new byte[BufferSize];
            int bytesRead = 0;
            while ((bytesRead = outputStream.Read(buffer, 0, BufferSize)) > 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
            outputStream.Close();
            response.Flush();
            response.End();
        }

        private static void MakeHeader(Worksheet sheet, DataTable dt)
        {
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                Cell cell = sheet.Cells[0, col];
                cell.SetStyle(MakeHealStyle());
            }
        }

        /// <summary>
        /// 创建默认的excel头样式
        /// </summary>
        /// <returns></returns>
        private static Style MakeHealStyle()
        {
            var style = new Style();
            style.Font.Name = "微软雅黑";//文字字体
            style.Font.Size = 14;//文字大小
            style.IsLocked = false;//单元格解锁
            style.Font.IsBold = true;//粗体
            style.Font.IsBold = true;
            style.Pattern = BackgroundType.Solid; //设置背景样式
            style.IsTextWrapped = true;//单元格内容自动换行
            style.SetBorder(BorderType.TopBorder, CellBorderType.Thin, Color.Black);
            style.SetBorder(BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
            style.SetBorder(BorderType.RightBorder, CellBorderType.Thin, Color.Black);
            style.HorizontalAlignment = TextAlignmentType.Center;//文字居中
            style.IsTextWrapped = false;//setTextWrapped
            return style;
        }

        private static void MakeBody(Worksheet sheet, DataTable dt)
        {
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sheet.Cells[r + 1, c].PutValue(dt.Rows[r][c].ToString());
                }
            }
        }

        /// <summary>
        /// 把dt转换成excel的字节数组【不建议使用】
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Byte[] DatatableToExcelBytes(DataTable dt)
        {
            try
            {
                return DatatableToExcelStream(dt).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 把dt转换成excel的流
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static MemoryStream DatatableToExcelStream(DataTable dt)
        {
            try
            {
                var workbook = new Workbook();
                var sheet = workbook.Worksheets[0];
                sheet.Cells.ImportDataTable(dt, true, 0, 0);
                MakeHeader(sheet, dt);
                sheet.AutoFitColumns();
                sheet.AutoFitRows();
                var memory = new MemoryStream();
                workbook.Save(memory, SaveFormat.Xlsx);
                memory.Seek(0, SeekOrigin.Begin);
                return memory;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据Dt生成 excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="exportFileName"></param>
        /// <returns></returns>
        public static Boolean DatatableToExcel(DataTable dt, string exportFileName)
        {
            try
            {
                var workbook = new Workbook();
                var sheet = workbook.Worksheets[0];
                sheet.Cells.ImportDataTable(dt, true, 0, 0);
                MakeHeader(sheet, dt);
                sheet.AutoFitColumns();
                sheet.AutoFitRows();
                workbook.Save(exportFileName, SaveFormat.Xlsx);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 将集合对象装换成Excel的字节数组
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="data">集合</param>
        public static byte[] ConvertToExportByte<T>(IEnumerable<T> data)
        {
            return ConvertToExportStream<T>(data).ToArray();
        }

        /// <summary>
        /// 将对象转换成流
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static MemoryStream ConvertToExportStream<T>(IEnumerable<T> data)
        {
            var workbook = new Workbook();
            var sheet = workbook.Worksheets[0];
            PropertyInfo[] infos = typeof(T).GetProperties();
            var colIndex = "A";
            foreach (var p in infos)
            {
                Cell cell = sheet.Cells[colIndex + 1];
                Style style = MakeHealStyle();
                cell.SetStyle(style);
                cell.PutValue(p.Name);
                int i = 2;
                foreach (var d in data)
                {
                    sheet.Cells[colIndex + i].PutValue(p.GetValue(d, null));
                    i++;
                }
                colIndex = ((char)(colIndex[0] + 1)).ToString(CultureInfo.InvariantCulture);
            }
            var memory = new MemoryStream();
            workbook.Save(memory, SaveFormat.Xlsx);
            memory.Seek(0, SeekOrigin.Begin);
            return memory;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">集合</param>
        /// <param name="response">HttpResponse</param>
        /// <param name="exportFileName">导出的文件名称</param>
        public static void Export<T>(IEnumerable<T> data, HttpResponse response, String exportFileName)
        {
            response.Clear();
            response.Buffer = true;
            response.Charset = "UTF8";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString(CultureInfo.InvariantCulture));
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            Stream outputStream = ConvertToExportStream<T>(data);
            var buffer = new byte[BufferSize];
            int bytesRead = 0;
            while ((bytesRead = outputStream.Read(buffer, 0, BufferSize)) > 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
            outputStream.Close();
            response.Flush();
            response.End();
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="data">集合</param>
        /// <param name="response">HttpResponse</param>
        /// <param name="exportFileName">导出的文件名称</param>
        public static void Export<T>(IEnumerable<T> data, HttpResponseBase response, String exportFileName)
        {
            response.Clear();
            response.Buffer = true;
            response.Charset = "UTF8";
            response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(exportFileName, Encoding.UTF8).ToString(CultureInfo.InvariantCulture));
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            Stream outputStream = ConvertToExportStream<T>(data);
            var buffer = new byte[BufferSize];
            int bytesRead = 0;
            while ((bytesRead = outputStream.Read(buffer, 0, BufferSize)) > 0)
            {
                response.OutputStream.Write(buffer, 0, bytesRead);
            }
            outputStream.Close();
            response.Flush();
            response.End();
        }
    }
}
