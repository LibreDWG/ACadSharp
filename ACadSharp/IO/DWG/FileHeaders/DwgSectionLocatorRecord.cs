﻿namespace ACadSharp.IO.DWG
{
	public class DwgSectionLocatorRecord
	{
		/// <summary>
		/// Number of the record or id.
		/// </summary>
		public int? Number { get; set; }
		
		/// <summary>
		/// Offset where the record is.
		/// </summary>
		public long Seeker { get; set; }
		
		/// <summary>
		/// Size in bytes of this record.
		/// </summary>
		public long Size { get; set; }
		
		public DwgSectionLocatorRecord() { }

		public DwgSectionLocatorRecord(int number, int offset, int size)
		{
			this.Number = number;
			this.Seeker = offset;
			this.Size = size;
		}
		/// <summary>
		/// Check if the position is in the record.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public bool IsInTheRecord(int position)
		{
			return position >= this.Seeker && position < this.Seeker + this.Size;
		}
	}
}