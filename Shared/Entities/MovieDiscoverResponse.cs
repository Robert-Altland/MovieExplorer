using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared
{
	public class MovieDiscoverResponse
	{
		[JsonProperty ("page")]
		public int Page { get; set; }

		[JsonProperty ("total_results")]
		public int TotalResults { get; set; }

		[JsonProperty ("total_pages")]
		public int TotalPages { get; set; }

		[JsonProperty ("results")]
		public List<Movie> Results { get; set; }

		public MovieDiscoverResponse ()
		{
		}
	}
}