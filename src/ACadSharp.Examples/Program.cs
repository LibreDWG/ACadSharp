using ACadSharp.IO;
using ACadSharp.Examples.Common;
using ACadSharp.Tables;
using ACadSharp.Tables.Collections;
using System;
using System.Diagnostics;
using System.Linq;

namespace ACadSharp.Examples
{
	class Program
	{
		/// <summary>
		/// Explore a dwg or dxf file
		/// </summary>
		static void Main(string[] args)
		{
			CadDocument doc;
			string file;
			if (args.Length == 0) {
			    Console.WriteLine(".dwg or .dxf argument missing");
			    Environment.Exit(1);
			}
			file = args[0];
			if (file.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase))
			{
			    using (DwgReader reader = new DwgReader(file))
			    {
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				doc = reader.Read();
			    }
			    exploreDocument(doc);
			}
			else if (file.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase))
			{
			    using (DxfReader reader = new DxfReader(file))
			    {
				reader.OnNotification += NotificationHelper.LogConsoleNotification;
				doc = reader.Read();
			    }
			    exploreDocument(doc);
			}
		}

		/// <summary>
		/// Logs in the console the document information
		/// </summary>
		/// <param name="doc"></param>
		static void exploreDocument(CadDocument doc)
		{
			Console.WriteLine();
			Console.WriteLine("SUMMARY INFO:");
			Console.WriteLine($"\tTitle: {doc.SummaryInfo.Title}");
			Console.WriteLine($"\tSubject: {doc.SummaryInfo.Subject}");
			Console.WriteLine($"\tAuthor: {doc.SummaryInfo.Author}");
			Console.WriteLine($"\tKeywords: {doc.SummaryInfo.Keywords}");
			Console.WriteLine($"\tComments: {doc.SummaryInfo.Comments}");
			Console.WriteLine($"\tLastSavedBy: {doc.SummaryInfo.LastSavedBy}");
			Console.WriteLine($"\tRevisionNumber: {doc.SummaryInfo.RevisionNumber}");
			Console.WriteLine($"\tHyperlinkBase: {doc.SummaryInfo.HyperlinkBase}");
			Console.WriteLine($"\tCreatedDate: {doc.SummaryInfo.CreatedDate}");
			Console.WriteLine($"\tModifiedDate: {doc.SummaryInfo.ModifiedDate}");

			exploreTable(doc.AppIds);
			exploreTable(doc.BlockRecords);
			exploreTable(doc.DimensionStyles);
			exploreTable(doc.Layers);
			exploreTable(doc.LineTypes);
			exploreTable(doc.TextStyles);
			exploreTable(doc.UCSs);
			exploreTable(doc.Views);
			exploreTable(doc.VPorts);
		}

		static void exploreTable<T>(Table<T> table)
			where T : TableEntry
		{
			Console.WriteLine($"{table.ObjectName}");
			foreach (var item in table)
			{
				Console.WriteLine($"\tName: {item.Name}");

				if (item.Name.Equals(BlockRecord.PaperSpaceName, StringComparison.OrdinalIgnoreCase)
				    && item is BlockRecord pmodel)
				{
					Console.WriteLine($"\t\tPaperSpace Entities:");
					foreach (var e in pmodel.Entities.GroupBy(i => i.GetType().FullName))
					{
						Console.WriteLine($"\t\t{e.Key}: {e.Count()}");
					}
				}
				if (item.Name.Equals(BlockRecord.ModelSpaceName, StringComparison.OrdinalIgnoreCase)
				    && item is BlockRecord model)
				{
					Console.WriteLine($"\t\tModelSpace Entities:");
					foreach (var e in model.Entities.GroupBy(i => i.GetType().FullName))
					{
						Console.WriteLine($"\t\t{e.Key}: {e.Count()}");
					}
				}
			}
		}
	}
}
