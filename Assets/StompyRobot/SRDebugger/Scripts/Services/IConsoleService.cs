using System.Collections.Generic;
using UnityEngine;

namespace SRDebugger.Services
{

	public delegate void ConsoleUpdatedEventHandler(IConsoleService console);

	public interface IConsoleService
	{

		int ErrorCount { get; }
		int WarningCount { get; }
		int InfoCount { get; }

		event ConsoleUpdatedEventHandler Updated;

		/// <summary>
		/// List of ConsoleEntry objects since the last clear.
		/// </summary>
		IList<ConsoleEntry> Entries { get; }

		/// <summary>
		/// List of all ConsoleEntry objects, regardless of clear.
		/// </summary>
		IList<ConsoleEntry> AllEntries { get; } 

		void Clear();

	}

	public class ConsoleEntry
	{

		public string Message;
		public string StackTrace;
		public LogType LogType;

		/// <summary>
		/// Number of times this log entry has occured (if collapsing is enabled)
		/// </summary>
		public int Count = 1;

		public bool Matches(ConsoleEntry other)
		{
			if (ReferenceEquals(null, other)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			return string.Equals(Message, other.Message) && string.Equals(StackTrace, other.StackTrace) && LogType == other.LogType;
		}

		public ConsoleEntry() { }

		public ConsoleEntry(ConsoleEntry other)
		{
			Message = other.Message;
			StackTrace = other.StackTrace;
			LogType = other.LogType;
			Count = other.Count;
		}

	}

}