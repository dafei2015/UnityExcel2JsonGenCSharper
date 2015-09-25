using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
namespace ExcelToJsonGenerator
{
	public class ExcelHelper : IDisposable
	{
		private string fileName = null;
		private IWorkbook workbook = null;
		private FileStream fs = null;
		private bool disposed;
		public int cellCount;
		public int rowCount;
		public ExcelHelper(string fileName)
		{
			this.fileName = fileName;
			this.disposed = false;
		}
		public int DataTableToExcel(DataTable data, string sheetName, bool isColumnWritten)
		{
			this.fs = new FileStream(this.fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			if (this.fileName.IndexOf(".xlsx") > 0)
			{
				this.workbook = new XSSFWorkbook();
			}
			else
			{
				if (this.fileName.IndexOf(".xls") > 0)
				{
					this.workbook = new HSSFWorkbook();
				}
			}
			int result;
			try
			{
				if (this.workbook != null)
				{
					ISheet sheet = this.workbook.CreateSheet(sheetName);
					int num;
					if (isColumnWritten)
					{
						IRow row = sheet.CreateRow(0);
						for (int i = 0; i < data.Columns.Count; i++)
						{
							row.CreateCell(i).SetCellValue(data.Columns[i].ColumnName);
						}
						num = 1;
					}
					else
					{
						num = 0;
					}
					this.rowCount = data.Rows.Count;
					this.cellCount = data.Columns.Count;
					for (int j = 0; j < data.Rows.Count; j++)
					{
						IRow row = sheet.CreateRow(num);
						for (int i = 0; i < data.Columns.Count; i++)
						{
							row.CreateCell(i).SetCellValue(data.Rows[j][i].ToString());
						}
						num++;
					}
					this.workbook.Write(this.fs);
					result = num;
				}
				else
				{
					result = -1;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				result = -1;
			}
			return result;
		}
		public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn)
		{
			DataTable dataTable = new DataTable();
			DataTable result;
			try
			{
				this.fs = new FileStream(this.fileName, FileMode.Open, FileAccess.Read);
				if (this.fileName.IndexOf(".xlsx") > 0)
				{
					this.workbook = new XSSFWorkbook(this.fs);
				}
				else
				{
					if (this.fileName.IndexOf(".xls") > 0)
					{
						this.workbook = new HSSFWorkbook(this.fs);
					}
				}
				ISheet sheet;
				if (sheetName != null)
				{
					sheet = this.workbook.GetSheet(sheetName);
					if (sheet == null)
					{
						sheet = this.workbook.GetSheetAt(0);
					}
				}
				else
				{
					sheet = this.workbook.GetSheetAt(0);
				}
				if (sheet != null)
				{
					IRow row = sheet.GetRow(0);

					this.cellCount = (int)row.LastCellNum;
					int num;
					if (isFirstRowColumn)
					{
						for (int i = (int)row.FirstCellNum; i < this.cellCount; i++)
						{
							ICell cell = row.GetCell(i);
							if (cell != null)
							{

                                string stringCellValue = cell.StringCellValue;
								if (stringCellValue != null)
								{
									DataColumn column = new DataColumn(stringCellValue);
									dataTable.Columns.Add(column);
								}
							}
						}
                        num = sheet.FirstRowNum + 1;
					}
					else
					{
						num = sheet.FirstRowNum;
					}
					this.rowCount = sheet.LastRowNum;
					for (int i = num; i <= this.rowCount; i++)
					{
						IRow row2 = sheet.GetRow(i);
						if (row2 != null)
						{
							DataRow dataRow = dataTable.NewRow();
							for (int j = (int)row2.FirstCellNum; j < this.cellCount; j++)
							{
								if (row2.GetCell(j) != null)
								{
									dataRow[j] = row2.GetCell(j).ToString();
								}
							}
							dataTable.Rows.Add(dataRow);
						}
					}
				}
				result = dataTable;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				result = null;
			}
			return result;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.fs != null)
					{
						this.fs.Close();
					}
				}
				this.fs = null;
				this.disposed = true;
			}
		}
	}
}
