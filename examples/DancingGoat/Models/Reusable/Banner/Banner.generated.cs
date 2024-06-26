//--------------------------------------------------------------------------------------------------
// <auto-generated>
//
//     This code was generated by code generator tool.
//
//     To customize the code use your own partial class. For more info about how to use and customize
//     the generated code see the documentation at https://docs.xperience.io/.
//
// </auto-generated>
//--------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using CMS.ContentEngine;

namespace DancingGoat.Models
{
	/// <summary>
	/// Represents a content item of type <see cref="Banner"/>.
	/// </summary>
	[RegisterContentTypeMapping(CONTENT_TYPE_NAME)]
	public partial class Banner : IContentItemFieldsSource
	{
		/// <summary>
		/// Code name of the content type.
		/// </summary>
		public const string CONTENT_TYPE_NAME = "DancingGoat.Banner";


		/// <summary>
		/// Represents system properties for a content item.
		/// </summary>
		[SystemField]
		public ContentItemFields SystemFields { get; set; }


		/// <summary>
		/// BannerBackgroundImage.
		/// </summary>
		public IEnumerable<Image> BannerBackgroundImage { get; set; }


		/// <summary>
		/// BannerHeaderText.
		/// </summary>
		public string BannerHeaderText { get; set; }


		/// <summary>
		/// BannerText.
		/// </summary>
		public string BannerText { get; set; }
	}
}