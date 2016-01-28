using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class GetVideosForMovieResponse
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("results")]
		public List<Video> Results { get; set; }

		public GetVideosForMovieResponse () {
			
		}
	}
}