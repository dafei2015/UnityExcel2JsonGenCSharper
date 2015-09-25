using System;
namespace ExcelToJsonGenerator
{
	public class Entrance
	{
		private static Entrance instance;
		public ExcelProcess ex
		{
			get;
			set;
		}
		public static Entrance Instance
		{
			get
			{
				Entrance ins;
				if ((ins = Entrance.instance) == null)
				{
					ins = (Entrance.instance = new Entrance());
				}
				return ins;
			}
		}
		public void Init(string fullPath, string sheetName)
		{
			this.ex = new ExcelProcess(fullPath, sheetName);
		}
		public string GetJsonString()
		{
			return this.ex.GetJsonString();
		}
		public string GetSchema()
		{
			return this.ex.GetJsonSchema();
		}
        
	}
}
