﻿using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace NewstonTools.FileHelpers.Implement
{
    public class EPPlusSaveXlsxHelper
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        public bool IsAutoFitColumns { get; set; }

        public Encoding FileEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// EPPlusSaveXlsxHelper构造函数
        /// </summary>
        /// <param name="file">示例：C:\\Temp\\Sample.xlsx</param>
        public EPPlusSaveXlsxHelper(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            FilePath = fileInfo.DirectoryName;
            FileName = fileInfo.Name;



        }
        public EPPlusSaveXlsxHelper(FileInfo fileInfo)
        {
            FilePath = fileInfo.DirectoryName;
            FileName = fileInfo.Name;
        }


        private bool FileCheck(out string errorMsg)
        {
            if (GlobalFunction.StringIsNullOrEmpty(FilePath, FileName))
            {
                errorMsg = $"文件路径或文件名不能为空,FilePath值:{FilePath ?? "空值"},FileName值:{FileName ?? "空值"}";
                return false;
            }
            try
            {
                FileInfo fileInfo = new FileInfo(Path.Combine(FilePath, FileName));
                if (!fileInfo.Directory.Exists) { fileInfo.Directory.Create(); }
                errorMsg = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }

        }


        /// <summary>
        /// 保存xlsx文件
        /// </summary>
        /// <param name="datas">数据字典，string代表工作表名称，int代表行号，int代表列号，object代表单元格值，先行后列</param>
        /// <returns></returns>
        public bool WriteXlsx(Dictionary<string, Dictionary<int, Dictionary<int, object>>> datas)
        {
            // 创建新的Excel包
            using (var package = new ExcelPackage())
            {
                foreach (var sheet in datas)
                {
                    var worksheet = package.Workbook.Worksheets.Add(sheet.Key);

                    foreach (var row in sheet.Value)
                    {
                        foreach (var culomn in row.Value)
                        {
                            worksheet.Cells[row.Key, culomn.Key].Value = culomn.Value;
                        }
                    }
                }

                try
                {
                    string errmsg = string.Empty;
                    if (FileCheck(out errmsg))
                    {
                        string fileStr = Path.Combine(FilePath, FileName);
                        var file = new FileInfo(fileStr);
                        package.SaveAs(file);
                        System.Console.WriteLine($"文件已保存至: {fileStr}");
                        return true;
                    }
                    else
                    {
                        throw new ArgumentException(errmsg);
                    }

                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"保存文件时出错: {ex.Message}");
                    return false;
                }
            }
        }

        public bool WriteXlsx(DataSet datas)
        {
            // 创建新的Excel包
            using (var package = new ExcelPackage(new FileInfo(Path.Combine(FilePath, FileName))))
            {
                foreach (DataTable table in datas.Tables)
                {
                    var worksheet = package.Workbook.Worksheets.Add(table.TableName);
                    worksheet.Cells.LoadFromDataTable(table, true);
                }

                try
                {
                    string errmsg = string.Empty;
                    if (FileCheck(out errmsg))
                    {
                        string fileStr = Path.Combine(FilePath, FileName);
                        var file = new FileInfo(fileStr);
                        using (FileStream stream = file.Create())
                        {
                            stream.Close();
                            package.Save();
                            System.Console.WriteLine($"文件已保存至: {fileStr}");
                            return true;
                        }

                    }
                    else
                    {
                        throw new ArgumentException(errmsg);
                    }

                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"保存文件时出错: {ex.Message}");
                    return false;
                }
            }
        }


        public bool ReadXlsx(string filePath,out DataSet)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                //package.Workbook.Part.GetRelationships();//这里的Part出现在程序集的内部
            }
        }  
    }
}
