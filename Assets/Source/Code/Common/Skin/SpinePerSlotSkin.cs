using System.Collections.Generic;
using Spine.Unity;

namespace RedRockStudio.SZD.Common
{
	public class SpinePerSlotSkin : ISkin
	{
		private readonly ISkeletonAnimation _animation;
		private readonly IDictionary<string, string> _attachments;
		private readonly IEnumerable<string> _disabledSlots;
		private readonly SkinAttachmentsData _attachmentsData;
		private readonly string _name;

		public SpinePerSlotSkin(
			ISkeletonAnimation animation, IDictionary<string, string> attachments, IEnumerable<string> disabledSlots)
		{
			_animation = animation;
			_attachments = attachments;
			_disabledSlots = disabledSlots;
		}
		
		public void Apply()
		{
			foreach ((string slot, string attachment) in _attachments)
				_animation.Skeleton.SetAttachment(slot, attachment);
			foreach (string slot in _disabledSlots)
				_animation.Skeleton.SetAttachment(slot, null);
			_animation.Skeleton.UpdateCache();
		}
	}
}