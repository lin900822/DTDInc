﻿using Fusion;
using Fusion.KCC;
using UnityEngine;

namespace KCC_Processor
{
	public class PushKCCProcessor : NetworkKCCProcessor
	{
		[SerializeField] private LimitKinematicVelocityKCCProcessor limitKinematicVelocityProcessor;

		[Networked] private float impluseMagnitude { get; set; }
		[Networked] private Vector3 centerPoint { get; set; }

		public void SetCenterPoint(Vector3 centerPoint, float impluseMagnitude)
		{
			this.centerPoint = centerPoint;
			this.impluseMagnitude = impluseMagnitude;
		}

		public override EKCCStages GetValidStages(KCC kcc, KCCData data)
		{
			return EKCCStages.SetDynamicVelocity;
		}

		public override void SetDynamicVelocity(KCC kcc, KCCData data)
		{
			if (kcc.IsInFixedUpdate == false)
				return;

			var impluse = -(centerPoint - kcc.transform.position).normalized * impluseMagnitude;

			data.DynamicVelocity += impluse;
		}

		public override void OnEnter(KCC kcc, KCCData data)
		{
			if (kcc.IsInFixedUpdate == false)
				return;

			kcc.SetKinematicVelocity(Vector3.zero);

			kcc.SetPosition(data.TargetPosition);

			kcc.AddModifier(limitKinematicVelocityProcessor);
		}
	}
}