using ACadSharp.IO;
using ACadSharp.Examples.Common;
using ACadSharp.Tables;
using ACadSharp.Tables.Collections;
using System;
using System.IO;
using System.Diagnostics;
using System.Linq;

namespace ACadSharp.Examples
{
	class Program
	{
		/// <summary>
		/// Explore a dwg or dxf file, and converts it to the other format.
	        /// Accepts a -v loglevel, -b for BinaryDxf and a --as VERSION argument.
		/// </summary>
		static void Main(string[] args)
		{
			CadDocument doc;
			string file;
			string version_string = ""; // version up/downgrades currently un-supported
			int loglevel = 0;
			bool binary = false;
			int i = 0;

			if (args.Length == 0) {
			    Console.WriteLine(".dwg or .dxf argument missing");
			    Environment.Exit(1);
			}
			if (args[i] == "--as")
			{
			    version_string = args[i + 1];
			    Console.WriteLine($"--as {version_string} up-/downgrades still unsupported");
			    i += 2;
			}
			if (args[i] == "-v")
			{
			    loglevel = int.Parse(args[i + 1]);
			    i += 2;
			}
			if (args[i] == "-b")
			{
			    binary = true;
			    i += 1;
			}
			if (args.Length < i) {
			    Console.WriteLine(".dwg or .dxf argument missing");
			    Environment.Exit(1);
			}
			file = args[i];
			if (file.EndsWith(".dwg", StringComparison.OrdinalIgnoreCase))
			{
			    using (DwgReader reader = new DwgReader(file))
			    {
					reader.OnNotification += NotificationHelper.LogConsoleNotification;
					doc = reader.Read();
			    }
			    exploreDocument(doc);

			    if (version_string.Length > 0) {
					ACadVersion version = GetVersionFromName(version_string);
					doc.Header.Version = version;
			    }
			    string dxffile = Path.GetFileNameWithoutExtension(file) + ".dxf";
			    using (DxfWriter writer = new DxfWriter(dxffile, doc, binary))
			    {
					writer.OnNotification += NotificationHelper.LogConsoleNotification;
					writer.Write();
			    }
			    Console.WriteLine($"Wrote {dxffile}");
			}
			else if (file.EndsWith(".dxf", StringComparison.OrdinalIgnoreCase))
			{
			    using (DxfReader reader = new DxfReader(file))
			    {
					reader.OnNotification += NotificationHelper.LogConsoleNotification;
					doc = reader.Read();
			    }
			    exploreDocument(doc);

			    if (version_string.Length > 0) {
					ACadVersion version = GetVersionFromName(version_string);
					doc.Header.Version = version;
			    }
			    string dwgfile = Path.GetFileNameWithoutExtension(file) + ".dwg";
			    using (DwgWriter writer = new DwgWriter(dwgfile, doc))
			    {
					writer.OnNotification += NotificationHelper.LogConsoleNotification;
					writer.Write();
			    }
			    Console.WriteLine($"Wrote {dwgfile}");
			}
		}

		static ACadVersion GetVersionFromName(string name)
		{
			//Modify the format of the name
			string vname = name.Replace('.', '_').ToUpper();

			if (Enum.TryParse(vname, out ACadVersion version))
				return version;
			else
				return ACadVersion.Unknown;
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
