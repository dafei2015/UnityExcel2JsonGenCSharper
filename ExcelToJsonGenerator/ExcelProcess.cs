using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
namespace ExcelToJsonGenerator
{
    public class ExcelProcess
    {
        private ExcelHelper helper
        {
            get;
            set;
        }
        private DataTable table
        {
            get;
            set;
        }
        public string Json
        {
            get;
            set;
        }
        public string FileName
        {
            get;
            set;
        }
        public List<string> ColumnList
        {
            get;
            set;
        }
        public List<string> TypeList
        {
            get;
            set;
        }
        public ExcelProcess(string path, string className)
        {
            this.helper = new ExcelHelper(path);
            this.table = this.helper.ExcelToDataTable(className, true);
            this.ResolveDataTable();
        }
        private void ResolveDataTable()
        {
            this.ColumnList = new List<string>();
            this.TypeList = new List<string>();
            foreach (DataColumn dataColumn in this.table.Columns)
            {
                this.ColumnList.Add(dataColumn.ColumnName);
            }
            for (int i = 0; i < this.helper.cellCount; i++)
            {
                this.TypeList.Add(this.table.Rows[0][i].ToString());
            }
        }

        public string GetJsonString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int i = 1; i < this.helper.rowCount; i++)
            {
                for (int j = 0; j < this.helper.cellCount; j++)
                {
                    if (j == 0)
                    {
                        //第一列
                        if (this.TypeList[j].ToUpper() == "INT")
                        {
                            stringBuilder.Append(string.Format("{0}:", this.table.Rows[i][0].ToString()));
                        }
                        if (this.TypeList[j].ToUpper() == "STRING")
                        {
                            stringBuilder.Append(string.Format("\"{0}\":", this.table.Rows[i][0].ToString()));
                        }
                        if (this.TypeList[j].ToUpper() == "DOUBLE")
                        {
                            stringBuilder.Append(string.Format("{0}:", this.table.Rows[i][0].ToString()));
                        }
                        if (this.TypeList[j].ToUpper() == "FLOAT")
                        {
                            stringBuilder.Append(string.Format("{0}:", this.table.Rows[i][0].ToString()));
                        }
                        stringBuilder.Append("{");
                    }

                    //所有列
                    if (this.TypeList[j].ToUpper() == "INT")
                    {
                        stringBuilder.Append(string.Format("\"{0}\":{1}", this.ColumnList[j], this.table.Rows[i][j]));
                    }
                    if (this.TypeList[j].ToUpper() == "STRING")
                    {
                        stringBuilder.Append(string.Format("\"{0}\":\"{1}\"", this.ColumnList[j], this.table.Rows[i][j]));
                    }
                    if (this.TypeList[j].ToUpper() == "DOUBLE")
                    {
                        stringBuilder.Append(string.Format("\"{0}\":{1}", this.ColumnList[j], this.table.Rows[i][j]));
                    }
                    if (this.TypeList[j].ToUpper() == "FLOAT")
                    {
                        stringBuilder.Append(string.Format("\"{0}\":{1}", this.ColumnList[j], this.table.Rows[i][j]));
                    }
                    if (j != this.helper.cellCount - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                stringBuilder.Append("}");
                if (i != this.helper.rowCount - 1)
                {
                    stringBuilder.Append(",");
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 生成需要模版
        /// </summary>
        public string GetJsonSchema()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int i = 0; i < this.ColumnList.Count; i++)
            {
                if (this.TypeList[i].ToUpper() == "INT")
                {
                    stringBuilder.Append(string.Format("{0}:0,", this.ColumnList[i]));
                }
                if (this.TypeList[i].ToUpper() == "STRING")
                {
                    stringBuilder.Append(string.Format("\"{0}\":\"Schema\",", this.ColumnList[i]));
                }
                if (this.TypeList[i].ToUpper() == "DOUBLE")
                {
                    stringBuilder.Append(string.Format("{0}:0.11,", this.ColumnList[i]));
                }
                if (this.TypeList[i].ToUpper() == "FLOAT")
                {
                    stringBuilder.Append(string.Format("{0}:0.11,", this.ColumnList[i]));
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}
