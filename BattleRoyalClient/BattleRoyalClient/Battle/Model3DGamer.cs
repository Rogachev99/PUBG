﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using CSInteraction.Common;
using System.Drawing;

namespace BattleRoyalClient.Battle
{
	class Model3DGamer : Model3D
	{
		protected TypesWeapon lastCurrentWeapon;
		protected float koefStretch = 1;

		public Model3DGamer(Model3DGroup models, IModelObject modelObject) : base(models, modelObject)
		{
			lastCurrentWeapon = TypesWeapon.Not;
			SetKoef();
		}

		private void SetKoef()
		{
			switch (lastCurrentWeapon)
			{
				case TypesWeapon.Not:
					koefStretch = 1.1f;
					break;
				case TypesWeapon.Gun:
					koefStretch = 1.25f;
					break;
			}
			lastSize = new SizeF(koefStretch * modelObject.Size.Width,
				koefStretch * modelObject.Size.Height);
		}
		public override void Update()
		{
			var pos = modelObject.Location3D;

			translateTransform.OffsetX = pos.X;
			translateTransform.OffsetY = pos.Y;
			translateTransform.OffsetZ = pos.Z;

			if (modelObject is Gamer)
			{
				var gamer = (Gamer)modelObject;
				if (lastCurrentWeapon != gamer.CurrentWeapon)
				{
					lastCurrentWeapon = gamer.CurrentWeapon;
					Remove();
					//устанавливаем коээфициент увеличения для этого изображения
					SetKoef();
					CreateImage();
				}
			}
		}
	}
}
