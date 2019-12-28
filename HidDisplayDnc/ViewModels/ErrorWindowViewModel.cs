using System;
using System.Collections.Generic;
using System.Text;

namespace HidDisplayDnc.ViewModels
{
    /// <summary>
    /// View model for error window.
    /// </summary>
    public class ErrorWindowViewModel
    {
        /// <summary>
        /// Gets or sets text content.
        /// </summary>
        public string TextContent { get; set; }

        /// <summary>
        /// Gets or sets header message.
        /// </summary>
        public string FriendlyMessage { get; set; } = "Unhandled exception";

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWindowViewModel"/> class.
        /// </summary>
        public ErrorWindowViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorWindowViewModel"/> class.
        /// </summary>
        /// <param name="ex">Exception info.</param>
        public ErrorWindowViewModel(Exception ex)
        {
            TextContent = ErrorWindowViewModel.GetExceptionInfo(ex);
        }

        /// <summary>
        /// Serializes exception in the usual manner.
        /// </summary>
        /// <param name="ex">Exception to extract.</param>
        /// <returns>Exception content.</returns>
        public static string GetExceptionInfo(Exception ex)
        {
            var sb = new StringBuilder();

            string currentDepth = "Exception";
            var currentEx = ex;

            do
            {
                sb.AppendLine($"Exception current depth: {currentDepth}");
                sb.AppendLine();
                sb.AppendLine($"Exception message:");
                sb.AppendLine(currentEx.Message);
                sb.AppendLine();
                sb.AppendLine($"Exception type: {ex.GetType().FullName}");
                sb.AppendLine();
                sb.AppendLine($"Exception stack trace:");
                sb.Append(currentEx.StackTrace);
                sb.AppendLine();
                sb.AppendLine("==================================================");
                sb.AppendLine();

                currentDepth += ".InnerException";
                currentEx = currentEx.InnerException;

            } while (currentEx != null);

            return sb.ToString();
        }
    }
}
