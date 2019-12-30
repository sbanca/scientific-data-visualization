﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Shims;
using UnityEngine;

namespace Siccity.GLTFUtility {
	// https://github.com/KhronosGroup/glTF/blob/master/specification/2.0/README.md#asset-1
	/// <summary> Metadata about the glTF asset </summary>
	public class GLTFAsset {
		[Preserve] public GLTFAsset() { }

		/// <summary> A copyright message suitable for display to credit the content creator. </summary>
		public string copyright;
		/// <summary> Tool that generated this glTF model. Useful for debugging. </summary>
		public string generator;
		/// <summary> The glTF version that this asset targets. </summary>
		[JsonProperty(Required = Required.Always)] public string version;
		/// <summary> The minimum glTF version that this asset targets. </summary>
		public string minVersion;
	}
}