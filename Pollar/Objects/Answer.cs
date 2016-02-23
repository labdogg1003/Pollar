using System;
using Newtonsoft.Json;

namespace Pollzit
{
	public class Answer
	{
		[JsonProperty(PropertyName = "answer")]
		public string answer { get; set;}

		[JsonProperty(PropertyName = "count")]
		public int count { get; set;}

		public Answer ()
		{
			//set the initial count to 0
			this.count = 0;
		}
	}
}

