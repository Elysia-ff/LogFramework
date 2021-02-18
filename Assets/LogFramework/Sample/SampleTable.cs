using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LogFramework.Sample
{
    public class SampleTable
	{
		private static SampleTable m_Table = null;
		public static SampleTable Table
		{
			get
			{
				if (m_Table == null)
				{
					m_Table = new SampleTable();
				}

				return m_Table;
			}
		}

		public class Row
		{
			public int idx;
			public string some;
			public string meaningless;
			public string fields;
			public string log_name;
		}

		private List<Row> rowList = new List<Row>();
		public List<Row> RowList { get { return rowList; } }

		public SampleTable()
		{
			rowList.Clear();
			List<string[]> grid = Parse();
			for (int i = 1; i < grid.Count; i++)
			{
				Row row = new Row();
				row.idx = int.Parse(grid[i][0]);
				row.some = grid[i][1];
				row.meaningless = grid[i][2];
				row.fields = grid[i][3];
				row.log_name = grid[i][4];

				rowList.Add(row);
			}
		}

		private List<string[]> Parse()
		{
			using (StreamReader reader = new StreamReader(Application.dataPath + "/LogFramework/Sample/sample table.csv"))
			{
				List<string[]> result = new List<string[]>();
				while (true)
				{
					string line = reader.ReadLine();
					if (string.IsNullOrEmpty(line))
						break;

					string[] fields = line.Split(',');
					result.Add(fields);
				}

				return result;
			}
        }

		public Row Find_idx(int find)
		{
			return rowList.Find(x => x.idx == find);
		}
	}
}
