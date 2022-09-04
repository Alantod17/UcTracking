// using System;
// using System.Collections.Generic;
// using System.ComponentModel;
// using System.Data;
// using System.Diagnostics;
// using System.Globalization;
// using System.Linq;
// using System.Threading.Tasks;
// using DevExpress.Spreadsheet;
//
// namespace AwesomeDi.Api.Helpers
// {
//     public static class HelperExcel
//     {
// 		private const int FieldCountRequiredDefault = 2;
// 		private static int _fieldCountRequired = 1;
// 		private static int _maxRowCheck = 10;
// 		private static bool _allFieldRequired = false;
//
//
// 		public static List<T> LoadExcelSpreadsheetUsingGenericType<T>(byte[] bytes, string extension, bool allFieldRequired, int? fieldCountRequired = null)
// 		{
// 			_allFieldRequired = allFieldRequired;
// 			_fieldCountRequired = fieldCountRequired ?? FieldCountRequiredDefault;
// 			return LoadExcelSpreadsheetUsingGenericType<T>(bytes, extension);
// 		}
//
// 		public static List<T> LoadExcelSpreadsheetUsingGenericType<T>(byte[] bytes, string extension)
// 		{
// 			var result = new List<T>();
//
// 			var workbook = GetWorkbook(bytes, extension);
//
// 			foreach (var worksheet in workbook.Worksheets)
// 				if (ShouldProcessWorksheet(worksheet))
// 					result.AddRange(PopulateTableFromSheet<T>(worksheet));
//
// 			return result;
// 		}
//
// 		private static List<T> PopulateTableFromSheet<T>(Worksheet worksheet)
// 		{
// 			var result = new List<T>();
// 			var columnCount = worksheet.Columns.LastUsedIndex + 1;
// 			var rowCount = worksheet.Rows.LastUsedIndex + 1;
//
// 			var columnHeadings = new string[columnCount];
// 			var headingRowIndex = MakeColumnHeadingsForVisibleColumns(columnHeadings, worksheet, rowCount, columnCount);
// 			columnHeadings = columnHeadings.Where(x => x != null).ToArray();
//
// 			if (headingRowIndex != -1)
// 				result.AddRange(PopulateGenericType<T>(columnHeadings, headingRowIndex, worksheet, rowCount, columnHeadings.Length));
// 			return result;
// 		}
//
// 		private static List<T> PopulateGenericType<T>(string[] columnHeadings, int headingRow, Worksheet worksheet, int rows, int columns)
// 		{
// 			var result = new List<T>();
// 			for (var rowIndex = headingRow + 1; rowIndex < rows; ++rowIndex)
// 			{
// 				var temp = typeof(T);
// 				var obj = Activator.CreateInstance<T>();
// 				var properties = temp.GetProperties();
//
// 				var foundCount = 0;
// 				for (var columnIndex = 0; columnIndex < columns; ++columnIndex)
// 				{
// 					var columnHeading = columnHeadings[columnIndex];
// 					if (string.IsNullOrEmpty(columnHeading) == false)
// 					{
// 						var cell = worksheet.Cells[rowIndex, columnIndex];
// 						if (cell != null)
// 						{
// 							var value = cell.Value.ToString();
// 							if (string.IsNullOrEmpty(value) == false)
// 							{
// 								value = value.Replace('\n', ' ')
// 									.Replace('\r',
// 										' '); //LCM 2Apr12 fields must not contain CR or LF - makes new record for saved .csv
//
// 								var property = temp.GetProperty(columnHeading);
// 								if (property != null)
// 								{
// 									var t = property.PropertyType;
// 									try
// 									{
// 										if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
// 											property.SetValue(obj,
// 												string.IsNullOrEmpty(value)
// 													? null
// 													: Convert.ChangeType(value, t.GetGenericArguments()[0]));
// 										else
// 											property.SetValue(obj, Convert.ChangeType(value, t));
// 										++foundCount;
// 									}
// 									catch
// 									{
// 										// ignored
// 									}
// 								}
// 							}
// 						}
// 					}
// 				}
// 				if (_allFieldRequired)
// 				{
// 					if (foundCount == columnHeadings.Length)
// 						result.Add(obj);
// 				}
// 				else if (foundCount >= _fieldCountRequired)
// 				{
// 					result.Add(obj);
// 				}
// 			}
// 			return result;
// 		}
//
// 		public static DataSet ConvertExcelSpreadsheetByteArrayToDataSet(byte[] byteArray, string fileExtension, bool headerRowsIncluded, int fieldCountRequired)
// 		{
// 			_fieldCountRequired = fieldCountRequired;
// 			var dataSet = new DataSet();
// 			var workbook = GetWorkbook(byteArray, fileExtension);
//
// 			foreach (var worksheet in workbook.Worksheets)
// 				if (ShouldProcessWorksheet(worksheet))
// 					dataSet.Tables.Add(CreateTableFromSheet(worksheet, headerRowsIncluded));
// 			return dataSet;
// 		}
//
//
// 		private static DocumentFormat GetDocumentFormat(string extension)
// 		{
// 			var format = DocumentFormat.Undefined;
//
// 			if (extension.ToLower() == ".xlsx") format = DocumentFormat.Xlsx;
// 			if (extension.ToLower() == ".xls") format = DocumentFormat.Xls;
// 			if (extension.ToLower() == ".csv") format = DocumentFormat.Csv;
// 			if (format == DocumentFormat.Undefined)
// 				throw new Exception("Invalid file format - xls, xlsx, csv supported");
// 			return format;
// 		}
//
// 		private static Workbook GetWorkbook(byte[] byteArray, string extension)
// 		{
// 			var format = GetDocumentFormat(extension);
// 			var workbook = new Workbook();
// 			workbook.Options.Import.Csv.Culture = CultureInfo.CurrentCulture;
// 			workbook.LoadDocument(byteArray, format);
//
// 			return workbook;
// 		}
//
//
// 		private static bool ShouldProcessWorksheet(Worksheet worksheet)
// 		{
// 			var shouldProcess = true;
// 			if (worksheet.Name.ToLower() == "documentation")
// 				shouldProcess = false;
// 			if (worksheet.Name.ToLower().Contains("donotuse"))
// 				shouldProcess = false;
//
// 			return shouldProcess;
// 		}
//
// 		private static DataTable CreateTableFromSheet(Worksheet worksheet, bool headerRowsIncluded)
// 		{
// 			var dataTable = new DataTable(worksheet.Name);
// 			var columnCount = worksheet.Columns.LastUsedIndex + 1;
// 			var rowCount = worksheet.Rows.LastUsedIndex + 1;
//
// 			var columnHeadings = new string[columnCount];
// 			int headingRowIndex = -1;
// 			if (headerRowsIncluded)
// 			{
// 				headingRowIndex = MakeColumnHeadingsForVisibleColumns(columnHeadings, worksheet, rowCount, columnCount);
// 				if (headingRowIndex != -1)
// 					PopulateDataTable(dataTable, columnHeadings, headingRowIndex, worksheet, rowCount, columnCount);
// 			}
// 			else
// 			{   // create column headings
// 				for (int offset = 0; offset < columnCount; ++offset)
// 				{
// 					columnHeadings[offset] = "Column" + (offset + 1);
// 				}
// 				headingRowIndex = -1;
// 				PopulateDataTable(dataTable, columnHeadings, headingRowIndex, worksheet, rowCount, columnCount);
// 			}
// 			return dataTable;
// 		}
//
// 		private static void PopulateDataTable(DataTable dataTable, string[] columnHeadings, int headingRow,
// 			Worksheet worksheet, int rows, int columns)
// 		{
// 			// add columns to table
// 			foreach (var columnName in columnHeadings)
// 				dataTable.Columns.Add(new DataColumn(columnName));
//
// 			for (var rowIndex = headingRow + 1; rowIndex < rows; ++rowIndex)
// 			{
// 				var dataRow = dataTable.NewRow();
// 				var foundCount = 0;
// 				for (var columnIndex = 0; columnIndex < columns; ++columnIndex)
// 				{
// 					var columnHeading = columnHeadings[columnIndex];
// 					if (string.IsNullOrEmpty(columnHeading) == false)
// 					{
// 						var cell = worksheet.Cells[rowIndex, columnIndex];
// 						if (cell != null)
// 						{
// 							var value = cell.Value;
// 							if (value.IsEmpty == false)
// 							{
// 								// MSR 14oct16 not sure this is a good idea.  May cause more issues with barcodes that end up as 1212412+E12
// 								//if (value.IsNumeric)
// 								//	dataRow[columnHeading] = value.NumericValue;
// 								//else if (value.IsDateTime)
// 								//	dataRow[columnHeading] = value.DateTimeValue;
// 								//else
// 								dataRow[columnHeading] = cell.DisplayText;
// 								++foundCount;
// 							}
// 						}
// 					}
// 				}
// 				if (_allFieldRequired)
// 				{
// 					if (foundCount == columnHeadings.Length)
// 						dataTable.Rows.Add(dataRow);
// 				}
// 				else if (foundCount >= _fieldCountRequired)
// 				{
// 					dataTable.Rows.Add(dataRow);
// 				}
// 			}
// 		}
//
//
// 		private static int MakeColumnHeadingsForVisibleColumns(string[] columnHeadings,
// 			Worksheet worksheet, int rowCount, int columnCount)
// 		{
// 			var headingRowIndex = -1;
// 			var maxRowCheck = rowCount - 1;
// 			if (maxRowCheck > _maxRowCheck) maxRowCheck = _maxRowCheck;
// 			for (var rowIndex = 0; rowIndex <= maxRowCheck; ++rowIndex)
// 			{
// 				Debug.Write("Row " + rowIndex);
// 				var foundCount = 0;
// 				for (var columnIndex = 0; columnIndex <= columnCount; ++columnIndex)
// 				{
// 					var column = worksheet.Columns[columnIndex];
// 					if (column.Visible)
// 					{
// 						var cell = worksheet.Cells[rowIndex, columnIndex];
// 						var value = cell.Value.ToString();
// 						if (string.IsNullOrEmpty(value) == false)
// 							++foundCount;
// 					}
// 				}
// 				if (foundCount >= _fieldCountRequired)
// 				{
// 					headingRowIndex = rowIndex;
// 					break;
// 				}
// 			}
// 			if (headingRowIndex != -1)
// 				for (var columnIndex = 0; columnIndex <= columnCount; ++columnIndex)
// 				{
// 					var column = worksheet.Columns[columnIndex];
// 					if (column.Visible)
// 					{
// 						var cell = worksheet.Cells[headingRowIndex, columnIndex];
// 						var value = cell.Value.ToString();
// 						if (string.IsNullOrEmpty(value) == false)
// 						{
// 							value = value.Replace(" ", "");
// 							if (value.ToLower() == "desc") value = "Description";
// 							columnHeadings[columnIndex] = value;
// 						}
// 					}
// 				}
// 			return headingRowIndex;
// 		}
//
// 		public static byte[] GetExcelSpreadsheetByteArrayXlsxFromList<T>(IList<T> data)
// 		{
// 			var dataTable = ConvertListToDataTable(data);
// 			return GetExcelSpreadsheetByteArrayFromDataTable(dataTable, ".xlsx");
// 		}
//
// 		public static byte[] GetExcelSpreadsheetByteArrayFromDataTable(DataTable table, string fileExtension, bool addHeader = true)
// 		{
// 			var workbook = new Workbook();
// 			var sheet = workbook.Worksheets[0];
// 			sheet.Import(table, addHeader, 0, 0);
//
// 			var format = DocumentFormat.Xls;
// 			if (fileExtension.ToLower() == ".xlsx") format = DocumentFormat.Xlsx;
//
// 			var byteArray = workbook.SaveDocument(format);
// 			return byteArray;
// 		}
//
// 		public static DataTable ConvertListToDataTable<T>(IList<T> data)
// 		{
// 			PropertyDescriptorCollection properties =
// 				TypeDescriptor.GetProperties(typeof(T));
// 			DataTable table = new DataTable();
// 			foreach (PropertyDescriptor prop in properties)
// 				table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
// 			foreach (T item in data)
// 			{
// 				DataRow row = table.NewRow();
// 				foreach (PropertyDescriptor prop in properties)
// 					row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
// 				table.Rows.Add(row);
// 			}
// 			return table;
// 		}
// 	}
// }
