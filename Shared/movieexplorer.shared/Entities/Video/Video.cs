using System;
using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared.Entities.Video
{
	public class Video
	{
		[JsonProperty ("id")]
		public string Id { get; set; }

		[JsonProperty ("key")]
		public string Key { get; set; }

		[JsonProperty ("site")]
		public string Site { get; set; }

		public Video () {
			
		}
	}
}