using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace RedRockStudio.SZD.Common
{
	[Serializable]
	public class SkinAttachmentsData
	{
		[SerializeField] private SkeletonDataAsset _data;
		
		[SpineAttachment(dataField = nameof(_data), returnAttachmentPath = true)] [SerializeField] 
		private string[] _attachments;

		[field: SpineSlot(dataField = nameof(_data))] [field: SerializeField] 
		public string[] DisabledSlots { get; set; }

		public IDictionary<string, string> GetEnabledAttachments()
		{
			var result = new Dictionary<string, string>();
			foreach (string attachment in _attachments)
			{
				string[] path = attachment.Split("/");
				Debug.Assert(path.Length == 3, _data);
				result.Add(path[1], path[2]);
			}
			return result;
		}
	}
}