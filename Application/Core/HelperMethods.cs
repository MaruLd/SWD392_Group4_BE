using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlCleanser;

namespace Application.Core
{
	public static class HTMLHelper
	{
		public static string CleanupHTML(String rawHtml)
		{
			IHtmlCleanser cleanser = new HtmlCleanser.HtmlCleanser();
			var cleanHtml = cleanser.CleanseFull(rawHtml);
			return cleanHtml;
		}

	}
}