using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace com.interactiverobert.prototypes.movieexplorer.shared.Entities.Configuration
{
	public class ConfigurationResponse
	{
		[JsonProperty("images")]
		public ImageConfiguration Images { get; set; }

		[JsonProperty("change_keys")]
		public List<string> ChangeKeys { get; set; }

		public ConfigurationResponse () {
			
		}
	}
}