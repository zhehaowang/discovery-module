﻿using System;
using net.named_data.jndn;

namespace remap.NDNMOG.DiscoveryModule
{
	/// <summary>
	/// Callback for a set location prototype function, which should be implemented in Unity
	/// </summary>
	public delegate bool SetPosCallback(string name, Vector3 location);

	public enum EntityType
	{
		Player,
		NPC
	};

	public class GameEntity
	{
		private string name_;
		private Vector3 location_;

		private EntityType entityType_;
		private int timeoutCount_;

		private Exclude exclude_;
		private long previousRespondTime_;

		private SetPosCallback setPosCallback_;

		public GameEntity (string name, EntityType entityType)
		{
			name_ = name;
			entityType_ = entityType;
			location_ = new Vector3 (0, 0, 0);
			timeoutCount_ = 0;
			setPosCallback_ = null;
			exclude_ = new Exclude ();
			previousRespondTime_ = 0;
		}

		public GameEntity (string name, EntityType entityType, Vector3 location)
		{
			name_ = name;
			entityType_ = entityType;
			location_ = new Vector3 (location);
			timeoutCount_ = 0;
			setPosCallback_ = null;
			exclude_ = new Exclude ();
			previousRespondTime_ = 0;
		}

		public GameEntity (string name, EntityType entityType, Vector3 location, SetPosCallback setPosCallback)
		{
			name_ = name;
			entityType_ = entityType;
			location_ = new Vector3 (location);
			timeoutCount_ = 0;
			setPosCallback_ = setPosCallback;
			exclude_ = new Exclude ();
			previousRespondTime_ = 0;
		}

		public GameEntity (string name, EntityType entityType, float x, float y, float z)
		{
			name_ = name;
			entityType_ = entityType;
			location_ = new Vector3 (x, y, z);
			timeoutCount_ = 0;
			setPosCallback_ = null;
			exclude_ = new Exclude ();
			previousRespondTime_ = 0;
		}

		public string getName()
		{
			return name_;
		}

		public Vector3 getLocation()
		{
			return location_;
		}

		public void setPreviousRespondTime(long respondTime)
		{
			previousRespondTime_ = respondTime;
		}

		public long getPreviousRespondTime()
		{
			return previousRespondTime_;
		}

		public void setLocation(Vector3 location, bool invokeCallback)
		{
			location_ = location;
			if (invokeCallback) {
				if (setPosCallback_ == null) {
					Console.WriteLine ("setPosCallback_ for setLocation function is null.");
				} else {
					setPosCallback_ (name_, location_);
				}
			}
		}

		public void setLocation(float x, float y, float z)
		{
			location_.x_ = x;
			location_.y_ = y;
			location_.z_ = z;
			return;
		}

		public EntityType getType()
		{
			return entityType_;
		}

		/// <summary>
		/// Increments the timeout count.
		/// </summary>
		/// <returns><c>true</c>True, if timeout received in a row reached the cap value.<c>false</c> otherwise.</returns>
		public bool incrementTimeOut()
		{
			timeoutCount_++;
			if (timeoutCount_ > Constants.DropTimeoutCount) {
				return true;
			} else {
				return false;
			}
		}

		public void resetExclude()
		{
			exclude_ = new Exclude ();
		}

		public void addExclude(long versionNum)
		{
			exclude_.appendComponent (new Name().appendVersion(versionNum).get(0));
		}

		public Exclude getExclude()
		{
			return exclude_;
		}

		public void resetTimeOut()
		{
			timeoutCount_ = 0;
		}
	}
}

