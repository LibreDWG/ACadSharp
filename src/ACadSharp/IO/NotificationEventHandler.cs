using System;
using System.Collections.Generic;
using System.Text;

namespace ACadSharp.IO
{
	public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);

	public enum NotificationType
	{
		NotImplemented = -1,
		None = 0,
		Info = 1,
		NotSupported = 2,
		Warning = 3,
		Error = 4,
	}

	public class NotificationEventArgs : EventArgs
	{
		public string Message { get; }

		public NotificationType NotificationType { get; }

		public Exception Exception { get; }

		public NotificationEventArgs(string message, NotificationType notificationType = NotificationType.None, Exception exception = null)
		{
			this.Message = message;
			this.NotificationType = notificationType;
			this.Exception = exception;
		}
	}
}
